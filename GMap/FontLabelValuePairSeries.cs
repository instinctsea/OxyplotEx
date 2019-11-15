using OxyPlot;
using OxyPlot.Axes;
using OxyplotEx.GMap.Theme;
using OxyplotEx.Model;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace OxyplotEx.GMap
{
    class FontLabelValuePairSeries:PointLineSeries
    {
        List<FontLabelValuePairModel> _points = new List<FontLabelValuePairModel>();
        public FontLabelValuePairSeries(FontFamily family, int fontSize):base()
        {
            LabelVisible = false;
            this.FontFamily = family;
            this.FontSize = fontSize;
        }

        public FontFamily FontFamily
        {
            get; set;
        }

        public new int FontSize
        {
            get; set;
        }

        public override int Count
        {
            get
            {
                return _points.Count;
            }
        }

        public override Priority Priority
        {
            get
            {
                return Priority.High;
            }
        }

        public override void AddPoint(PointModel point)
        {
            if(point is FontLabelValuePairModel)
            {
                _points.Add(point as FontLabelValuePairModel);
                double value = ((FontLabelValuePairModel)point).Y;

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

        public override bool AjustAxis(double sourceMinimum, double SourceMaximum, out double minimum, out double maximum)
        {
            minimum = 0;
            maximum = 0;
            if (Count == 0)
                return false;

            IAxis y_axis = this.YAxis as IAxis;
            if (y_axis == null)
            {
                y_axis = ((MapPlotModel)PlotModel).GetAxis(this.YKey) as IAxis;
            }

            if (!(y_axis is IAxis))
                return false;

            double cur_min = _minimum < sourceMinimum ? _minimum : sourceMinimum;
            double cur_max = _maximum > SourceMaximum ? _maximum : SourceMaximum;
            PointLineSeries.AjustYAxis(false, TextOffset.Y, this.ActualFontSize, cur_min, cur_max, 0.15, y_axis.Bound, out maximum, out minimum, 15);
            //maximum = cur_max;
            //minimum = cur_min;
            //double scale = 0.15;
            //if (maximum == minimum)
            //{
            //    if (maximum == 0)
            //    {
            //        minimum = -10;
            //        maximum = 10;
            //    }
            //    minimum = minimum * (1 - scale);
            //    maximum = maximum * (1 + scale);
            //    return false;
            //}

            //double font_size = this.FontSize;
            //double rest_length =y_axis.Bound.Bottom-y_axis.Bound.Top-2*font_size ;
            //OxyRect bound = y_axis.Bound;
            //if (rest_length <= 0)
            //{
            //    rest_length = (bound.Bottom - bound.Top) >font_size  ? font_size : bound.Bottom - bound.Top;
            //}

            //double max2 = font_size / rest_length * (maximum - minimum) + maximum;
            //double min2 = minimum - (maximum - minimum) *font_size / rest_length;

            //if (max2 == min2)
            //{
            //    if (min2 == 0)
            //    {
            //        min2 = -10;
            //        max2 = 10;
            //    }
            //    min2 = min2 * (1 - scale);
            //    max2 = max2 * (1 + scale);
            //}

            //minimum = min2;
            //maximum = max2;
            return true;
        }

        public override void ClearData()
        {
            this._points.Clear();
        }

        public override void Render(IRenderContext rc,PlotModel model1)
        {
            PlotModel model = this.PlotModel;
            Axis y_axis = ((MapPlotModel)model).GetAxis(this.YKey);

            if (!SeriesVisible || !((IAxis)y_axis).AxisVisible)
                return;

            if (Count == 0)
                return;

            OxyColor average_color = OxyColors.Green;
            OxyColor limit_color = OxyColors.Red;
            Color color = System.Drawing.Color.Red;
            if (Theme != null)
            {
                LineSeriesStyle style = Theme.GetStyle(ThemeMode) as LineSeriesStyle;
                this.Color = Helper.ConvertColorToOxyColor(style.LineColor);
                color = style.LineColor;
                average_color = Helper.ConvertColorToOxyColor(style.AverageColor);
                limit_color = Helper.ConvertColorToOxyColor(style.AlarmColor);
            }

            rc.ResetClip();
            OxyRect clippingRect = model.PlotArea;
            List<ScreenPoint> sps = new List<ScreenPoint>();
            var field = rc.GetType().GetField("g", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            object o = field.GetValue(rc);
            Graphics g = (Graphics)o;

            if (FontFamily == null)
                return;

            using (Font f = new System.Drawing.Font(FontFamily, FontSize))
            {
                using (Brush brush = new SolidBrush(color))
                {
                    for (int i = 0; i < _points.Count; i++)
                    {
                        double x = this.XAxis.Transform(_points[i].Index);
                        double y = y_axis.Transform(_points[i].Y);
                        if (double.IsNaN(y))
                            continue;

                        ScreenPoint sp = new ScreenPoint(x, y);
                        if (_points[i].Value == "9999")
                            continue;

                        //using (Brush rec = new SolidBrush(System.Drawing.Color.Blue))
                        //{
                        //    g.FillRectangle(rec, new RectangleF((float)x - 1, (float)y - 1, 2, 2));
                        //}

                        FontLabelSeries.DrawText(g, new ScreenPoint(x, y), _points[i].Value,
                            brush, f, _points[i].Angle,
                            HorizontalAlignment.Center, VerticalAlignment.Middle);
                    }
                }
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
