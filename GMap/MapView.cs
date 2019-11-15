using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using OxyplotEx.GMap.Theme;
using OxyplotEx.Model;
using OxyplotEx.Model.Styles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OxyplotEx.GMap
{
    public sealed class MapView:PlotView,IMap
    {
        LegendTheme _legend_theme = new LegendTheme();
        SolidBrush _back_brush = new SolidBrush(Color.White);
        public MapView()
        {
            this.Model = new MapPlotModel();
            this.Model.LegendPlacement = OxyPlot.LegendPlacement.Outside;
            this.Model.LegendPosition = OxyPlot.LegendPosition.TopRight;
            this.Model.LegendOrientation = LegendOrientation.Horizontal;
            this.Model.PlotAreaBorderColor = OxyColors.Transparent;
            this.Model.IsLegendVisible = false;
            this.HideTracker();
        }

        protected override void Dispose(bool disposing)
        {
            _back_brush.Dispose();
            base.Dispose(disposing);
        }

        public eThemeMode ThemeMode
        {
            get; set;
        }

        public object Content
        {
            get
            {
                return this;
            }
        }

        public int SeriesCount
        {
            get
            {
                IEnumerable<ISeries> serieses = GetAllSeries();

                return serieses == null ? 0 : serieses.Count<ISeries>();
            }
        }

        public void AddSeries(ISeries series)
        {
            this.Model.Series.Add((Series)series);
        }

        public void AddAxis(IAxis axis)
        {
            this.Model.Axes.Add(axis as Axis);
            if (axis is MultyAxes)
            {
                foreach (IAxis sub in ((MultyAxes)axis))
                {
                    this.Model.Axes.Add(sub as Axis);
                }
            }
        }

        public void AddPoint(PointModel pm)
        {
            ISeries series = FindSeries(pm.Name);
            if (series == null)
                return;

            series.AddPoint(pm);
        }

        public ISeries FindSeries(string id)
        {
            foreach (Series se in this.Model.Series)
            {
                ISeries series = se as ISeries;
                if (series == null)
                    continue;

                if (series.Id == id)
                {
                    return series;
                }
            }

            return null;
        }

        public IAxis FindAxis(string name)
        {
            foreach (Axis axis in this.Model.Axes)
            {
                IAxis cur = axis as IAxis;
                if (cur != null && cur.Name == name)
                    return cur;
            }

            return null;
        }

        public IEnumerable<ISeries> GetAllSeries()
        {
            List<ISeries> series = new List<ISeries>();
            foreach (Series se in this.Model.Series)
            {
                if(se is ISeries)
                    series.Add((ISeries)se);
            }

            return series;
        }

        public void SetTitle(string title)
        {
            this.Model.Title = title;
        }

        public void Prefer()
        {
            foreach (Series se in this.Model.Series)
            {
                if (se is IPerform)
                    ((IPerform)se).Prefer();
            }
        }

        public void Perform()
        {
            foreach (Series se in this.Model.Series)
            {
                if (se is IPerform)
                    ((IPerform)se).Perform();
            }
        }

        public void RefreshView()
        {
            base.InvalidatePlot(false);
        }

        public void Clear()
        {
            this.Model.Series.Clear();
            this.Model.Axes.Clear();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            LegendStyle style = _legend_theme.GetStyle(ThemeMode) as LegendStyle;
            Model.LegendBorder = Helper.ConvertColorToOxyColor( style.BorderColor);
            Model.LegendTextColor = Helper.ConvertColorToOxyColor(style.LabelColor);
            Model.TitleColor = Helper.ConvertColorToOxyColor(style.TitleColor);
            Color back_color = Helper.GetBackColorByThemeMode(ThemeMode);


            _back_brush.Color = back_color;
            e.Graphics.FillRectangle(_back_brush, this.Bounds);
            for (int i = 0; i < this.Model.Series.Count - 1; i++)
            {
                ISeries pre = this.Model.Series[i] as ISeries;
                for (int j = i + 1; j < this.Model.Series.Count; j++)
                {
                    ISeries cur = this.Model.Series[j] as ISeries;
                    if (pre.Priority > cur.Priority)
                    {
                        Series temp = this.Model.Series[i];
                        this.Model.Series[i] = this.Model.Series[j];
                        this.Model.Series[j] = temp;
                    }
                }
            }

#if DEBUG_MAP
            Console.WriteLine("onpaint");
#endif
            base.OnPaint(e);
        }

        public void AddRange(IEnumerable<PointModel> pms)
        {
            foreach (PointModel pm in pms)
                this.AddPoint(pm);
        }

        public void SetGroupVisible(string id,bool visible)
        {
            IAxis axis= FindAxis(id);
            if(axis!=null)
                axis.AxisVisible = visible;
        }

        public event EventHandler<SeriesVisibleChangedEventArgs> SeriesVisibleChanged;
        void OnSeriesVisibleChanged(object sender, SeriesVisibleChangedEventArgs e)
        {
            var handler = SeriesVisibleChanged;
            if (handler != null)
                handler(sender, e);
        }

        public void InitlizeLegend()
        {
            this.Model.IsLegendVisible = false;
            LegendSeries legend = new LegendSeries();
            legend.LegendClick += (s, e) =>
              {
                  if (e.MouseButton == MouseButtons.Left)
                  {
                      e.Series.SeriesVisible = !e.Series.SeriesVisible;
                      RefreshView();
                      OnSeriesVisibleChanged(this, new SeriesVisibleChangedEventArgs(e.Series.Id,e.Series.SeriesVisible));
                  }
              };
            this.Model.Series.Add(legend);
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            foreach (Series se in Model.Series)
            {
                if (se is ISeries)
                {
                    if (((ISeries)se).SeriesVisible)
                    {
                        if (se is IMouseInputHandler)
                        {
                            if (((IMouseInputHandler)se).OnMouseHover(e))
                                break;
                        }
                    }
                }               
            }
            base.OnMouseMove(e);
            RefreshView();
        }

        protected override void OnMouseClick(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseClick(e);
            MouseOneClick(e);
        }

        protected override void OnMouseDoubleClick(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            MouseTwoClick(e);
        }

        void MouseOneClick(System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                foreach (Series se in Model.Series)
                {
                    if (se is IMouseInputHandler)
                    {
                        ((IMouseInputHandler)se).OnMouseClick(e);
                    }
                }
            }
            catch
            { }
        }

        void MouseTwoClick(System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                foreach (Series se in Model.Series)
                {
                    if (se is IMouseInputHandler)
                    {
                        ((IMouseInputHandler)se).OnMouseDoubleClick(e);
                    }
                }
            }
            catch
            { }
        }


        public void InverseData()
        {
            foreach (Series se in this.Model.Series)
            {
                if (se is IInverseData)
                {
                    ((IInverseData)se).InverseData();
                }
            }

            foreach (Axis axis in this.Model.Axes)
            {
                if (axis is IInverseData)
                {
                    ((IInverseData)axis).InverseData();
                }
            }
        }

        public Image CopyImage()
        {
            Bitmap bitmap = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bitmap, new Rectangle(0, 0, this.Width, this.Height));
            return bitmap;
        }

        public ISeries FindSeries(Predicate<ISeries> match)
        {
            foreach (Series se in this.Model.Series)
            {
                ISeries series = se as ISeries;
                if (series == null)
                    continue;

                if (match(series))
                {
                    return series;
                }
            }

            return null;
        }

        public void RemoveSeries(string id)
        {
            Series series = FindSeries(id) as Series;
            if (series != null)
                this.Model.Series.Remove(series);
        }

        public void ClearSeries()
        {
            this.Model.Series.Clear();
        }

        public void ClearAxes()
        {
            this.Model.Axes.Clear();
        }

        public void ResetAllAxes()
        {
            foreach (Axis axis in this.Model.Axes)
            {
                axis.Reset();
            }
        }

        public IEnumerable<IAxis> GetAllAxes()
        {
            List<IAxis> axes = new List<IAxis>();
            foreach (Axis axis in this.Model.Axes)
            {
                IAxis cur = axis as IAxis;
                axes.Add(cur);
            }

            return axes;
        }

        public void SortGroupSeries(Action<IEnumerable<ISeries>> sortgroup)
        {
            if (sortgroup != null)
            {
                sortgroup(this.GetAllSeries());
            }
        }


        public void RemoveSeries(ISeries se)
        {
            if (se == null)
                return;

            this.Model.Series.Remove(se as Series);
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            this.Focus();
            base.Capture = true;
        }        
    }
}
