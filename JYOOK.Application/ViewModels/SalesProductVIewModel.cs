using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace JYOOK.Application
{
    public class SalesProductViewModel : ViewModelBase
    {
        private string _Name;
        private IProduction _Production;
        public static List<double> WeightList => new List<double> { 0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0, 5.5, 6.0, 6.5, 7.0, 7.5, 8.0, 8.5, 9.0, 9.5, 10.0 };

        public int Id { get; set; }
        public string Name
        {
            get => _Name;
            set
            {
                if (_Name == value)
                    return;
                _Name = value;
                PackResources.ProductName = _Name;
                OnPropertyChanged();
            }
        }
        public double Weight { get; set; }

        public double SalesPricePerWeight => _Production.SalesPrice;

        public double SalesPrice => SalesPricePerWeight * Weight;

        public double ProductCost => _Production.ProductCost * Weight;

        public double CommisionRate { get; set; }

        public double Commision => SalesPrice * (CommisionRate / 100);

        public double ShippingCost { get; set; }

        public bool HasPackResources => PackResources.ItemList.Count > 0;

        public ResourceListViewModel PackResources { get; set; } = new ResourceListViewModel();

        public double PackingCost => PackResources.TotalPrice;

        public double Profit => SalesPrice - ProductCost - PackingCost - ShippingCost - Commision;


        public event Action<SalesProductViewModel> PackingInfoUpdated;

        public event Action<SalesProductViewModel> PackingResourceUpdated;


        public SalesProductViewModel(IProduction production)
        {
            _Production = production;
            _Production.ProductCostChanged += Production_ProductCostChanged;
            _Production.SalesPriceChanged += Production_SalesPriceChanged;
            PackResources.PropertyChanged += PackResources_PropertyChanged;
        }

        private void Production_SalesPriceChanged()
        {
            OnPropertyChanged(nameof(SalesPricePerWeight));
            OnPropertyChanged(nameof(SalesPrice));
            OnPropertyChanged(nameof(Commision));
            OnPropertyChanged(nameof(Profit));
        }

        private void Production_ProductCostChanged()
        {
            OnPropertyChanged(nameof(ProductCost));
            OnPropertyChanged(nameof(Profit));
        }

        private void PackResources_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(PackingCost));
            OnPropertyChanged(nameof(Profit));

            switch (e.PropertyName)
            {
                case nameof(PackResources.PackingWeight):
                    {
                        PackingInfoUpdated?.Invoke(this);
                        break;
                    }
                case nameof(PackResources.ItemList):
                    {
                        PackingResourceUpdated?.Invoke(this);
                        OnPropertyChanged(nameof(HasPackResources));
                        break;
                    }
            }
        }

    }
}
