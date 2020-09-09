using System;

namespace JYOOK.Application
{
    public interface IProduction
    {
        double SalesPrice { get; }
        double ProductCost { get; }

        event Action SalesPriceChanged;

        event Action ProductCostChanged;
    }
}