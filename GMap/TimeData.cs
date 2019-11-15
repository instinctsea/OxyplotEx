using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap
{
    public class TimeModel
    {
        public TimeModel(DateTime time, int duration,int index)
        {
            Time = time;
            Duration = duration;
            Index = index;
            //Count = count;
        }

        public DateTime Time { get; private set; }
        public int Duration { get; private set; }
        public int Index { get; private set; }

        public void InverseIndex(int count)
        {
            Index = count -1 - Index;
        }

        public DateTime CompsiteTime
        {
            get { return Time.AddHours(Duration); }
        }
    }
}
