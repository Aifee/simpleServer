/*
 *************************************************************
 * Copyright (c) 2017 - 2019 liuaf
 * Create time：2017/11/8 14:35:32
 * Created by：liuaf
 * Contact information：329737941@qq.com
 **************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.Network
{
    public abstract class Service : IService, IDisposable
    {
        /// <summary>
		/// 将函数调用加入IService线程
		/// </summary>
		/// <param name="action"></param>
		public abstract void Add(Action action);

        public abstract Channel GetChannel(long id);

        public abstract Task<Channel> AcceptChannel();

        public abstract Channel ConnectChannel(string host, int port);

        public abstract void Remove(long channelId);

        public abstract void Update();

        public abstract void Dispose();
    }
}
