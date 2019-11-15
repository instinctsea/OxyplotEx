/*
 * Copyright (c) 2014-2015 jiayongqiang. All rights reserved";
 * CLR version: 4.0.30319.18444"
 * File name:   IVerticalValueRange.cs "
 * Date:        2015/8/10 11:47:48 
 * Author :  jiayongqiang Developer
 * Description: 
	
 * History:  created by jiayongqiang 2015/8/10 11:47:48
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module.MICAPSDataChart.Model.DataSeries
{
    interface IValueRange
    {
        double ValueMaximum { get; set; }
        double ValueMinimum { get; set; }
    }
}
