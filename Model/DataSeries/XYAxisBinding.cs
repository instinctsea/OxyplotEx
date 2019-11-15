/*
 * Copyright (c) 2014-2015 jiayongqiang. All rights reserved";
 * CLR version: 4.0.30319.18444"
 * File name:   XYAxisBinding.cs "
 * Date:        2015/8/5 18:36:18 
 * Author :  jiayongqiang Developer
 * Description: 
	
 * History:  created by jiayongqiang 2015/8/5 18:36:18
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model.DataSeries
{
    class XYAxisBinding:IXYAxisBinding
    {
        public XYAxisBinding(string xKey, string yKey)
        {
            this.XAxisBingdingKey = xKey;
            this.YAxisBingdingKey = yKey;
        }
        public string XAxisBingdingKey
        {
            get;
            set;
        }

        public string YAxisBingdingKey
        {
            get;
            set;
        }
    }
}
