/*
 *************************************************************
 * Copyright (c) 2017 - 2019 liuaf
 * Create time：2017/11/8 14:35:32
 * Created by：liuaf
 * Contact information：329737941@qq.com
 **************************************************************
 */

using System;
using System.Threading;

namespace L.Utility
{
    public class TryLock : IDisposable
    {
        private object locked;

        public bool HasLock { get; private set; }

        public TryLock(object obj)
        {
            if (!Monitor.TryEnter(obj))
            {
                return;
            }

            this.HasLock = true;
            this.locked = obj;
        }

        public void Dispose()
        {
            if (!this.HasLock)
            {
                return;
            }

            Monitor.Exit(this.locked);
            this.locked = null;
            this.HasLock = false;
        }
    }
}