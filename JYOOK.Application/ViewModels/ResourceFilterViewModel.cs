using JYOOK.Domain;
using System;

namespace JYOOK.Application
{
    public class ResourceFilterViewModel : ViewModelBase
    {
        private bool _IsCheckingBox = true;
        private bool _IsCheckingIcePack = true;
        private bool _IsCheckingPackingSheet = true;
        private bool _IsCheckingShippingCost = true;
       

        public bool IsCheckingAll
        {
            get => _IsCheckingBox && _IsCheckingIcePack && _IsCheckingPackingSheet && _IsCheckingShippingCost;
            set
            {
                _IsCheckingBox = value;
                _IsCheckingIcePack = value;
                _IsCheckingPackingSheet = value;
                _IsCheckingShippingCost = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCheckingBox));
                OnPropertyChanged(nameof(IsCheckingIcePack));
                OnPropertyChanged(nameof(IsCheckingPackingSheet));
                OnPropertyChanged(nameof(IsCheckingShippingCost));

                UpdateFilter();
            }
        }

        public bool IsCheckingBox
        {
            get => _IsCheckingBox;
            set
            {
                if (_IsCheckingBox == value)
                    return;
                _IsCheckingBox = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCheckingAll));

                UpdateFilter();
            }
        }

        public bool IsCheckingIcePack
        {
            get => _IsCheckingIcePack;
            set
            {
                if (_IsCheckingIcePack == value)
                    return;
                _IsCheckingIcePack = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCheckingAll));

                UpdateFilter();
            }
        }

        public bool IsCheckingPackingSheet
        {
            get => _IsCheckingPackingSheet;
            set
            {
                if (_IsCheckingPackingSheet == value)
                    return;
                _IsCheckingPackingSheet = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCheckingAll));

                UpdateFilter();
            }
        }

        public bool IsCheckingShippingCost
        {
            get => _IsCheckingShippingCost;
            set
            {
                if (_IsCheckingShippingCost == value)
                    return;
                _IsCheckingShippingCost = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCheckingAll));

                UpdateFilter();
            }
        }

        public event Action<object, ResourceType> FilterChanged;

        private void UpdateFilter()
        {
            ResourceType newFilterValue = 0;

            if (_IsCheckingBox)
                newFilterValue |= ResourceType.Box;
            if (_IsCheckingIcePack)
                newFilterValue |= ResourceType.IcePack;
            if (_IsCheckingPackingSheet)
                newFilterValue |= ResourceType.PackingSheet;
            if (_IsCheckingShippingCost)
                newFilterValue |= ResourceType.ShippingCost;

            FilterChanged?.Invoke(this, newFilterValue);
        }

    }
}