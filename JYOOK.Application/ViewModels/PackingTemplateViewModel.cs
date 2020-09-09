using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace JYOOK.Application
{
    public class PackingTemplateViewModel : ViewModelBase
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double PackingWeight { get; set; }

        public ObservableCollection<PackResourceViewModel> Resources { get; set; } = new ObservableCollection<PackResourceViewModel>();

        public double TotalPrice => Resources.Sum(d => d.TotalPrice.GetValueOrDefault());

        public double PricePerWeight => TotalPrice / PackingWeight;
    }
}