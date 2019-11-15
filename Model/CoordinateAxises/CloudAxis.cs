using OxyplotEx.Model;
using OxyplotEx.Model.Themes;
using OxyplotEx.Model.DataSeries;
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
using OxyplotEx.GMap;
using OxyplotEx.GMap.Theme;
using OxyplotEx.Model.TimeSeries;
using OxyplotEx.Model.Styles;

namespace OxyplotEx.Model.CoordinateAxises
{
    public class CloudAxis : CategoryAxis, IAxis, IAxisAddSeqData, IInverseData, IGetXLabel
    {
        private const float FontsizeFactor = 0.8f;
        private PrivateFontCollection _PrivateFontCollection;
        double step = 0;
        private Dictionary<string, Dictionary<int, SeqData>> _element_value_mapping = new Dictionary<string, Dictionary<int, SeqData>>();
        private Dictionary<string, bool> _element_visible_mapping = new Dictionary<string, bool>();
        private Dictionary<string, PointF> _element_offsets = new Dictionary<string, PointF>();
        private string[] Value_Elements = new string[] { ElementNames.AxisTemp, ElementNames.AxisPressure, ElementNames.Visible, ElementNames.Var3Pressure };
        private string[] Font_Elements = new string[] { ElementNames.Cloud, ElementNames.Wind, ElementNames.WW, ElementNames.CH, ElementNames.CM, ElementNames.CL };
        private Dictionary<string, ITheme> _thems = new Dictionary<string, ITheme>();
        public CloudAxis(PrivateFontCollection privatefont)
            : base()
        {
            _PrivateFontCollection = privatefont;
            _cloud = new Dictionary<int, SeqData>();
            _wind_speeds = new Dictionary<int, SeqData>();
            _tempers = new Dictionary<int, SeqData>();
            _pressures = new Dictionary<int, SeqData>();
            _visibles = new Dictionary<int, SeqData>();
            _var3_pressures = new Dictionary<int, SeqData>();
            _ww = new Dictionary<int, SeqData>();
            _cl = new Dictionary<int, SeqData>();
            _cm = new Dictionary<int, SeqData>();
            _ch = new Dictionary<int, SeqData>();

            _element_value_mapping.Add(ElementNames.Cloud, Cloud);
            _element_value_mapping.Add(ElementNames.Wind, WindSpeeds);
            _element_value_mapping.Add(ElementNames.AxisTemp, Tempers);
            _element_value_mapping.Add(ElementNames.AxisPressure, Pressures);
            _element_value_mapping.Add(ElementNames.Visible, Visibles);
            _element_value_mapping.Add(ElementNames.Var3Pressure, Var3Pressures);
            _element_value_mapping.Add(ElementNames.WW, WW);
            _element_value_mapping.Add(ElementNames.CL, CL);
            _element_value_mapping.Add(ElementNames.CM, CM);
            _element_value_mapping.Add(ElementNames.CH, CH);

            _element_visible_mapping.Add(ElementNames.Cloud, true);
            _element_visible_mapping.Add(ElementNames.Wind, true);
            _element_visible_mapping.Add(ElementNames.AxisTemp, true);
            _element_visible_mapping.Add(ElementNames.AxisPressure, true);
            _element_visible_mapping.Add(ElementNames.Visible, true);
            _element_visible_mapping.Add(ElementNames.Var3Pressure, true);
            _element_visible_mapping.Add(ElementNames.WW, true);
            _element_visible_mapping.Add(ElementNames.CL, true);
            _element_visible_mapping.Add(ElementNames.CM, true);
            _element_visible_mapping.Add(ElementNames.CH, true);

            Theme = new Model.Themes.ColorTheme();
        }

        private Dictionary<int, SeqData> _cloud;
        public Dictionary<int, SeqData> Cloud
        {
            get { return _cloud; }
        }

        private Dictionary<int, SeqData> _wind_speeds;
        public Dictionary<int, SeqData> WindSpeeds
        {
            get { return _wind_speeds; }
        }

        private Dictionary<int, SeqData> _tempers;
        public Dictionary<int, SeqData> Tempers
        {
            get { return _tempers; }
        }

        private Dictionary<int, SeqData> _pressures;
        public Dictionary<int, SeqData> Pressures
        {
            get { return _pressures; }
        }

        private Dictionary<int, SeqData> _visibles;
        public Dictionary<int, SeqData> Visibles
        {
            get { return _visibles; }
        }

        private Dictionary<int, SeqData> _var3_pressures;
        public Dictionary<int, SeqData> Var3Pressures
        {
            get { return _var3_pressures; }
        }

        private Dictionary<int, SeqData> _ww;
        public Dictionary<int, SeqData> WW
        {
            get { return _ww; }
        }

        private Dictionary<int, SeqData> _cl;
        public Dictionary<int, SeqData> CL
        {
            get { return _cl; }
        }

        private Dictionary<int, SeqData> _cm;
        public Dictionary<int, SeqData> CM
        {
            get { return _cm; }
        }

        private Dictionary<int, SeqData> _ch;
        public Dictionary<int, SeqData> CH
        {
            get { return _ch; }
        }

        public int WindSize
        {
            get;
            set;
        }

        public int CloundSize
        {
            get;
            set;
        }

        public ITheme Theme
        {
            get;
            set;
        }

        public override void Render(OxyPlot.IRenderContext rc, PlotModel model1, AxisLayer axisLayer, int pass)
        {
#if DEBUG_MAP
            Console.WriteLine("render clound axis " + this.Name);
#endif
            Render(rc, model1);
        }

        public override void Pan(double delta)
        {
            base.Pan(delta);
            step = step + delta;
        }

        void DrawSymbol(Graphics g,
            Dictionary<int, SeqData> element_seq,
            string name,
            int font_index,
            float font_size = 12F,
            HorizontalAlignment halign = HorizontalAlignment.Center,
            VerticalAlignment valign = VerticalAlignment.Middle,
            int offset_x = 0,
            int offset_y = 0)
        {
            ITheme theme;
            if (!_thems.TryGetValue(name, out theme))
                theme = new Model.Themes.ColorTheme();
            Color color = theme.GetThemeColor(ThemeMode);
            using (Brush brush = new SolidBrush(color))
            {
                bool visible = true;
                if (_element_visible_mapping.ContainsKey(name))
                    visible = _element_visible_mapping[name];

                if (visible && element_seq.Count > 0)
                {
                    foreach (KeyValuePair<int, SeqData> item in element_seq)
                    {
                        if (item.Value != null &&
                        _PrivateFontCollection.Families.Length > font_index &&
                        item.Value.Angle != Helper.InvalidData&&
                            item.Value.Y!="9999")
                        {
                            using (Font f = new System.Drawing.Font(_PrivateFontCollection.Families[font_index], font_size))
                            {
                                float x = (float)this.Transform(item.Key);
                                float y = (float)PlotModel.Height-50;

                                PointF offset;
                                if (!_element_offsets.TryGetValue(name, out offset))
                                    offset = new PointF(0, 0);

                                GMap.FontLabelSeries.DrawText(g, new ScreenPoint(x, y), item.Value.Y,
                                  brush, f, item.Value.Angle,
                                  halign, valign, (int)offset.X + offset_x, (int)offset.Y + offset_y);
                            }
                        }
                    }
                    
                }
            }
        }

        AxisTheme IAxis.Theme
        {
            get;set;
        }

        public bool AxisVisible
        {
            get
            {
                return IsAxisVisible;
            }

            set
            {
                IsAxisVisible=value;
            }
        }

        public string Name
        {
            get;set;
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

        private OxyRect _bound;
        public OxyRect Bound
        {
            get
            {
                return _bound;
            }
        }

        private OxyRect _label_bound;
        public OxyRect LabelBound
        {
            get
            {
                return _label_bound;
            }
        }

        public bool GridLineVisible
        {
            get;set;
        }

        public eThemeMode ThemeMode
        {
            get;set;
        }

        public void AddElement(string elementName, SeqData data)
        {
            Dictionary<int, SeqData> values;
            if (_element_value_mapping.TryGetValue(elementName, out values))
            {
                values[data.Seq] = data;
            }
        }

        public void SetElementVisible(string elementName, bool visible)
        {
            if (string.IsNullOrEmpty(elementName))
                return;

            _element_visible_mapping[elementName] = visible;
        }

        public void SetElementColor(string elementName, Color color)
        {
            if (_thems.ContainsKey(elementName))
            {
                _thems[elementName].SetThemeColor(ThemeMode,color);
            }
        }

        public void SetElementOffset(string elementName, PointF offset)
        {
            _element_offsets[elementName] = offset;
        }

        public void SetTheme(string elementName, ITheme theme)
        {
            _thems[elementName] = theme;
        }

        private void ReverseDictionary(ref Dictionary<int, SeqData> source, int sumSeq)
        {
            Dictionary<int, SeqData> target = new Dictionary<int, SeqData>();
            foreach (KeyValuePair<int, SeqData> pair in source)
            {
                int seq = pair.Key;
                seq = seq < 0 ? seq : sumSeq - 1 - seq;
                target[seq] = pair.Value;
            }

            source = target;
        }

        public string GetLabel(double x)
        {
            int index = (int)x;
            if (index < this.Labels.Count)
                return this.Labels[index];

            return null;
        }

        public void UpdateBound(BoundParameter bound)
        {
            _bound = bound.RegionBound;
            _label_bound = bound.LabelBound;
        }

        public void UpdateTransform()
        {
            /////throw new NotImplementedException();
        }

        public void UpdateRange(double minimum, double maximum)
        {
            ///////throw new NotImplementedException();
        }

        public void Render(IRenderContext rc, PlotModel model)
        {
            if (Theme != null)
            {
                OxyColor color = Convertor.ConvertColorToOxyColor(Theme.GetThemeColor(ThemeMode));
                TicklineColor = color;
                TextColor = color;
                TitleColor = color;
            }
            ///base.Render(rc, model, axisLayer, pass);
            /// //绘制线标值和线条
            IList<IList<ScreenPoint>> points = new List<IList<ScreenPoint>>();
            FeatureTextIntersector intersector = new FeatureTextIntersector(FeatureTextIntersector.SortStyle.Horizontal, 3);
            for (int i = 0; i < this.Labels.Count; i++)
            {
                string label = this.Labels[i];
                OxySize text_size = rc.MeasureText(label, this.ActualFont, this.ActualFontSize, this.ActualFontWeight);

                double x = Transform(i);
                if (x < model.PlotArea.Left || x > model.PlotArea.Right)
                    continue;

                double y = model.PlotArea.Bottom;
                double y2 = y + 5;

                IList<ScreenPoint> sps = new List<ScreenPoint>();
                sps.Add(new ScreenPoint(x, y));
                sps.Add(new ScreenPoint(x, model.PlotArea.Top));

                points.Add(sps);
                intersector.Add(new FeatureText(label, new ScreenPoint(x, y2), text_size,new ScreenPoint(x,y)));

                // rc.DrawText(new ScreenPoint(x, y + 7), label, TextColor, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, this.Angle, HorizontalAlignment.Left, VerticalAlignment.Middle);
            }

            List<FeatureText> fearures = intersector.DiscaredIntersection();
            if (fearures != null)
            {
                foreach (FeatureText ft in fearures)
                {
                    rc.DrawText(ft.Position, ft.Text, this.TextColor, this.ActualFont, this.ActualFontSize, this.ActualFontWeight, this.Angle, HorizontalAlignment.Center);
                }
            }

            foreach (IList<ScreenPoint> line in points)
            {
                rc.DrawLine(line, this.ExtraGridlineColor, 1, LineStyle.Dash.GetDashArray());
            }
            if (model.Series.Count == 0)
                return;


            var field = rc.GetType().GetField("g", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            object o = field.GetValue(rc);
            Graphics g = (Graphics)o;         

            foreach (string element in Font_Elements)
            {
                Dictionary<int, SeqData> datas;
                if (_element_value_mapping.TryGetValue(element, out datas))
                {
                    PointF offset;
                    if (!_element_offsets.TryGetValue(element, out offset))
                        offset = new PointF(0, 0);

                    switch (element)
                    {
                        case ElementNames.Wind:
                            offset = new PointF(-15, 15);
                            DrawSymbol(g, datas, element, 1, 30F, HorizontalAlignment.Left, VerticalAlignment.Bottom, (int)offset.X, (int)offset.Y);
                            break;
                        default:
                            DrawSymbol(g, datas, element, 2);
                            break;
                    }
                }
            }
            foreach (string element in Value_Elements)
            {
                Dictionary<int, SeqData> current_points;
                bool visible = true;
                if (_element_visible_mapping.ContainsKey(element))
                    visible = _element_visible_mapping[element];

                if (visible && _element_value_mapping.TryGetValue(element, out current_points))
                {
                    PointF offset;
                    if (!_element_offsets.TryGetValue(element, out offset))
                        offset = new PointF(0, 0);
                    double y = model.Height - 50;
                    y += offset.Y;
                    ITheme theme;
                    if (!_thems.TryGetValue(element, out theme))
                        theme = new Model.Themes.ColorTheme();
                    OxyColor color = Helper.ConvertColorToOxyColor(theme.GetThemeColor(ThemeMode));
                    foreach (KeyValuePair<int, SeqData> item in current_points)
                    {
                        double x = Transform(item.Key);
                        x += offset.X;

                        string data = item.Value.Y;
                        rc.DrawText(new ScreenPoint(x, y), data, color);
                    }
                }
            }
        }

        public void InverseData()
        {
            this.Labels.Reverse();

            int sum_seq = this.Labels.Count;
            Dictionary<string, Dictionary<int, SeqData>> ret = new Dictionary<string, Dictionary<int, SeqData>>();
            foreach (KeyValuePair<string, Dictionary<int, SeqData>> item in _element_value_mapping)
            {
                Dictionary<int, SeqData> cur = item.Value;
                ReverseDictionary(ref cur, sum_seq);
                ret[item.Key] = cur;
            }

            _element_value_mapping = ret;
        }        

          
        string FindValue(double x,List<SeqData> datas)
        {
            if(datas==null)
                return Helper.InvalidDataStr;
            for (int i = 0; i < datas.Count; i++)
            {
                if (datas[i].X == x)
                    return datas[i].Y;
            }

            return Helper.InvalidDataStr;
        }

        SeqData FindPoint(double x, List<SeqData> datas)
        {
            if (datas == null)
                return null;
            for (int i = 0; i < datas.Count; i++)
            {
                if (datas[i].X == x)
                    return datas[i];
            }

            return null;
        }

        List<SeqData> ToList(Dictionary<int, SeqData> datas) 
        {
            if (datas == null)
                return null;

            return datas.Values.ToList<SeqData>();
        }
    }
}
