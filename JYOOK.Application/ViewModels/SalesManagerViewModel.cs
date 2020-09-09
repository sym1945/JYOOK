using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace JYOOK.Application
{
    public class SalesManagerViewModel : ViewModelBase, ILiveStockInfo
    {
        private double _Weight;
        private uint _UnitPrice;

        public int Id { get; set; }

        public string Name { get; set; }

        public double Weight
        {
            get => _Weight;
            set
            {
                if (_Weight == value)
                    return;
                _Weight = value;
                WeightChanged?.Invoke();
            }
        }

        public uint UnitPrice
        {
            get => _UnitPrice;
            set
            {
                if (_UnitPrice == value)
                    return;
                _UnitPrice = value;
                UnitPriceChanged?.Invoke();
            }
        }

        public ObservableCollection<ProductionViewModel> Productions { get; set; } = new ObservableCollection<ProductionViewModel>();

        public ObservableCollection<SalesProductViewModel> SalesProducts { get; set; } = new ObservableCollection<SalesProductViewModel>();


        public double LivestockProductCost => UnitPrice * Weight;

        public double TotalProductCost => Productions.Sum(d => d.TotalProductCost);

        public double TotalSalesPrice => Productions.Sum(d => d.TotalSalesPrice);

        public double ProductProfit => TotalProductCost - LivestockProductCost;

        public double TotalProfit => TotalSalesPrice + ProductProfit - LivestockProductCost;

        public double TotalMarginRate => (TotalProfit / (TotalSalesPrice + ProductProfit)) * 100;


        public event Action WeightChanged;
        public event Action UnitPriceChanged;


        public SalesManagerViewModel()
        {
            Productions.CollectionChanged += Prodcutions_CollectionChanged;
            //SalesProducts.CollectionChanged += SalesProducts_CollectionChanged;
        }

        private void SalesProducts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        foreach (INotifyPropertyChanged newItem in e.NewItems)
                        {
                            newItem.PropertyChanged += SalesProduct_PropertyChanged;
                        }
                        break;
                    }
            }
        }

        private void Prodcutions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        foreach (INotifyPropertyChanged newItem in e.NewItems)
                        {
                            newItem.PropertyChanged += Production_PropertyChanged;
                        }
                        break;
                    }
            }
        }

        private void SalesProduct_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        private void Production_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TotalProductCost":
                    {
                        OnPropertyChanged(nameof(TotalProductCost));
                        OnPropertyChanged(nameof(TotalProfit));
                        OnPropertyChanged(nameof(TotalMarginRate));
                        OnPropertyChanged(nameof(ProductProfit));
                        break;
                    }
                case "TotalSalesPrice":
                    {
                        OnPropertyChanged(nameof(TotalSalesPrice));
                        OnPropertyChanged(nameof(TotalProfit));
                        OnPropertyChanged(nameof(TotalMarginRate));
                        break;
                    }
            }
        }

    }
}