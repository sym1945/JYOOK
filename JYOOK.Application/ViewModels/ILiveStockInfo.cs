using System;

namespace JYOOK.Application
{
    public interface ILiveStockInfo
    {
        double Weight { get; set; }

        uint UnitPrice { get; set; }

        event Action WeightChanged;

        event Action UnitPriceChanged; 
    }
}