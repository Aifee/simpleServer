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
using System.Net.Sockets;
using System.Threading.Tasks;

namespace L.Network
{
    public interface IChannel
    {
        long Id { get; }
        ChannelType ChannelType { get; }
        string RemoteAddress { get; }

        event Action<Channel, SocketError> ErrorCallback;

        void Send(byte[] buffer, byte channelID = 0, PacketFlags flags = PacketFlags.Reliable);

        void Send(List<byte[]> buffers, byte channelID = 0, PacketFlags flags = PacketFlags.Reliable);

        Task<byte[]> Recv();
    }
}