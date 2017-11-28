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
using System.Net.Sockets;

namespace L.Network
{
    /// <summary>
    /// 通讯管道
    /// TCP 或者 UDP 通讯都要继承此管道
    /// </summary>
    public abstract class Channel : IChannel, IDisposable
    {
        public long Id { get; private set; }

        public ChannelType ChannelType { get; private set; }

        protected Service service;

        public string RemoteAddress { get; protected set; }

        private event Action<Channel, SocketError> errorCallback;

        public event Action<Channel, SocketError> ErrorCallback
        {
            add
            {
                this.errorCallback += value;
            }
            remove
            {
                this.errorCallback -= value;
            }
        }

        protected void OnError(Channel channel, SocketError e)
        {
            this.errorCallback(channel, e);
        }


        protected Channel(Service service, ChannelType channelType)
        {
            this.Id = IdGenerater.GenerateId();
            this.ChannelType = channelType;
            this.service = service;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        public abstract void Send(byte[] buffer, byte channelID = 0, PacketFlags flags = PacketFlags.Reliable);

        public abstract void Send(List<byte[]> buffers, byte channelID = 0, PacketFlags flags = PacketFlags.Reliable);

        /// <summary>
        /// 接收消息
        /// </summary>
        public abstract Task<byte[]> Recv();

        public virtual void Dispose()
        {
            if (this.Id == 0)
            {
                return;
            }

            this.service.Remove(this.Id);

            this.Id = 0;
        }
    }
}
