using CMA.MICAPS.ReactNative.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Module.MICAPSDataChart.GMap.Theme;
using OxyPlot;
using Module.MICAPSDataChart.Service;

namespace Module.MICAPSDataChart.GMap.EChartGMap
{
    class EChartLineAxisEx : EChartAxis, IAxis
    {
        public EChartLineAxisEx(string id) : base(id)
        {
            base.AxisDirection = AxisDirection.Y;
            base.AxisType = AxisType.Value;
        }

        public OxyRect LabelBound => throw new NotImplementedException();

        public bool AxisVisible { get; set; }
        public bool GridLineVisible { get; set; }
        public string AxisKey { get; set; }
        public string Title
        {
            get { return base.Name; }
            set
            {
                base.Name = value;
            }
        }

        public OxyRect Bound => new OxyRect(10, 10, 20, 20);

        AxisTheme _theme;
        public AxisTheme Theme
        {
            set
            {
                _theme = value;
                AxisTheme = ThemeConvertor.ConvertToAxisTheme(value);
                AxisTheme.RefreshTheme(StartUp.Current.GetService<ThemeModesManager>().CurrentReactNativeMode);
            }
            get
            {
                return _theme;
            }
        }

        public void Render(IRenderContext rc, PlotModel model)
        {

        }

        public double Transform(double x)
        {
            return x;
        }

        public void UpdateBound(BoundParameter bound)
        {

        }

        public void UpdateRange(double minimum, double maximum)
        {

        }

        public void UpdateTransform()
        {

        }
    }
}
