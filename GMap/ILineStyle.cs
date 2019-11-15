using OxyplotEx.Model.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap
{
    interface ILineStyle
    {
        double LineWidth { get; set; }
        eLineStyle SeriesLineStyle { get; set; }
        ePointLineStyle PointLineStyle { get; set; }
        bool Smooth { get; set; }
    }
}
