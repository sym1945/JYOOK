using JYOOK.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JYOOK.Infrastructure.Data
{
    public class ProductionRepository : IProductionRepository
    {
        private const string CONFIG_PATH = @"Configs\Productions.xml";

        private readonly XmlSerializer<List<Production>> _XmlSerializer = new XmlSerializer<List<Production>>(CONFIG_PATH);


        public ProductionRepository()
        {
        }


        public async Task<List<Production>> GetProductions()
        {
            return await Task.Run(() =>
            {
                try
                {
                    return _XmlSerializer.LoadXml();
                }
                catch
                {
                    var defaultValue = new List<Production>
                    {
                        new Production
                        {
                            Id = 1,
                            LivestockProductId = 1,
                            Name = "미삼",
                            Weight = 15,
                            TransferRate = 280,
                            MarginRate = 39
                        },
                        new Production
                        {
                            Id = 2,
                            LivestockProductId = 1,
                            Name = "목살",
                            Weight = 5,
                            TransferRate = 260,
                            MarginRate = 25
                        },
                        new Production
                        {
                            Id = 3,
                            LivestockProductId = 1,
                            Name = "갈비",
                            Weight = 2.9,
                            TransferRate = 120,
                            MarginRate = 40
                        },
                        new Production
                        {
                            Id = 4,
                            LivestockProductId = 1,
                            Name = "미전지",
                            Weight = 7,
                            TransferRate = 120,
                            MarginRate = 40
                        },
                        new Production
                        {
                            Id = 5,
                            LivestockProductId = 1,
                            Name = "미후지",
                            Weight = 16,
                            TransferRate = 50,
                            MarginRate = 40
                        },
                        new Production
                        {
                            Id = 6,
                            LivestockProductId = 1,
                            Name = "등심",
                            Weight = 3.6,
                            TransferRate = 100,
                            MarginRate = 40
                        },
                        new Production
                        {
                            Id = 7,
                            LivestockProductId = 1,
                            Name = "안심",
                            Weight = 1.2,
                            TransferRate = 100,
                            MarginRate = 40
                        },
                        new Production
                        {
                            Id = 8,
                            LivestockProductId = 1,
                            Name = "사태",
                            Weight = 3.3,
                            TransferRate = 120,
                            MarginRate = 40
                        },
                        new Production
                        {
                            Id = 9,
                            LivestockProductId = 1,
                            Name = "등갈비",
                            Weight = 1.3,
                            TransferRate = 230,
                            MarginRate = 40
                        },
                        new Production
                        {
                            Id = 10,
                            LivestockProductId = 1,
                            Name = "항정살",
                            Weight = 0.3,
                            TransferRate = 300,
                            MarginRate = 40
                        },
                        new Production
                        {
                            Id = 11,
                            LivestockProductId = 1,
                            Name = "가브리살",
                            Weight = 0.3,
                            TransferRate = 300,
                            MarginRate = 40
                        },
                        new Production
                        {
                            Id = 12,
                            LivestockProductId = 1,
                            Name = "갈매기살",
                            Weight = 0.3,
                            TransferRate = 300,
                            MarginRate = 40
                        },
                        new Production
                        {
                            Id = 13,
                            LivestockProductId = 1,
                            Name = "등뼈",
                            Weight = 4.8,
                            TransferRate = 50,
                            MarginRate = 10
                        },
                    };

                    _XmlSerializer.SaveXml(defaultValue);

                    return defaultValue;
                }
            });
        }

        public async Task Save(List<Production> productions)
        {
            await Task.Run(() =>
            {
                _XmlSerializer.SaveXml(productions);
            });
        }

    }
}