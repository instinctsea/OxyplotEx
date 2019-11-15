using OxyplotEx.Model.DataSeries;
/*
 * Copyright (c) 2014-2015 jiayongqiang. All rights reserved";
 * CLR version: 4.0.30319.18444"
 * File name:   IAxisAddSeqData.cs "
 * Date:        2015/8/6 14:56:16 
 * Author :  jiayongqiang Developer
 * Description: 
	
 * History:  created by jiayongqiang 2015/8/6 14:56:16
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model.CoordinateAxises
{
    interface IAxisAddSeqData
    {
        void AddElement(string elementName, SeqData data);
    }
}
