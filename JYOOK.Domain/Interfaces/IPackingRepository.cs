using System.Collections.Generic;
using System.Threading.Tasks;

namespace JYOOK.Domain
{
    public interface IPackingRepository
    {
        Task<List<PackingInfo>> GetPackingInfos();

        Task Save(List<PackingInfo> packingInfos);
    }
}