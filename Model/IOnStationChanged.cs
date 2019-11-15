/*
 * Copyright (c) 2014-2015 jiayongqiang. All rights reserved";
 * CLR version: 4.0.30319.18444"
 * File name:   IOnStationChanged.cs "
 * Date:        2015/8/4 16:24:07 
 * Author :  jiayongqiang Developer
 * Description: 
	
 * History:  created by jiayongqiang 2015/8/4 16:24:07
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model
{
    interface IOnStationChanged
    {
        void OnStationChanged(string stationId);
    }
}
