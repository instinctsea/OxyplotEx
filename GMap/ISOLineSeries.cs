using OxyPlot;
using OxyPlot.Axes;
using OxyplotEx.GMap.Theme;
using OxyplotEx.Model;
using OxyplotEx.Properties;
using System;
using System.Collections.Generic;

namespace OxyplotEx.GMap
{
    class ISOLineSeries:PointLineSeries
    {
        ISOLineAlgorithem _iso_line = new ISOLineAlgorithem();
        VPointCollection _points = new VPointCollection();
        bool _performed = false;
        public ISOLineSeries()
        {
            LabelVisible = false;
            this.LineWidth = 1;
            this.LineStyle = LineStyle.Solid;
            this.DashLimit = 0;
        }

        public double Interval
        {
            get
            {
                return _iso_line.Interval;
            }
            set
            {
                _iso_line.Interval = (float)value;
            }
        }

        public double DashLimit
        {
            get;set;
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
            double value;
            if (point is ValuePairPointModel)
            {
                ValuePairPointModel data = point as ValuePairPointModel;
                value = data.Y;
                _points.Add(data);

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

        public override void ClearData()
        {
            this._points.Clear();
        }

        public override void Perform()
        {
            //执行分析
            _iso_line.Analysis(_points);
            _performed = true;
            this.LineStyle = LineStyle.Solid;
        }

        public override void Render(IRenderContext rc,PlotModel modle1)
        {
            PlotModel model = this.PlotModel;
            Axis axis = ((MapPlotModel)model).GetAxis(this.YKey);
            rc.ResetClip();

            if (!SeriesVisible || !((IAxis)axis).AxisVisible)
                return;

            if (_points.Count > 0)
            {
                if (!_performed)
                {
                    double tip_font_size = 20;
                    string tip = this.Title;
                    OxySize size = rc.MeasureText(tip, this.ActualFont, tip_font_size);
                    OxyRect bound = ((IAxis)axis).Bound;
                    double y = bound.Top + (bound.Height-size.Height) / 2;
                    double x = (bound.Width - size.Width) / 2 + bound.Left;
                    rc.DrawText(new ScreenPoint(x, y), tip, this.Color, this.ActualFont, tip_font_size);
                }
            }

            if (_iso_line.LineStrings.Count == 0 )
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

            OxyRect clippingRect = model.PlotArea;
            for (int i = 0; i < _iso_line.LineStrings.Count; i++)
            {
                LineString line = _iso_line.LineStrings[i];
                bool invalid = false;
                if (line.VPoints.Count > 0)
                {
                    for (int j = 0; j < line.VPoints.Count; j++)
                    {
                        double x = this.XAxis.Transform(line.VPoints[j].X);
                        double y = axis.Transform(line.VPoints[j].Y);

                        if (clippingRect.Contains(x, y))
                        {
                            invalid = true;
                        }
                    }
                }
                else
                    invalid = true;
                if (!invalid)
                    continue;

                if (line.Points.Count > 0)
                {
                    IList<ScreenPoint> sps = new List<ScreenPoint>();
                    for (int j = 0; j < line.Points.Count; j++)
                    {
                        double x = this.XAxis.Transform(line.Points[j].X);
                        double y = axis.Transform(line.Points[j].Y);
                        sps.Add(new ScreenPoint(x, y));
                    }

                    if (line.Value >= 99999 || line.Value<-99999)
                    {
                        continue;
                    }
                    if (line.Value > DashLimit)
                        base.RenderLine(rc, clippingRect, sps, Color, this.LineWidth,this.LineStyle);
                    else
                    {
                        base.RenderLine(rc, clippingRect, sps, Color, this.LineWidth, LineStyle.Dash);
                    }
                }
                
                if (line.VPoints.Count > 0)
                {
                    for (int j = 0; j < line.VPoints.Count; j++)
                    {
                        double x = this.XAxis.Transform(line.VPoints[j].X);
                        double y = axis.Transform(line.VPoints[j].Y);

                        if (clippingRect.Contains(x, y))
                        {
                            rc.DrawText(new ScreenPoint(x, y), line.Value.ToString("f1"), this.Color);
                        }
                    }
                }
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
            return true;
        }

        public override void InverseData()
        {
            if (_points.Count == 0)
                return;

            foreach (PointModel pt in _points)
            {
                pt.InverseIndex();
            }

            _iso_line.Analysis(_points);
        }
    }
}
