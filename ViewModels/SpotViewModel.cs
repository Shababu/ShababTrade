using BinanceApiLibrary;
using FancyCandles;
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

        #region Chart Properties

        private ICandlesSource _spotCandles = new CandlesSource(TimeFrame.M5);

        public ICandlesSource SpotCandles
        {
            get => _spotCandles;
            set { Set(ref _spotCandles, value); }
        }

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

        public async void GetChartData()
        {
            TimeFrame timeFrame = FancyCandles.TimeFrame.M5;
            CandlesSource candles = new CandlesSource(timeFrame);

            while (true)
            {
                List<TradingCommonTypes.ICandle> exchangeCandles = new List<TradingCommonTypes.ICandle>();

                try
                {
                    exchangeCandles = AppUser.MarketInfo.GetCandles(Symbol24Stats.Symbol, "5m", 500);
                }

                catch { continue; }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    candles.Clear();
                    foreach (var candle in exchangeCandles)
                    {
                        candles.Add(new Candle(candle.OpenTime, candle.Open, candle.High, candle.Low, candle.Close, candle.Volume));
                    }
                });

                SpotCandles = candles;
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

            Symbol24Stats.Symbol = "XRPUSDT";
            var statsGetter = new Thread(Get24Stats);
            var candlesGetter = new Thread(GetChartData);
            statsGetter.Start();
            candlesGetter.Start();
        }
    }
}
