using OxyplotEx.Model.Styles;
using OxyplotEx.Model.Themes;
/*
 * Copyright (c) 2014-2015 jiayongqiang. All rights reserved";
 * CLR version: 4.0.30319.18444"
 * File name:   IColumnSeriesStyle.cs "
 * Date:        2015/8/10 17:01:40 
 * Author :  jiayongqiang Developer
 * Description: 
	
 * History:  created by jiayongqiang 2015/8/10 17:01:40
*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model.Styles
{
    public interface ISeriesStyle
    {
        Color MainColor { get; set; }
        bool Visible { get; set; }
        string SeriesTitle { get; set; }
        ITheme Theme { get; set; }
        int LineWidth { get; set; }
        eLineStyle SeriesLineStyle { get; set; }
    }
}
