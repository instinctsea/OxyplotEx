using OxyPlot;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap
{
    class MapPlotModel:PlotModel
    {
        public Axis GetAxis(string key)
        {
            foreach (Axis axis in this.Axes)
            {
                if (axis is MultyAxes)
                {
                    if (((MultyAxes)axis).ContainsAxis(key))
                        return ((MultyAxes)axis).TryGetAxis(key) as Axis;
                }
                else
                {
                    if (axis.Key == key)
                        return axis;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the axis for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultAxis">The default axis.</param>
        /// <returns>The axis, or the defaultAxis if the key is not found.</returns>
        public new Axis GetAxisOrDefault(string key, Axis defaultAxis)
        {
            foreach (Axis axis in this.Axes)
            {
                if (axis is MultyAxes)
                {
                    if (((MultyAxes)axis).ContainsAxis(key))
                        return ((MultyAxes)axis).TryGetAxis(key) as Axis;
                }
                else
                {
                    if (axis.Key == key)
                        return axis;
                }
            }

            return defaultAxis;
        }
    }
}
