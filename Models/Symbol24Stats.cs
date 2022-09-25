using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TradingCommonTypes;

namespace ShababTrade.Models
{
    internal class Symbol24Stats
    {
        public string Symbol { get; set; }
        public decimal CurrentPrice { get; set; }
        public string BaseAsset { get; set; }
        public string QuoteAsset { get; set; }
        public double Change24H { get; set; }
        public decimal High24H { get; set; }
        public decimal Low24H { get; set; }
        public decimal BaseVolume24H { get; set; }
        public decimal QuoteVolume24H { get; set; }
        public SolidColorBrush CurrentPriceForeground { get; set; }
        public SolidColorBrush PercentChangeForeground { get; set; }

        public Symbol24Stats(string baseAsset, string quoteAsset, decimal currentPrice, double change24H, decimal high24H, decimal low24H, 
            decimal baseVolume24H, decimal quoteVolume24H, Symbol24Stats prevStats)
        {
            int pricePrecision = CountPrecision(currentPrice);
            int high24HPrecision = CountPrecision(high24H);
            int low24HPrecision = CountPrecision(low24H);
            int baseVolume24HPrecision = CountPrecision(baseVolume24H);
            int quoteVolume24HPrecision = CountPrecision(quoteVolume24H);

            Symbol = baseAsset + quoteAsset;
            BaseAsset = baseAsset;
            QuoteAsset = quoteAsset;
            CurrentPrice = Math.Round(currentPrice, pricePrecision);
            Change24H = Math.Round(change24H, 2);
            High24H = Math.Round(high24H, high24HPrecision);
            Low24H = Math.Round(low24H, low24HPrecision);
            BaseVolume24H = Math.Round(baseVolume24H, baseVolume24HPrecision);
            QuoteVolume24H = Math.Round(quoteVolume24H, quoteVolume24HPrecision);

            SetCurrentPriceColor(prevStats, currentPrice);
            SetPercentChangeColor(change24H);
        }

        public Symbol24Stats(IAssetStatus assetStatus, string newSymbol, decimal precision)
        {
            Symbol = newSymbol;
            CurrentPrice = assetStatus.LastPrice;
            CurrentPrice = Math.Round(CurrentPrice, (int)precision);
                //Price > 99 ? Math.Round(Price, 2).ToString() : Price > 0.99M ? Math.Round(Price, 4).ToString() : Price > 0.0099M ? Math.Round(Price, 6).ToString() : Math.Round(Price, 9).ToString();
            Change24H = Math.Round(assetStatus.PriceChangePercent, 2);
            PercentChangeForeground = assetStatus.PriceChangePercent < 0.00 ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Color.FromArgb(255, 12, 202, 0));
        }

        #region Methods

        #region Count Precision

        private int CountPrecision(decimal value)
        {
            if (value > 1m)
                return 2;
            else if (value > 0.1m)
                return 4;
            else if (value > 0.01M)
                return 5;
            else if (value > 0.001M)
                return 6;
            else if (value > 0.0001M)
                return 7;
            else if (value > 0.00001M)
                return 8;
            else if (value > 0.000001M)
                return 9;

            return 2;
        }

        #endregion

        private void SetCurrentPriceColor(Symbol24Stats prevStats, decimal currentPrice)
        {
            if (currentPrice > prevStats.CurrentPrice)
            {
                CurrentPriceForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#25D695"));
            }
            else if (currentPrice < prevStats.CurrentPrice)
            {
                CurrentPriceForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D81D3C"));
            }
            else
            {
                CurrentPriceForeground = new SolidColorBrush(Colors.White);
            }
        }

        private void SetPercentChangeColor(double percentChange)
        {
            if (percentChange > 0)
            {
                PercentChangeForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#25D695"));
            }
            else if (percentChange < 0)
            {
                PercentChangeForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D81D3C"));
            }
            else
            {
                PercentChangeForeground = new SolidColorBrush(Colors.White);
            }
        }


        #endregion

        public Symbol24Stats() { }
    }
}
