using JYOOK.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JYOOK.Infrastructure.Data
{
    public class PackingTemplateRepository : IPackingTemplateRepository
    {
        public const string PACKING_TEMPLATE_CONFIG_PATH = @"Configs\PackingTemplates.xml";
        public const string PACK_RESOURCE_TEMPLATE_CONFIG_PATH = @"Configs\PackResourceTemplates.xml";

        private readonly XmlSerializer<List<ExtraResource>> _ExtraResourceXmlSerailizer = new XmlSerializer<List<ExtraResource>>(ResourceRepository.EXTRA_RESOURCE_CONFIG_PATH);
        private readonly XmlSerializer<List<PackingTemplate>> _PackingTemplateXmlSerializer = new XmlSerializer<List<PackingTemplate>>(PACKING_TEMPLATE_CONFIG_PATH);
        private readonly XmlSerializer<List<PackResourceTemplate>> _PackResourceTemplateXmlSerializer = new XmlSerializer<List<PackResourceTemplate>>(PACK_RESOURCE_TEMPLATE_CONFIG_PATH);

        public async Task<List<PackingTemplate>> GetPackinTemplates()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var resources = _ExtraResourceXmlSerailizer.LoadXml();
                    var packResources = _PackResourceTemplateXmlSerializer.LoadXml();
                    var packTemplates = _PackingTemplateXmlSerializer.LoadXml();

                    foreach (var packResource in packResources)
                    {
                        packResource.Resource = resources.FirstOrDefault(d => d.Id == packResource.ExtraResourceId);
                    }
                    foreach (var packTemplate in packTemplates)
                    {
                        packTemplate.Resources.AddRange(packResources.Where(d => d.PackingTemplateId == packTemplate.Id));
                    }

                    return packTemplates;
                }
                catch
                {
                    return new List<PackingTemplate>();
                }
            });
        }

        public async Task Save(List<PackingTemplate> packingInfos)
        {
            await Task.Run(() =>
            {
                _PackResourceTemplateXmlSerializer.SaveXml(packingInfos.SelectMany(d => d.Resources).Distinct().ToList());

                _PackingTemplateXmlSerializer.SaveXml(packingInfos);
            });
        }
    }
}