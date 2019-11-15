using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap
{
    class FontLabelValuePairModel:ValuePairPointModel
    {
        public FontLabelValuePairModel(string name, int index, string value, int indexcount, double y, double angle = 0) : base(name, index, value, indexcount, y)
        {
            this.Angle = angle;
        }

        public double Angle;
    }
}
