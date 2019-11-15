using Module.MICAPSDataChart.GMap.Theme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Module.MICAPSDataChart.Model.Styles;
using System.Windows.Forms;
using Module.MICAPSDataChart.Service;

namespace Module.MICAPSDataChart.GMap.EChartGMap
{
    class EChartLineSeriesEx:CMA.MICAPS.ReactNative.Charts.EChartLineSeries,ISeries, IPerform, IAjustAxis, IInverseData, ILineStyle, IMouseInputHandler
    {
        public EChartLineSeriesEx(string id) : base(id)
        {

        }

        LineSeriesTheme _line_series_theme;
        public LineSeriesTheme LineSeriesTheme
        {
            get { return _line_series_theme; }
            set
            {
                _line_series_theme = value;
            }
        }

        public string Id
        {
            get =>base.ID;
            set =>base.ID= value;
        }

        public string Title { get =>base.Name; set => base.Name=value; }
        public string XKey { get; set; }
        public string YKey { get; set; }
        public string LegendTitle { get => base.Name; set => base.Name=value; }
        ///public bool LabelVisible { get { return base.LabelVisible; } set { base.LabelVisible = value; } }
        public bool SeriesVisible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;
            }
        }
        public LineType LineType { get; set; }

        ThemeBase ISeries.Theme { get => LineSeriesTheme; set => LineSeriesTheme =value as LineSeriesTheme; }

        public double Maximum => 100;

        public double Minimum => 0;

        public object UserData { get; set; }

        public Priority Priority =>  Priority.Low;
        public eLineStyle SeriesLineStyle { get; set; }
        public ePointLineStyle PointLineStyle { get; set; }
        public bool Smooth { get => base.IsSmooth; set => base.IsSmooth = value; }

        public void AddPoint(PointModel point)
        {
            double value;
            if (!double.TryParse(point.Value, out value))
                return;

            if (point is TimePointModel)
            {
                _points.Add(PointConverter.ConvertToEChartTimePoint(point as TimePointModel));
            }
            else
                _points.Add(PointConverter.ConvertToEChartPoint(point));
        }

        public void ClearData()
        {
            _points.Clear();
        }

        public void Prefer()
        {
            
        }

        public void Perform()
        {
            
        }

        public bool AjustAxis(double sourceMinimum, double SourceMaximum, out double minimum, out double maximum)
        {
            minimum = 0;
            maximum = 0;
            return true;
        }

        public void InverseData()
        {
            _points.Reverse();
        }

        public void OnMouseDoubleClick(MouseEventArgs e)
        {
            
        }

        public void OnMouseClick(MouseEventArgs e)
        {
            
        }

        public bool OnMouseHover(MouseEventArgs e)
        {
            return false;
        }

        public override string ToJavascript()
        {
            LineTheme = ThemeConvertor.ConvertToLineTheme(_line_series_theme);
            LineTheme.RefreshTheme(StartUp.Current.GetService<ThemeModesManager>().CurrentReactNativeMode);
            return base.ToJavascript();
        }
    }
}
