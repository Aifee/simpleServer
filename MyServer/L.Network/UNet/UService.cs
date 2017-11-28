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

namespace L.Network.UNet
{
    public sealed class UService : Service
    {
        private UPoller poller;

        private readonly Dictionary<long, UChannel> idChannels = new Dictionary<long, UChannel>();

        /// <summary>
        /// 即可做client也可做server
        /// </summary>
        public UService(string host, int port)
        {
            poller = new UPoller(host, (ushort)port);
        }

        /// <summary>
        /// 只能做client
        /// </summary>
        public UService()
        {
            poller = new UPoller();
        }

        public override void Dispose()
        {
            if (poller == null)
            {
                return;
            }

            foreach (long id in idChannels.Keys.ToArray())
            {
                UChannel channel = idChannels[id];
                channel.Dispose();
            }

            poller = null;
        }

        public override void Add(Action action)
        {
            poller.Add(action);
        }

        public override async Task<Channel> AcceptChannel()
        {
            USocket socket = await poller.AcceptAsync();
            UChannel channel = new UChannel(socket, this);
            idChannels[channel.Id] = channel;
            return channel;
        }

        public override Channel ConnectChannel(string host, int port)
        {
            USocket newSocket = new USocket(poller);
            UChannel channel = new UChannel(newSocket, host, port, this);
            idChannels[channel.Id] = channel;
            return channel;
        }

        public override Channel GetChannel(long id)
        {
            UChannel channel = null;
            idChannels.TryGetValue(id, out channel);
            return channel;
        }

        public override void Remove(long id)
        {
            UChannel channel;
            if (!idChannels.TryGetValue(id, out channel))
            {
                return;
            }
            if (channel == null)
            {
                return;
            }
            idChannels.Remove(id);
            channel.Dispose();
        }

        public override void Update()
        {
            poller.Update();
        }
    }
}
