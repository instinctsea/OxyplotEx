using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OxyPlot;
using OxyPlot.Axes;
using OxyplotEx.GMap.Theme;
using OxyplotEx.Model;
using System.Windows.Forms;
using OxyplotEx.Model.Styles;

namespace OxyplotEx.GMap
{
    class LegendModel
    {
        public ISeries Series;
        public OxyRect Rect;
    }
    class LegendSeries : Series,IMouseInputHandler,ISeries
    {
        const int TopPadding = 5;
        const int RightPadding = 5;
        const int TextPadding = 3;
        List<LegendModel> _legends = new List<LegendModel>();

        public string Id
        {
            get;set;
        }

        public string XKey
        {
            get;set;
        }

        public string YKey
        {
            get;set;
        }

        public eThemeMode ThemeMode
        {
            get; set;
        }

        public string LegendTitle
        {
            get;set;
        }

        public bool LabelVisible
        {
            get;set;
        }

        public bool SeriesVisible
        {
            get;set;
        }

        public LineType LineType
        {
            get; set;
        }

        public ThemeBase Theme
        {
            get;set;
        }

        public double Maximum
        {
            get;private set;
        }

        public double Minimum
        {
            get;private set;
        }

        public object UserData
        {
            get;set;
        }

        public Priority Priority
        {
            get
            {
                return Priority.Low;
            }
        }

        public LegendSeries()
        {
            this.FontSize = 14;
        }

        public event EventHandler<LegendClickEventArgs> LegendClick;
        public event EventHandler<LegendClickEventArgs> LegendDoubleClick;
        public override void Render(IRenderContext rc,PlotModel model1)
        {
            PlotModel model = this.PlotModel;
            //throw new NotImplementedException();
            _legends.Clear();
            Dictionary<OxyRect,Tuple< List<Series>,OxyColor>> legends = new Dictionary<OxyRect,Tuple< List<Series>,OxyColor>>();
            foreach (Axis axis in model.Axes)
            {
                IAxis axis_cur = axis as IAxis;
                if (!axis_cur.AxisVisible)
                    continue;

                List<Series> seriess = new List<Series>();
                OxyColor color = ((Axis)axis_cur).TicklineColor;
                for (int i = 0; i < model.Series.Count; i++)
                {
                    ISeries se = model.Series[i] as ISeries;
                    if (se!=null&&se.YKey==axis_cur.AxisKey)
                    {
                        seriess.Add(model.Series[i]);
                    }
                }
                double x = axis_cur.Bound.Left + axis_cur.Bound.Width / 2;
                double y = axis_cur.Bound.Top + axis_cur.Bound.Height / 2;
                bool contain = false;
                foreach (KeyValuePair<OxyRect,Tuple< List<Series>,OxyColor>> pair in legends)
                {
                    if (pair.Key.Contains(x,y))
                    {
                        contain = true;
                        List<Series> collection = pair.Value.Item1;
                        collection.AddRange(seriess);
                        break;
                    }
                }

                if (!contain)
                {
                    legends[axis_cur.Bound] =new Tuple<List<Series>, OxyColor>( seriess,color);
                }
            }

            OxyColor fill_color = OxyColors.Transparent;
            if (Theme != null)
            {
                LineSeriesStyle style = Theme.GetStyle(ThemeMode) as LineSeriesStyle;
                if (style != null)
                {
                    fill_color =Helper.ConvertColorToOxyColor(style.LineColor);
                }
            }
            
            foreach (KeyValuePair<OxyRect,Tuple< List<Series>,OxyColor>> pair in legends)
            {
                List<Series> seriess = pair.Value.Item1;
                OxyRect bound = pair.Key;
                
                if (seriess.Count > 0)
                {
                    double total_height = 0;
                    List<List<Series>> group_series = new List<List<Series>>();
                    for (int i = 0; i < seriess.Count; i++)
                    {
                        ISeries series_cur = seriess[i] as ISeries;
                        OxySize size = rc.MeasureText(series_cur.Title);
                        total_height += size.Height + TextPadding;

                        if (group_series.Count == 0)
                        {
                            group_series.Add(new List<Series>());
                        }

                        if (total_height <= bound.Height)
                        {
                            group_series[group_series.Count - 1].Add(seriess[i]);
                        }
                        else
                        {
                            total_height = 0;
                            group_series.Add(new List<Series>());
                            group_series[group_series.Count - 1].Add(seriess[i]);
                        }
                    }

                    double total_width = 0;
                    double max_height = 0;
                    List<Tuple<ScreenPoint, string, OxyColor>> cache = new List<Tuple<ScreenPoint, string, OxyColor>>();
                    for (int i = group_series.Count - 1; i >= 0; i--)
                    {
                        double cur_max_width = 0;
                        double top = bound.Top + TopPadding;
                        double cu_height = 0;
                        for (int j = 0; j < group_series[i].Count; j++)
                        {
                            ISeries series_cur = seriess[j] as ISeries;
                            OxySize size = rc.MeasureText(series_cur.Title);
                            if (size.Width > cur_max_width)
                                cur_max_width = size.Width;

                            double left = bound.Right - RightPadding - total_width-size.Width;
                            OxyRect rect = new OxyRect(left, top, size.Width, size.Height);
                            _legends.Add(new LegendModel { Series = series_cur, Rect = rect });

                            OxyColor color = OxyColors.Blue;
                            if (series_cur.Theme != null)
                            {
                                LineSeriesStyle style = series_cur.Theme.GetStyle(ThemeMode) as LineSeriesStyle;
                                color = Helper.ConvertColorToOxyColor(style.LineColor);
                            }
                            if (!series_cur.SeriesVisible)
                                color = OxyColors.Gray;
                            cache.Add(new Tuple<ScreenPoint, string, OxyColor>(new ScreenPoint(left, top), series_cur.Title, color));
                            top += size.Height + TextPadding;
                            cu_height += size.Height + TextPadding;
                        }
                        total_width += cur_max_width+2*TextPadding;
                        if (max_height < cu_height)
                            max_height = cu_height;
                    }
                    max_height += TopPadding;
                    rc.DrawRectangle(new OxyRect(bound.Right - total_width, bound.Top, total_width, max_height), fill_color, pair.Value.Item2);
                    for (int i = 0; i < cache.Count; i++)
                    {
                        rc.DrawText(cache[i].Item1, cache[i].Item2, cache[i].Item3);
                    }
                }               
            }
        }       

        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            //throw new NotImplementedException();
        }

        protected override bool AreAxesRequired()
        {
            return false;
        }

        protected override void EnsureAxes()
        {
            //throw new NotImplementedException();
        }

        protected override bool IsUsing(Axis axis)
        {
            //throw new NotImplementedException();
            return false;
        }

        protected override void SetDefaultValues(PlotModel model)
        {
            //throw new NotImplementedException();
        }

        protected override void UpdateAxisMaxMin()
        {
            //throw new NotImplementedException();
        }

        protected override void UpdateData()
        {
            //throw new NotImplementedException();
        }

        protected override void UpdateMaxMin()
        {
            //throw new NotImplementedException();
        }

        protected override void UpdateValidData()
        {
            //throw new NotImplementedException();
        }

        protected virtual void OnLegendClick(object sender, LegendClickEventArgs e)
        {
            if (LegendClick != null)
                LegendClick(sender, e);
        }

        protected virtual void OnLegendDoubleClick(object sender, LegendClickEventArgs e)
        {
            if (LegendDoubleClick != null)
                LegendDoubleClick(sender, e);
        }

        public void OnMouseClick(System.Windows.Forms.MouseEventArgs e)
        {
            if (_legends.Count > 0)
            {
                foreach (LegendModel legend in _legends)
                {
                    if (legend.Rect.Contains(e.Location.X, e.Location.Y))
                    {
                        OnLegendClick(this, new LegendClickEventArgs(legend.Series, e.Button, e.X, e.Y, e.Clicks));
                        break;
                    }
                }
            }
        }

        public bool OnMouseHover(System.Windows.Forms.MouseEventArgs e)
        {
            return false;
        }

        public void AddPoint(PointModel point)
        {
            //throw new NotImplementedException();
        }

        public void ClearData()
        {
            //throw new NotImplementedException();
        }

        public void OnMouseDoubleClick(System.Windows.Forms.MouseEventArgs e)
        {
            if (_legends.Count > 0)
            {
                foreach (LegendModel legend in _legends)
                {
                    if (legend.Rect.Contains(e.Location.X, e.Location.Y))
                    {
                        OnLegendDoubleClick(this, new LegendClickEventArgs(legend.Series, e.Button, e.X, e.Y, e.Clicks));
                        break;
                    }
                }
            }
        }
    }
}
