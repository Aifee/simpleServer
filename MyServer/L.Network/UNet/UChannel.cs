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
using System.Net.Sockets;
using System.Threading.Tasks;

namespace L.Network.UNet
{
    internal class UChannel : Channel
    {
        private readonly USocket socket;

        private TaskCompletionSource<byte[]> recvTcs;

        /// <summary>
        /// connect
        /// </summary>
        public UChannel(USocket _socket, string host, int port, UService _service) : base(_service, ChannelType.Connect)
        {
            socket = _socket;
            service = _service;
            RemoteAddress = host + ":" + port;
            socket.ConnectAsync(host, (ushort)port);
            socket.Received += OnRecv;
            socket.Disconnect += () => { OnError(this, SocketError.SocketError); };
        }

        /// <summary>
        /// accept
        /// </summary>
        public UChannel(USocket _socket, UService _service) : base(_service, ChannelType.Accept)
        {
            socket = _socket;
            service = _service;
            RemoteAddress = socket.RemoteAddress;
            socket.Received += OnRecv;
            socket.Disconnect += () => { OnError(this, SocketError.SocketError); };
        }

        public override void Dispose()
        {
            if (Id == 0)
            {
                return;
            }
            base.Dispose();
            socket.Dispose();
        }

        public override void Send(byte[] buffer, byte channelID = 0, PacketFlags flags = PacketFlags.Reliable)
        {
            if (Id == 0)
            {
                throw new Exception("UChannel已经被Dispose, 不能发送消息");
            }
            socket.SendAsync(buffer, channelID, flags);
        }

        public override void Send(List<byte[]> buffers, byte channelID = 0, PacketFlags flags = PacketFlags.Reliable)
        {
            if (Id == 0)
            {
                throw new Exception("UChannel已经被Dispose, 不能发送消息");
            }
            int size = buffers.Select(b => b.Length).Sum();
            var buffer = new byte[size];
            int index = 0;
            foreach (byte[] bytes in buffers)
            {
                Array.Copy(bytes, 0, buffer, index, bytes.Length);
                index += bytes.Length;
            }
            socket.SendAsync(buffer, channelID, flags);
        }

        public override Task<byte[]> Recv()
        {
            if (Id == 0)
            {
                throw new Exception("UChannel已经被Dispose, 不能接收消息");
            }

            var recvQueue = socket.RecvQueue;
            if (recvQueue.Count > 0)
            {
                return Task.FromResult(recvQueue.Dequeue());
            }

            recvTcs = new TaskCompletionSource<byte[]>();
            return recvTcs.Task;
        }

        private void OnRecv()
        {
            var tcs = recvTcs;
            recvTcs = null;
            tcs.SetResult(socket.RecvQueue.Dequeue());
        }
    }
}