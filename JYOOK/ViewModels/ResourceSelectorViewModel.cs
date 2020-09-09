using JYOOK.Application;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace JYOOK
{
    public class ResourceSelectorViewModel : ViewModelBase
    {
        private readonly PackResourceService _ResourceService = PackResourceService.Instance;


        public bool IsSaved { get; private set; } = false;

        public bool IsReadOnly { get; private set; } = true;

        public ResourceFilterViewModel Filter { get; set; } = new ResourceFilterViewModel();

        public ObservableCollection<PackResourceViewModel> Resources { get; set; } = new ObservableCollection<PackResourceViewModel>();

        public IEnumerable<PackResourceViewModel> SelectedResources => Resources.Where(d => d.IsSelected);

        public ICollectionView ResourcesView { get; set; }


        public ICommand LoadedCommand
        {
            get => new CommandBase
            {
                ExecuteAction = async (param) =>
                {
                    var resources = (await _ResourceService.GetResources()).Select(d => new PackResourceViewModel(d)).ToList();
                    Resources = new ObservableCollection<PackResourceViewModel>(resources.OrderBy(r => r.Type));

                    var collectionViewSource = new CollectionViewSource
                    {
                        Source = Resources
                    };
                    ResourcesView = collectionViewSource.View;

                    Filter.FilterChanged += (s, filter) =>
                    {
                        ResourcesView.Filter = (item) => filter.HasFlag((item as PackResourceViewModel).Type);
                    };
                },
            };
        }

        public ICommand SaveCommand
        {
            get => new CommandBase
            {
                ExecuteAction = (param) =>
                {
                    IsSaved = true;

                    OnCloseRequest();
                },
            };
        }

        public ResourceSelectorViewModel()
        {
        }

    }
}