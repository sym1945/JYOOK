using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace JYOOK.Application
{
    public class ProductionViewModel : ViewModelBase, IProduction
    {
        private ILiveStockInfo _LiveStockInfo;

        public int Id { get; set; }

        public string Name { get; set; }

        public double Weight { get; set; }

        public double TransferRate { get; set; }

        public double MarginRate { get; set; }

        public double ProductCost => _LiveStockInfo.UnitPrice * (TransferRate / 100);

        public double TotalProductCost => ProductCost * Weight;

        [XmlIgnore]
        public double SalesPrice
        {
            get => ProductCost / (1 - MarginRate / 100);
            set
            {
                MarginRate = 100 * (1 - (ProductCost / value));
            }
        } 

        public double TotalSalesPrice => SalesPrice * Weight;


        public event Action SalesPriceChanged;

        public event Action ProductCostChanged;

        public ProductionViewModel()
        { 
        }

        public ProductionViewModel(ILiveStockInfo liveStockInfo)
        {
            _LiveStockInfo = liveStockInfo;
            _LiveStockInfo.UnitPriceChanged += LiveStockInfo_UnitPriceChanged;
            _LiveStockInfo.WeightChanged += LiveStockInfo_WeightChanged;
        }

        private void LiveStockInfo_WeightChanged()
        {
            OnPropertyChanged(nameof(Weight));
            OnPropertyChanged(nameof(TotalProductCost));
            OnPropertyChanged(nameof(TotalSalesPrice));
        }

        private void LiveStockInfo_UnitPriceChanged()
        {
            OnPropertyChanged(nameof(ProductCost));
            OnPropertyChanged(nameof(TotalProductCost));
            OnPropertyChanged(nameof(SalesPrice));
            OnPropertyChanged(nameof(TotalSalesPrice));
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            switch (propertyName)
            {
                case nameof(SalesPrice): SalesPriceChanged?.Invoke(); break;
                case nameof(ProductCost): ProductCostChanged?.Invoke(); break;
            }

            base.OnPropertyChanged(propertyName);
        }

    }
}
