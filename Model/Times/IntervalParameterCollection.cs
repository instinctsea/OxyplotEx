using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model.Time
{
    /// <summary>
    /// 间隔参数集合
    /// </summary>
    public class IntervalParameterCollection : IEnumerable<IntervalParameter>
    {       
        /// <summary>
        /// ctr.
        /// </summary>
        public IntervalParameterCollection()
        {

        }
        List<IntervalParameter> _parameters = new List<IntervalParameter>();

        /// <summary>
        /// intervalparameter count
        /// </summary>
        public int Count
        {
            get
            {
                return _parameters.Count;
            }
        }

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IntervalParameter this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    return null;

                return _parameters[index];
            }
        }

        /// <summary>
        /// add parameter
        /// </summary>
        /// <param name="parameter"></param>
        public void Add(IntervalParameter parameter)
        {
            this._parameters.Add(parameter);
        }

        /// <summary>
        /// remove at index
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            this._parameters.RemoveAt(index);
        }

        /// <summary>
        /// remove the parameter object
        /// </summary>
        /// <param name="parameter"></param>
        public void Remove(IntervalParameter parameter)
        {
            this._parameters.Remove(parameter);
        }

        /// <summary>
        /// clear all parameters
        /// </summary>
        public void Clear()
        {
            this._parameters.Clear();
        }

        /// <summary>
        /// 桉顺序升序排列
        /// </summary>
        public void Sort()
        {
            _parameters.Sort((l, r) =>
            {
                if ((int)l.Style < (int)r.Style)
                    return -1;
                else if ((int)l.Style == (int)r.Style)
                {
                    if (l.Value < r.Value)
                        return -1;
                    else if (l.Value == r.Value)
                        return 0;
                    else
                        return 1;
                }
                else
                    return 1;
            });
        }

        //public static void CompareByIntervalTypeAscIntervalValueAsc(IntervalParameter l,IntervalParameter r)
        //{

        //}
        public bool Contains(IntervalParameter interval)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Equals(interval))
                    return true;
            }

            return false;
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IntervalParameter> GetEnumerator()
        {
            return _parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
