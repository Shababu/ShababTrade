using FancyCandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShababTrade.Models
{
    public class CandlesSource : System.Collections.ObjectModel.ObservableCollection<ICandle>, FancyCandles.ICandlesSource
    {
        public CandlesSource(FancyCandles.TimeFrame timeFrame)
        {
            this.timeFrame = timeFrame;
        }

        private readonly FancyCandles.TimeFrame timeFrame;
        public FancyCandles.TimeFrame TimeFrame { get { return timeFrame; } }
    }
}