using ShababTrade.Commands;
using ShababTrade.ViewModels.Base;
using System.Windows.Input;
using System.Windows.Media;

namespace ShababTrade.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
        #region Properties

        #region Account Tab - Background

        private SolidColorBrush _accountTabBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D81D3C"));

        public SolidColorBrush AccountTabBackground
        {
            get => _accountTabBackground;
            set => Set(ref _accountTabBackground, value);
        }

        #endregion

        #region Trade History Tab - Background

        private SolidColorBrush _tradeHistoryTabBackground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#2D2D2D"));

        public SolidColorBrush TradeHistoryTabBackground
        {
            get => _tradeHistoryTabBackground;
            set => Set(ref _tradeHistoryTabBackground, value);
        }

        #endregion

        #region Spot Tab - Background

        private SolidColorBrush _spotTabBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D2D2D"));

        public SolidColorBrush SpotTabBackground
        {
            get => _spotTabBackground;
            set => Set(ref _spotTabBackground, value);
        }

        #endregion

        #region Auto Trading Tab - Background

        private SolidColorBrush _autoTradingTabBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D2D2D"));

        public SolidColorBrush AutoTradingTabBackground
        {
            get => _autoTradingTabBackground;
            set => Set(ref _autoTradingTabBackground, value);
        }

        #endregion

        #region Futures Tab - Background

        private SolidColorBrush _futuresTabBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D2D2D"));

        public SolidColorBrush FuturesTabBackground
        {
            get => _futuresTabBackground;
            set => Set(ref _futuresTabBackground, value);
        }

        #endregion

        #endregion

        #region Commands

        #region SelectAccountTabCommand

        public ICommand SelectAccountTabCommand { get; }

        public bool CanSelectAccountTabCommandExecute(object p) => true;

        public void OnSelectAccountTabCommandExecuted(object p)
        {
            var activeTabColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D81D3C"));

            SetMainTabsBackgroundToDefault();

            switch (p as string)
            {
                case "Account":
                    AccountTabBackground = activeTabColor;
                    break;
                case "History":
                    TradeHistoryTabBackground = activeTabColor;
                    break;
                case "Spot":
                    SpotTabBackground = activeTabColor;
                    break;
                case "Auto":
                    AutoTradingTabBackground = activeTabColor;
                    break;
                case "Futures":
                    FuturesTabBackground = activeTabColor;
                    break;
            }
        }

        private void SetMainTabsBackgroundToDefault()
        {
            var defaultColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D2D2D"));
            AccountTabBackground = TradeHistoryTabBackground = SpotTabBackground =
                AutoTradingTabBackground = FuturesTabBackground = defaultColor;
        }

        #endregion

        #endregion

        public MainWindowViewModel()
        {
            SelectAccountTabCommand = new RelayCommand(OnSelectAccountTabCommandExecuted, CanSelectAccountTabCommandExecute);
        }
    }
}
