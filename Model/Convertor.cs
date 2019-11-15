using OxyPlot;
using OxyplotEx.Model.DataSeries;
using OxyplotEx.Model.Styles;
using System;
using System.Drawing;

namespace OxyplotEx.Model
{
    public static class Convertor
    {
        public static DataPoint ConvertDataPairToDataPoint(SeqData data)
        {
            double y;
            double.TryParse(data.Y, out y);
            DataPoint dp = new DataPoint(data.X, y);
            return dp;
        }

        public static OxyColor ConvertColorToOxyColor(Color color)
        {
            return OxyColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static Color ConvertOxyColorToColor(OxyColor color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static eMarkerStyle ConvertMarkerTypeToMarkerStyle(MarkerType markerType)
        {
            int marker_token = (int)markerType;
            return (eMarkerStyle)marker_token;
        }

        public static MarkerType ConvertMarkerStyleToMarkerType(eMarkerStyle markerStyle)
        {
            int marker_token = (int)markerStyle;
            return (MarkerType)marker_token;
        }

        public static DateTime ConvertTimeGroupToTime(DateTime date, int hour)
        {
            return new DateTime(date.Year, date.Month, date.Day, hour, 0, 0);
        }
    }
}
