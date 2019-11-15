using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap
{
    class FontLabelModel:PointModel
    {
        public FontLabelModel(string name, double index, string value, int indexcount, double angle = 0) : base(name, index, value, indexcount)
        {
            Angle = angle;
        }

        public double Angle
        {
            get;set;
        }
    }
}
