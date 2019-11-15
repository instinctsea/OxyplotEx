using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model
{
    class ValuePair
    {
        public ValuePair(double y, string value)
        {
            this.Value = value;
            this.Y = y;
        }

        public string Value;
        public double Y;
    }
}
