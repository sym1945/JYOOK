using System.Windows.Input;

namespace JYOOK.Application
{
    public class CalcMarginRateViewModel : ViewModelBase
    {
        private double _LivestockProductCost;
        private double _TotalProductCost;

        public bool IsApplied { get; private set; }

        public double WantedMarginRate { get; set; }

        public double ProductMarginRate { get; private set; }


        public ICommand SaveCommand
        {
            get => new CommandBase
            {
                ExecuteAction = (param) =>
                {
                    if (OnRequestMessage("적용하시겠습니까?"))
                    {
                        ProductMarginRate = 1 - ((_TotalProductCost * ((WantedMarginRate / 100) - 1)) / ((1 - (WantedMarginRate / 100)) * (_TotalProductCost - _LivestockProductCost) - _LivestockProductCost));

                        ProductMarginRate *= 100;

                        IsApplied = true;

                        OnCloseRequest();
                    }
                }
            };
        }


        public CalcMarginRateViewModel(double livestockProductCost, double totalProductCost)
        {
            _LivestockProductCost = livestockProductCost;
            _TotalProductCost = totalProductCost;
        }

    }
}