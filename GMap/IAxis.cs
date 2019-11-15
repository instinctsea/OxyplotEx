using OxyplotEx.GMap.Theme;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OxyplotEx.Model.Styles;

namespace OxyplotEx.GMap
{
    public interface IAxis
    {
        eThemeMode ThemeMode { get; set; }
        ////eAxisPosition APosition { get; set; }
        OxyRect LabelBound { get;}
        AxisTheme Theme { get; set; }
        bool AxisVisible { get; set; }
        bool GridLineVisible { get; set; }
        string Name { get; set; }
        string AxisKey { get; set; }
        string Title { get; set; }
        OxyRect Bound { get; }
        double Transform(double x);
        void UpdateBound(BoundParameter bound);
        void UpdateTransform();
        void UpdateRange(double minimum, double maximum);
        void Render(IRenderContext rc, PlotModel model);
    }
}
