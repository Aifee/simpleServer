/*
 *************************************************************
 * Copyright (c) 2017 - 2019 liuaf
 * Create time：2017/11/8 14:35:32
 * Created by：liuaf
 * Contact information：329737941@qq.com
 **************************************************************
 */

using System.Collections;
using System.Collections.Generic;

namespace L.Utility
{
    public class EQueue<T> : IEnumerable
    {
        private readonly LinkedList<T> list = new LinkedList<T>();

        public void Enqueue(T t)
        {
            this.list.AddLast(t);
        }

        public T Dequeue()
        {
            T t = this.list.First.Value;
            this.list.RemoveFirst();
            return t;
        }

        public int Count
        {
            get
            {
                return this.list.Count;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        public void Clear()
        {
            this.list.Clear();
        }
    }
}