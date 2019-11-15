using OxyPlot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model
{
    class FeatureTextIntersector : IEnumerable<FeatureText>
    {
        public enum SortStyle
        {
            Horizontal,
            Vertical
        }
        SortStyle _style;
        List<FeatureText> _features = new List<FeatureText>();
        int _padding;
        public FeatureTextIntersector(SortStyle style = SortStyle.Vertical,int padding=0)
        {
            _style = style;
            _padding = padding;
        }

        public int Count
        {
            get { return _features.Count; }
        }

        public FeatureText this[int index]
        {
            get { return _features[index]; }
        }

        public void Add(FeatureText feature)
        {
            _features.Add(feature);
        }

        public List<FeatureText> DiscaredIntersection(double angle=0)
        {
            if (_features.Count <= 1)
                return _features;

            switch (_style)
            {
                case SortStyle.Vertical:
                    _features.Sort((left, right) =>
                    {
                        if (left.Position.Y > right.Position.Y)
                            return 1;
                        else if (left.Position.Y == right.Position.Y)
                            return 0;
                        else
                            return -1;
                    });
                    break;
                case SortStyle.Horizontal:
                    _features.Sort((left, right) =>
                    {
                        if (left.Position.X > right.Position.X)
                            return 1;
                        else if (left.Position.X == right.Position.X)
                            return 0;
                        else
                            return -1;
                    });
                    break;
            }

            List<FeatureText> ret = new List<FeatureText>();
            ret.Add(_features[0]);

            for (int i = 1; i < _features.Count; i++)
            {
                OxyRect pre = CreateRect(ret[ret.Count - 1].Position, ret[ret.Count - 1].Size,angle);
                OxyRect cur = CreateRect(_features[i].Position, _features[i].Size,angle);

                if (Intersect(cur, pre))
                    continue;

                ret.Add(_features[i]);
            }

            return ret;
        }      

        public IEnumerator<FeatureText> GetEnumerator()
        {
            return _features.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _features.GetEnumerator();
        }

        bool Intersect(OxyRect rect1, OxyRect rect2)
        {
            ScreenPoint left_top = new ScreenPoint(rect1.Left, rect1.Top);
            ScreenPoint right_top = new ScreenPoint(rect1.Right, rect1.Top);
            ScreenPoint left_bottom = new ScreenPoint(rect1.Left, rect1.Bottom);
            ScreenPoint right_bottom = new ScreenPoint(rect1.Right, rect1.Bottom);

            return ContainsPoint(rect2, left_top) || ContainsPoint(rect2, right_top) ||
                ContainsPoint(rect2, left_bottom) || ContainsPoint(rect2, right_bottom);
        }

        bool ContainsPoint(OxyRect rect, ScreenPoint sp)
        {
            return (sp.X >= rect.Left && sp.X <= rect.Right) &&
                (sp.Y >= rect.Top && sp.Y <= rect.Bottom);
        }

        OxyRect CreateRect(ScreenPoint sp, OxySize size,double angle)
        {
            double width = size.Width + 2 * _padding;
            double height = size.Height + 2 * _padding;

            double x2 = sp.X + Math.Cos(angle) * width;
            double y2 = sp.Y + width * Math.Sin(angle);

            double x3 = sp.X - height * Math.Sin(angle);
            double y3 = sp.Y + height * Math.Cos(angle);

            double x4 = x2 - height * Math.Sin(angle);
            double y4 = y2 + height * Math.Cos(angle);

            double left, right, top, bottom;
            GetMinMax(out left, out right, sp.X, x2, x3, x4);
            GetMinMax(out top, out bottom, sp.Y, y2, y3, y4);

            return new OxyRect(left,top,right-left,bottom-top);
        }

        public void GetMinMax(out double min,out double max, params double[] values)
        {
            min = double.MaxValue;
            max = double.MinValue;

            foreach (double value in values)
            {
                if (min > value)
                    min = value;

                if (max < value)
                    max = value;
            }
        }
    }
}
