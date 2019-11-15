/*
 * Copyright (c) 2014-2015 jiayongqiang. All rights reserved";
 * CLR version: 4.0.30319.18444"
 * File name:   IDatapairScheduler.cs "
 * Date:        2015/8/6 10:55:09 
 * Author :  jiayongqiang Developer
 * Description: 
	
 * History:  created by jiayongqiang 2015/8/6 10:55:09
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model.DataSeries
{
    public interface ISeqDataScheduler
    {
        int SumSeq { get; set; }
        void AddSeqData(SeqData data);
        void AddRange(IEnumerable<SeqData> datas);
        void Clear();
    }
}
