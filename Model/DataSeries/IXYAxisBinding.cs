/*
 * Copyright (c) 2014-2015 jiayongqiang. All rights reserved";
 * CLR version: 4.0.30319.18444"
 * File name:   IXYAxisBingding.cs "
 * Date:        2015/8/5 11:25:28 
 * Author :  jiayongqiang Developer
 * Description: 
	
 * History:  created by jiayongqiang 2015/8/5 11:25:28
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model.DataSeries
{
    public interface IXYAxisBinding
    {
        string XAxisBingdingKey { get; set; }
        string YAxisBingdingKey { get; set; }
    }
}
