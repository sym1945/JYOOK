using JYOOK.Application;
using System.Windows;
using System.Windows.Input;

namespace JYOOK
{
    /// <summary>
    /// ResourceManageWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ResourceManageWindow : Window
    {
        public ResourceManageWindow()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                if (!(DataContext is ViewModelBase viewModel))
                    return;

                viewModel.InfoMessage += (ss, infoMsg) => MessageBox.Show(infoMsg, "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                viewModel.ErrorMessage += (ss, errorMsg) => MessageBox.Show(errorMsg, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                viewModel.CloseRequest += (ss) => Close();
            };
            KeyDown += (s, e) =>
            {
                if (e.Key == Key.Escape)
                {
                    Close();
                }
            };
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
