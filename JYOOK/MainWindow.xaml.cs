﻿using JYOOK.Application;
using System.Windows;
using System.Windows.Input;

namespace JYOOK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ResourceListWindow _ResourceWindow = new ResourceListWindow();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += async (s, e) =>
            {
                _ResourceWindow.Owner = this;
                _ResourceWindow.WindowStartupLocation = WindowStartupLocation.Manual;
                _ResourceWindow.Closing += (s, e) =>
                {
                    e.Cancel = true;
                    _ResourceWindow.Hide();
                    Activate();
                };

                var viewModel = new MainViewModel();
                await viewModel.Initailize();
                DataContext = viewModel;
            };
            KeyDown += (s, e) =>
            {
                if (e.Key == Key.Escape)
                {
                    _ResourceWindow.Hide();
                }
            };
        }

        private void ShowResourceManager_Click(object sender, RoutedEventArgs e)
        {
            var resourceManagerWindow = new ResourceManageWindow();
            resourceManagerWindow.Owner = this;
            resourceManagerWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            resourceManagerWindow.DataContext = new ResourceManagerViewModel();
            resourceManagerWindow.ShowDialog();
        }

        private void ResourceEdit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var salesProduct = ((FrameworkElement)sender).DataContext as SalesProductViewModel;
            var position = PointToScreen(Mouse.GetPosition(this));

            _ResourceWindow.Left = position.X + 30;
            _ResourceWindow.Top = position.Y;
            _ResourceWindow.DataContext = salesProduct.PackResources;
            _ResourceWindow.Show();
        }

        private void ResourceRemove_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var salesProduct = ((FrameworkElement)sender).DataContext as SalesProductViewModel;
            salesProduct.PackResources.PackingWeight = 1;
            salesProduct.PackResources.ItemList.Clear();
        }

        private void TotalMarginRate_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var salesManager = ((FrameworkElement)sender).DataContext as SalesManagerViewModel;
            if (salesManager == null)
                return;

            if (e.ClickCount.Equals(2))
            {
                var calcMarginRateViewModel = new CalcMarginRateViewModel(salesManager.LivestockProductCost, salesManager.TotalProductCost);
                var calcMarginRateWindow = new CalcMarginRateWindow();
                calcMarginRateWindow.Owner = this;
                calcMarginRateWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                calcMarginRateWindow.DataContext = calcMarginRateViewModel;
                calcMarginRateWindow.ShowDialog();

                if (calcMarginRateViewModel.IsApplied)
                {
                    foreach (var salesProduct in salesManager.Productions)
                    {
                        salesProduct.MarginRate = calcMarginRateViewModel.ProductMarginRate;
                    }
                }

            }
        }

    }
}
