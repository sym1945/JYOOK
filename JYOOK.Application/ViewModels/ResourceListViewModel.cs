using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace JYOOK.Application
{
    public class ResourceListViewModel : ViewModelBase
    {
        public string Title => $"{ProductName} 부자재 리스트";

        public string ProductName { get; set; }

        public ObservableCollection<PackResourceViewModel> ItemList { get; set; } = new ObservableCollection<PackResourceViewModel>();

        public double PackingWeight { get; set; } = 1;

        public double TotalPrice => ItemList.Sum(d => d.TotalPrice.GetValueOrDefault());

        public double PricePerWeight => TotalPrice / PackingWeight;


        public event Action<object, IEnumerable<PackResourceViewModel>> ApplyToAllProductRequest;

        public ICommand ApplyToAllProductCommand
        {
            get => new CommandBase
            {
                ExecuteAction = (param) =>
                {
                    if (OnRequestMessage("모든 상품에 적용하시겠습니까?"))
                    {
                        ApplyToAllProductRequest?.Invoke(this, ItemList);
                        OnCloseRequest();
                    }
                },
            };
        }


        public ResourceListViewModel()
        {
            ItemList.CollectionChanged += PackResources_CollectionChanged;
        }

        public void ChangePackResources(IEnumerable<PackResourceViewModel> packResouces)
        {
            ItemList.CollectionChanged -= PackResources_CollectionChanged;
            ItemList.Clear();
            foreach (var packResouce in packResouces)
            {
                var newItem = packResouce.Clone();
                newItem.RemoveRequest += Resource_RemoveRequest;
                newItem.PropertyChanged += Resource_PropertyChanged;
                ItemList.Add(newItem);
            }

            ItemList.CollectionChanged += PackResources_CollectionChanged;
            OnPropertyChanged(nameof(ItemList));
            OnPropertyChanged(nameof(TotalPrice));
            OnPropertyChanged(nameof(PricePerWeight));
        }
        private void PackResources_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        foreach (PackResourceViewModel addedItem in e.NewItems)
                        {
                            addedItem.RemoveRequest += Resource_RemoveRequest;
                            addedItem.PropertyChanged += Resource_PropertyChanged;
                        }

                        OnPropertyChanged(nameof(TotalPrice));
                        OnPropertyChanged(nameof(PricePerWeight));
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (PackResourceViewModel removedItem in e.OldItems)
                        {
                            removedItem.RemoveRequest -= Resource_RemoveRequest;
                            removedItem.PropertyChanged -= Resource_PropertyChanged;
                        }

                        OnPropertyChanged(nameof(TotalPrice));
                        OnPropertyChanged(nameof(PricePerWeight));
                        break;
                    }
                case NotifyCollectionChangedAction.Reset:
                    {
                        OnPropertyChanged(nameof(TotalPrice));
                        OnPropertyChanged(nameof(PricePerWeight));
                        break;
                    }
            }

            OnPropertyChanged(nameof(ItemList));
        }

        private void Resource_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(TotalPrice));
            OnPropertyChanged(nameof(PricePerWeight));
            OnPropertyChanged(nameof(ItemList));
        }

        private void Resource_RemoveRequest(PackResourceViewModel resource)
        {
            ItemList.Remove(resource);
        }

    }
}