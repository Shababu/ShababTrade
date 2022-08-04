using BinanceApiLibrary;
using BitrueApiLibrary;
using ShababTrade.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCommonTypes;

namespace ShababTrade.Models
{
    internal class AppUser
    {
        public IExchangeUser ExchangeUser { get; set; }
        public IAccountInfo AccountInfo { get; set; }
        public IMarketInfo MarketInfo { get; set; }
        public IWalletInfo WalletInfo { get; set; }
        public IExchangeInfo ExchangeInfo { get; set; }
        public ITrader Trader { get; set; }

        public AppUser(string currentExchange, UserLoginInfo userLoginInfo)
        {
            switch (currentExchange)
            {
                case "Binance":
                    InitializeBinanceAppUser(userLoginInfo);
                    break;
                case "Bitrue":
                    InitializeBitrueAppUser(userLoginInfo);
                    break;
            }
        }

        private void InitializeBinanceAppUser(UserLoginInfo userLoginInfo)
        {
            ExchangeUser = new BinanceApiUser(userLoginInfo.PublicKey, userLoginInfo.PrivateKey);
            AccountInfo = new BinanceAccountInfo();
            MarketInfo = new BinanceMarketInfo();
            WalletInfo = new BinanceWalletInfo();
            ExchangeInfo = new BinanceExchangeInfo();
            Trader = new BinanceTrader();
        }

        private void InitializeBitrueAppUser(UserLoginInfo userLoginInfo)
        {
            ExchangeUser = new BitrueApiUser(userLoginInfo.PublicKey, userLoginInfo.PrivateKey);
            AccountInfo = new BitrueAccountInfo();
            MarketInfo = new BitrueMarketInfo();
            WalletInfo = new BitrueWalletInfo();
            ExchangeInfo = new BitrueExchangeInfo();
            Trader = new BitrueTrader();
        }
    }
}
