using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCommonTypes;

namespace ShababTrade.Models
{
    public class AppCandle : FancyCandles.ICandle
    {
        public DateTime t { get; set; }
        public double O { get; set; }
        public double H { get; set; }
        public double L { get; set; }
        public double C { get; set; }
        public double V { get; set; }

        public AppCandle(DateTime t, double O, double H, double L, double C, double V)
        {
            this.t = t;
            this.O = O;
            this.H = H;
            this.L = L;
            this.C = C;
            this.V = V;
        }

        public static AppCandle ConvertToCandle(ICandle exchangeCandle)
        {
            return new AppCandle(exchangeCandle.OpenTime, exchangeCandle.Open, exchangeCandle.High, exchangeCandle.Low, exchangeCandle.Close, exchangeCandle.Volume);
        }
    }
}
