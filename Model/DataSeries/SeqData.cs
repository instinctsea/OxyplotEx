/*
 * Copyright (c) 2014-2015 jiayongqiang. All rights reserved";
 * CLR version: 4.0.30319.18444"
 * File name:   DataPair.cs "
 * Date:        2015/8/5 10:44:43 
 * Author :  jiayongqiang Developer
 * Description: 
	
 * History:  created by jiayongqiang 2015/8/5 10:44:43
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model.DataSeries
{
    public class SeqData
    {
        private int seq;
        private double x;
        private string y;
        public SeqData(double x, string y,int seq,float angle)
        {
            this.x = x;
            this.y = y;
            this.seq = seq;
            this.Angle = angle;
        }
        
        public double X 
        {
            get { return x; }
            set
            {
                x = value;
            }
        }

        public string RawString
        {
            get;set;
        }

        public string Y 
        { 
            get{return y;}
            set
            {
                y=value;
            }
        }

        public int Seq
        {
            get { return seq; }
            set
            {
                seq = value;
            }
        }

        public float Angle
        {
            get; set;
        }
    }
}
