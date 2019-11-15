using OxyPlot.Series;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Axes;
using OxyplotEx.Model;
using OxyplotEx.GMap.Theme;
using System;
using OxyplotEx.Model.TimeSeries;
using OxyplotEx.Model.Styles;

namespace OxyplotEx.GMap
{
    class PointColumnSeries : LineSeries, ISeries,IAjustAxis,IPerform,IInverseData
    {
        double _maximum = double.MinValue, _minimum = double.MaxValue;
        const double Scale = 0.1;
        ScreenPoint TextOffset = new ScreenPoint(0, -6);
        const int TextPadding = 3;
        List<PointModel> _pts = new List<PointModel>();
        
        public PointColumnSeries()
        {
            FillColor = OxyColors.Green;
            ColumnWidth = 15;
            LabelVisible = true;
            ShowEverage = false;
            ShowLimit = false;
        }

        public string Id
        {
            get;set;
        }

        public PointModel this[int index]
        {
            get
            {
                return _pts[index];
            }
        }

        public OxyColor FillColor
        {
            get;
            set;
        }

        public float ColumnWidth
        {
            get;
            set;
        }

        public bool LabelVisible
        {
            get;set;
        }

        public LineType LineType
        {
            get;set;
        }

        public int Count
        {
            get
            {
                return _pts.Count;
            }
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

        public eThemeMode ThemeMode
        {
            get; set;
        }

        public object UserData
        {
            get;set;
        }

        public Priority Priority
        {
            get
            {
                return Priority.Normal;
            }
        }

        public void AddPoint(PointModel point)
        {
            double value;
            if (!double.TryParse(point.Value, out value))
                return;

            if (value > _maximum)
                _maximum = value;
            if (value < _minimum)
                _minimum = value;

            _pts.Add(point);
        }

        public bool AjustAxis(double sourceMinimum, double SourceMaximum, out double minimum, out double maximum)
        {
            minimum = 0;
            maximum = 0;
            if (this._pts.Count == 0)
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
            PointLineSeries.AjustYAxis(LabelVisible, TextOffset.Y, ActualFontSize, cur_min,cur_max, Scale, ((IAxis)y_axis).Bound,out maximum,out minimum);
            return true;
        }

        public override void Render(IRenderContext rc,PlotModel model1)
        {
            PlotModel model = this.PlotModel;
            this.YAxis = ((MapPlotModel)model).GetAxis(this.YKey);
            if (!SeriesVisible || !((IAxis)this.YAxis).AxisVisible)
                return;
            //AjustAxis();
            if (_pts.Count == 0)
                return;
            OxyColor average_color = OxyColors.Green;
            OxyColor limit_color = OxyColors.Red;
            if (Theme != null)
            {
                LineSeriesStyle style = Theme.GetStyle(ThemeMode) as LineSeriesStyle;
                this.FillColor = Helper.ConvertColorToOxyColor(style.LineColor);
                average_color = Helper.ConvertColorToOxyColor(style.AverageColor);
                limit_color = Helper.ConvertColorToOxyColor(style.AlarmColor);
            }
           
            rc.ResetClip();
            IAxis y_axis = this.YAxis as IAxis;
            OxyRect clippingRect =y_axis.Bound;

            double width = model.PlotArea.Width;

            List<FeatureText> features = new List<FeatureText>();
            //compute offset
            List<string> column_serieses = new List<string>();
            for (int i = 0; i < model1.Series.Count; i++)
            {
                if (model1.Series[i] is PointColumnSeries ||(model1.Series[i] is PointLineSeries && ((PointLineSeries)model1.Series[i]).PointLineStyle== ePointLineStyle.Cloumn))
                {

                    column_serieses.Add(((ISeries)model.Series[i]).Id);
                }
            }
            
            int column_padding = 3;
            double total_width= this.XAxis.Transform(1)- this.XAxis.Transform(0);
            double column_width = this.ColumnWidth;
            if ((this.ColumnWidth + column_padding) * column_serieses.Count > total_width)
            {
                //auto width;
                column_width = (total_width - column_serieses.Count * column_padding) / column_serieses.Count;
            }
            if (column_width < 1)
                column_width = 1;
            int index = column_serieses.IndexOf(this.Id);
            double total_column_width = column_width * column_serieses.Count + (column_serieses.Count - 1) * column_padding;
            double offset = 0;
            if (index >= 0)
            {
                offset = index * (column_width + column_padding) - total_column_width / 2 + column_width / 2; ;
            }
            for (int i = 0; i < Count; i++)
            {
                double value=double.Parse(this[i].Value);
                double x = this.XAxis.Transform(this[i].Index)+offset;
                if (value == 0)
                    continue;
                double y = this.YAxis.Transform(value);
                ScreenPoint center_point = new ScreenPoint(x, y);

                string text = PointLineSeries.FormatValue(value);
                if (double.Parse(text) == 0)
                    continue;
                features.Add(new FeatureText(text, center_point, new OxySize(10, 10),center_point));

                ScreenPoint left_top = new ScreenPoint(center_point.X - ColumnWidth / 2,center_point.Y);
                ScreenPoint right_top = new ScreenPoint(center_point.X + ColumnWidth / 2, center_point.Y);

                double y1 = this.YAxis.Transform(0);
                ScreenPoint right_bottom = new ScreenPoint(right_top.X, y1 - 2);
                ScreenPoint left_bottom = new ScreenPoint(left_top.X, y1 - 2);

                IList<ScreenPoint> poligon = new List<ScreenPoint>();
                poligon.Add(left_top);
                poligon.Add(right_top);
                poligon.Add(right_bottom);
                poligon.Add(left_bottom);
                rc.DrawClippedPolygon(clippingRect, poligon, 2, FillColor, FillColor);
            }

            if (_is_average&&ShowEverage)
            {
                double y = this.YAxis.Transform(_average);
                double left = model.PlotArea.Left;
                double right = model.PlotArea.Right;

                rc.DrawLine(left, y, right, y, new OxyPen(average_color, 1, LineStyle.Dash, LineJoin.Miter));
                string average_text =PointLineSeries.FormatValue( _average);
                rc.DrawText(new ScreenPoint(left + 10, y - 15), average_text, average_color, "Arial");
            }

            if (LimitValue != Helper.InvalidData&&ShowLimit)
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

            if (LabelVisible)
            {
                foreach (FeatureText feature in features)
                {
                    if (double.Parse(feature.Text) == 0)
                        continue;
                    PointLineSeries.RenderBoxText(rc,ThemeMode, clippingRect, feature.Text, TextPadding,feature.Position, TextOffset, FillColor, PointLineSeries.BoxPosition.Middle);
                }
            }
        }

        bool _is_average = false;
        double _average = 0;
        public bool TryGetAverage(out double average)
        {
            average = 0;
            if (Count == 0)
                return false;

            double sum = 0;
            for (int i = 0; i < Count; i++)
            {
                double value =double.Parse(this[i].Value);
                sum += value;
            }

            average = sum / Count;
            return true;
        }

        public void Prefer()
        {

        }

        public void Perform()
        {
            _is_average = TryGetAverage(out _average);
        }

        public void InverseData()
        {
            if (Count == 0)
                return;

            foreach (PointModel pt in _pts)
            {
                pt.InverseIndex();
            }

            _pts.Sort(PointModel.SortAsc);
        }

        public void ClearData()
        {
            _pts.Clear();
        }        

        string FindValue(double x)
        {
            for (int i = 0; i < _pts.Count; i++)
            {
                if (_pts[i].Index == x)
                    return _pts[i].Value;
            }

            return Helper.InvalidDataStr;
        }
    }
}
