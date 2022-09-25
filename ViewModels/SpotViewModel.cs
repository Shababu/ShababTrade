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
using System.Windows.Input;
using TradingCommonTypes;

namespace ShababTrade.ViewModels
{
    internal partial class MainWindowViewModel : BaseViewModel
    {
        #region Spot

        BackgroundWorker spotBackgroundWorker_GetTradingPairs = new BackgroundWorker();
        Task GetChartInfoTask;
        Task Get24hStatsTask;

        #region Properties

        #region Current Base Asset

        private string _currentBaseAsset = "XRP";

        public string CurrentBaseAsset
        {
            get => _currentBaseAsset;
            set => Set(ref _currentBaseAsset, value);
        }

        #endregion

        #region Current Quote Asset

        private string _currentQuoteAsset = "USDT";

        public string CurrentQuoteAsset
        {
            get => _currentQuoteAsset;
            set => Set(ref _currentQuoteAsset, value);
        }

        #endregion

        #region CurrentSymbol

        private string _currentSymbol = "XRPUSDT";

        public string CurrentSymbol
        {
            get => CurrentBaseAsset + CurrentQuoteAsset;
            set => Set(ref _currentSymbol, value);
        }

        #endregion

        #region Свойство Items Source для BaseAssets
        private List<string> _baseAssets;

        public List<string> BaseAssets
        {
            get => _baseAssets;
            set { Set(ref _baseAssets, value); }
        }
        #endregion

        #region Свойство Items Source для QuoteAssets
        private List<string> _quoteAssets;

        public List<string> QuoteAssets
        {
            get => _quoteAssets;
            set { Set(ref _quoteAssets, value); }
        }
        #endregion

        #region Spot Symbols
        private List<Symbol24Stats> _spotSymbols;

        public List<Symbol24Stats> SpotSymbols
        {
            get => _spotSymbols;
            set { Set(ref _spotSymbols, value); }
        }
        #endregion

        #region Desired Symbol
        private string _desiredSymbol;

        public string DesiredSymbol
        {
            get => _desiredSymbol;
            set { Set(ref _desiredSymbol, value); }
        }
        #endregion

        #region Spot Visibility

        private Visibility _spotVisibility = Visibility.Collapsed;

        public Visibility SpotVisibility
        {
            get => _spotVisibility;
            set => Set(ref _spotVisibility, value);
        }

        #endregion

        #region Spot Spinner Visibility

        private Visibility _spotSpinnerVisibility = Visibility.Collapsed;

        public Visibility SpotSpinnerVisibility
        {
            get => _spotSpinnerVisibility;
            set => Set(ref _spotSpinnerVisibility, value);
        }

        #endregion

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

        #region Spot Candles

        private ICandlesSource _spotCandles = new CandlesSource(TimeFrame.M5);

        public ICandlesSource SpotCandles
        {
            get => _spotCandles;
            set { Set(ref _spotCandles, value); }
        }

        #endregion

        #endregion

        #endregion

        #region Async Methods

        public async Task Get24Stats()
        {
            while (!isSpotNeedsToBeCancelled)
            {
                try
                {
                    UserLoginInfo userLoginInfo = ExchangeUsers.Where(user => user.Exchange == SelectedExchange).First();

                    AppUser = new AppUser(SelectedExchange, userLoginInfo);
                    IAssetStatus stats = AppUser.MarketInfo.Get24HourStatOnAsset("XRPUSDT");

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Symbol24Stats = new Symbol24Stats("XRP", "USDT", stats.LastPrice, stats.PriceChangePercent, stats.HighPrice, stats.LowPrice,
                            stats.Volume, stats.QuoteVolume, Symbol24Stats);
                    });
                }

                catch (ThreadInterruptedException ex) { return; }
            }
        }

        public async Task GetChartDataAsync()
        {
            TimeFrame timeFrame = FancyCandles.TimeFrame.M5;
            CandlesSource candles = new CandlesSource(timeFrame);

            while (!isSpotNeedsToBeCancelled)
            {
                if (isSpotNeedsToBeCancelled)
                {
                    return;
                }


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
                        candles.Add(AppCandle.ConvertToCandle(candle));
                    }
                });

                SpotCandles = candles;
            }
        }

        #endregion

        #region Commands

        #region Spot Selected Exchange Changed Command

        public ICommand Spot_SelectedExchangeChangedCommand { get; }

        public bool CanSpot_SelectedExchangeChangedCommandExecute(object p) => true;

        public void OnSpot_SelectedExchangeChangedCommandExecuted(object p)
        {
            if (SpotVisibility == Visibility.Visible)
            {
                var userLoginInfo = ExchangeUsers.Where(user => user.Exchange == p.ToString()).First();
                AppUser = new AppUser(p.ToString(), userLoginInfo);

                SpotSpinnerVisibility = Visibility.Visible;
                IsExchangeSelectionEnabled = false;

                spotBackgroundWorker_GetTradingPairs.RunWorkerAsync();
            }
        }

        #endregion

        #endregion

        #region Event Handlers

        #region backgroundWorker_GetTradingPairs_RunWorkerCompleted

        private void SpotBackgroundWorker_GetTradingPairs_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            SpotSpinnerVisibility = Visibility.Collapsed;
            IsExchangeSelectionEnabled = true;
        }

        #endregion

        #region backgroundWorker_GetTradingPairs_DoWork

        private void SpotBackgroundWorker_GetTradingPairs_DoWork(object? sender, DoWorkEventArgs e)
        {
            GetTradingPairs();
        }

        #endregion

        #endregion

        #region Methods

        private void GetTradingPairs()
        {
            var exchangeInfo = AppUser.ExchangeInfo.GetExchangeInfo();

            BaseAssets = new List<string>();
            QuoteAssets = new List<string>();

            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var asset in exchangeInfo.ExchangeSymbols.ToList())
                {
                    BaseAssets.Add(asset.BaseAsset);
                    QuoteAssets.Add(asset.QuoteAsset);
                }
            });

            BaseAssets  = BaseAssets.Distinct().ToList();
            QuoteAssets = QuoteAssets.Distinct().ToList();

            GetQuoteAssetsList();
        }
        public List<Symbol24Stats> FormatSymbols(List<IAssetStatus> assets)
        {
            List<Symbol24Stats> symbol24Stats = new List<Symbol24Stats>();
            string symbol = "";

            foreach (var asset in assets)
            {
                foreach (var quoteAsset in QuoteAssets)
                {
                    if (asset.Symbol.EndsWith(quoteAsset))
                    {
                        int index = asset.Symbol.LastIndexOf(quoteAsset);
                        symbol = asset.Symbol.Substring(0, index) + '/' + quoteAsset;
                        break;
                    }
                    symbol = "";
                }

                if (symbol != "")
                {
                    decimal precision = AppUser.ExchangeInfo.ExchangeSymbolsInfo.Where(symbolInfo => symbolInfo.Symbol == symbol.Replace("/", "")).First().QuotePrecision;
                    symbol24Stats.Add(new Symbol24Stats(asset, symbol, precision));
                }
            }

            return symbol24Stats.Where(asset => asset.CurrentPrice > 0).ToList();
        }
        public void GetQuoteAssetsList()
        {
            AppUser.ExchangeInfo = AppUser.ExchangeInfo.GetExchangeInfo();
            QuoteAssets = new List<string>();

            foreach (var asset in AppUser.ExchangeInfo.ExchangeSymbols)
            {
                QuoteAssets.Add(asset.QuoteAsset);
            }

            QuoteAssets = QuoteAssets.Distinct().ToList();
            SpotSymbols = FormatSymbols(AppUser.MarketInfo.Get24HourStatOnAllAssets());
        }

        #endregion

        #endregion
    }
}
