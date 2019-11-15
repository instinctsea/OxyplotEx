using OxyPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap
{
    interface IMeasureTop
    {
        OxySize MeasureTop(IRenderContext rc, PlotModel model);
        void RenderTop(IRenderContext rc, PlotModel model,OxyRect rect);
    }
}
