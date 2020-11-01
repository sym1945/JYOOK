using JYOOK.Application;
using JYOOK.Domain;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace JYOOK
{
    public class MainViewModel : ViewModelBase
    {
        private readonly SalesProductService _SalesProductService = SalesProductService.Instance;

        private readonly PackResourceService _PackResourceService = PackResourceService.Instance;


        public string Title => $"JYOOK - {GetType().Assembly.GetName().Version}";


        public SalesManagerViewModel SalesManagerViewModel { get; set; }


        public MainViewModel()
        {
            _PackResourceService.ResourceModified += PackResourceService_ResourceModified;
            _PackResourceService.ResourceRemoved += PackResourceService_ResourceRemoved;
        }

        public async Task Initailize()
        {
            if (SalesManagerViewModel == null)
            {
                var salesProducts = await _SalesProductService.GetSalesProducts();
                var pigStock = salesProducts
                    .Select(d => d.Production.LivestockProduct)
                    .FirstOrDefault(d => d.Name.Equals("돼지"));

                // Create SalesManagerViewModel
                SalesManagerViewModel = new SalesManagerViewModel
                {
                    Id = pigStock.Id,
                    Name = pigStock.Name,
                    UnitPrice = pigStock.UnitPrice,
                    Weight = pigStock.Weight,
                };

                foreach (var salesProduct in salesProducts.Where(d => d.Production.LivestockProduct.Name.Equals("돼지")))
                {
                    // Add ProductionViewModel
                    var productionViewMdoel = new ProductionViewModel(SalesManagerViewModel)
                    {
                        Id = salesProduct.Id,
                        Name = salesProduct.Production.Name,
                        Weight = salesProduct.Production.Weight,
                        TransferRate = salesProduct.Production.TransferRate,
                        MarginRate = salesProduct.Production.MarginRate,
                    };

                    productionViewMdoel.PropertyChanged += ProductionViewMdoel_PropertyChanged;
                    SalesManagerViewModel.Productions.Add(productionViewMdoel);

                    // Add SalesProductViewModel
                    var salesProductViewModel = new SalesProductViewModel(productionViewMdoel)
                    {
                        Id = salesProduct.Id,
                        Name = salesProduct.Production.Name,
                        CommisionRate = salesProduct.CommssionRate,
                        ShippingCost = salesProduct.ShippingCost,
                        Weight = salesProduct.Weight,
                    };

                    var packingInfo = salesProduct.PackingInfo;
                    if (packingInfo != null)
                    {
                        salesProductViewModel.PackResources.PackingWeight = packingInfo.PackingWeight;
                        foreach (var packResource in packingInfo.Resources)
                        {
                            salesProductViewModel.PackResources.ItemList.Add(new PackResourceViewModel
                            {
                                Id = packResource.Resource.Id,
                                Name = packResource.Resource.Name,
                                Price = packResource.Resource.Price,
                                Type = packResource.Resource.Type,
                                Count = packResource.Count,
                            });
                        }
                    }

                    salesProductViewModel.PackResources.ApplyToAllProductRequest += PackResources_ApplyToAllProductRequest;
                    salesProductViewModel.PackingInfoUpdated += SalesProductViewModel_PackingInfoUpdated;
                    salesProductViewModel.PackingResourceUpdated += SalesProductViewModel_PackingResourceUpdated;
                    salesProductViewModel.PropertyChanged += SalesProductViewModel_PropertyChanged;
                    SalesManagerViewModel.SalesProducts.Add(salesProductViewModel);
                }

                // Initialize PropertyChangedEvent
                SalesManagerViewModel.PropertyChanged += SalesManagerViewModel_PropertyChanged;
            }
        }

        public void SetSalesManager(SalesManagerViewModel newModel)
        {
            if (newModel == null)
                return;

            SalesManagerViewModel.UnitPrice = newModel.UnitPrice;
            SalesManagerViewModel.Weight = newModel.Weight;

            foreach (var newProduction in newModel.Productions)
            {
                var production = SalesManagerViewModel.Productions.FirstOrDefault(d => d.Id == newProduction.Id);
                if (production != null)
                {
                    production.Weight = newProduction.Weight;
                    production.TransferRate = newProduction.TransferRate;
                    production.MarginRate = newProduction.MarginRate;
                }
            }
        }

        /// <summary>
        /// Production 변경 시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ProductionViewMdoel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var production = (ProductionViewModel)sender;

            switch (e.PropertyName)
            {
                case nameof(production.Name):
                case nameof(production.Weight):
                case nameof(production.TransferRate):
                case nameof(production.MarginRate):
                    {
                        await _SalesProductService.UpdateProduction(new Production
                        {
                            Id = production.Id,
                            Name = production.Name,
                            Weight = production.Weight,
                            TransferRate = production.TransferRate,
                            MarginRate = production.MarginRate,
                        });
                        break;
                    }
            }
        }

        private void PackResources_ApplyToAllProductRequest(object sender, IEnumerable<PackResourceViewModel> packResources)
        {
            // TODO: 돼지 한정임...
            var eventProduct = SalesManagerViewModel.SalesProducts.FirstOrDefault(d => d.PackResources.Equals(sender));
            if (eventProduct == null)
                return;

            foreach (var salesProduct in SalesManagerViewModel.SalesProducts)
            {
                if (salesProduct.Equals(eventProduct))
                    continue;

                salesProduct.PackResources.ChangePackResources(packResources);
                salesProduct.PackResources.PackingWeight = eventProduct.PackResources.PackingWeight;
            }
        }

        private async void SalesProductViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var salesProduct = (SalesProductViewModel)sender;

            switch (e.PropertyName)
            {
                case nameof(salesProduct.Name):
                case nameof(salesProduct.Weight):
                case nameof(salesProduct.CommisionRate):
                case nameof(salesProduct.ShippingCost):
                    {
                        await _SalesProductService.UpdateSalesProduct(new SalesProduct
                        {
                            Id = salesProduct.Id,
                            Weight = salesProduct.Weight,
                            CommssionRate = salesProduct.CommisionRate,
                            ShippingCost = salesProduct.ShippingCost,
                        });
                        break;
                    }
            }
        }

        private async void SalesProductViewModel_PackingResourceUpdated(SalesProductViewModel salesProduct)
        {
            await _SalesProductService.UpdatePackResources(new PackingInfo
            {
                SalesProductId = salesProduct.Id,
                PackingWeight = salesProduct.PackResources.PackingWeight,
                Resources = salesProduct.PackResources.ItemList.Select(d => new PackResource
                {
                    ExtraResourceId = d.Id,
                    Count = d.Count,
                    SalesProductId = salesProduct.Id,
                }).ToList(),
            });
        }

        private async void SalesProductViewModel_PackingInfoUpdated(SalesProductViewModel salesProduct)
        {
            await _SalesProductService.UpdatePackinInfo(new PackingInfo
            {
                SalesProductId = salesProduct.Id,
                PackingWeight = salesProduct.PackResources.PackingWeight,
            });
        }

        /// <summary>
        /// Livestock Info 변경 시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SalesManagerViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var salesManager = (SalesManagerViewModel)sender;

            switch (e.PropertyName)
            {
                case nameof(salesManager.Weight):
                case nameof(salesManager.UnitPrice):
                    {
                        await _SalesProductService.UpdateLivestockInfo(new LivestockProduct
                        {
                            Id = salesManager.Id,
                            Name = salesManager.Name,
                            Weight = salesManager.Weight,
                            UnitPrice = salesManager.UnitPrice,
                        });
                        break;
                    }
            }
        }

        private void PackResourceService_ResourceRemoved(object sender, IEnumerable<ExtraResource> removedResources)
        {
            var removedResourceIds = removedResources.Select(d => d.Id).ToList();

            // TODO: 돼지 한정
            foreach (var salesProduct in SalesManagerViewModel.SalesProducts)
            {
                foreach (var packResource in salesProduct.PackResources.ItemList.ToList())
                {
                    if (removedResourceIds.Contains(packResource.Id))
                    {
                        salesProduct.PackResources.ItemList.Remove(packResource);
                    }
                }
            }
        }

        private void PackResourceService_ResourceModified(object sender, IEnumerable<ExtraResource> modifiedResources)
        {
            // TODO: 돼지 한정
            foreach (var salesProduct in SalesManagerViewModel.SalesProducts)
            {
                foreach (var packResource in salesProduct.PackResources.ItemList)
                {
                    var modifiedResource = modifiedResources.FirstOrDefault(d => d.Id.Equals(packResource.Id));
                    if (modifiedResource != null)
                    {
                        packResource.Name = modifiedResource.Name;
                        packResource.Price = modifiedResource.Price;
                        packResource.Type = modifiedResource.Type;
                    }
                }
            }
        }


    }


}