using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OxyPlot;
using OxyPlot.Axes;
using OxyplotEx.Model;
using OxyplotEx.GMap.Theme;

namespace OxyplotEx.GMap
{
    class TlogpAxis:LineRegionAxis
    {
        public TlogpAxis():base()
        {
            this.MinVisible = 0;
        }
        public override void Render(IRenderContext rc, PlotModel model)
        {
            base.Render(rc, model);
        }

        public override double Transform(double x)
        {
            ////this.Minimum = 0;
            if (Minimum == Maximum)
                return Bound.Top;
            //double _ky = (float)(Bound.Height / (Math.Log(Maximum) - Math.Log(Minimum)));

            //return Bound.Top + _ky * (float)(Math.Log(Minimum) - Math.Log(x));

            return Bound.Top + Bound.Height * (x - Minimum) / (Maximum - Minimum);
        }

        protected override void UpdateTransform2(OxyRect bound)
        {
            double x0 = bound.Left;
            double x1 = bound.Right;
            double y0 = bound.Bottom;
            double y1 = bound.Top;

            this.ScreenMin = new ScreenPoint(x0, y1);
            this.ScreenMax = new ScreenPoint(x1, y0);

            double a0 = this.IsHorizontal() ? x0 : y0;
            double a1 = this.IsHorizontal() ? x1 : y1;

            double dx = a1 - a0;
            a1 = a0 + (this.EndPosition * dx);
            a0 = a0 + (this.StartPosition * dx);
            this.ScreenMin = new ScreenPoint(a0, a1);
            this.ScreenMax = new ScreenPoint(a1, a0);

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
    }
}
