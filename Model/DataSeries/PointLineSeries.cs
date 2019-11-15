
using Module.MICAPSDataChart.GMap;
using Module.MICAPSDataChart.Model.CoordinateAxises;
using Module.MICAPSDataChart.Model.Styles;
using OxyPlot;
using OxyPlot.Series;
/*
 * Copyright (c) 2014-2015 jiayongqiang. All rights reserved";
 * CLR version: 4.0.30319.18444"
 * File name:   AsyncLineSeries.cs "
 * Date:        2015/8/5 10:38:34 
 * Author :  jiayongqiang Developer
 * Description: 
	
 * History:  created by jiayongqiang 2015/8/5 10:38:34
*/
using System.Collections.Generic;
using System.Linq;
using System;
using System.Windows.Forms;

namespace Module.MICAPSDataChart.Model.DataSeries
{
    class PointLineSeries:LineSeries,IPointLineSeries,ILineSeriesStyle,IValueRange,ISeriesStyle,IMouseInputHandler,IReverseData,IPrepare
    {
        public PointLineSeries()
        {
            MainColor = System.Drawing.Color.Blue;
            LineStyle = OxyPlot.LineStyle.None;           
            Smooth = false;
            BrokenLineStyle = LineStyle.Dot;
            BrokenLineThickness = 0.5;
            CanTrackerInterpolatePoints = false;
            BrokenLineColor = OxyColors.Blue;
            LineStyle = LineStyle.None;
            MarkerStyle = eMarkerStyle.Square;
            ResetVerticalValueRange();
            SumSeq = 0;
        }
        protected const int Distance = 5;
        protected const int TextPadding = 3;
        public int SumSeq
        {
            get;
            set;
        }

        public string Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public void AddSeqData(SeqData data)
        {
            //SumSeq++;
            DataPoint dp = Convertor.ConvertDataPairToDataPoint(data);
            if (dp.Y == Helper.NoData)
                return;
            this.Points.Add(dp);
            UpdateVerticalValueRange(data);
        }

        public void AddRange(IEnumerable<SeqData> datas)
        {
            foreach (SeqData data in datas)
                this.AddSeqData(data);
        }

        public void Clear()
        {
            this.Points.Clear();
            //设置线条样式为None
            this.LineStyle = OxyPlot.LineStyle.None;
            ResetVerticalValueRange();
            SumSeq = 0;
        }

        public void Perform()
        {
            DataPoint[] pts = this.Points.OrderBy(p => p.X).ToArray();
            this.Points.Clear();
            Points.AddRange(pts);
            LineStyle = (LineStyle)(int)SeriesLineStyle;
        }

        public string XAxisBingdingKey
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

        public string YAxisBingdingKey
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

        public System.Drawing.Color MainColor
        {
            get
            {
                if (Theme != null)
                    this.Color = Convertor.ConvertColorToOxyColor(Theme.GetThemeColor());

                return Convertor.ConvertOxyColorToColor(this.Color);
            }
            set
            {
                this.Color = Convertor.ConvertColorToOxyColor(value);
                this.MarkerStroke = Color;
                this.MarkerFill = Color;
                if (Theme != null)
                {
                    Theme.SetThemeColor(value);
                }
            }
        }


        public eMarkerStyle MarkerStyle
        {
            get
            {
                return Convertor.ConvertMarkerTypeToMarkerStyle(this.MarkerType);
            }
            set
            {
                this.MarkerType = Convertor.ConvertMarkerStyleToMarkerType(value);
            }
        }

        public int LineWidth
        {
            get
            {
                return (int)StrokeThickness;
            }
            set
            {
                StrokeThickness = value;
            }
        }

        public string XDataField
        {
            get
            {
                return DataFieldX;
            }
            set
            {
                DataFieldX = value;
            }
        }

        public string YDataField
        {
            get
            {
                return DataFieldY;
            }
            set
            {
                DataFieldY = value;
            }
        }

        public string SeriesTitle
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

        public double ValueMaximum
        {
            get;
            set;
        }

        public double ValueMinimum
        {
            get;
            set;
        }

        private void UpdateVerticalValueRange(SeqData data)
        {
            double y;
            if (double.TryParse(data.Y, out y)&&y!=Helper.NoData)
            {
                if (y > ValueMaximum)
                    ValueMaximum = y;
                if (y < ValueMinimum)
                    ValueMinimum = y;
            }
        }

        private void ResetVerticalValueRange()
        {
            ValueMaximum = double.MinValue;
            ValueMinimum = double.MaxValue;
        }

        public bool Visible
        {
            get
            {
                return this.IsVisible;
            }
            set
            {
                this.IsVisible = value;
            }
        }

        public void ReverseData()
        {
            List<DataPoint> dps = new List<DataPoint>();
            foreach (DataPoint dp in Points)
            {
                int seq = (int)dp.X;
                seq = seq < 0 ? seq : SumSeq - 1 - seq;
                dps.Add(new DataPoint(seq, dp.Y));
            }

            this.Points.Clear();
            this.Points.AddRange(dps);
        }

        public void Prepare()
        {
            LineStyle = OxyPlot.LineStyle.None; 
        }


        public bool LineSmooth
        {
            get
            {
                return this.Smooth;
            }
            set
            {
                this.Smooth = value;
            }
        }


        public MICAPSDataChart.Model.Themes.ITheme Theme
        {
            get;
            set;
        }

        public override void Render(IRenderContext rc,PlotModel model1)
        {
            PlotModel model = this.PlotModel;
            if (Theme != null)
            {
                OxyColor color = Convertor.ConvertColorToOxyColor(Theme.GetThemeColor());
                this.Color = color;
                this.MarkerStroke = color;
                this.MarkerFill = color;
            }
            base.Render(rc,model1);

            if (this.Visible&&_render_hover)
            {
                RenderCurrentLegend(rc, _mouse_move_point);
            }
        }

        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            if (interpolate)
            {
                // Cannot interpolate if there is no line
                if (this.ActualColor.IsInvisible() || this.StrokeThickness.Equals(0))
                {
                    return null;
                }

                if (!this.CanTrackerInterpolatePoints)
                {
                    return null;
                }
            }

            if (interpolate && this.Smooth && this.SmoothedPoints != null)
            {
                //var result = this.GetNearestInterpolatedPointInternal(this.SmoothedPoints, point);
                //result.Text = this.Format(
                //    this.TrackerFormatString,
                //    result.Item,
                //    this.Title,
                //    XTitle ?? XYAxisSeries.DefaultXAxisTitle,
                //    this.XAxis.GetValue(result.DataPoint.X),
                //    YTitle ?? XYAxisSeries.DefaultYAxisTitle,
                //    this.YAxis.GetValue(result.DataPoint.Y));
            }

            return GetNearestPointSelf(point, interpolate);
        }

        public string XTitle
        {
            get;
            set;
        }

        public string YTitle
        {
            get;
            set;
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
                LineStyle = (LineStyle)(int)value;
            }
        }

        TrackerHitResult GetNearestPointSelf(ScreenPoint point, bool interpolate)
        {
            if (interpolate && !this.CanTrackerInterpolatePoints)
            {
                return null;
            }

            TrackerHitResult result = null;
            if (interpolate)
            {
                result = this.GetNearestInterpolatedPointInternal(this.ActualPoints, point);
            }

            if (result == null)
            {
                result = this.GetNearestPointInternal(this.ActualPoints, point);
            }

            if (result != null)
            {
                //result.Text = this.Format(
                //    this.TrackerFormatString,
                //    result.Item,
                //    this.Title,
                //    XTitle ?? XYAxisSeries.DefaultXAxisTitle,
                //    this.XAxis.GetValue(result.DataPoint.X),
                //    YTitle ?? XYAxisSeries.DefaultYAxisTitle,
                //    this.YAxis.GetValue(result.DataPoint.Y));

                //this.format
            }

            return result;
        }

        public void OnMouseDoubleClick(System.Windows.Forms.MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void OnMouseClick(System.Windows.Forms.MouseEventArgs e)
        {
            ///throw new NotImplementedException();
        }

        protected DataPoint _mouse_move_point = new DataPoint(0,0);
        bool _render_hover = false;
        public bool OnMouseHover(System.Windows.Forms.MouseEventArgs e)
        {
            _render_hover = false;
            if (this.XAxis == null || this.YAxis == null)
                return false;

            for (int i = 0; i < this.Points.Count; i++)
            {
                double x = this.XAxis.Transform(Points[i].X);
                double y = this.YAxis.Transform(Points[i].Y);

                if (Math.Abs(x - e.Location.X) < Distance && Math.Abs(y - e.Location.Y) < Distance)
                {
                    _mouse_move_point = Points[i];
                    _render_hover = true;
                    return true;
                }
            }

            return false;
        }

        void RenderCurrentLegend(IRenderContext rc, DataPoint model)
        {
            if (!(this.XAxis is IGetXLabel))
                return;

            string label = ((IGetXLabel)this.XAxis).GetLabel(model.X);
            if (string.IsNullOrEmpty(label))
                return;

            string text = this.Title + "\n" + label + "  " + FormatString(model.Y.ToString());
            OxySize text_size = rc.MeasureText(text);

            double x = this.XAxis.Transform(model.X);
            double y = this.YAxis.Transform(model.Y);

            OxyRect bound = this.PlotModel.PlotArea;
            if (y - text_size.Height - 2 * TextPadding < bound.Top)
            {

            }
            else
            {
                y = y - text_size.Height - 2 * TextPadding;
            }

            if (x + text_size.Width + 2 * TextPadding > bound.Right)
            {
                x -= text_size.Width + 2 * TextPadding;
            }
            else
            {

            }

            rc.DrawRectangle(new OxyRect(x, y, text_size.Width + 2 * TextPadding, text_size.Height + 2 * TextPadding), OxyColors.LimeGreen, OxyColors.Transparent);/// OxyColor.FromArgb(255,138,187,74)
            rc.DrawText(new ScreenPoint(x + TextPadding, y + TextPadding), text, OxyColors.White);
        }

        string FormatString(string value)
        {
            double num;
            if (double.TryParse(value, out num))
            {
                if (num == (int)num)
                    return num.ToString("f0");
                else if (num > -1 && num < 1)
                    return num.ToString("f2");
                else
                    return num.ToString("f1");
            }
            else
            {
                return value;
            }
        }
    }
}
