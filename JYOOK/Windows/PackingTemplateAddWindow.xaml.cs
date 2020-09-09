using JYOOK.Application;
using System.Windows;
using System.Windows.Input;

namespace JYOOK
{
    /// <summary>
    /// PackingTemplateAddWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PackingTemplateAddWindow : Window
    {
        public PackingTemplateAddWindow()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                if (!(DataContext is ViewModelBase viewModel))
                    return;

                viewModel.InfoMessage += (ss, infoMsg) => MessageBox.Show(infoMsg, "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                viewModel.ErrorMessage += (ss, errorMsg) => MessageBox.Show(errorMsg, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                viewModel.CloseRequest += (ss) => Close();

                templateNameTextBox.Focus();
            };
            KeyDown += (s, e) =>
            {
                if (e.Key == Key.Escape)
                {
                    Close();
                }
            };
        }
    }
}
