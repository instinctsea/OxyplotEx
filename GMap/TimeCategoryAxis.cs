using OxyPlot;
using OxyPlot.Axes;
using OxyplotEx.GMap.Theme;
using OxyplotEx.Model;
using OxyplotEx.Model.Styles;
using OxyplotEx.Model.Time;
using System.Collections.Generic;

namespace OxyplotEx.GMap
{
    class TimeCategoryAxis:CategoryAxis,ITimeXAxis,IMeasureTop
    {
        OxyRect _bound = new OxyRect(0, 0, 0, 0);
        OxyRect _label_boune = new OxyRect(0, 0, 0, 0);
        TimeDataManager  _times= new TimeDataManager();
        public TimeCategoryAxis()
        {
            this.Position = AxisPosition.Bottom;
            this.IsPanEnabled = false;
            this.IsZoomEnabled = false;
            this.APosition = eAxisPosition.Bottom;
            this.Angle = 45;
            this.AxisTickToLabelDistance = 30;
            this.AxisTitleDistance = 40;
            this.TickStyle = TickStyle.None;
        }

        

        public string AxisKey
        {
            get
            {
                return this.Key;
            }

            set
            {
                this.Key = value;
            }
        }

        public bool AxisVisible
        {
            get
            {
                return IsAxisVisible;
            }

            set
            {
                IsAxisVisible = value;
            }
        }

        public OxyRect Bound
        {
            get
            {
                return _bound;
            }
            set
            {
                _bound = value;
            }
        }

        public TimeLinesCollection TimeLines
        {
            get;set;
        }

        public List<TimeLine> Grids
        {
            get;set;
        }

        public string Name
        {
            get;set;
        }

        public AxisTheme Theme
        {
            get;set;
        }

        public eAxisPosition APosition
        {
            get;set;
        }

        public OxyRect LabelBound
        {
            get
            {
                return _label_boune;
            }
        }

        public bool GridLineVisible
        {
            get;set;
        }
        public eThemeMode ThemeMode
        {
            get; set;
        }

        public void AddLabel(TimeModel time)
        {
            string label = time.Time.AddHours(time.Duration).ToString("dd-HH");
            this.Labels.Add(label);
            _times.Add(time);
        }

        public void ClearLabels()
        {
            _times.Clear();
            this.Labels.Clear();
        }

        public void InverseData()
        {
            if (this.Labels.Count == 0)
                return;

            string[] labels = new string[Labels.Count];
            this.Labels.CopyTo(labels);

            this.Labels.Clear();
            for (int i = labels.Length - 1; i >= 0; i--)
                this.Labels.Add(labels[i]);

            _times.Reverse();

            if (TimeLines != null)
                TimeLines.Reverse();
        }

        public void Render(IRenderContext rc, PlotModel model)
        {
            //throw new NotImplementedException();
        }

        public override void Render(IRenderContext rc, PlotModel model1, AxisLayer axisLayer, int pass)
        {
#if DEBUG_MAP
            Console.WriteLine("render timecategoryaxis " + this.Name);
#endif
            PlotModel model = this.PlotModel;
            OxyColor live_color = OxyColors.Red;
            OxyColor forecast_color = OxyColors.Red;
            OxyColor separator_color = OxyColors.Red;
            OxyColor grid_color = OxyColors.Red;
            //OxyColor line_color = OxyColors.Red;
            if (Theme != null)
            {
                AxisStyle style = Theme.GetStyle(ThemeMode) as AxisStyle;
                TitleColor = Helper.ConvertColorToOxyColor(style.TitleColor);
                TextColor = Helper.ConvertColorToOxyColor(style.LabelColor);
                TicklineColor = Helper.ConvertColorToOxyColor(style.LineColor);
                live_color = Helper.ConvertColorToOxyColor(style.LiveColor);
                forecast_color = Helper.ConvertColorToOxyColor(style.ForecastColor);
                separator_color = Helper.ConvertColorToOxyColor(style.SepatorColor);
                grid_color = Helper.ConvertColorToOxyColor(style.GridColor);
            }

            //找到所有的y轴
            if (TimeLines != null)
            {
                //绘制线标值和线条
                FeatureTextIntersector intersector = new FeatureTextIntersector(FeatureTextIntersector.SortStyle.Horizontal, 3);
                for (int i = 0; i < TimeLines.Count; i++)
                {
                    string label = TimeLines[i].Time.ToString("dd-HH");
                    OxySize size = rc.MeasureText(label);
                    double x= TransformX(TimeLines[i].Index);
                    if (x < model.PlotArea.Left || x > model.PlotArea.Right)
                        continue;

                    double y = model.PlotArea.Bottom;
                    double y2 = y + 5;

                    //if (TimeLines[i].TimeStyle == TimeStyle.Seperator)
                    //    continue;

                    intersector.Add(new FeatureText(label, new ScreenPoint(x, y + 7), size,new ScreenPoint(x,y)));                
                }
                List<FeatureText> features= intersector.DiscaredIntersection(this.Angle);
                if (features != null)
                {
                    foreach (FeatureText feature in features)
                    {
                        rc.DrawText(feature.Position,feature.Text, TextColor, this.ActualFont, this.ActualFontSize, 200, this.Angle, HorizontalAlignment.Left, VerticalAlignment.Middle);
                    }
                }
                //绘制纵线
                foreach (Axis axis in model.Axes)
                {
                    if (axis.Position == AxisPosition.Left || axis.Position == AxisPosition.Right)
                    {
                        if (axis is IAxis)
                        {
                            IAxis y_axis = axis as IAxis;
                            if (!y_axis.AxisVisible)
                                continue;
                            foreach (TimeLine line in TimeLines)
                            {
                                OxyColor color = OxyColors.Red;
                                double thickness = 1;
                                LineStyle style = LineStyle.Solid;
                                switch (line.TimeStyle)
                                {
                                    case TimeStyle.Forecast:
                                        color = forecast_color;
                                        style = LineStyle.Dash;
                                        break;
                                    case TimeStyle.Live:
                                        color = live_color;
                                        break;
                                    case TimeStyle.Seperator:
                                        color = separator_color;
                                        style = LineStyle.Dash;
                                        thickness = 2;
                                        break;
                                }

                                IList<ScreenPoint> sps = new List<ScreenPoint>();
                                double x = TransformX(line.Index);
                                double top = y_axis.Bound.Top;
                                sps.Add(new ScreenPoint(x, top));
                                sps.Add(new ScreenPoint(x, y_axis.Bound.Bottom));
                                
                                rc.DrawClippedLine(y_axis.Bound,sps,2,color,thickness,style.GetDashArray(),LineJoin.Round,false);
                            }

                        }
                    }
                }
            }
        }

        double TransformX(double x)
        {
            int min = (int)x;
            int max = min + 1;

            double min_x = Transform(min);
            double max_x = Transform(max);
            if (min == max)
                return min_x;

            return (x - min) * (max_x - min_x) / (max - min) + min_x;
        }

        public void UpdateBound(BoundParameter bound)
        {
            _bound = bound.RegionBound;
            _label_boune = bound.LabelBound;
        }

        public void UpdateRange(double minimum, double maximum)
        {
            //throw new NotImplementedException();
        }

        public void UpdateTransform()
        {
            //throw new NotImplementedException();
        }

        public string GetLabel(double x)
        {
            foreach (TimeModel model in _times)
            {
                if (model.Index == x)
                    return model.CompsiteTime.ToString("MM-dd日 HH时 ");
            }

            return null;
        }

        public OxySize MeasureTop(IRenderContext rc, PlotModel model)
        {
            return new OxySize(model.Width, 20);
        }

        public void RenderTop(IRenderContext rc, PlotModel model, OxyRect rect)
        {
            if (TimeLines == null)
                return;

            OxyRect bound = rect;
            TimeLine live, seperator, forecast;
            TimeLines.GetLive(out live);
            TimeLines.GetSeperator(out seperator);
            TimeLines.GetForecast(out forecast);
            if (Theme != null)
            {
                AxisStyle style = Theme.GetStyle(ThemeMode) as AxisStyle;
                TitleColor = Helper.ConvertColorToOxyColor(style.TitleColor);
                TextColor = Helper.ConvertColorToOxyColor(style.LabelColor);
                TicklineColor = Helper.ConvertColorToOxyColor(style.LineColor);
            }
                if (seperator != null)
            {
                double x = this.TransformX(seperator.Index);
                if (live != null)
                {
                    double lx = this.TransformX(live.Index);

                    double center_x = 0;
                    if (lx > x)
                    {
                        center_x = bound.Right - (bound.Right - x) / 2;
                    }
                    else
                    {
                        center_x = bound.Left + (x - bound.Left) / 2;
                    }

                    if (!string.IsNullOrEmpty(live.GetTimeStyleDesc()))
                    {
                        OxySize text_size = rc.MeasureText(live.GetTimeStyleDesc());
                        double y = bound.Top;
                        rc.DrawText(new ScreenPoint(center_x - text_size.Width / 2, y), live.GetTimeStyleDesc(), TextColor);
                    }
                }

                if (forecast != null)
                {
                    double lx = this.TransformX(forecast.Index);
                    double center_x = 0;
                    if (lx > x)
                    {
                        center_x = bound.Right - (bound.Right - x) / 2;
                    }
                    else
                    {
                        center_x = bound.Left + (x - bound.Left) / 2;
                    }


                    if (!string.IsNullOrEmpty(forecast.GetTimeStyleDesc()))
                    {
                        OxySize text_size = rc.MeasureText(forecast.GetTimeStyleDesc());
                        double y = bound.Top;
                        rc.DrawText(new ScreenPoint(center_x - text_size.Width / 2, y), forecast.GetTimeStyleDesc(), TextColor);
                    }
                }
            }
        }
    }
}
