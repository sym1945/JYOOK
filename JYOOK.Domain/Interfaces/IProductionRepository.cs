using System.Collections.Generic;
using System.Threading.Tasks;

namespace JYOOK.Domain
{
    public interface IProductionRepository
    {
        Task<List<Production>> GetProductions();

        Task Save(List<Production> productions);
    }
}
