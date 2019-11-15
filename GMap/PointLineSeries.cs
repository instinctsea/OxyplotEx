using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyplotEx.GMap.Theme;
using OxyplotEx.Model;
using OxyplotEx.Model.Styles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OxyplotEx.GMap
{
    class PointLineSeries : LineSeries,ISeries,IPerform,IAjustAxis,IInverseData,ILineStyle,IMouseInputHandler
    { 
        protected double _maximum = double.MinValue, _minimum = double.MaxValue;
        protected const double Scale = 0.15;
        protected ScreenPoint TextOffset = new ScreenPoint(9, -9);
        protected const int TextPadding = 3;
        protected const int TopMaxShiftSize = 15;
        protected const int LinePadding = 5;
        protected const int Distance = 5;
        List<PointModel> _points = new List<PointModel>();
        protected PointModel _mouse_move_point = null;
        protected IList<ScreenPoint> _smooth_points=new List<ScreenPoint>();
        private const int ToleranceDivisor = 200;
        public PointLineSeries()
        {
            LineType = LineType.Solid;
            this.MarkerType = MarkerType.Circle;
            this.LabelVisible = true;
            Color = OxyColors.Green;
            this.MarkerSize = 4;
            ShowLimit = false;
            this.LineWidth = 1;
            ShowEverage = false;
            _line_style = eLineStyle.Solid;
            this.PointLineStyle = ePointLineStyle.Polygon;
        }

        public string Id
        {
            get;set;
        }

        public bool LabelVisible
        {
            get;set;
        }

        public LineType LineType
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

        public virtual int Count
        {
            get { return Points.Count; }
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
            get; set;
        }

        public bool ShowEverage
        {
            get; set;
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

        public double LineWidth
        {
            get;set;
        }

        private eLineStyle _line_style;
        public eLineStyle SeriesLineStyle
        {
            get
            {
                return _line_style;
            }

            set
            {
                _line_style = value;
                LineStyle = (LineStyle)((int)value);
            }
        }

        public object UserData
        {
            get;set;
        }

        public virtual Priority Priority
        {
            get
            {
                return Priority.Middle;
            }
        }

        public ePointLineStyle PointLineStyle
        {
            get;set;
        }

        public virtual void AddPoint(PointModel point)
        {
            double value;
            if (!double.TryParse(point.Value, out value))
                return;

            this.Points.Add(new OxyPlot.DataPoint(point.Index, value));

            if (value > _maximum)
                _maximum = value;
            if (value < _minimum)
                _minimum = value;

            _points.Add(point);
        }

        bool _is_average = false;
        double _average = 0;
        public virtual void Perform()
        {
            this.LineStyle = (OxyPlot.LineStyle)_line_style;
            List<OxyPlot.DataPoint> list = this.Points;
            SortDataPoints(ref list);
            _is_average = TryGetAverage(out _average);
        }

        public void Prefer()
        {
            this.LineStyle = OxyPlot.LineStyle.None;
        }

        /// <summary>
        /// Force the smoothed points to be re-evaluated.
        /// </summary>
        protected void ResetSmoothedPoints(List<ScreenPoint> sps)
        {
            _smooth_points.Clear();
            double tolerance = Math.Abs(Math.Max(this.MaxX - this.MinX, this.MaxY - this.MinY) / ToleranceDivisor);
            tolerance = 0.2;
            List<DataPoint> dps = CanonicalSplineHelper.ScreenPointToDataPoint(sps);
            List<DataPoint> smooth_points = CanonicalSplineHelper.CreateSpline(dps, 0.5, null, false, tolerance);
            if (smooth_points!=null&&smooth_points.Count>0)
            {
                foreach (DataPoint dp in smooth_points)
                {
                    _smooth_points.Add(new ScreenPoint(dp.X, dp.Y));
                }
            }
        }

        public static void SortDataPoints(ref List<OxyPlot.DataPoint> list)
        {
            if (list == null || list.Count <= 1)
                return;

            list.Sort((left, right) =>
            {
                if (left.X > right.X)
                    return 1;
                else if (left.X < right.X)
                    return -1;
                else
                    return 0;
            });
        }

        public virtual bool AjustAxis(double sourceMinimum,double SourceMaximum, out double minimum, out double maximum)
        {
            minimum = 0;
            maximum = 0;
            if (Count == 0)
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
            AjustYAxis(LabelVisible, TextOffset.Y, ActualFontSize,cur_min,cur_max, Scale,((IAxis)y_axis).Bound,out maximum,out minimum,15);
            return true;     
        }

        public override void Render(IRenderContext rc,PlotModel model1)
        {
            PlotModel model = this.PlotModel;
            this.YAxis = ((MapPlotModel)model).GetAxis(this.YKey);
            //if (this.YAxis == null)
            //    return;
            if (!SeriesVisible || !((IAxis)this.YAxis).AxisVisible)
                return;

            //AjustAxis();
            if (Points.Count == 0)
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
            List<ScreenPoint> sps = new List<ScreenPoint>();
            for (int i = 0; i < Points.Count; i++)
            {
                double x = this.XAxis.Transform(Points[i].X);
                double y = this.YAxis.Transform(Points[i].Y);
                ScreenPoint sp = new ScreenPoint(x, y);

                sps.Add(sp);
            }

            BoxPosition box_position = BoxPosition.Left;
            switch(PointLineStyle)
            {
                case ePointLineStyle.Smooth:
                    box_position = BoxPosition.Left;
                    ResetSmoothedPoints(sps);
                    RenderLine(rc, clippingRect, _smooth_points, Color, this.LineWidth, this.LineStyle);
                    break;
                case ePointLineStyle.Polygon:
                    RenderLine(rc, clippingRect, sps, Color, this.LineWidth, this.LineStyle);
                    box_position = BoxPosition.Left;
                    break;
                case ePointLineStyle.Cloumn:
                    RenderColumn(rc,model, clippingRect, sps, Color);
                    box_position = BoxPosition.Middle;
                    break;
            }

            if (_is_average&&ShowEverage)
            {
                double y = this.YAxis.Transform(_average);
                double left = model.PlotArea.Left;
                double right = model.PlotArea.Right;

                rc.DrawLine(left, y, right, y, new OxyPen(average_color, 1, LineStyle.Dash, LineJoin.Miter));
                string average_text =FormatValue(_average);
                rc.DrawText(new ScreenPoint(left + 10, y - 15), average_text, average_color, "Arial");
            }

            if (LimitValue != Helper.InvalidData&&ShowEverage)
            {
                double y = this.YAxis.Transform(LimitValue);
                double left = model.PlotArea.Left;
                double right = model.PlotArea.Right;

                if (y >= this.YAxis.ScreenMin.Y && y <= this.YAxis.ScreenMax.Y)
                {
                    rc.DrawLine(left, y, right, y, new OxyPen(limit_color, 1, LineStyle.Solid, LineJoin.Miter));
                    string limit_text = FormatValue(LimitValue);
                    rc.DrawText(new ScreenPoint(left + 10, y - 15), limit_text, limit_color, "Arial");
                }
            }

            if (PointLineStyle == ePointLineStyle.Polygon || PointLineStyle == ePointLineStyle.Smooth)
            {
                for (int i = 0; i < Points.Count; i++)
                {
                    if (PointLineStyle != ePointLineStyle.Cloumn)
                        rc.DrawMarker(clippingRect, sps[i], this.MarkerType, null, MarkerSize, Color, MarkerStroke, 1);
                    if (LabelVisible)
                    {
                        string text = FormatValue(Points[i].Y);
                        RenderBoxText(rc,ThemeMode, clippingRect, text, TextPadding, sps[i], TextOffset, Color, box_position);
                    }
                }
            }

            RenderCurrentLegend(rc, _mouse_move_point);
        }

        public enum BoxPosition
        {
            Left,
            Middle,
            Right
        }

        void RenderColumn(IRenderContext rc,PlotModel model1, OxyRect clippingRect, IList<ScreenPoint> pointsToRender, OxyColor fillColor,int columnWith=15)
        {
            //compute offset
            List<string> column_serieses = new List<string>();
            for (int i = 0; i < model1.Series.Count; i++)
            {
                if (model1.Series[i] is PointColumnSeries || (model1.Series[i] is PointLineSeries && ((PointLineSeries)model1.Series[i]).PointLineStyle == ePointLineStyle.Cloumn))
                {

                    column_serieses.Add(((ISeries)model1.Series[i]).Id);
                }
            }

            int column_padding = 3;
            double total_width = this.XAxis.Transform(1) - this.XAxis.Transform(0);
            double column_width = columnWith;
            if ((columnWith + column_padding) * column_serieses.Count > total_width)
            {
                //auto width;
                column_width = (total_width - column_serieses.Count * column_padding) / column_serieses.Count;
            }
            if (column_width < 1)
                column_width = 1;
            double total_column_width = column_width * column_serieses.Count + (column_serieses.Count - 1) * column_padding;
            int index = column_serieses.IndexOf(this.Id);
            double offset = 0;
            if (index >= 0)
            {
                offset = index * (column_width + column_padding)-total_column_width/2+column_width/2;
            }

            for (int i = 0; i < pointsToRender.Count; i++)
            {
                double x = pointsToRender[i].X+offset;
                double y = pointsToRender[i].Y;
                string text = FormatValue(Points[i].Y);
                if (y == 0 || double.Parse(text) == 0)
                    continue;
                ScreenPoint center_point = new ScreenPoint(x, y);

                ScreenPoint left_top = new ScreenPoint(center_point.X - column_width * 1.0 / 2, center_point.Y);
                ScreenPoint right_top = new ScreenPoint(center_point.X + column_width * 1.0 / 2, center_point.Y);

                double y1 = this.YAxis.Transform(0);
                ScreenPoint right_bottom = new ScreenPoint(right_top.X, y1 - 2);
                ScreenPoint left_bottom = new ScreenPoint(left_top.X, y1 - 2);

                IList<ScreenPoint> poligon = new List<ScreenPoint>();
                poligon.Add(left_top);
                poligon.Add(right_top);
                poligon.Add(right_bottom);
                poligon.Add(left_bottom);

                rc.DrawClippedPolygon(clippingRect, poligon, 2, fillColor, fillColor);

                if (LabelVisible)
                {
                    RenderBoxText(rc,ThemeMode, clippingRect, text, TextPadding,new ScreenPoint(x,y), TextOffset, Color, BoxPosition.Middle);
                }
            }
        }

        protected override void UpdateData()
        {
            base.UpdateData();
        }

        public eThemeMode ThemeMode
        {
            get; set;
        }

        public static bool AjustYAxis(bool labelvisible,double yoffset,double fontsize,double min,double max,double scale,OxyRect bound, out double maximum,out double minimum,double linepadding=25)
        {
            maximum = max;
            minimum = min;
            double text_box_maximum_distance = -yoffset + fontsize * 2;
            double rest_length = bound.Bottom - bound.Top - text_box_maximum_distance - 2 * linepadding;

            if (rest_length <= 0)
            {
                rest_length = (bound.Bottom - bound.Top) > text_box_maximum_distance ? text_box_maximum_distance*1 : bound.Bottom - bound.Top;
            }

            if (!labelvisible)
                text_box_maximum_distance = 0;

            double max2 = (text_box_maximum_distance + linepadding) / rest_length * (max - min) + max;
            double min2 = min - (max - min) * linepadding / rest_length;

            if (max2 == min2)
            {
                if (min2 == 0)
                {
                    min2 = -10;
                    max2 = 10;
                }
                min2 = min2 *(1 - scale);
                max2 = max2 * (1 + scale);
            }

            minimum = min2;
            maximum = max2;
            if (maximum - minimum < 0.001)
            {
                minimum = double.Parse(minimum.ToString("f2"));
                maximum = minimum + 10;
            }
            return true;
        }

        public static void RenderBoxText(IRenderContext rc,eThemeMode themeMode, OxyRect clippingRect, string text, int textPadding, ScreenPoint sp, ScreenPoint offset, OxyColor color,BoxPosition position= BoxPosition.Left,string font= "Arial")
        {
            OxySize text_size = rc.MeasureText(text,font);

            ScreenPoint down_line_sp = new ScreenPoint(sp.X + offset.X, sp.Y + offset.Y);
            ScreenPoint text_sp = new ScreenPoint(sp.X + offset.X + TextPadding, sp.Y - text_size.Height - TextPadding + offset.Y);

            IList<ScreenPoint> line = new List<ScreenPoint>();
            line.Add(sp);
            

            double box_width = text_size.Width + 2 * TextPadding;
            double box_height = text_size.Height + 2 * TextPadding;
            OxyRect rect;
            switch (position)
            {
                case BoxPosition.Left:
                    rect= new OxyRect(down_line_sp.X,down_line_sp.Y-box_height,box_width,box_height);
                    break;
                case BoxPosition.Middle:
                    down_line_sp = new ScreenPoint(down_line_sp.X - offset.X, down_line_sp.Y);
                    rect = new OxyRect(down_line_sp.X - box_width / 2, down_line_sp.Y - box_height, box_width, box_height);
                    
                    text_sp = new ScreenPoint(down_line_sp.X - text_size.Width / 2, text_sp.Y);
                    break;
                case BoxPosition.Right:
                    rect = new OxyRect(down_line_sp.X - box_width, down_line_sp.Y - box_height, box_width, box_height);
                    text_sp = new ScreenPoint(down_line_sp.X - text_size.Width, text_sp.Y);
                    break;
                default:
                    rect = new OxyRect(0, 0, 0, 0);
                    break;
            }

            //ajust
            if (!Pan.ContainsRect(clippingRect,rect))
            {
                ScreenVector vector = Pan.AjustBound(rect, clippingRect);
                rect = Pan.PanRect(rect, vector);
                down_line_sp = Pan.PanPoint(down_line_sp, vector);
                text_sp = Pan.PanPoint(text_sp, vector);
            }

            line.Add(down_line_sp);
            rc.DrawLine(line, color);
            if (rect.Width > 0 && rect.Height > 0)
            {
                OxyColor back_color = Convertor.ConvertColorToOxyColor(Helper.DarkBackColor);
                OxyColor fore_color = Convertor.ConvertColorToOxyColor(Helper.LightBackColor);
                switch (themeMode)
                {
                    case eThemeMode.Dark:
                        back_color = Convertor.ConvertColorToOxyColor(Helper.DarkBackColor);
                        fore_color = OxyColors.White;
                        break;
                    case eThemeMode.Light:
                        back_color = Convertor.ConvertColorToOxyColor(Helper.LightBackColor);
                        fore_color = OxyColors.Black;
                        break;
                }
                rc.DrawRectangle(rect, back_color, fore_color);
                rc.DrawText(text_sp, text, fore_color,font);
            }
        }

        public static string FormatValue(double value,uint decimal_places=1)
        {
            string str = value.ToString();
            int point_index = str.IndexOf('.');
            if (point_index < 0)
                return str;

            if (str.Length - point_index-1 <= decimal_places)
            {
                return str;
            }

            string format = string.Format("f{0}", decimal_places);
            double cur =double.Parse(value.ToString(format));
            if (cur == 0)
                return "0";
            if (decimal_places >= 1)
            {
                if (cur == double.Parse(cur.ToString($"f{decimal_places - 1}")))
                {
                    return cur.ToString($"f{decimal_places - 1}");
                }
            }
            return value.ToString(format);
        }

        protected void RenderLine(IRenderContext rc, OxyRect clippingRect, IList<ScreenPoint> pointsToRender, OxyColor lineColor, double strokeness, LineStyle lineStyle)
        {
            //var screenPoints = pointsToRender;
            var resampledPoints = ScreenPointHelper.ResamplePoints(pointsToRender, 2);
            //screenPoints = CreateSpline(resampledPoints, 0.5, null, false, 0.25);

            var outputBuffer = new List<ScreenPoint>(pointsToRender.Count);

            if (this.LineStyle == LineStyle.None)
            {
                foreach (ScreenPoint sp in pointsToRender)
                {
                    rc.DrawMarker(clippingRect, sp, MarkerType.Diamond, null, 3, lineColor, lineColor, 1);
                }
            }
            else
            {
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

        public bool TryGetAverage(out double average)
        {
            average = 0;
            if (Points.Count == 0)
                return false;

            double sum = 0;
            for (int i = 0; i < Points.Count; i++)
            {
                sum += Points[i].Y ;
            }

            average = sum / Points.Count;
            return true;
        }

        public virtual void InverseData()
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

        public virtual void OnMouseClick(System.Windows.Forms.MouseEventArgs e)
        {
            
        }

        public virtual bool OnMouseHover(System.Windows.Forms.MouseEventArgs e)
        {
            _mouse_move_point=null;
            if (this.XAxis == null || this.YAxis == null)
                return false;
            
            for (int i=0;i< _points.Count;i++)
            {
                double x = this.XAxis.Transform(_points[i].Index);
                double y = this.YAxis.Transform(double.Parse(_points[i].Value));

                if (Math.Abs(x - e.Location.X) < Distance && Math.Abs(y - e.Location.Y) < Distance)
                {
                    _mouse_move_point = _points[i];
                    return true;
                }
            }

            return false;
        }

        void RenderCurrentLegend(IRenderContext rc, PointModel model)
        {
            if (model == null || this.XAxis == null || this.YAxis == null)
                return;

            if (!(this.XAxis is IGetXLabel))
                return;

            string label = ((IGetXLabel)this.XAxis).GetLabel(model.Index);
            if (string.IsNullOrEmpty(label))
                return;

            string text = this.Title + "\n" + label +"  "+ FormatString(model.Value);
            OxySize text_size = rc.MeasureText(text);

            double x = this.XAxis.Transform(model.Index);
            double y = this.YAxis.Transform(double.Parse(model.Value));

            OxyRect bound = ((IAxis)this.YAxis).Bound;
            if (y - text_size.Height-2*TextPadding < bound.Top)
            {

            }
            else
            {
                y =y- text_size.Height-2*TextPadding;
            }

            if (x + text_size.Width +2*TextPadding > bound.Right)
            {
                x -= text_size.Width+2*TextPadding;
            }
            else
            {

            }

            rc.DrawRectangle(new OxyRect(x, y, text_size.Width+2*TextPadding, text_size.Height+2*TextPadding),OxyColors.LimeGreen,OxyColors.Transparent);/// OxyColor.FromArgb(255,138,187,74)
            rc.DrawText(new ScreenPoint(x+TextPadding, y+TextPadding), text,OxyColors.White);           
        }

        static string FormatString(string value)
        {
            double num;
            if (double.TryParse(value, out num))
            {
                if (num == (int)num)
                    return num.ToString("f0");
                else
                    return num.ToString("f1");
            }
            else
            {
                return value;
            }
        }

        public virtual void ClearData()
        {
            this.Points.Clear();
            this._points.Clear();
        }

        public void OnMouseDoubleClick(System.Windows.Forms.MouseEventArgs e)
        {
            
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
