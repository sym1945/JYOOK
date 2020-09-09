using System.Collections.Generic;
using System.Threading.Tasks;

namespace JYOOK.Domain
{
    public interface ILiveStockRepository
    {
        Task<List<LivestockProduct>> GetLiveStockProducts();

        Task Save(List<LivestockProduct> livestockProducts);
    }
}
