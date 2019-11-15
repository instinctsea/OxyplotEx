
using OxyplotEx.Model.Themes;
/*
 * Copyright (c) 2014-2015 jiayongqiang. All rights reserved";
 * CLR version: 4.0.30319.18444"
 * File name:   ILineSeriesStyle.cs "
 * Date:        2015/8/5 16:43:51 
 * Author :  jiayongqiang Developer
 * Description: 
	
 * History:  created by jiayongqiang 2015/8/5 16:43:51
*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model.Styles
{
    public interface ILineSeriesStyle:ISeriesStyle
    {
        eMarkerStyle MarkerStyle { get; set; }
        bool LineSmooth { get; set; }
        string XDataField { get; set; }
        string YDataField { get; set; }
    }
}
