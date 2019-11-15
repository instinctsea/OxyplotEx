using OxyplotEx.Model.DataSeries;
/*
 * Copyright (c) 2014-2015 jiayongqiang. All rights reserved";
 * CLR version: 4.0.30319.18444"
 * File name:   ElementAxisBingdingKeyMapping.cs "
 * Date:        2015/8/5 18:35:42 
 * Author :  jiayongqiang Developer
 * Description: 
	
 * History:  created by jiayongqiang 2015/8/5 18:35:42
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model
{
    public class ElementAxisBingdingKeyMapping
    {
        public static IXYAxisBinding CreateXYBinding(string element, string yKey)
        {
            return new XYAxisBinding(element, yKey);
        }    
    }
}
