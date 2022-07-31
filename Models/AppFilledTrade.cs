using System;
using System.Collections.Generic;
using System.Windows.Media;
using TradingCommonTypes;

namespace ShababTrade.Models
{
    internal class AppFilledTrade : IFilledTrade
    {
        public long OrderId { get; set; }
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public decimal Qty { get; set; }
        public decimal QuoteQty { get; set; }
        public decimal Commission { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsBuyer { get; set; }
        public bool IsMaker { get; set; }
        public Sides Side { get; set; }
        public SolidColorBrush Foreground { get; set; }


        public AppFilledTrade(IFilledTrade trade)
        {
            OrderId = trade.OrderId;
            Symbol = trade.Symbol;
            Price = trade.Price;
            Qty = trade.Qty;
            QuoteQty = trade.QuoteQty;
            Commission = trade.Commission;
            TimeStamp = trade.TimeStamp;
            IsBuyer = trade.IsBuyer;
            IsMaker = trade.IsMaker;
            Side = trade.Side;

            if (Side.ToString() == "SELL")
            {
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D81D3C"));
            }
            else
            {
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#25D695"));
            }
        }

        public static List<AppFilledTrade> ConvertToAppFilledTrades(List<IFilledTrade> filledTrades)
        {
            List<AppFilledTrade> appFilledTrades = new List<AppFilledTrade>();

            foreach (var filledTrade in filledTrades)
            {
                appFilledTrades.Add(new AppFilledTrade(filledTrade));
            }

            return appFilledTrades;
        }
    }
}
