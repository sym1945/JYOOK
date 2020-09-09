using JYOOK.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JYOOK.Infrastructure.Data
{
    public class LiveStockRepository : ILiveStockRepository
    {
        private const string CONFIG_PATH = @"Configs\LiveStocks.xml";

        private readonly XmlSerializer<List<LivestockProduct>> _XmlSerializer = new XmlSerializer<List<LivestockProduct>>(CONFIG_PATH);


        public LiveStockRepository()
        {
        }

        public async Task<List<LivestockProduct>> GetLiveStockProducts()
        {
            return await Task.Run(() =>
            {
                try
                {
                    return _XmlSerializer.LoadXml();
                }
                catch
                {
                    var defaultValues = new List<LivestockProduct>
                    {
                        new LivestockProduct
                        {
                            Id = 1,
                            Name = "돼지",
                            UnitPrice = 5200,
                            Weight = 88,
                        },
                    };

                    _XmlSerializer.SaveXml(defaultValues);

                    return defaultValues;
                }
            });
        }

        public async Task Save(List<LivestockProduct> livestockProducts)
        {
            await Task.Run(() =>
            {
                _XmlSerializer.SaveXml(livestockProducts);
            });
        }

    }
}