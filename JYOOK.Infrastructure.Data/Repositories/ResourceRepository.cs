using JYOOK.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JYOOK.Infrastructure.Data
{
    public class ResourceRepository : IResourceRepository
    {
        // 임시
        public readonly static ResourceRepository Instance = new ResourceRepository();

        public const string EXTRA_RESOURCE_CONFIG_PATH = @"Configs\ExtraResources.xml";
        public const string PACK_RESOURCE_CONFIG_PATH = @"Configs\PackResources.xml";

        private readonly XmlSerializer<List<ExtraResource>> _ExtraResourceXmlSerializer = new XmlSerializer<List<ExtraResource>>(EXTRA_RESOURCE_CONFIG_PATH);
        private readonly XmlSerializer<List<PackResource>> _PackResourceXmlSerializer = new XmlSerializer<List<PackResource>>(PACK_RESOURCE_CONFIG_PATH);


        private ResourceRepository()
        {
        }

        public async Task<List<ExtraResource>> GetResources()
        {
            return await Task.Run(() =>
            {
                try
                {
                    return _ExtraResourceXmlSerializer.LoadXml();
                }
                catch
                {
                    return new List<ExtraResource>();
                }
            });
        }

        public async Task SaveResources(IEnumerable<ExtraResource> resources)
        {
            await Task.Run(() =>
            {
                _ExtraResourceXmlSerializer.SaveXml(resources.ToList());
            });
        }

        public async Task<List<PackResource>> GetPackResources()
        {
            return await Task.Run(() =>
            {
                try
                {
                    return _PackResourceXmlSerializer.LoadXml();
                }
                catch
                {
                    return new List<PackResource>();
                }
            });
        }

        public async Task SavePackResources(IEnumerable<PackResource> resources)
        {
            await Task.Run(() =>
            {
                _PackResourceXmlSerializer.SaveXml(resources.ToList());
            });
        }


    }
}

