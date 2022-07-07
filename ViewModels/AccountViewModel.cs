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

namespace ShababTrade.ViewModels
{
    internal class AccountViewModel : BaseViewModel
    {
        #region Properties

        #region Balances

        private ObservableCollection<ICryptoBalance> _balances = new ObservableCollection<ICryptoBalance>();
        public ObservableCollection<ICryptoBalance> Balances
        {
            get => _balances;
            set => Set(ref _balances, value);
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

        #region Exchange Users

        private List<ExchangeUser> _exchangeUsers = new List<ExchangeUser>();

        public List<ExchangeUser> ExchangeUsers
        {
            get => _exchangeUsers;
            set => Set(ref _exchangeUsers, value);
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
                    walletInfo = new BinanceWalletInfo();
                    var binanceUser = ExchangeUsers.Where(user => user.Exchange == "Binance").First();
                    List<ICryptoBalance> binanceBalances = walletInfo.GetWalletInfo(new BinanceApiUser(binanceUser.PublicKey, binanceUser.PrivateKey));
                    Balances = new ObservableCollection<ICryptoBalance>(binanceBalances);
                    break;
                case "Bitrue":
                    walletInfo = new BitrueWalletInfo();
                    var bitrueUser = ExchangeUsers.Where(user => user.Exchange == "Bitrue").First();
                    List<ICryptoBalance> bitrueBalances = walletInfo.GetWalletInfo(new BitrueApiUser(bitrueUser.PublicKey, bitrueUser.PrivateKey));
                    Balances = new ObservableCollection<ICryptoBalance>(bitrueBalances);
                    break;
            }
        }

        #endregion


        #endregion

        #region Methods

        #region Open Account View 

        public void OpenAccountView(List<ExchangeUser> exchangeUsers)
        {
            var currentExchange = exchangeUsers[0].Exchange;
            var publicKey = exchangeUsers[0].PublicKey;
            var privateKey = exchangeUsers[0].PrivateKey;
            SelectedExchange = currentExchange;

            IWalletInfo walletInfo;

            switch (SelectedExchange)
            {
                case "Binance":
                    walletInfo = new BinanceWalletInfo();
                    Balances = new ObservableCollection<ICryptoBalance>(walletInfo.GetWalletInfo(new BinanceApiUser(publicKey, privateKey)));
                    break;
                case "Bitrue":
                    walletInfo = new BitrueWalletInfo();
                    Balances = new ObservableCollection<ICryptoBalance>(walletInfo.GetWalletInfo(new BitrueApiUser(publicKey, privateKey)));
                    break;
            }
        }

        #endregion

        #endregion

        public AccountViewModel(List<ExchangeUser> appUsers)
        {
            ExchangeUsers = appUsers;

            foreach(var user in ExchangeUsers)
            {
                AvailableExchanges.Add(user.Exchange);
            }

            OpenAccountView(appUsers);

            SelectedExchangeChangedCommand = new RelayCommand(OnSelectedExchangeChangedCommandExecuted, CanSelectedExchangeChangedCommandExecute);
        }

    }
}
