using OxyplotEx.GMap.Theme;
using OxyplotEx.Model.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap
{
    public interface ISeries:IPriority
    {
        eThemeMode ThemeMode { get; set; }
        string Id { get; set; }
        string Title { get; set; }
        string XKey { get; set; }
        string YKey { get; set; }
        string LegendTitle { get; set; }
        bool LabelVisible { get; set; }
        bool SeriesVisible { get; set; }
        LineType LineType { get; set; }
        ThemeBase Theme { get; set; }
        double Maximum { get; }
        double Minimum { get; }
        void AddPoint(PointModel point);
        object UserData { get; set; }
        void ClearData();
    }
}
