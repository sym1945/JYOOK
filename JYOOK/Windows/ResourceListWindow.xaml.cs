using JYOOK.Application;
using System.Windows;
using System.Windows.Input;

namespace JYOOK
{
    /// <summary>
    /// ResourceListWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ResourceListWindow : Window
    {
        public ResourceListViewModel ViewModel => DataContext as ResourceListViewModel;

        new public double Top
        {
            get => base.Top;
            set
            {
                var screenHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                if (screenHeight < value + Height)
                {
                    value = screenHeight - Height;
                }

                base.Top = value;
            }
        }
        new public double Left
        {
            get => base.Left;
            set
            {
                var screenWidth = SystemParameters.MaximizedPrimaryScreenWidth;
                if (screenWidth < value + Width)
                {
                    value = screenWidth - Width;
                }

                base.Left = value;
            }
        }


        public ResourceListWindow()
        {
            InitializeComponent();

            DataContextChanged += (s, e) =>
            {
                if (!(DataContext is ViewModelBase viewModel))
                    return;

                viewModel.InfoMessage -= ViewModel_InfoMessage;
                viewModel.InfoMessage += ViewModel_InfoMessage;
                viewModel.ErrorMessage -= ViewModel_ErrorMessage;
                viewModel.ErrorMessage += ViewModel_ErrorMessage;
                viewModel.RequestMessage -= ViewModel_RequestMessage;
                viewModel.RequestMessage += ViewModel_RequestMessage;
                viewModel.CloseRequest -= ViewModel_CloseRequest;
                viewModel.CloseRequest += ViewModel_CloseRequest;
            };
            KeyDown += (s, e) =>
            {
                if (e.Key == Key.Escape)
                {
                    Close();
                }
            };
        }

        private void ShowResourceSelector_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
                return;

            var resourceSelectorViewModel = new ResourceSelectorViewModel();
            var resourceManagerWindow = new ResourceManageWindow();
            resourceManagerWindow.Owner = this;
            resourceManagerWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            resourceManagerWindow.DataContext = resourceSelectorViewModel;
            resourceManagerWindow.ShowDialog();

            if (resourceSelectorViewModel.IsSaved)
            {
                foreach (var resource in resourceSelectorViewModel.SelectedResources)
                {
                    ViewModel.ItemList.Add(resource.Clone());
                }
            }

        }

        private void SaveToTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
                return;

            if (ViewModel.ItemList.Count > 0)
            {
                var packingTemplateAddWindow = new PackingTemplateAddWindow();
                packingTemplateAddWindow.Owner = this;
                packingTemplateAddWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                packingTemplateAddWindow.DataContext = new PackingTemplateAddViewModel(ViewModel);
                packingTemplateAddWindow.ShowDialog();
            }
        }

        private void LoadFromTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
                return;

            var packingTemplateViewModel = new PackingTemplateManageViewModel();
            var packingTemplateManageWindow = new PackingTemplateManageWindow();
            packingTemplateManageWindow.Owner = this;
            packingTemplateManageWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            packingTemplateManageWindow.DataContext = packingTemplateViewModel;
            packingTemplateManageWindow.ShowDialog();

            if (packingTemplateViewModel.IsApplied)
            {
                ViewModel.ItemList.Clear();
                foreach (var resource in packingTemplateViewModel.SelectedTemplate.Resources)
                {
                    ViewModel.ItemList.Add(resource.Clone());
                }

                ViewModel.PackingWeight = packingTemplateViewModel.SelectedTemplate.PackingWeight;
            }

        }

        private void ViewModel_CloseRequest(object sender)
        {
            Close();
        }

        private bool ViewModel_RequestMessage(object sender, string message)
        {
            return MessageBox.Show(message, "NOTICE", MessageBoxButton.OKCancel, MessageBoxImage.Asterisk) == MessageBoxResult.OK;
        }

        private void ViewModel_ErrorMessage(object sender, string message)
        {
            MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ViewModel_InfoMessage(object sender, string message)
        {
            MessageBox.Show(message, "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
