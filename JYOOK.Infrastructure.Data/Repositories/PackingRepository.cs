using JYOOK.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JYOOK.Infrastructure.Data
{
    public class PackingRepository : IPackingRepository
    {
        private const string CONFIG_PATH = @"Configs\Packings.xml";

        private readonly XmlSerializer<List<PackingInfo>> _XmlSerializer = new XmlSerializer<List<PackingInfo>>(CONFIG_PATH);


        public PackingRepository()
        {
        }

        public async Task<List<PackingInfo>> GetPackingInfos()
        {
            return await Task.Run(() =>
            {
                try
                {
                    return _XmlSerializer.LoadXml();
                }
                catch
                {
                    return new List<PackingInfo>();
                }
            });
        }

        public async Task Save(List<PackingInfo> packingInfos)
        {
            await Task.Run(() =>
            {
                _XmlSerializer.SaveXml(packingInfos);
            });
        }
    }
}