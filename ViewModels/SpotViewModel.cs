using BinanceApiLibrary;
using ShababTrade.Data.Models;
using ShababTrade.Models;
using ShababTrade.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TradingCommonTypes;

namespace ShababTrade.ViewModels
{
    internal class SpotViewModel : BaseViewModel
    {
        #region Properties

        #region Top Panel Properties

        #region Symbol 24 Stats

        private Symbol24Stats _symbol24Stats = new Symbol24Stats();

        public Symbol24Stats Symbol24Stats
        {
            get => _symbol24Stats;
            set => Set(ref _symbol24Stats, value);
        }

        #endregion

        #endregion

        #endregion

        #region Async Methods

        public async void Get24Stats()
        {
            while (true)
            {
                BinanceMarketInfo binanceMarketInfo = new BinanceMarketInfo();
                IAssetStatus stats = binanceMarketInfo.Get24HourStatOnAsset("XRPUSDT");

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Symbol24Stats = new Symbol24Stats("XRP", "USDT", stats.LastPrice, stats.PriceChangePercent, stats.HighPrice, stats.LowPrice,
                        stats.Volume, stats.QuoteVolume, Symbol24Stats);
                });
            }      
        }

        #endregion

        public SpotViewModel(List<UserLoginInfo> appUsers, string selectedExchange)
        {
            SelectedExchange = selectedExchange;
            ExchangeUsers = appUsers;

            foreach (var user in ExchangeUsers)
            {
                AvailableExchanges.Add(user.Exchange);
            }

            var statsGetter = new Thread(Get24Stats);
            statsGetter.Start();
        }
    }
}
