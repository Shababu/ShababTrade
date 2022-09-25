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
using ShababTrade.Models;

namespace ShababTrade.ViewModels
{
    internal class AccountViewModel : BaseViewModel
    {
        private BackgroundWorker backgroundWorker = new BackgroundWorker();

        #region Properties

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

        public void OpenAccountView(List<UserLoginInfo> exchangeUsers)
        {
            var currentExchange = exchangeUsers.Where(user => user.Exchange == SelectedExchange).First();
            SelectedExchange = currentExchange.Exchange;

            GetAccountData(SelectedExchange);
        }

        #endregion

        #region Get Account Data

        private void GetAccountData(string selectedExchange)
        {
            UserLoginInfo userLoginInfo = ExchangeUsers.Where(user => user.Exchange == selectedExchange).First();
            AppUser = new AppUser(selectedExchange, userLoginInfo);

            
            List<ICryptoBalance> balances = AppUser.WalletInfo.GetWalletInfo(AppUser.ExchangeUser);
            var appBalances = new ObservableCollection<ICryptoBalance>(balances);
            var totalBalance = AppUser.WalletInfo.GetAccountTotalBalance(balances);

            DateTime startTime;
            DateTime endTime = DateTime.Now;

            startTime = DateTime.Now.Subtract(TimeSpan.FromDays(90));

            var deposits = AppUser.WalletInfo.GetRecentDeposits(AppUser.ExchangeUser, "XRP", startTime, endTime).Take(8);
            var appDeposits = new ObservableCollection<IDeposit>(deposits);

            foreach(var appDeposit in appDeposits)
            {
                appDeposit.Coin = appDeposit.Coin.ToUpper();
            }

            var withdrawals = AppUser.WalletInfo.GetRecentWithdrawals(AppUser.ExchangeUser, "XRP", startTime, endTime).Take(8);
            var appWithdrawals = new ObservableCollection<IWithdrawal>(withdrawals);

            Balances = appBalances;
            TotalBalance = totalBalance;
            Deposits = appDeposits;
            Withdrawals = appWithdrawals;

            Application.Current.Dispatcher.Invoke(() =>
            {
                UpdateBalancesPieChart();
            });
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

        #region BackgroundWorker_DoWork

        private void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            GetAccountData(SelectedExchange);

            Application.Current.Dispatcher.Invoke(() =>
            {
                UpdateBalancesPieChart();
            });
        }

        #endregion

        #region BackgroundWorker_RunWorkerCompleted

        private void BackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            AccountLoadingSpinnerVisibility = Visibility.Collapsed;
            IsExchangeSelectionEnabled = true;
        }

        #endregion

        #endregion

        public AccountViewModel(List<UserLoginInfo> appUsers, string selectedExchange)
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
