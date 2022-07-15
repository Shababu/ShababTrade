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

namespace ShababTrade.ViewModels
{
    internal class AccountViewModel : BaseViewModel
    {
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

        #endregion

        #region Commands

        #region Selected Exchange Changed Command

        public ICommand SelectedExchangeChangedCommand { get; }

        public bool CanSelectedExchangeChangedCommandExecute(object p) => true;

        public void OnSelectedExchangeChangedCommandExecuted(object p)
        {
            IWalletInfo walletInfo;

            switch (SelectedExchange)
            {
                case "Binance":
                    GetAccountDataForBinanceUser();
                    break;

                case "Bitrue":
                    GetAccountDataForBitrueUser();
                    break;
            }

            UpdateBalancesPieChart();
        }

        #endregion

        #endregion

        #region Methods

        #region Open Account View 

        public void OpenAccountView(List<ExchangeUser> exchangeUsers)
        {
            var currentExchange = exchangeUsers[0].Exchange;
            SelectedExchange = currentExchange;

            OnSelectedExchangeChangedCommandExecuted(null);
        }

        #endregion

        #region Get Account Data For Binance User

        private void GetAccountDataForBinanceUser()
        {
            var binanceWalletInfo = new BinanceWalletInfo();
            var binanceUser = ExchangeUsers.Where(user => user.Exchange == "Binance").First();
            List<ICryptoBalance> binanceBalances = binanceWalletInfo.GetWalletInfo(new BinanceApiUser(binanceUser.PublicKey, binanceUser.PrivateKey));
            Balances = new ObservableCollection<ICryptoBalance>(binanceBalances);

            var binanceDeposits = binanceWalletInfo.GetRecentDeposits(new BinanceApiUser(binanceUser.PublicKey, binanceUser.PrivateKey)).Take(5);
            Deposits = new ObservableCollection<IDeposit>(binanceDeposits);

            var binanceWithdrawals = binanceWalletInfo.GetRecentWithdrawals(new BinanceApiUser(binanceUser.PublicKey, binanceUser.PrivateKey)).Take(10);
            Withdrawals = new ObservableCollection<IWithdrawal>(binanceWithdrawals);
        }

        #endregion

        #region Get Account Data For Bitrue User

        private void GetAccountDataForBitrueUser()
        {
            var bitrueWalletInfo = new BitrueWalletInfo();
            var bitrueUser = ExchangeUsers.Where(user => user.Exchange == "Bitrue").First();
            List<ICryptoBalance> bitrueBalances = bitrueWalletInfo.GetWalletInfo(new BitrueApiUser(bitrueUser.PublicKey, bitrueUser.PrivateKey));
            Balances = new ObservableCollection<ICryptoBalance>(bitrueBalances);

            var bitrueDeposits = bitrueWalletInfo.GetRecentDeposits(new BinanceApiUser(bitrueUser.PublicKey, bitrueUser.PrivateKey), "XRP").Take(5);
            Deposits = new ObservableCollection<IDeposit>(bitrueDeposits);

            var bitrueWithdrawals = bitrueWalletInfo.GetRecentWithdrawals(new BinanceApiUser(bitrueUser.PublicKey, bitrueUser.PrivateKey), "XRP");
            Withdrawals = new ObservableCollection<IWithdrawal>(bitrueWithdrawals);
        }

        #endregion

        #region Команда "Обновить круговую диаграмму"

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

        public AccountViewModel(List<ExchangeUser> appUsers)
        {
            SelectedExchangeChangedCommand = new RelayCommand(OnSelectedExchangeChangedCommandExecuted, CanSelectedExchangeChangedCommandExecute);

            ExchangeUsers = appUsers;

            foreach(var user in ExchangeUsers)
            {
                AvailableExchanges.Add(user.Exchange);
            }

            OpenAccountView(appUsers);
            UpdateBalancesPieChart();
        }
    }
}
