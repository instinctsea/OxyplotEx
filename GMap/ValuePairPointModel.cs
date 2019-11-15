using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap
{
    class ValuePairPointModel:PointModel
    {
        public ValuePairPointModel(string name, int index, string value, int indexcount, double y) : base(name, index, value, indexcount)
        {
            this.Y = y;
        }

        public double Y;
    }
}
