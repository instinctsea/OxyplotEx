/*
 * Copyright (c) 2014-2015 jiayongqiang. All rights reserved";
 * CLR version: 4.0.30319.18444"
 * File name:   ISeriesTag.cs "
 * Date:        2015/8/5 16:48:30 
 * Author :  jiayongqiang Developer
 * Description: 
	
 * History:  created by jiayongqiang 2015/8/5 16:48:30
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module.MICAPSDataChart.Model.DataSeries
{
    public interface ISeriesTag
    {
        string Id { get; set; }
        string Name { get; set; }
    }
}
