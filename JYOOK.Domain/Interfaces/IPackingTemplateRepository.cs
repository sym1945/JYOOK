using System.Collections.Generic;
using System.Threading.Tasks;

namespace JYOOK.Domain
{
    public interface IPackingTemplateRepository
    {
        Task<List<PackingTemplate>> GetPackinTemplates();

        Task Save(List<PackingTemplate> packingInfos);
    }
}
