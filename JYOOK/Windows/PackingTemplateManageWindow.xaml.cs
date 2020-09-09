using JYOOK.Application;
using System.Windows;
using System.Windows.Input;

namespace JYOOK
{
    /// <summary>
    /// PackingTemplateManageWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PackingTemplateManageWindow : Window
    {
        public PackingTemplateManageWindow()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                if (!(DataContext is ViewModelBase viewModel))
                    return;

                viewModel.InfoMessage += (ss, infoMsg) => MessageBox.Show(infoMsg, "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                viewModel.ErrorMessage += (ss, errorMsg) => MessageBox.Show(errorMsg, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                viewModel.RequestMessage += (ss, reqMsg) => MessageBox.Show(reqMsg, "NOTICE", MessageBoxButton.OKCancel, MessageBoxImage.Asterisk) == MessageBoxResult.OK;
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

        
    }
}
