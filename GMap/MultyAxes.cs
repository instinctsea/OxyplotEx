using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Diagnostics;

namespace OxyplotEx.GMap
{
    class MultyAxes:LineRegionAxis,IEnumerable<IAxis>
    {
        List<IAxis> _axes = new List<IAxis>();
        const int RegionPadding = 10;
        public MultyAxes()
        {
            this.Position = AxisPosition.Left;
        }
        public void Add(IAxis axis)
        {
            _axes.Add(axis);
        }

        public bool ContainsAxis(string name)
        {
            return _axes.FirstOrDefault(item => item.Name == name) != null;
        }

        public IAxis TryGetAxis(string name)
        {
            IAxis axis = _axes.FirstOrDefault(item=>item.Name==name);
            return axis;
        }

        public override void UpdateBound(BoundParameter bound)
        {
            //base.UpdateBound(bound);
        }

        public override void Render(IRenderContext rc, PlotModel model1, AxisLayer axisLayer, int pass)
        {
            if (pass==1)
                return;
#if DEBUG_MAP
            Debug.WriteLine("render mul axes " + this.Name);
#endif
            PlotModel model = this.PlotModel;

            for (int i = 0; i < 2; i++)
            {
                bool render = i == 1;
                List<IAxis> left_axes = GetVisibleAxes(AxisPosition.Left);
                List<IAxis> right_axes = GetVisibleAxes(AxisPosition.Right);
                ////List<IAxis> bottom_axes = GetVisibleAxes(AxisPosition.Bottom);
                OxySize left_size = GetAxesSize(rc, left_axes);
                OxySize right_size = GetAxesSize(rc, right_axes);

#if DEBUG_MAP
            Debug.WriteLine(string.Format("left size:{0},{1}",left_size.Width,left_size.Height));
            Debug.WriteLine(string.Format("right size:{0},{1}", right_size.Width, right_size.Height));
#endif

                bool right_grid_visible = true;
                if (left_axes != null && left_axes.Count<IAxis>() > 0)
                    right_grid_visible = false;
                //draw region
                double top = model.PlotArea.Top;
                double bottom = model.PlotArea.Bottom;
                double left_axes_right = model1.PlotAndAxisArea.Right - right_size.Width;
                double left_axes_left = left_size.Width;
                for (int k = 0; k < model.Axes.Count;k++)
                {
                    IMeasureTop measure_top = model.Axes[k] as IMeasureTop;
                    if (measure_top == null)
                        continue;

                    OxySize size = measure_top.MeasureTop(rc, model1);
                    if(render)
                    measure_top.RenderTop(rc, model1, new OxyRect(left_axes_left, top, left_axes_right - left_axes_left, size.Height));
                    top += size.Height;
                }

                

                OxyRect region_bound = new OxyRect(left_axes_left, top, left_axes_right - left_axes_left, bottom - top);
                OxyRect left_label_bound = new OxyRect(model.PlotAndAxisArea.Left, top, left_size.Width, bottom - top);
                RenderAxes(model1, rc, left_axes, region_bound, left_label_bound, RegionPadding,true,render);

                //assign right axes
                /////OxyRect right_region_bound = new OxyRect(left_axes_left, top, left_axes_right - left_axes_left, bottom - top);
                OxyRect right_label_bound = new OxyRect(left_axes_right, top, right_size.Width, bottom - top);
                RenderAxes(model1, rc, right_axes, region_bound, right_label_bound, RegionPadding, right_grid_visible,render);
            }
        }

        void RenderAxes(PlotModel model, IRenderContext rc, IEnumerable<IAxis> axes, OxyRect regionBound, OxyRect labelBound, double padding, bool gridVisible = true,bool render=true)
        {
            if (axes == null || axes.Count<IAxis>() == 0)
                return;

            int count = axes.Count<IAxis>();
            double top = regionBound.Top;
            double cell_height = (regionBound.Height - (count - 1) * padding) / count;
            if (cell_height <= 0)
                return;

            foreach (IAxis item in axes)
            {
                OxyRect region_bound = new OxyRect(regionBound.Left, top,regionBound.Width,cell_height);
                OxyRect label_bound = new OxyRect(labelBound.Left,top,labelBound.Width,cell_height);
                item.UpdateBound(new BoundParameter(region_bound, label_bound));
                top += cell_height + padding;
            }

            foreach (IAxis item in axes)
            {
                List<ISeries> cur_ses = new List<ISeries>();
                foreach (Series se in model.Series)
                {
                    ISeries series = se as ISeries;
                    if (series != null && series.YKey == item.Name)
                    {
                        cur_ses.Add(series);
                    }
                }

                if (cur_ses.Count == 0)
                    continue;

                double minimum = double.MaxValue;
                double maximum = double.MinValue;

                double minim_value = double.MaxValue, maximum_value = double.MinValue;
                bool check = false;

                foreach (ISeries se in cur_ses)
                {
                    if (se is IAjustAxis)
                    {
                        double cur_minimum, cur_maximum;
                        if (((IAjustAxis)se).AjustAxis(minim_value, maximum_value, out cur_minimum, out cur_maximum))
                        {
                            if (minim_value > se.Minimum)
                                minim_value = se.Minimum;
                            if (maximum_value < se.Maximum)
                                maximum_value = se.Maximum;

                            if (minimum > cur_minimum)
                                minimum = cur_minimum;
                            if (maximum < cur_maximum)
                                maximum = cur_maximum;
                            check = true;
                        }
                    }
                }
                if (check == true)
                    item.UpdateRange(minimum, maximum);
                
                item.UpdateTransform();
                item.GridLineVisible = gridVisible;
                if(render)
                item.Render(rc, model);
            }
        }

        public override OxySize Measure(IRenderContext rc)
        {
            OxySize max_size = new OxySize(0, 0);

            foreach (Axis item in _axes)
            {
                OxySize cur = item.Measure(rc);
                if (max_size.Width < cur.Width)
                {
                    max_size = cur;
                }
            }

            return max_size;
        }

        List<IAxis> GetVisibleAxes(AxisPosition position)
        {
            List<IAxis> axes = new List<IAxis>();
            foreach (IAxis axis in _axes)
            {
                if (axis.AxisVisible&&((Axis)axis).Position==position)
                    axes.Add(axis);
            }

            return axes;
        }

        OxySize GetAxesSize(IRenderContext rc, AxisPosition position)
        {
            IEnumerable<IAxis> axes = GetVisibleAxes(position);

            return GetAxesSize(rc,axes);
        }

        OxySize GetAxesSize(IRenderContext rc,IEnumerable<IAxis> axes)
        {
            if (axes == null)
                return new OxySize(0, 0);

            double width = 0, height = 0;
            foreach (IAxis axis in axes)
            {
                Axis cur = axis as Axis;
                OxySize size = cur.Measure(rc);
                if (size.Width > width)
                    width = size.Width;
                if (size.Height > height)
                    height = size.Height;
            }

            return new OxySize(width, height);
        }

        public IEnumerator<IAxis> GetEnumerator()
        {
            return _axes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _axes.GetEnumerator();
        }
    }
}
