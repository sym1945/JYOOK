using JYOOK.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JYOOK.Infrastructure.Data
{
    public class SalesProductRepository : ISalesProductRepository
    {
        private const string CONFIG_PATH = @"Configs\SalesProducts.xml";

        private readonly XmlSerializer<List<SalesProduct>> _XmlSerializer = new XmlSerializer<List<SalesProduct>>(CONFIG_PATH);


        public SalesProductRepository()
        {
        }

        public async Task<List<SalesProduct>> GetSalesProduct()
        {
            return await Task.Run(() =>
            {
                try
                {
                    return _XmlSerializer.LoadXml();
                }
                catch
                {
                    var defaultValue = new List<SalesProduct>
                    {
                        new SalesProduct
                        {
                            Id = 1,
                            ProductionId = 1,
                            Weight = 0.5,
                            CommssionRate = 5,
                            ShippingCost = 0,
                        },
                        new SalesProduct
                        {
                            Id = 2,
                            ProductionId = 2,
                            Weight = 0.5,
                            CommssionRate = 5,
                            ShippingCost = 0,
                        },
                        new SalesProduct
                        {
                            Id = 3,
                            ProductionId = 3,
                            Weight = 0.5,
                            CommssionRate = 5,
                            ShippingCost = 0,
                        },
                        new SalesProduct
                        {
                            Id = 4,
                            ProductionId = 4,
                            Weight = 0.5,
                            CommssionRate = 5,
                            ShippingCost = 0,
                        },
                        new SalesProduct
                        {
                            Id = 5,
                            ProductionId = 5,
                            Weight = 0.5,
                            CommssionRate = 5,
                            ShippingCost = 0,
                        },
                        new SalesProduct
                        {
                            Id = 6,
                            ProductionId = 6,
                            Weight = 0.5,
                            CommssionRate = 5,
                            ShippingCost = 0
                        },
                        new SalesProduct
                        {
                            Id = 7,
                            ProductionId = 7,
                            Weight = 0.5,
                            CommssionRate = 5,
                            ShippingCost = 0
                        },
                        new SalesProduct
                        {
                            Id = 8,
                            ProductionId = 8,
                            Weight = 0.5,
                            CommssionRate = 5,
                            ShippingCost = 0
                        },
                        new SalesProduct
                        {
                            Id = 9,
                            ProductionId = 9,
                            Weight = 0.5,
                            CommssionRate = 5,
                            ShippingCost = 0
                        },
                        new SalesProduct
                        {
                            Id = 10,
                            ProductionId = 10,
                            Weight = 0.5,
                            CommssionRate = 5,
                            ShippingCost = 0
                        },
                        new SalesProduct
                        {
                            Id = 11,
                            ProductionId = 11,
                            Weight = 0.5,
                            CommssionRate = 5,
                            ShippingCost = 0
                        },
                        new SalesProduct
                        {
                            Id = 12,
                            ProductionId = 12,
                            Weight = 0.5,
                            CommssionRate = 5,
                            ShippingCost = 0
                        },
                        new SalesProduct
                        {
                            Id = 13,
                            ProductionId = 13,
                            Weight = 1.0,
                            CommssionRate = 5,
                            ShippingCost = 0
                        },
                    };

                    _XmlSerializer.SaveXml(defaultValue);

                    return defaultValue;
                }
            });
        }

        public async Task Save(List<SalesProduct> salesProducts)
        {
            await Task.Run(() =>
            {
                _XmlSerializer.SaveXml(salesProducts);
            });
        }

    }
}