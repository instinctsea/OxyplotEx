
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap
{
    public static class AxisFactory
    {
        public static IAxis CreateAxis(int axisStyle)
        {
            IAxis axis = null;
            switch (axisStyle)
            {
                case 1:
                    axis = new LineRegionAxis();
                    break;
                case 2:
                    axis = new TlogpAxis();
                    break;
                default:
                    axis = new LineRegionAxis();
                    break;
            }

            return axis;
        }
    }
}
