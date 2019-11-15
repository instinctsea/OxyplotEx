using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap
{
    interface IAjustAxis
    {
        bool AjustAxis(double sourceMinimum, double SourceMaximum, out double minimum,out double maximum);
    }
}
