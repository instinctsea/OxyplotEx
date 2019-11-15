using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OxyPlot;
using OxyPlot.Axes;
using OxyplotEx.Model.DataSeries;
using OxyplotEx.Model;
using OxyplotEx.GMap.Theme;
using OxyplotEx.Model.TimeSeries;
using OxyplotEx.Model.Styles;

namespace OxyplotEx.GMap
{
    class PointAreaSeries : AreaSeries,ISeries,IPerform,IAjustAxis,IInverseData
    {
        double _maximum = double.MinValue, _minimum = double.MaxValue;
        const double Scale = 0.1;
        ScreenPoint TextOffset = new ScreenPoint(9, -9);
        const int TextPadding = 3;
        List<PointModel> _points = new List<PointModel>();
        public PointAreaSeries()
        {
            LineType = LineType.Solid;
            LabelVisible = true;
            Color = OxyColors.Green;
            ShowLimit = false;
            ShowEverage = false;
        }
        public string Id
        {
            get;set;
        }

        public eThemeMode ThemeMode
        {
            get; set;
        }

        public bool LabelVisible
        {
            get;set;
        }

        public LineType LineType
        {
            get;
            set;
        }

        public string XKey
        {
            get
            {
                return base.XAxisKey;
            }

            set
            {
                base.XAxisKey = value;
            }
        }

        public string YKey
        {
            get
            {
                return base.YAxisKey;
            }

            set
            {
                base.YAxisKey = value;
            }
        }

        public new Axis YAxis
        {
            get;
            private set;
        }

        public string LegendTitle
        {
            get
            {
                return Title;
            }

            set
            {
                Title = value;
            }
        }

        public ThemeBase Theme
        {
            get;set;
        }

        public double LimitValue
        {
            get;set;
        }

        public bool ShowLimit
        {
            get;set;
        }

        public bool ShowEverage
        {
            get;set;
        }

        public bool SeriesVisible
        {
            get
            {
                return IsVisible;
            }

            set
            {
                IsVisible = value;
            }
        }

        public string Name
        {
            get;set;
        }

        public double Maximum
        {
            get
            {
                return _maximum;
            }
        }

        public double Minimum
        {
            get
            {
                return _minimum;
            }
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

        public void AddPoint(PointModel point)
        {
            double value;
            if (!double.TryParse(point.Value, out value))
                return;

            this.Points.Add(new OxyPlot.DataPoint(point.Index,value));
            if (value > _maximum)
                _maximum = value;
            if (value < _minimum)
                _minimum = value;

            _points.Add(point);
        }

        public bool AjustAxis(double sourceMinimum, double SourceMaximum, out double minimum, out double maximum)
        {
            minimum = 0;
            maximum = 0;
            if (this.Points.Count == 0)
                return false;
            if (this.YAxis == null)
            {
                this.YAxis = ((MapPlotModel)PlotModel).GetAxis(this.YKey);
            }
            Axis y_axis = this.YAxis;
            if (!(y_axis is IAxis))
                return false;

            double cur_min = _minimum < sourceMinimum ? _minimum : sourceMinimum;
            double cur_max = _maximum > SourceMaximum ? _maximum : SourceMaximum;
            PointLineSeries.AjustYAxis(LabelVisible, TextOffset.Y, ActualFontSize, cur_min,cur_max, Scale,((IAxis)y_axis).Bound,out maximum,out minimum);
            return true;
        }

        public void Perform()
        {
            this.LineType = LineType;
            List<OxyPlot.DataPoint> list = this.Points;
            PointLineSeries.SortDataPoints(ref list);
        }

        public void Prefer()
        {         
            this.LineStyle = OxyPlot.LineStyle.None;
        }

        public override void Render(IRenderContext rc,PlotModel model1)
        {
            PlotModel model = this.PlotModel;
            this.YAxis = ((MapPlotModel)model).GetAxis(this.YKey);
            if (!SeriesVisible || !((IAxis)this.YAxis).AxisVisible)
                return;
            //AjustAxis();
            if (Points.Count == 0)
                return;

            OxyColor limit_color = OxyColors.Red;
            if (Theme != null)
            {
                LineSeriesStyle style = Theme.GetStyle(ThemeMode) as LineSeriesStyle;
                this.Color = Helper.ConvertColorToOxyColor(style.LineColor);
                limit_color = Helper.ConvertColorToOxyColor(style.AlarmColor);
            }
           
            rc.ResetClip();
            OxyRect clippingRect = model.PlotArea;
            List<ScreenPoint> sps = new List<ScreenPoint>();
            for (int i = 0; i < Points.Count; i++)
            {
                double x = this.XAxis.Transform(Points[i].X);
                double y = this.YAxis.Transform(Points[i].Y);
                ScreenPoint sp = new ScreenPoint(x, y);
                sps.Add(sp);

                if (LabelVisible)
                {
                    string text =PointLineSeries.FormatValue(Points[i].Y);
                    PointLineSeries.RenderBoxText(rc,ThemeMode, clippingRect, text, TextPadding, sp, TextOffset, Color);
                }
            }

            if (ShowLimit)
            {
                if (LimitValue != Helper.InvalidData)
                {
                    double y = this.YAxis.Transform(LimitValue);
                    double left = model.PlotArea.Left;
                    double right = model.PlotArea.Right;

                    if (y >= this.YAxis.ScreenMin.Y && y <= this.YAxis.ScreenMax.Y)
                    {
                        rc.DrawLine(left, y, right, y, new OxyPen(limit_color, 1, LineStyle.Solid, LineJoin.Miter));
                        string limit_text = PointLineSeries.FormatValue(LimitValue);
                        rc.DrawText(new ScreenPoint(left + 10, y - 15), limit_text, limit_color, "Arial");
                    }
                }
            }

            if (Points.Count > 0)
            {
                double x = this.XAxis.Transform(Points[0].X);
                double y = this.YAxis.Transform(YAxis.Minimum)-2;

                double x1 = this.XAxis.Transform(Points[Points.Count-1].X);
                double y1 = y;

                ScreenPoint sp = new ScreenPoint(x, y);
                ScreenPoint sp1 = new ScreenPoint(x1, y1);

                IList<ScreenPoint> poligon = new List<ScreenPoint>();
                poligon.Add(sp);
                foreach (ScreenPoint item in sps)
                {
                    poligon.Add(item);
                }
                poligon.Add(sp1);

                rc.DrawClippedPolygon(clippingRect,poligon, 2, OxyColor.FromAColor(150, Color), OxyColors.Transparent);
                RenderLine(rc, clippingRect, sps, Color, 2, this.LineStyle);
            }
            
        }

        protected void RenderLine(IRenderContext rc, OxyRect clippingRect, IList<ScreenPoint> pointsToRender, OxyColor lineColor, double strokeness, LineStyle lineStyle)
        {
            var screenPoints = pointsToRender;
            var resampledPoints = ScreenPointHelper.ResamplePoints(pointsToRender, 2);
            screenPoints = CreateSpline(resampledPoints, 0.5, null, false, 0.25);

            var outputBuffer = new List<ScreenPoint>(pointsToRender.Count);

            var dash_array = lineStyle.GetDashArray();
            rc.DrawClippedLine(
                clippingRect,
                pointsToRender,
                0,
                lineColor,
                strokeness,
                dash_array,
                LineJoin.Round,
                false,
                outputBuffer);
        }

        protected void RenderUnClippedLine(IRenderContext rc, IList<ScreenPoint> pointsToRender, OxyColor lineColor, double strokeness, LineStyle lineStyle)
        {
            var screenPoints = pointsToRender;
            var resampledPoints = ScreenPointHelper.ResamplePoints(pointsToRender, 2);
            screenPoints = CreateSpline(resampledPoints, 0.5, null, false, 0.25);

            var outputBuffer = new List<ScreenPoint>(pointsToRender.Count);

            var dash_array = lineStyle.GetDashArray();

            rc.DrawLine(pointsToRender, lineColor, strokeness, dash_array, LineJoin.Round, false);
        }
        /// <summary>
        /// Creates a spline of data points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="tension">The tension.</param>
        /// <param name="tensions">The tensions.</param>
        /// <param name="isClosed">True if the spline is closed.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns>A list of data points.</returns>
        List<DataPoint> CreateSpline(List<DataPoint> points, double tension, IList<double> tensions, bool isClosed, double tolerance)
        {
            var screenPoints = points.Select(p => new ScreenPoint(p.X, p.Y)).ToList();
            var interpolatedScreenPoints = CreateSpline(screenPoints, tension, tensions, isClosed, tolerance);
            var interpolatedDataPoints = new List<DataPoint>(interpolatedScreenPoints.Count);

            foreach (var s in interpolatedScreenPoints)
            {
                interpolatedDataPoints.Add(new DataPoint(s.X, s.Y));
            }

            return interpolatedDataPoints;
        }

        /// <summary>
        /// Creates a spline of screen points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="tension">The tension.</param>
        /// <param name="tensions">The tensions.</param>
        /// <param name="isClosed">True if the spline is closed.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns>A list of screen points.</returns>
        List<ScreenPoint> CreateSpline(
             IList<ScreenPoint> points, double tension, IList<double> tensions, bool isClosed, double tolerance)
        {
            var result = new List<ScreenPoint>();
            if (points == null)
            {
                return result;
            }

            int n = points.Count;
            if (n < 1)
            {
                return result;
            }

            if (n < 2)
            {
                result.AddRange(points);
                return result;
            }

            if (n == 2)
            {
                if (!isClosed)
                {
                    GetSegment(result, points[0], points[0], points[1], points[1], tension, tension, tolerance);
                }
                else
                {
                    GetSegment(result, points[1], points[0], points[1], points[0], tension, tension, tolerance);
                    GetSegment(result, points[0], points[1], points[0], points[1], tension, tension, tolerance);
                }
            }
            else
            {
                bool useTensionCollection = tensions != null && tensions.Count > 0;

                for (int i = 0; i < n; i++)
                {
                    double t1 = useTensionCollection ? tensions[i % tensions.Count] : tension;
                    double t2 = useTensionCollection ? tensions[(i + 1) % tensions.Count] : tension;

                    if (i == 0)
                    {
                        GetSegment(
                            result,
                            isClosed ? points[n - 1] : points[0],
                            points[0],
                            points[1],
                            points[2],
                            t1,
                            t2,
                            tolerance);
                    }
                    else if (i == n - 2)
                    {
                        GetSegment(
                            result,
                            points[i - 1],
                            points[i],
                            points[i + 1],
                            isClosed ? points[0] : points[i + 1],
                            t1,
                            t2,
                            tolerance);
                    }
                    else if (i == n - 1)
                    {
                        if (isClosed)
                        {
                            GetSegment(result, points[i - 1], points[i], points[0], points[1], t1, t2, tolerance);
                        }
                    }
                    else
                    {
                        GetSegment(result, points[i - 1], points[i], points[i + 1], points[i + 2], t1, t2, tolerance);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// The segment.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="pt0">The pt 0.</param>
        /// <param name="pt1">The pt 1.</param>
        /// <param name="pt2">The pt 2.</param>
        /// <param name="pt3">The pt 3.</param>
        /// <param name="t1">The t 1.</param>
        /// <param name="t2">The t 2.</param>
        /// <param name="tolerance">The tolerance.</param>
        void GetSegment(
            IList<ScreenPoint> points,
            ScreenPoint pt0,
            ScreenPoint pt1,
            ScreenPoint pt2,
            ScreenPoint pt3,
            double t1,
            double t2,
            double tolerance)
        {
            // See Petzold, "Programming Microsoft Windows with C#", pages 645-646 or
            // Petzold, "Programming Microsoft Windows with Microsoft Visual Basic .NET", pages 638-639
            // for derivation of the following formulas:
            double sx1 = t1 * (pt2.X - pt0.X);
            double sy1 = t1 * (pt2.Y - pt0.Y);
            double sx2 = t2 * (pt3.X - pt1.X);
            double sy2 = t2 * (pt3.Y - pt1.Y);

            double ax = sx1 + sx2 + (2 * pt1.X) - (2 * pt2.X);
            double ay = sy1 + sy2 + (2 * pt1.Y) - (2 * pt2.Y);
            double bx = (-2 * sx1) - sx2 - (3 * pt1.X) + (3 * pt2.X);
            double by = (-2 * sy1) - sy2 - (3 * pt1.Y) + (3 * pt2.Y);

            double cx = sx1;
            double cy = sy1;
            double dx = pt1.X;
            double dy = pt1.Y;

            var num = (int)((Math.Abs(pt1.X - pt2.X) + Math.Abs(pt1.Y - pt2.Y)) / tolerance);

            // Notice begins at 1 so excludes the first point (which is just pt1)
            for (int i = 1; i < num; i++)
            {
                double t = (double)i / (num - 1);
                var pt = new ScreenPoint(
                    (ax * t * t * t) + (bx * t * t) + (cx * t) + dx,
                    (ay * t * t * t) + (by * t * t) + (cy * t) + dy);
                points.Add(pt);
            }
        }

        public void InverseData()
        {
            if (Points.Count == 0)
                return;

            foreach (PointModel pt in _points)
            {
                pt.InverseIndex();
            }

            _points.Sort(PointModel.SortAsc);

            Points.Clear();
            foreach (PointModel pt in _points)
            {
                double value;
                if (!double.TryParse(pt.Value, out value))
                    continue;

                this.Points.Add(new OxyPlot.DataPoint(pt.Index, value));
            }
        }

        public void ClearData()
        {
            this.Points.Clear();
            _points.Clear();
        }

        

        string FindValue(double x)
        {
            for (int i = 0; i < _points.Count; i++)
            {
                if (_points[i].Index == x)
                    return _points[i].Value;
            }

            return Helper.InvalidDataStr;
        }
    }
}
