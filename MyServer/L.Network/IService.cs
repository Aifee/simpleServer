/*
 *************************************************************
 * Copyright (c) 2017 - 2019 liuaf
 * Create time：2017/11/8 14:35:32
 * Created by：liuaf
 * Contact information：329737941@qq.com
 **************************************************************
 */

using System;
using System.Threading.Tasks;

namespace L.Network
{
    public interface IService
    {
        void Add(Action action);

        Channel GetChannel(long id);

        Task<Channel> AcceptChannel();

        Channel ConnectChannel(string host, int port);

        void Remove(long channelId);

        void Update();
    }
}