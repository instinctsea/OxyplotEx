using CMA.MICAPS.ReactNative.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module.MICAPSDataChart.GMap.EChartGMap
{
    static class PointConverter
    {
        internal static EChartPoint ConvertToEChartPoint(PointModel point)
        {
            EChartPoint pt = new EChartPoint();
            pt.X = point.Index;

            pt.Text = point.Value;
            double number;
            if (double.TryParse(point.Value, out number))
                pt.Y = number;

            return pt;
        }

        internal static EChartTimePoint ConvertToEChartTimePoint(PointModel point,DateTime start)
        {
            EChartTimePoint pt = new EChartTimePoint();
            pt.X = start.AddHours(point.Index);
            pt.Text = point.Value;

            double number;
            if (double.TryParse(point.Value, out number))
                pt.Y = number;
            pt.Y = number;

            return pt;
        }

        internal static EChartTimePoint ConvertToEChartTimePoint(TimePointModel point)
        {
            EChartTimePoint pt = new EChartTimePoint();
            pt.X = point.Time;
            pt.Text = point.Value;

            double number;
            if (double.TryParse(point.Value, out number))
                pt.Y = number;
            pt.Y = number;

            return pt;
        }
    }
}
