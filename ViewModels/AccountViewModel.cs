using ShababTrade.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinanceApiLibrary;
using TradingCommonTypes;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Media;
using ShababTrade.Commands;
using BitrueApiLibrary;
using ShababTrade.Data;
using ShababTrade.Data.Models;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using System.ComponentModel;
using System.Windows;

namespace ShababTrade.ViewModels
{
    internal class AccountViewModel : BaseViewModel
    {
        private BackgroundWorker backgroundWorker = new BackgroundWorker();

        #region Properties

        #region Exchange Users

        private List<ExchangeUser> _exchangeUsers = new List<ExchangeUser>();

        public List<ExchangeUser> ExchangeUsers
        {
            get => _exchangeUsers;
            set => Set(ref _exchangeUsers, value);
        }

        #endregion

        #region Available Exchanges

        private ObservableCollection<string> _availableExchanges = new ObservableCollection<string>();
        public ObservableCollection<string> AvailableExchanges
        {
            get => _availableExchanges;
            set => Set(ref _availableExchanges, value);
        }

        #endregion

        #region Selected Exchange

        private string _selectedExchange;
        public string SelectedExchange
        {
            get => _selectedExchange;
            set => Set(ref _selectedExchange, value);
        }

        #endregion

        #region Balances

        private ObservableCollection<ICryptoBalance> _balances = new ObservableCollection<ICryptoBalance>();
        public ObservableCollection<ICryptoBalance> Balances
        {
            get => _balances;
            set => Set(ref _balances, value);
        }

        #endregion

        #region Свойство ItemsSource для круговой диаграммы балансов

        private SeriesCollection _balancesCollection_Chart;

        public SeriesCollection BalancesCollection_Chart
        {
            get => _balancesCollection_Chart;
            set { Set(ref _balancesCollection_Chart, value); }
        }

        #endregion

        #region Deposits

        private ObservableCollection<IDeposit> _deposits = new ObservableCollection<IDeposit>();
        public ObservableCollection<IDeposit> Deposits
        {
            get => _deposits;
            set => Set(ref _deposits, value);
        }

        #endregion

        #region Withdrawals

        private ObservableCollection<IWithdrawal> _withdrawals = new ObservableCollection<IWithdrawal>();
        public ObservableCollection<IWithdrawal> Withdrawals
        {
            get => _withdrawals;
            set => Set(ref _withdrawals, value);
        }

        #endregion

        #region Total Balance

        private decimal _totalBalance;
        public decimal TotalBalance
        {
            get => _totalBalance;
            set => Set(ref _totalBalance, value);
        }

        #endregion

        #region Account Loading Spinner Visibility

        private Visibility _accountLoadingSpinnerVisibility = Visibility.Collapsed;
        public Visibility AccountLoadingSpinnerVisibility
        {
            get => _accountLoadingSpinnerVisibility;
            set => Set(ref _accountLoadingSpinnerVisibility, value);
        }

        #endregion

        #endregion

        #region Commands

        #region Selected Exchange Changed Command

        public ICommand SelectedExchangeChangedCommand { get; }

        public bool CanSelectedExchangeChangedCommandExecute(object p) => true;

        public void OnSelectedExchangeChangedCommandExecuted(object p)
        {
            AccountLoadingSpinnerVisibility = Visibility.Visible;
            IsExchangeSelectionEnabled = false;
            backgroundWorker.RunWorkerAsync();
        }

        #endregion

        #endregion

        #region Methods

        #region Open Account View 

        public void OpenAccountView(List<ExchangeUser> exchangeUsers)
        {
            var currentExchange = exchangeUsers.Where(user => user.Exchange == SelectedExchange).First();
            SelectedExchange = currentExchange.Exchange;

            switch (SelectedExchange)
            {
                case "Binance":
                    GetAccountDataForBinanceUser();
                    break;

                case "Bitrue":
                    GetAccountDataForBitrueUser();
                    break;
            }
        }

        #endregion

        #region Get Account Data For Binance User

        private void GetAccountDataForBinanceUser()
        {
            var binanceWalletInfo = new BinanceWalletInfo();
            var binanceUser = ExchangeUsers.Where(user => user.Exchange == "Binance").First();
            List<ICryptoBalance> binanceBalances = binanceWalletInfo.GetWalletInfo(new BinanceApiUser(binanceUser.PublicKey, binanceUser.PrivateKey));
            var balances = new ObservableCollection<ICryptoBalance>(binanceBalances);
            var totalBalance = binanceWalletInfo.GetAccountTotalBalance(binanceBalances);

            var binanceDeposits = binanceWalletInfo.GetRecentDeposits(new BinanceApiUser(binanceUser.PublicKey, binanceUser.PrivateKey)).Take(5);
            var deposits = new ObservableCollection<IDeposit>(binanceDeposits);

            var binanceWithdrawals = binanceWalletInfo.GetRecentWithdrawals(new BinanceApiUser(binanceUser.PublicKey, binanceUser.PrivateKey)).Take(10);
            var withdrawals = new ObservableCollection<IWithdrawal>(binanceWithdrawals);

            Balances = balances;
            TotalBalance = totalBalance;
            Deposits = deposits;
            Withdrawals = withdrawals;

            Application.Current.Dispatcher.Invoke(() =>
            {
                UpdateBalancesPieChart();
            });
        }

        #endregion

        #region Get Account Data For Bitrue User

        private void GetAccountDataForBitrueUser()
        {
            var bitrueWalletInfo = new BitrueWalletInfo();
            var bitrueUser = ExchangeUsers.Where(user => user.Exchange == "Bitrue").First();
            List<ICryptoBalance> bitrueBalances = bitrueWalletInfo.GetWalletInfo(new BitrueApiUser(bitrueUser.PublicKey, bitrueUser.PrivateKey));
            var balances = new ObservableCollection<ICryptoBalance>(bitrueBalances);
            var totalBalance = bitrueWalletInfo.GetAccountTotalBalance(bitrueBalances);

            var bitrueDeposits = bitrueWalletInfo.GetRecentDeposits(new BinanceApiUser(bitrueUser.PublicKey, bitrueUser.PrivateKey), "XRP").Take(5);
            var deposits = new ObservableCollection<IDeposit>(bitrueDeposits);

            var bitrueWithdrawals = bitrueWalletInfo.GetRecentWithdrawals(new BinanceApiUser(bitrueUser.PublicKey, bitrueUser.PrivateKey), "XRP");
            var withdrawals = new ObservableCollection<IWithdrawal>(bitrueWithdrawals);

            Balances = balances;
            TotalBalance = totalBalance;
            Deposits = deposits;
            Withdrawals = withdrawals;
        }

        #endregion

        #region Update PieChart

        public void UpdateBalancesPieChart()
        {
            BalancesCollection_Chart = new SeriesCollection();

            foreach (var balance in Balances)
            {
                BalancesCollection_Chart.Add(
                new PieSeries
                {
                    Values = new ChartValues<ObservableValue> { new ObservableValue((double)balance.RubValue) },
                    DataLabels = false,
                    Title = balance.Asset,
                });
            }
        }
        #endregion

        #endregion

        #region Event Handlers

        #region BackgroundWorker_RunWorkerCompleted

        private void BackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            AccountLoadingSpinnerVisibility = Visibility.Collapsed;
            IsExchangeSelectionEnabled = true;
        }

        #endregion

        #region BackgroundWorker_DoWork

        private void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            switch (SelectedExchange)
            {
                case "Binance":
                    GetAccountDataForBinanceUser();
                    break;

                case "Bitrue":
                    GetAccountDataForBitrueUser();
                    break;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                UpdateBalancesPieChart();
            });
        }

        #endregion

        #endregion

        public AccountViewModel(List<ExchangeUser> appUsers, string selectedExchange)
        {
            SelectedExchange = selectedExchange;
            SelectedExchangeChangedCommand = new RelayCommand(OnSelectedExchangeChangedCommandExecuted, CanSelectedExchangeChangedCommandExecute);

            ExchangeUsers = appUsers;

            foreach (var user in ExchangeUsers)
            {
                AvailableExchanges.Add(user.Exchange);
            }

            OpenAccountView(appUsers);

            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }        
    }
}
