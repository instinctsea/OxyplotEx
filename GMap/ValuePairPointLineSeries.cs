using OxyPlot;
using OxyPlot.Axes;
using OxyplotEx.GMap.Theme;
using OxyplotEx.Model;
using System;
using System.Collections.Generic;

namespace OxyplotEx.GMap
{
    class ValuePairPointLineSeries:PointLineSeries
    {
        List<ValuePairPointModel> _points = new List<ValuePairPointModel>();
        public ValuePairPointLineSeries()
        {
            LabelVisible = false;
        }
        public override int Count
        {
            get
            {
                return _points.Count;
            }
        }

        public override void AddPoint(PointModel point)
        {
            if(point is ValuePairPointModel)
            {
                _points.Add(point as ValuePairPointModel);

                double value = ((ValuePairPointModel)point).Y;

                this.Points.Add(new OxyPlot.DataPoint(point.Index, value));

                if (value > _maximum)
                    _maximum = value;
                if (value < _minimum)
                    _minimum = value;
            }
            else
            {
                throw new NotSupportedException();
            }
            
        }

        public override void Render(IRenderContext rc,PlotModel ddd)
        {
            PlotModel model = this.PlotModel;
            Axis axis = ((MapPlotModel)model).GetAxis(this.YKey);

            if (!SeriesVisible || !((IAxis)axis).AxisVisible)
                return;

            //AjustAxis();
            if (Count == 0)
                return;

            OxyColor average_color = OxyColors.Green;
            OxyColor limit_color = OxyColors.Red;
            if (Theme != null)
            {
                LineSeriesStyle style = Theme.GetStyle(ThemeMode) as LineSeriesStyle;
                this.Color = Helper.ConvertColorToOxyColor(style.LineColor);
                average_color = Helper.ConvertColorToOxyColor(style.AverageColor);
                limit_color = Helper.ConvertColorToOxyColor(style.AlarmColor);
            }

            rc.ResetClip();
            OxyRect clippingRect = model.PlotArea;

            for (int i = 0; i < _points.Count; i++)
            {
                double x = this.XAxis.Transform(_points[i].Index);
                double y = axis.Transform(_points[i].Y);
                if (double.IsNaN(y))
                    continue;

                string text = _points[i].Value;
                ScreenPoint sp = new ScreenPoint(x, y);

                rc.DrawText(sp, text, this.Color, "Arial", this.ActualFontSize);
            }
        }

        public override void InverseData()
        {
            if (_points.Count == 0)
                return;

            foreach (PointModel pt in _points)
            {
                pt.InverseIndex();
            }
        }
    }
}
