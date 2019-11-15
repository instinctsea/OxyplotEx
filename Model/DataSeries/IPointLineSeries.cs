using OxyPlot;
/*
 * Copyright (c) 2014-2015 jiayongqiang. All rights reserved";
 * CLR version: 4.0.30319.18444"
 * File name:   IAsyncLineSeries.cs "
 * Date:        2015/8/5 10:39:16 
 * Author :  jiayongqiang Developer
 * Description: 
	
 * History:  created by jiayongqiang 2015/8/5 10:39:16
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module.MICAPSDataChart.Model.DataSeries
{
    /// <summary>
    /// 先绘制点，然后调用perform连成线
    /// </summary>
    interface IPointLineSeries:IXYAxisBinding,ISeriesTag,ISeqDataScheduler
    {
        string XTitle { get; set; }
        string YTitle { get; set; }
        void Perform();
    }
}
