using System.Collections.Generic;
using System.Threading.Tasks;

namespace JYOOK.Domain
{
    public interface ISalesProductRepository
    {
        Task<List<SalesProduct>> GetSalesProduct();

        Task Save(List<SalesProduct> salesProducts);
    }
}
