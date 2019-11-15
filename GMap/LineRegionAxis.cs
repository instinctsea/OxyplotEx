using OxyPlot;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OxyplotEx.Model;
using OxyplotEx.GMap.Theme;
using System.Drawing;
using System.IO;
using OxyplotEx.Model.Styles;

namespace OxyplotEx.GMap
{
    class LineRegionAxis:LinearAxis,IAxis
    {
        protected OxyRect _bound;
        protected OxyRect _label_bond;
        string label_font = "Arial";
        public LineRegionAxis()
        {
            base.IsPanEnabled = false;
            TitleColor = OxyColors.Green;
            TextColor = OxyColors.Green;
            AxisVisible = true;
            this.AxisDistance = 15;
            this.MinVisible = -1000000;
        }

        public eThemeMode ThemeMode
        {
            get; set;
        }

        public string Name
        {
            get;
            set;
        }

        public string AxisKey
        {
            get { return base.Key; }
            set
            {
                base.Key = value;
            }
        }

        public new int PositionTier
        {
            get { return base.PositionTier; }
            set
            {
                base.PositionTier = value;
            }
        }

        public new string Title
        {
            get { return base.Title; }
            set
            {
                base.Title = value;
            }
        } 

        public OxyRect Bound
        {
            get { return _bound; }
        }

        public AxisTheme Theme
        {
            get;set;
        }

        public bool AxisVisible
        {
            get { return IsAxisVisible; }
            set { IsAxisVisible = value; }
        }

        public eAxisPosition APosition
        {
            get;set;
        }

        public OxyRect LabelBound
        {
            get
            {
                return _label_bond;
            }
        }

        public bool GridLineVisible
        {
            get;set;
        }

        public double MinVisible { get; set; }

        List<double> _points = new List<double>();
        public virtual void UpdateBound(BoundParameter bound)
        {
            _bound = bound.RegionBound;
            _label_bond = bound.LabelBound;
            UpdateTransform2(_bound);

#if DEBUG_MAP
            Console.WriteLine(this.Name+" call function updatebound:{0} {1} {2} {3}",bound.RegionBound.Left,bound.RegionBound.Top,bound.RegionBound.Width,bound.RegionBound.Height);
#endif
        }

        public void UpdateTransform()
        {
            double am = 3;
            UpdateTransform2(_bound);
            OxyRect source = new OxyRect(_bound.Left, _bound.Height, _bound.Width, _bound.Height * am >this.PlotModel.PlotArea.Height? this.PlotModel.PlotArea.Height: _bound.Height*am);
            UpdateIntervals2(source);// _bound);
        }

        public override double Transform(double x)
        {
            return base.Transform(x);
        }

        public override ScreenPoint Transform(double x, double y, Axis yaxis)
        {
            return base.Transform(x, y, yaxis);
        }

        void UpdateTransform(OxyRect bound)
        {
            double x0 = bound.Left;
            double x1 = bound.Right;
            double y0 = bound.Bottom;
            double y1 = bound.Top;

            if (bound.Width == 0 || bound.Height == 0)
                return;

            this.ScreenMin = new ScreenPoint(x0, y1);
            this.ScreenMax = new ScreenPoint(x1, y0);

            double a0 = this.IsHorizontal() ? x0 : y0;
            double a1 = this.IsHorizontal() ? x1 : y1;

            double dx = a1 - a0;
            a1 = a0 + (this.EndPosition * dx);
            a0 = a0 + (this.StartPosition * dx);
            this.ScreenMin = new ScreenPoint(a0, a1);
            this.ScreenMax = new ScreenPoint(a1, a0);

            if (this.ActualMaximum - this.ActualMinimum < double.Epsilon)
            {
                this.ActualMaximum = this.ActualMinimum + 1;
            }

            double max = this.PreTransform(this.ActualMaximum);
            double min = this.PreTransform(this.ActualMinimum);

            double da = a0 - a1;
            double newOffset, newScale;
            if (Math.Abs(da) > double.Epsilon)
            {
                newOffset = (a0 / da * max) - (a1 / da * min);
            }
            else
            {
                newOffset = 0;
            }

            double range = max - min;
            if (Math.Abs(range) > double.Epsilon)
            {
                newScale = (a1 - a0) / range;
            }
            else
            {
                newScale = 1;
            }

            this.SetTransform(newScale, newOffset);
        }

        protected virtual void UpdateTransform2(OxyRect bound)
        {
            double x0 = bound.Left;
            double x1 = bound.Right;
            double y0 = bound.Bottom;
            double y1 = bound.Top;

            //if (this.IsHorizontal())
            //{
            //    this.StartPosition= x0;
            //    this.EndPosition = x1;
            //}
            //else
            //{
            //    this.StartPosition = y1;
            //    this.StartPosition = y0;
            //}

            this.ScreenMin = new ScreenPoint(x0, y1);
            this.ScreenMax = new ScreenPoint(x1, y0);

            double a0 = this.IsHorizontal() ? x0 : y0;
            double a1 = this.IsHorizontal() ? x1 : y1;

            double dx = a1 - a0;
            a1 = a0 + (this.EndPosition * dx);
            a0 = a0 + (this.StartPosition * dx);
            this.ScreenMin = new ScreenPoint(a0, a1);
            this.ScreenMax = new ScreenPoint(a1, a0);

            if (this.ActualMajorStep > 1000000)
            {

            }

            if (double.IsNaN(Maximum) || double.IsNaN(Minimum))
                return;

            double max = this.PreTransform(this.Maximum);
            double min = this.PreTransform(this.Minimum);

            double da = a0 - a1;
            double newScale, newOffset;
            if (Math.Abs(da) > double.Epsilon)
            {
                newOffset = (a0 / da * max) - (a1 / da * min);
            }
            else
            {
                newOffset = 0;
            }

            double range = max - min;
            if (Math.Abs(range) > double.Epsilon)
            {
                newScale = (a1 - a0) / range;
            }
            else
            {
                newScale = 1;
            }

            this.SetTransform(newScale, newOffset);
        }

        public override void Render(IRenderContext rc, PlotModel model, AxisLayer axisLayer, int pass)
        {
            //base.Render(rc, model, axisLayer, pass);            
        }

        public virtual void Render(IRenderContext rc, PlotModel model)
        {
#if DEBUG_MAP
            Console.WriteLine("render line region axis " + this.Name);
#endif
            if (!AxisVisible)
                return;

            OxyColor grid_color = OxyColors.Red;
            if (Theme != null)
            {
                AxisStyle style = Theme.GetStyle(ThemeMode) as AxisStyle;
                TitleColor = Helper.ConvertColorToOxyColor(style.TitleColor);
                TextColor = Helper.ConvertColorToOxyColor(style.LabelColor);
                TicklineColor = Helper.ConvertColorToOxyColor(style.LineColor);
                grid_color =Helper.ConvertColorToOxyColor( style.GridColor);
            }
            
            UpdateAutoPoints();
            switch (Position)
            {
                case AxisPosition.Left:
                    UpdateDataLeft(rc,model);
                    break;
                case AxisPosition.Right:
                    UpdateDataRight(rc, model);
                    break;
                case AxisPosition.Top:
                    break;
                case AxisPosition.Bottom:
                    break;
            }

            RenderAxis(rc, model,grid_color);
            //render border
            rc.DrawRectangle(_bound, OxyColors.Transparent, TicklineColor, 1);
        }

        public override OxySize Measure(IRenderContext rc)
        {
            OxySize size = new OxySize(0, 0);
            
            double total_width = 0;
            double total_height = 0;

            UpdateAutoPoints();
            switch (Position)
            {
                case AxisPosition.Left:
                    UpdateDataLeft(rc, null);
                    break;
                case AxisPosition.Right:
                    UpdateDataRight(rc, null);
                    break;
                case AxisPosition.Top:
                    break;
                case AxisPosition.Bottom:
                    break;
            }
            double text_width=0, text_height = 0;
            if(_features!=null)
            foreach (FeatureText major_label_value in _features)
            {
                string text =major_label_value.Text;
                OxySize text_size = rc.MeasureText(text);
                if (text_size.Width > text_width)
                    text_width = text_size.Width;
                if (text_size.Height > text_height)
                    text_height = text_size.Height;
            }

            OxySize title_size = new OxySize(0, 0);
            if (!string.IsNullOrEmpty(Title))
            {
                title_size = rc.MeasureText(Title, label_font);
            }

            switch (this.Position)
            {
                case AxisPosition.Left:
                case AxisPosition.Right:
                    total_width = title_size.Height + text_width + MajorTickSize + AxisDistance + AxisTitleDistance + AxisTickToLabelDistance;
                    break;
                case AxisPosition.Top:
                case AxisPosition.Bottom:
                    total_height = title_size.Height + text_height + MajorTickSize + AxisDistance + AxisTitleDistance + AxisTickToLabelDistance;
                    break;
            }


            size = new OxySize(total_width, total_height);            
            return size;
            /////return base.Measure(rc);
        }

        protected void UpdateDataLeft(IRenderContext rc, PlotModel model)
        {
            _features = null;
            _grid_lines = new List<List<ScreenPoint>>();

            double left = _bound.Left;

            //render label
            FeatureTextIntersector feature_text_intersector = new FeatureTextIntersector();
            foreach (double major_label_value in _points)
            {
                string text = FormatString(major_label_value);
                OxySize text_size = rc.MeasureText(text);
                double x = left - MajorTickSize - AxisTickToLabelDistance - text_size.Width;
                double y = this.Transform(major_label_value);
                if (double.IsNaN(y) || y < _bound.Top || y > _bound.Bottom)
                    continue;

                double y_center =y- text_size.Height / 2;
                feature_text_intersector.Add(new FeatureText(text, new ScreenPoint(x, y_center), text_size,new ScreenPoint(left,y)));
            }

            _features = feature_text_intersector.DiscaredIntersection();
            double text_width=0;
            if (_features != null)
            {
                foreach (FeatureText feature in _features)
                {
                    List<ScreenPoint> major_line = new List<ScreenPoint>();
                    ScreenPoint source = feature.SourcePosition;
                    ScreenPoint left_sp =new ScreenPoint(source.X - MajorTickSize,source.Y);

                    major_line.Add(source);
                    major_line.Add(left_sp);
                    _grid_lines.Add(major_line);

                    if (text_width < feature.Size.Width)
                        text_width = feature.Size.Width;
                }
            }

            if (!string.IsNullOrEmpty(Title))
            {
                OxySize title_size = rc.MeasureText(Title, label_font);
                double title_x = _label_bond.Left + AxisTitleDistance;
                double title_y = _bound.Top + _bound.Height / 2;
                _title_position = new ScreenPoint(title_x, title_y);
            }
        }

        protected void UpdateDataRight(IRenderContext rc, PlotModel model)
        {
            _features = null;
            _grid_lines = new List<List<ScreenPoint>>();

            double left = _bound.Right;

            FeatureTextIntersector feature_text_intersector = new FeatureTextIntersector();
            foreach (double major_label_value in _points)
            {
                string text = FormatString(major_label_value);
                OxySize text_size = rc.MeasureText(text);
                double x = left + MajorTickSize + AxisTickToLabelDistance;
                double y = this.Transform(major_label_value);
                if (double.IsNaN(y) || y < _bound.Top || y > _bound.Bottom)
                    continue;

                double y_center = y - text_size.Height / 2;
                feature_text_intersector.Add(new FeatureText(text, new ScreenPoint(x, y_center), text_size, new ScreenPoint(left, y)));
            }

            _features = feature_text_intersector.DiscaredIntersection();
            double text_width = 0;
            if (_features != null)
            {
                foreach (FeatureText feature in _features)
                {
                    List<ScreenPoint> major_line = new List<ScreenPoint>();
                    ScreenPoint source = feature.SourcePosition;
                    ScreenPoint right_sp = new ScreenPoint(source.X + MajorTickSize, source.Y);

                    major_line.Add(source);
                    major_line.Add(right_sp);
                    _grid_lines.Add(major_line);

                    if (text_width < feature.Size.Width)
                        text_width = feature.Size.Width;
                }
            }

            if (!string.IsNullOrEmpty(Title))
            {
                OxySize title_size = rc.MeasureText(Title, label_font);
                double title_x = _label_bond.Right - title_size.Height - AxisTitleDistance;
                double title_y = _bound.Top + _bound.Height / 2;
                _title_position = new ScreenPoint(title_x, title_y);
            }
        }

        protected List<FeatureText> _features=null;
        protected List<List<ScreenPoint>> _grid_lines = new List<List<ScreenPoint>>();
        protected ScreenPoint _title_position = new ScreenPoint(0, 0);

        protected void RenderAxis(IRenderContext rc, PlotModel model, OxyColor grid_color)
        {
            //render title                   
            if (!string.IsNullOrEmpty(Title))
            {
                using (Font font = new System.Drawing.Font("宋体", (float)ActualFontSize))
                {
                    OxySize title_size = rc.MeasureText(Title, label_font, ActualFontSize, ActualFontWeight);
                    ScreenPoint sp = new ScreenPoint(_title_position.X, _title_position.Y);
                    //rc.DrawText(sp, Title, TitleColor,label_font, ActualFontSize, ActualFontWeight, -90, HorizontalAlignment.Center, VerticalAlignment.Bottom);
                    //rc.DrawText(new ScreenPoint(100,100), Title, TitleColor, label_font, ActualFontSize, ActualFontWeight,10, HorizontalAlignment.Center, VerticalAlignment.Middle);

                    double y = sp.Y + title_size.Width / 2;
                    var field = rc.GetType().GetField("g", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    object o = field.GetValue(rc);
                    Graphics g = (Graphics)o;
                    g.TranslateTransform((float)sp.X, (float)y);
                    g.RotateTransform(-90);
                    using (Brush brush = new SolidBrush(Helper.ConvertOxyColorToColor(TitleColor)))
                    {
                        g.DrawString(Title, font,brush , new PointF(0, 0));
                    }
                       
                    g.ResetTransform();
                }
                    
            }


            if (double.IsNaN(Maximum) || double.IsNaN(Minimum))
            {
                RenderYAxisGrids(rc, this, null, grid_color);
            }
            else
            {
                if (_features != null)
                {
                    foreach (FeatureText feature in _features)
                    {
                        rc.DrawText(feature.Position, feature.Text, TextColor);
                    }

                    if (GridLineVisible)
                    {
                        RenderYAxisGrids(rc, this, _features, grid_color);
                    }
                    else
                    {
                        double y0 = this.Transform(0);
                        if (y0 >= this.Bound.Top && y0 <= this.Bound.Bottom)
                        {
                            IList<ScreenPoint> sps = new List<ScreenPoint>();
                            ScreenPoint sp1 = new ScreenPoint(this.Bound.Left, y0);
                            ScreenPoint sp2 = new ScreenPoint(this.Bound.Right, y0);

                            sps.Add(sp1);
                            sps.Add(sp2);
                            rc.DrawLine(sps, grid_color, 3);
                        }
                    }
                }

                if (_grid_lines != null)
                {
                    foreach (List<ScreenPoint> line in _grid_lines)
                    {
                        if (line != null)
                            rc.DrawLine(line, TicklineColor);
                    }
                }
            }
        }

        public void UpdateRange(double minimum, double maximum)
        {
            this.Maximum = maximum;
            this.Minimum = minimum;
            this.ActualMaximum = maximum;
            this.ActualMinimum = minimum;
        }

        public static void RenderYAxisGrids(IRenderContext rc,IAxis yAxis, List<FeatureText> features,OxyColor gridcolor)
        {
            if (features == null||features.Count==0)
            {
                double cell_height = yAxis.Bound.Height / 4;
                if (cell_height <= 0)
                    return;

                double y_start = yAxis.Bound.Top + cell_height;
                IList<double> screen_ys = new List<double>();                

                while (y_start < yAxis.Bound.Bottom)
                {
                    screen_ys.Add(y_start);
                    y_start += cell_height;
                }
            }
            else
            {
                foreach (FeatureText value in features)
                {
                    double y = value.SourcePosition.Y;

                    IList<ScreenPoint> sps = new List<ScreenPoint>();
                    ScreenPoint sp1 = new ScreenPoint(yAxis.Bound.Left, y);
                    ScreenPoint sp2 = new ScreenPoint(yAxis.Bound.Right, y);

                    sps.Add(sp1);
                    sps.Add(sp2);
                    rc.DrawLine(sps, gridcolor);
                }

                double y0= yAxis.Transform(0);
                if (y0 >= yAxis.Bound.Top && y0 <= yAxis.Bound.Bottom)
                {
                    IList<ScreenPoint> sps = new List<ScreenPoint>();
                    ScreenPoint sp1 = new ScreenPoint(yAxis.Bound.Left, y0);
                    ScreenPoint sp2 = new ScreenPoint(yAxis.Bound.Right, y0);

                    sps.Add(sp1);
                    sps.Add(sp2);
                    rc.DrawLine(sps, gridcolor,3);
                }
            }
        }

        internal virtual void UpdateIntervals(OxyRect plotArea)
        {
            this.IntervalLength = 18;
            double labelSize = this.IntervalLength;
            double length = this.IsHorizontal() ? plotArea.Width : plotArea.Height;
            length *= Math.Abs(this.EndPosition - this.StartPosition);

            labelSize = length / 20;
            if (labelSize < this.IntervalLength)
                labelSize = this.IntervalLength;
            this.ActualMajorStep = !double.IsNaN(this.MajorStep)
                                       ? this.MajorStep
                                       : this.CalculateActualInterval(length, labelSize);

            this.ActualMinorStep = !double.IsNaN(this.MinorStep)
                                       ? this.MinorStep
                                       : this.CalculateMinorInterval(this.ActualMajorStep);

            if (double.IsNaN(this.ActualMinorStep))
            {
                this.ActualMinorStep = 2;
            }

            if (double.IsNaN(this.ActualMajorStep))
            {
                this.ActualMajorStep = 10;
            }

            this.ActualMinorStep = Math.Max(this.ActualMinorStep, 0);
            this.ActualMajorStep = Math.Max(this.ActualMajorStep, 0);

            this.ActualStringFormat = this.StringFormat;

            if (this.ActualStringFormat == null)
            {
                this.ActualStringFormat = "g6";
            }

#if DEBUG_MAP
            StringBuilder buffer = new StringBuilder();
            buffer.Append(string.Format("Interval length:{0}",this.IntervalLength ));
            Console.WriteLine(buffer.ToString());
#endif
        }

        internal virtual void UpdateIntervals2(OxyRect plotArea)
        {
            double labelSize = this.IntervalLength;
            double length = this.IsHorizontal() ? plotArea.Width : plotArea.Height;
            length *= Math.Abs(this.EndPosition - this.StartPosition);

            /////File.AppendAllText("oxyplot.txt", $"length:{length} labelsize{labelSize} major isnan:{double.IsNaN(MajorStep)} isinifi{double.IsInfinity(MajorStep)} isnagati{double.IsNegativeInfinity(MajorStep)} ispost:{double.IsPositiveInfinity(MajorStep)}");
            this.ActualMajorStep = !double.IsNaN(this.MajorStep)
                                       ? this.MajorStep
                                       : this.CalculateActualInterval(length, labelSize);

            this.ActualMinorStep = !double.IsNaN(this.MinorStep)
                                       ? this.MinorStep
                                       : this.CalculateMinorInterval(this.ActualMajorStep);

            ////File.AppendAllText("oxyplot.txt", $"major step:{this.ActualMajorStep} minsteo{this.ActualMinorStep}");

            if (double.IsNaN(this.ActualMinorStep))
            {
                this.ActualMinorStep = 2;
            }

            if (double.IsNaN(this.ActualMajorStep))
            {
                this.ActualMajorStep = 10;
            }

            if (!double.IsNaN(this.MinorStep) && !double.IsNaN(MajorStep))
            {
                this.ActualMinorStep = Math.Max(this.ActualMinorStep, this.MinorStep);
                this.ActualMajorStep = Math.Max(this.ActualMajorStep, this.MajorStep);
            }
            ///this.ActualStringFormat = this.StringFormat ?? this.GetDefaultStringFormat();
        }

        void UpdateAutoPoints()
        {
            
            _points.Clear();
            if (double.IsNaN(this.ActualMaximum)||double.IsNaN(this.ActualMinimum)||double.IsNaN(this.ActualMajorStep)||double.IsNaN(this.ActualMinorStep))
                return;
            IList<double> major_label_values, major_tick_values, min_tick_values;
            GetTickValues(out major_label_values, out major_tick_values, out min_tick_values);
            if (this.ActualMinimum > 1000000)
            {

            }

            IList<double> temp = new List<double>();
            if (min_tick_values.Count > 10)
            {
                for (int i = 0; i < min_tick_values.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        temp.Add(min_tick_values[i]);
                    }
                }

                min_tick_values = temp;
            }

            IList<double> values = major_label_values.Count > 4 ? major_label_values : min_tick_values;
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] >= MinVisible)
                    _points.Add(values[i]);
            }
            /////Console.WriteLine($"{this.Name} step{this.ActualMajorStep} min{this.ActualMinimum} max{this.ActualMaximum} start{this.StartPosition} end{this.EndPosition}");
            /////#if DEBUG_MAP
            StringBuilder buffer = new StringBuilder();
            foreach (double value in _points)
            {
                buffer.Append(value + ",");
            }
            buffer.Append(string.Format(" rect:{0},{1},{2},{3}", _bound.Left, _bound.Top, _bound.Width, _bound.Height));
            /////Console.WriteLine(this.Title+":"+ buffer.ToString());
            ////File.AppendAllText("oxyplot.txt", buffer.ToString());
            ////#endif
        }

        public static string FormatString(string value)
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

        public static string FormatString(double number)
        {
            if (number == (int)number)
                return number.ToString("f0");
            else if (number == float.Parse(number.ToString("f1")))
                return number.ToString("f0");
            else if (float.Parse(number.ToString("f1")) == float.Parse(number.ToString("f2")))
                return number.ToString("f1");
            else
                return number.ToString("f2");
        }
    }
}
