using JYOOK.Application;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace JYOOK
{
    public class ResourceManagerViewModel : ViewModelBase
    {
        private readonly PackResourceService _ResourceService = PackResourceService.Instance;

        public bool IsReadOnly { get; private set; } = false;
        public ResourceFilterViewModel Filter { get; set; } = new ResourceFilterViewModel();

        public ObservableCollection<PackResourceViewModel> Resources { get; set; } = new ObservableCollection<PackResourceViewModel>();

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
                ExecuteAction = async (param) =>
                {
                    if (Resources.Any(r => r.Name == null || r.Type == null || r.Price == null))
                    {
                        OnErrorMessage("완성되지 않은 데이터가 존재합니다. 저장할 수 없습니다.");
                        return;
                    }

                    await _ResourceService.SaveResources(Resources.Select(d => d.ToResource()));

                    OnInfoMessage("저장 성공!");
                    OnCloseRequest();
                },
            };
        }

        public ResourceManagerViewModel()
        {
        }

    }
}