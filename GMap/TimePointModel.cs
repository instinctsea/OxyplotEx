using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap
{
    class TimePointModel:PointModel
    {
        public TimePointModel(string name, double index, string value, int indexcount) : base(name,index,value,indexcount)
        {

        }

        public DateTime Time { get; set; }
    }
}
