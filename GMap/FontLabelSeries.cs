using OxyPlot;
using OxyPlot.Axes;
using OxyplotEx.GMap.Theme;
using OxyplotEx.Model;
using OxyplotEx.Model.Styles;
using OxyplotEx.Model.TimeSeries;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace OxyplotEx.GMap
{
    class FontLabelSeries : PointLineSeries
    {
        List<FontLabelModel> _points = new List<FontLabelModel>();
        public FontLabelSeries(FontFamily fontFamily,float fontSize):base()
        {
            this.FontFamily = fontFamily;
            this.FontSize = fontSize;

            this._maximum = double.NaN;
            this._minimum = double.NaN;

            LabelVisible = false;
        }
        public override int Count
        {
            get
            {
                return _points.Count;
            }
        }


        public FontFamily FontFamily
        {
            get;set;
        }

        public new float FontSize
        {
            get;set;
        }

        public override Priority Priority
        {
            get
            {
                return  Priority.High;
            }
        }

        public override void AddPoint(PointModel point)
        {
            if(point is FontLabelModel)
            {
                _points.Add(point as FontLabelModel);
            }
            else
            {
                throw new NotSupportedException();
            }
            
        }

        public eThemeMode ThemeMode
        {
            get; set;
        }

        public override void ClearData()
        {
            this._points.Clear();
        }

        public override bool AjustAxis(double sourceMinimum, double SourceMaximum, out double minimum, out double maximum)
        {
            minimum = 0;
            maximum = 0;
            return false;
        }

        public override void Render(IRenderContext rc,PlotModel model1)
        {
            PlotModel model = this.PlotModel;
            Axis y_axis = ((MapPlotModel)model).GetAxis(this.YKey);

            if (!SeriesVisible||!((IAxis)y_axis).AxisVisible)
                return;

            if (Count ==0)
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

            using (Font f = new System.Drawing.Font(FontFamily,FontSize))
            {
                using (Brush brush = new SolidBrush(color))
                {
                    for (int i = 0; i < _points.Count; i++)
                    {
                        double x = this.XAxis.Transform(_points[i].Index);
                        double y = (y_axis.ScreenMax.Y + y_axis.ScreenMin.Y) / 2;
                        if (double.IsNaN(y))
                            continue;
                        ScreenPoint sp = new ScreenPoint(x, y);
                        
                        DrawText(g, new ScreenPoint(x, y), _points[i].Value,
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

        public static void DrawText(
            Graphics g,
            ScreenPoint p,
            string text,
            Brush brush,
            Font font,
            double rotate,
            HorizontalAlignment halign,
            VerticalAlignment valign,
            int offset_x = 0,
            int offset_y = 0,
            OxySize? maxSize = null)
        {
            if (font.Name == "Meteorological")
            {
                var size = g.MeasureString(text, font);
                PointF cur = new PointF((float)p.X, (float)p.Y);
                PointF top = new PointF(cur.X, cur.Y - size.Height);
                g.TranslateTransform(cur.X, cur.Y);
                g.RotateTransform((float)rotate);

                float offset = 5.0f / 20 * font.Size;
                g.DrawString(text, font, brush, 0 - offset, 0 - size.Height + offset);
                g.ResetTransform();
            }
            else
            {
                using (var sf = new StringFormat { Alignment = StringAlignment.Near })
                {
                    var size = g.MeasureString(text, font);
                    if (maxSize != null)
                    {
                        if (size.Width > maxSize.Value.Width)
                        {
                            size.Width = (float)maxSize.Value.Width;
                        }

                        if (size.Height > maxSize.Value.Height)
                        {
                            size.Height = (float)maxSize.Value.Height;
                        }
                    }

                    float dx = 0;
                    if (halign == HorizontalAlignment.Center)
                    {
                        dx = -size.Width / 2;
                    }

                    if (halign == HorizontalAlignment.Right)
                    {
                        dx = -size.Width;
                    }

                    float dy = 0;
                    sf.LineAlignment = StringAlignment.Near;
                    if (valign == VerticalAlignment.Middle)
                    {
                        dy = -size.Height / 2;
                    }

                    if (valign == VerticalAlignment.Bottom)
                    {
                        dy = -size.Height;
                    }
                    g.TranslateTransform((float)p.X, (float)p.Y);
                    if (Math.Abs(rotate) > double.Epsilon)
                    {
                        g.RotateTransform((float)rotate);

                    }

                    g.TranslateTransform(dx + offset_x, dy + offset_y);

                    var layoutRectangle = new RectangleF(0, 0, size.Width + 0.1f, size.Height + 0.1f);
                    g.DrawString(text, font, brush, layoutRectangle, sf);

                    g.ResetTransform();
                }
            }       
        }


        FontLabelModel FindValue(double x)
        {
            for (int i = 0; i < _points.Count; i++)
            {
                if (_points[i].Index == x)
                    return _points[i];
            }

            return null;
        }
    }
}
