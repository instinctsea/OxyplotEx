using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap
{
    public struct BoundParameter
    {
        public BoundParameter(OxyRect regionBound, OxyRect labelBound):this()
        {
            RegionBound = regionBound;
            LabelBound = labelBound;
        }

        public OxyRect RegionBound { get; private set; }
        public OxyRect LabelBound { get; private set; }
    }
}
