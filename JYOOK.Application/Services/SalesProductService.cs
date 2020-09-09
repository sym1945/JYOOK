using JYOOK.Domain;
using JYOOK.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JYOOK.Application
{
    public class SalesProductService
    {
        private readonly static AsyncLock _AsyncLock = new AsyncLock();
        // 임시
        public readonly static SalesProductService Instance = new SalesProductService();
        // 임시
        private readonly ILiveStockRepository _LiveStockRepo = new LiveStockRepository();
        // 임시
        private readonly IProductionRepository _ProductionRepo = new ProductionRepository();
        // 임시
        private readonly ISalesProductRepository _SalesProductRepo = new SalesProductRepository();
        // 임시
        private readonly IResourceRepository _ResourceRepo = ResourceRepository.Instance;
        // 임시
        private readonly IPackingRepository _PackingRepo = new PackingRepository();


        private List<SalesProduct> _SalesProducts;


        private SalesProductService()
        {

        }


        public async Task<List<SalesProduct>> GetSalesProducts()
        {
            if (_SalesProducts == null)
            {
                _SalesProducts = new List<SalesProduct>();

                var livestocks = await _LiveStockRepo.GetLiveStockProducts();
                var productions = await _ProductionRepo.GetProductions();
                var salesProducts = await _SalesProductRepo.GetSalesProduct();
                var packingInfos = await _PackingRepo.GetPackingInfos();
                var resources = await _ResourceRepo.GetResources();
                var packResources = await _ResourceRepo.GetPackResources();

                foreach (var packResouce in packResources)
                {
                    packResouce.Resource = resources.FirstOrDefault(d => d.Id == packResouce.ExtraResourceId);
                }

                foreach (var packingInfo in packingInfos)
                {
                    packingInfo.Resources.AddRange(packResources.Where(d => d.SalesProductId == packingInfo.SalesProductId));
                }

                foreach (var production in productions)
                {
                    production.LivestockProduct = livestocks.FirstOrDefault(d => d.Id == production.LivestockProductId);
                }

                foreach (var salesProduct in salesProducts)
                {
                    salesProduct.Production = productions.FirstOrDefault(d => d.Id == salesProduct.ProductionId);
                    salesProduct.PackingInfo = packingInfos.FirstOrDefault(d => d.SalesProductId == salesProduct.Id) ?? new PackingInfo { SalesProductId = salesProduct.Id, PackingWeight = 1 };
                }

                _SalesProducts.AddRange(salesProducts);
            }

            return _SalesProducts;
        }

        public async Task UpdateProduction(Production updatedItem)
        {
            var production = _SalesProducts.Select(d => d.Production).FirstOrDefault(d => d.Id == updatedItem.Id);
            if (production == null)
                return;

            production.Name = updatedItem.Name;
            production.Weight = updatedItem.Weight;
            production.TransferRate = updatedItem.TransferRate;
            production.MarginRate = updatedItem.MarginRate;

            var productions = _SalesProducts.Select(d => d.Production).Distinct().ToList();

            await _ProductionRepo.Save(productions);
        }

        public async Task UpdateSalesProduct(SalesProduct updatedItem)
        {
            var salesProduct = _SalesProducts.FirstOrDefault(d => d.Id == updatedItem.Id);
            if (salesProduct == null)
                return;

            salesProduct.Weight = updatedItem.Weight;
            salesProduct.CommssionRate = updatedItem.CommssionRate;
            salesProduct.ShippingCost = updatedItem.ShippingCost;

            await _SalesProductRepo.Save(_SalesProducts);
        }

        public async Task UpdatePackResources(PackingInfo updatedItem)
        {
            using (await _AsyncLock.LockAsync())
            {
                var salesProduct = _SalesProducts.FirstOrDefault(d => d.Id == updatedItem.SalesProductId);
                if (salesProduct == null)
                    return;

                salesProduct.PackingInfo.Resources.Clear();
                salesProduct.PackingInfo.Resources.AddRange(updatedItem.Resources);

                var packResources = _SalesProducts.SelectMany(d => d.PackingInfo.Resources).Distinct().ToList();
                var packInfos = _SalesProducts.Select(d => d.PackingInfo).Distinct().ToList();

                await _ResourceRepo.SavePackResources(packResources);

                await _PackingRepo.Save(packInfos);
            }
        }

        public async Task UpdateLivestockInfo(LivestockProduct updateItem)
        {
            var livestocks = _SalesProducts.Select(d => d.Production.LivestockProduct).Distinct().ToList();
            var targetItem = livestocks.FirstOrDefault(d => d.Id == updateItem.Id);
            if (targetItem == null)
                return;

            targetItem.Name = updateItem.Name;
            targetItem.UnitPrice = updateItem.UnitPrice;
            targetItem.Weight = updateItem.Weight;

            await _LiveStockRepo.Save(livestocks);
        }

        public async Task UpdatePackinInfo(PackingInfo updateItem)
        {
            var packInfos = _SalesProducts.Select(d => d.PackingInfo).Distinct().ToList();
            var targetItem = packInfos.FirstOrDefault(d => d.SalesProductId == updateItem.SalesProductId);
            if (targetItem == null)
                return;

            targetItem.PackingWeight = updateItem.PackingWeight;

            await _PackingRepo.Save(packInfos);
        }


    }
}