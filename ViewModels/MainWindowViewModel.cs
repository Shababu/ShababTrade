using BinanceApiLibrary;
using BitrueApiLibrary;
using ShababTrade.Commands;
using ShababTrade.Data;
using ShababTrade.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TradingCommonTypes;

namespace ShababTrade.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {

        #region Properties

        #region Login View Visibility

        private Visibility _loginViewVisibility = Visibility.Visible;

        public Visibility LoginViewVisibility
        {
            get => _loginViewVisibility;
            set => Set(ref _loginViewVisibility, value);
        }

        #endregion

        #region Main Menu Visibility

        private Visibility _mainMenuVisibility = Visibility.Collapsed;

        public Visibility MainMenuVisibility
        {
            get => _mainMenuVisibility;
            set => Set(ref _mainMenuVisibility, value);
        }

        #endregion        

        #region Exchange Users

        private List<ExchangeUser> _exchangeUsers = new List<ExchangeUser>();

        public List<ExchangeUser> ExchangeUsers
        {
            get => _exchangeUsers;
            set => Set(ref _exchangeUsers, value);
        }

        #endregion   

        #region Username

        private string _username = String.Empty;

        public string Username
        {
            get => _username;
            set => Set(ref _username, value);
        }

        #endregion

        #region Password

        private string _password = String.Empty;
        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        #endregion

        #region Current View Model

        public BaseViewModel _currentViewModel;

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set => Set(ref _currentViewModel, value);
        }

        #endregion

        #region Account Tab - Background

        private SolidColorBrush _accountTabBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D81D3C"));

        public SolidColorBrush AccountTabBackground
        {
            get => _accountTabBackground;
            set => Set(ref _accountTabBackground, value);
        }

        #endregion

        #region Trade History Tab - Background

        private SolidColorBrush _tradeHistoryTabBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D2D2D"));

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

        #region SelectMainMenuTabCommand

        public ICommand SelectMainMenuTabCommand { get; }

        public bool CanSelectMainMenuTabCommandExecute(object p) => true;

        public void OnSelectMainMenuTabCommandExecuted(object p)
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

        #region Login Command

        public ICommand LoginCommand { get; }
        public bool CanLoginCommandExecute(object p) => true;
        public void OnLoginCommandExecuted(object p)
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                ExchangeUsers = ExchangeUser.GetAppUserByLoginAndPawwsord(Username, Password);
                if (ExchangeUsers.Count > 0)
                {
                    List<string> avaliableExchanges = new List<string>();
                    foreach(var user in ExchangeUsers)
                    {
                        avaliableExchanges.Add(user.Exchange);
                    }
                    CurrentViewModel = new AccountViewModel(ExchangeUsers);
                    LoginViewVisibility = Visibility.Collapsed;
                    MainMenuVisibility = Visibility.Visible;                    
                }
            }
        }

        #endregion

        #endregion

        public MainWindowViewModel()
        {
            LoginCommand = new RelayCommand(OnLoginCommandExecuted, CanLoginCommandExecute);
            SelectMainMenuTabCommand = new RelayCommand(OnSelectMainMenuTabCommandExecuted, CanSelectMainMenuTabCommandExecute);
        }
    }
}
