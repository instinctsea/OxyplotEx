using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;

namespace Module.TimeSeriesPlot.Model
{
    public class CloudAxis : CategoryAxis
    {
        private const float FontsizeFactor = 0.8f;
        private PrivateFontCollection _PrivateFontCollection;
        double step = 0;

        public CloudAxis(PrivateFontCollection privatefont)
        {
            _PrivateFontCollection = privatefont;
        }

        public Dictionary<int, string> Cloud
        {
            get;
            set;
        }

        public Dictionary<int, string> WindSpeeds
        {
            get;
            set;
        }

        public Dictionary<int, string> WindDirs
        {
            get;
            set;
        }

        public Color CloudColor
        {
            get;
            set;
        }
        public Color WindColor
        {
            get;
            set;
        }

        public bool IsWindVisible
        {
            get;
            set;
        }

        public bool IsCloudVisble
        {
            get;
            set;
        }

        public override void Render(OxyPlot.IRenderContext rc, OxyPlot.PlotModel model, AxisLayer axisLayer, int pass)
        {
            base.Render(rc, model, axisLayer, pass);
            if (model.Series.Count == 0)
                return;

            var field = rc.GetType().GetField("g", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            object o = field.GetValue(rc);
            Graphics g = (Graphics)o;

            

            LineSeries series = model.Series.First(s => { if (s is LineSeries) return true; else return false; }) as LineSeries;
            for (int i = 0; i < series.Points.Count; i++)
            {
                ScreenPoint sp = series.Transform(series.Points[i]);
                int key = (int)series.Points[i].X;
                if (IsCloudVisble && Cloud.Count > 0 && (Cloud[key]) != "9999")
                {
                    if (Cloud[key] != null)
                    {
                        using (Font f = new System.Drawing.Font(_PrivateFontCollection.Families[1], 12F))
                        {
                            float x = (float)sp.X;
                            float y = (float)model.Height - 50;
                            DrawText(g, new ScreenPoint(x, y), Cloud[key],
                              OxyColor.FromRgb(CloudColor.R, CloudColor.G, CloudColor.B), f, 0F,
                              HorizontalAlignment.Center, VerticalAlignment.Middle);
                        }
                    }
                }

                if (IsWindVisible && WindSpeeds.Count > 0 && (WindSpeeds[key]) != "9999"/* && (WindSpeeds[key]) != "0"*/)
                {
                    if (WindDirs[key] != null)
                    {
                        using (Font f = new System.Drawing.Font(_PrivateFontCollection.Families[0], 30F))
                        {
                            float x = (float)sp.X;
                            float y = (float)model.Height - 50;
                            DrawText(g, new ScreenPoint(x, y), WindSpeeds[key],
                                OxyColor.FromRgb(WindColor.R, WindColor.G, WindColor.B), f, float.Parse(WindDirs[key]),
                                HorizontalAlignment.Left, VerticalAlignment.Bottom, -15, 15);
                        }
                    }
                }
            }

        }

        public override void Pan(double delta)
        {
            base.Pan(delta);
            step = step + delta;
        }
        void DrawText(
            Graphics g,
            ScreenPoint p,
            string text,
            OxyColor fill,
            Font font,
            double rotate,
            HorizontalAlignment halign,
            VerticalAlignment valign,
            int offset_x = 0,
            int offset_y = 0,
            OxySize? maxSize = null)
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
                //sf.LineAlignment = StringAlignment.Near;
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
                //g.FillRectangle(new SolidBrush(Color.Red), layoutRectangle);
                g.DrawString(text, font, fill.ToBrush(), layoutRectangle, sf);

                g.ResetTransform();
            }
        }
    }
}
