﻿using BinanceApiLibrary;
using BitrueApiLibrary;
using FancyCandles;
using ShababTrade.Commands;
using ShababTrade.Data;
using ShababTrade.Data.Models;
using ShababTrade.Models;
using ShababTrade.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TradingCommonTypes;

namespace ShababTrade.ViewModels
{
    internal partial class MainWindowViewModel : BaseViewModel
    {
        private BackgroundWorker loginBackgroundWorker = new BackgroundWorker();
        private BackgroundWorker accountBackgroundWorker = new BackgroundWorker();
        private BackgroundWorker tradeHistoryBackgroundWorker = new BackgroundWorker();
        private BackgroundWorker spotBackgroundWorker = new BackgroundWorker();

        #region Properties

        #region Login View Visibility

        private Visibility _loginViewVisibility = Visibility.Visible;

        public Visibility LoginViewVisibility
        {
            get => _loginViewVisibility;
            set => Set(ref _loginViewVisibility, value);
        }

        #endregion

        #region Login Label Visibility

        private Visibility _loginLabelVisibility = Visibility.Visible;

        public Visibility LoginLabelVisibility
        {
            get => _loginLabelVisibility;
            set => Set(ref _loginLabelVisibility, value);
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

        #region Login Error Visibility

        private Visibility _loginErrorVisibility = Visibility.Hidden;

        public Visibility LoginErrorVisibility
        {
            get => _loginErrorVisibility;
            set => Set(ref _loginErrorVisibility, value);
        }

        #endregion 

        #region Keys Setting Section Visibility

        private Visibility _keysSettingSectionVisibility = Visibility.Collapsed;

        public Visibility KeysSettingSectionVisibility
        {
            get => _keysSettingSectionVisibility;
            set => Set(ref _keysSettingSectionVisibility, value);
        }

        #endregion 

        #region Signup Hyperlink Visibility

        private Visibility _signupHyperlinkVisibility = Visibility.Visible;

        public Visibility SignupHyperlinkVisibility
        {
            get => _signupHyperlinkVisibility;
            set => Set(ref _signupHyperlinkVisibility, value);
        }

        #endregion

        #region Login Hyperlink Visibility

        private Visibility _loginHyperlinkVisibility = Visibility.Collapsed;

        public Visibility LoginHyperlinkVisibility
        {
            get => _loginHyperlinkVisibility;
            set => Set(ref _loginHyperlinkVisibility, value);
        }

        #endregion

        #region Register User Result Visibility

        private Visibility _registerUserResultVisibility = Visibility.Collapsed;

        public Visibility RegisterUserResultVisibility
        {
            get => _registerUserResultVisibility;
            set => Set(ref _registerUserResultVisibility, value);
        }

        #endregion

        #region Main Menu Spinner Visibility

        private Visibility _mainMenuSpinnerVisibility = Visibility.Collapsed;

        public Visibility MainMenuSpinnerVisibility
        {
            get => _mainMenuSpinnerVisibility;
            set => Set(ref _mainMenuSpinnerVisibility, value);
        }

        #endregion        

        #region Register User Result Text

        private string _registerUserResultText = string.Empty;

        public string RegisterUserResultText
        {
            get => _registerUserResultText;
            set => Set(ref _registerUserResultText, value);
        }

        #endregion

        #region Register User Result Foreground

        private SolidColorBrush _registerUserResultForeground = new SolidColorBrush();

        public SolidColorBrush RegisterUserResultForeground
        {
            get => _registerUserResultForeground;
            set => Set(ref _registerUserResultForeground, value);
        }

        #endregion

        #region Register Public Key

        private string _registerPublicKey = string.Empty;

        public string RegisterPublicKey
        {
            get => _registerPublicKey;
            set => Set(ref _registerPublicKey, value);
        }

        #endregion

        #region Register Private Key

        private string _registerPrivateKey = string.Empty;

        public string RegisterPrivateKey
        {
            get => _registerPrivateKey;
            set => Set(ref _registerPrivateKey, value);
        }

        #endregion




        #region Login View Width

        private double _loginViewWidth = 350;

        public double LoginViewWidth
        {
            get => _loginViewWidth;
            set => Set(ref _loginViewWidth, value);
        }

        #endregion

        #region Login View Height

        private double _loginViewHeight = 430;

        public double LoginViewHeight
        {
            get => _loginViewHeight;
            set => Set(ref _loginViewHeight, value);
        }

        #endregion 

        #region Login Spinner Visibility

        private Visibility _loginSpinnerVisibility = Visibility.Collapsed;

        public Visibility LoginSpinnerVisibility
        {
            get => _loginSpinnerVisibility;
            set => Set(ref _loginSpinnerVisibility, value);
        }

        #endregion 

        #region Login Button Visibility

        private Visibility _loginButtonVisibility = Visibility.Visible;

        public Visibility LoginButtonVisibility
        {
            get => _loginButtonVisibility;
            set => Set(ref _loginButtonVisibility, value);
        }

        #endregion         

        #region Is Sign Up Button Clicked

        private bool _isSignUpButtonClicked = false;

        public bool IsSignUpButtonClicked
        {
            get => _isSignUpButtonClicked;
            set => Set(ref _isSignUpButtonClicked, value);
        }

        #endregion

 

        #region Username

        private string _username = "Shabab";

        public string Username
        {
            get => _username;
            set => Set(ref _username, value);
        }

        #endregion

        #region Password

        private SecureString _password = new SecureString(); 
        public SecureString Password
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

            if(GetChartInfoTask?.Status == TaskStatus.RanToCompletion && Get24hStatsTask?.Status == TaskStatus.RanToCompletion)
            {
                GetChartInfoTask.Dispose();
                Get24hStatsTask.Dispose();
            }

            if (p as string != "Spot")
            {
                isSpotNeedsToBeCancelled = true;
                SpotVisibility = Visibility.Collapsed;
            }

            switch (p as string)
            {
                case "Account":
                    accountBackgroundWorker.DoWork += AccountBackgroundWorker_DoWork;
                    accountBackgroundWorker.RunWorkerCompleted += AccountBackgroundWorker_RunWorkerCompleted;
                    accountBackgroundWorker.RunWorkerAsync();
                    break;
                case "History":
                    tradeHistoryBackgroundWorker.DoWork += TradeHistoryBackgroundWorker_DoWork;
                    tradeHistoryBackgroundWorker.RunWorkerCompleted += TradeHistoryBackgroundWorker_RunWorkerCompleted;
                    tradeHistoryBackgroundWorker.RunWorkerAsync();
                    break;
                case "Spot":
                    isSpotNeedsToBeCancelled = false;
                    spotBackgroundWorker.DoWork += SpotBackgroundWorker_DoWork;
                    spotBackgroundWorker.RunWorkerCompleted += SpotBackgroundWorker_RunWorkerCompleted;
                    spotBackgroundWorker.RunWorkerAsync();
                    break;
                case "Auto":
                    AutoTradingTabBackground = activeTabColor;
                    break;
                case "Futures":
                    FuturesTabBackground = activeTabColor;
                    break;
            }

            MainMenuSpinnerVisibility = Visibility.Visible;
            IsExchangeSelectionEnabled = false;
        }
        
        #endregion

        #region Login Command

        public ICommand LoginCommand { get; }
        public bool CanLoginCommandExecute(object p) => true;
        public void OnLoginCommandExecuted(object p)
        {
            ShowLoadingSpinner();
            LoginErrorVisibility = Visibility.Hidden;
            IsExchangeSelectionEnabled = false;

            loginBackgroundWorker.RunWorkerAsync();
        }

        #endregion

        #region Open Sign Up View Command

        public ICommand OpenSignUpViewCommand { get; }
        public bool CanOpenSignUpViewCommandExecute(object p) => true;
        public void OnOpenSignUpViewCommandExecuted(object p)
        {
            if (IsSignUpButtonClicked)
            {
                LoginLabelVisibility = Visibility.Visible;
                KeysSettingSectionVisibility = Visibility.Collapsed;
                LoginHyperlinkVisibility = Visibility.Collapsed;
                SignupHyperlinkVisibility = Visibility.Visible;
            }
            else
            {
                KeysSettingSectionVisibility = Visibility.Visible;
                LoginHyperlinkVisibility = Visibility.Visible;
                SignupHyperlinkVisibility = Visibility.Collapsed;
            }
            IsSignUpButtonClicked = !IsSignUpButtonClicked;
        }

        #endregion

        #region Add Api Keys Command

        public ICommand AddApiKeysCommand { get; }
        public bool CanAddApiKeysCommandExecute(object p) => true;
        public void OnAddApiKeysCommandExecuted(object p)
        {
            string resultMessage = string.Empty;

            if(string.IsNullOrEmpty(Username) || Password.Length == 0 ||
               string.IsNullOrEmpty(RegisterPublicKey) || string.IsNullOrEmpty(RegisterPrivateKey))
            {
                RegisterUserResultForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD20909"));
                RegisterUserResultText = "All fields must be filled";
            }
            else
            {
                if (RegisterPublicKey.Length != 64)
                {
                    RegisterUserResultForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD20909"));
                    RegisterUserResultText = "Public key must contain 64 characters";
                    RegisterUserResultVisibility = Visibility.Visible;
                    return;
                }
                if (RegisterPrivateKey.Length != 64)
                {
                    RegisterUserResultForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD20909"));
                    RegisterUserResultText = "Private key must contain 64 characters";
                    RegisterUserResultVisibility = Visibility.Visible;
                    return;
                }

                bool insertResult = ShababTradeDataAccessor.TryInsertNewUser(new NetworkCredential(Username, Password), SelectedExchange, RegisterPublicKey, RegisterPrivateKey, out resultMessage);
                if (insertResult)
                {
                    RegisterUserResultForeground = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    RegisterUserResultForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD20909"));
                }
                RegisterUserResultText = resultMessage;
            }
            RegisterUserResultVisibility = Visibility.Visible;
        }

        #endregion

        #endregion

        #region Methods

        #region Show Login Button

        private void ShowLoginButton()
        {
            LoginSpinnerVisibility = Visibility.Collapsed;
            LoginButtonVisibility = Visibility.Visible;
        }

        #endregion

        #region Show Loading Spinner

        private void ShowLoadingSpinner()
        {
            LoginSpinnerVisibility = Visibility.Visible;
            LoginButtonVisibility = Visibility.Collapsed;
        }

        #endregion

        #region SetMainTabsBackgroundToDefault
        private void SetMainTabsBackgroundToDefault()
        {
            var defaultColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D2D2D"));
            AccountTabBackground = TradeHistoryTabBackground = SpotTabBackground =
                AutoTradingTabBackground = FuturesTabBackground = defaultColor;
        }

        #endregion

        #region Block All Inputs

        private void BlockAllInputs(BaseViewModel viewModel)
        {
            if(viewModel is AccountViewModel)
            {
                try
                {
                    ((AccountViewModel)CurrentViewModel).IsExchangeSelectionEnabled = false;
                }
                catch (Exception ex) { }
            }
            else if (viewModel is TradeHistoryViewModel)
            {
                try
                {
                    ((TradeHistoryViewModel)CurrentViewModel).IsExchangeSelectionEnabled = false;
                }
                catch (Exception ex) { }
            }
        }

        #endregion

        #endregion

        #region Event Handlers

        #region Login

        public void LoginBackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            if (!string.IsNullOrEmpty(Username) && Password.Length > 0)
            {
                ExchangeUsers = ShababTradeDataAccessor.GetExchangeUsersByUsernameAndPawwsord(new NetworkCredential(Username, Password));
                if (ExchangeUsers.Count > 0)
                {
                    AvailableExchanges = new ObservableCollection<string>();
                    foreach (var user in ExchangeUsers)
                    {
                        AvailableExchanges.Add(user.Exchange);
                    }
                    CurrentViewModel = new AccountViewModel(ExchangeUsers, SelectedExchange);
                    LoginViewVisibility = Visibility.Collapsed;
                    MainMenuVisibility = Visibility.Visible;
                }

                else
                {
                    LoginErrorVisibility = Visibility.Visible;
                    ShowLoginButton();
                }
            }

            else
            {
                LoginErrorVisibility = Visibility.Visible;
                ShowLoginButton();
            }
        }

        #endregion

        #region Login Completed

        private void LoginBackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            IsExchangeSelectionEnabled = true;
        }

        #endregion

        #region AccountBackgroundWorker_DoWork

        private void AccountBackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            BlockAllInputs(CurrentViewModel);
            CurrentViewModel = new AccountViewModel(ExchangeUsers, SelectedExchange);
        }

        #endregion

        #region AccountBackgroundWorker_RunWorkerCompleted

        private void AccountBackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            SetMainTabsBackgroundToDefault();
            AccountTabBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D81D3C"));
            MainMenuSpinnerVisibility = Visibility.Collapsed;
            IsExchangeSelectionEnabled = true;
        }

        #endregion

        #region TradeHistoryBackgroundWorker_DoWork

        private void TradeHistoryBackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            BlockAllInputs(CurrentViewModel);
            CurrentViewModel = new TradeHistoryViewModel(ExchangeUsers, SelectedExchange);
        }

        #endregion

        #region TradeHistoryBackgroundWorker_RunWorkerCompleted

        private void TradeHistoryBackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            SetMainTabsBackgroundToDefault();
            TradeHistoryTabBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D81D3C"));
            MainMenuSpinnerVisibility = Visibility.Collapsed;
            IsExchangeSelectionEnabled = true;
        }

        #endregion

        #region SpotBackgroundWorker_DoWork

        private void SpotBackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            BlockAllInputs(CurrentViewModel);
            //CurrentViewModel = new SpotViewModel(ExchangeUsers, SelectedExchange);
            CurrentViewModel = null;
        }

        #endregion

        #region SpotBackgroundWorker_RunWorkerCompleted

        private async void SpotBackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            SetMainTabsBackgroundToDefault();
            SpotTabBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D81D3C"));
            MainMenuSpinnerVisibility = Visibility.Collapsed;
            IsExchangeSelectionEnabled = true;

            GetChartInfoTask = Task.Run(() => GetChartDataAsync());
            Get24hStatsTask = Task.Run(() => Get24Stats());

            SpotVisibility = Visibility.Visible;
        }

        #endregion

        #endregion

        public MainWindowViewModel()
        {
            AddApiKeysCommand = new RelayCommand(OnAddApiKeysCommandExecuted, CanAddApiKeysCommandExecute);
            LoginCommand = new RelayCommand(OnLoginCommandExecuted, CanLoginCommandExecute);
            OpenSignUpViewCommand = new RelayCommand(OnOpenSignUpViewCommandExecuted, CanOpenSignUpViewCommandExecute);
            SelectMainMenuTabCommand = new RelayCommand(OnSelectMainMenuTabCommandExecuted, CanSelectMainMenuTabCommandExecute);
            Spot_SelectedExchangeChangedCommand = new RelayCommand(OnSpot_SelectedExchangeChangedCommandExecuted, CanSpot_SelectedExchangeChangedCommandExecute);

            loginBackgroundWorker.DoWork += LoginBackgroundWorker_DoWork;
            loginBackgroundWorker.RunWorkerCompleted += LoginBackgroundWorker_RunWorkerCompleted;

            spotBackgroundWorker_GetTradingPairs.DoWork += SpotBackgroundWorker_GetTradingPairs_DoWork;
            spotBackgroundWorker_GetTradingPairs.RunWorkerCompleted += SpotBackgroundWorker_GetTradingPairs_RunWorkerCompleted;

        }
    }
}
