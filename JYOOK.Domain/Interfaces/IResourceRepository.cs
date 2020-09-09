using System.Collections.Generic;
using System.Threading.Tasks;

namespace JYOOK.Domain
{
    public interface IResourceRepository
    {
        Task<List<ExtraResource>> GetResources();

        Task SaveResources(IEnumerable<ExtraResource> resources);

        Task<List<PackResource>> GetPackResources();

        Task SavePackResources(IEnumerable<PackResource> resources);
    }
}
