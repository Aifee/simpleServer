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
using L.Network;
using L.Network.TNet;
using L.Network.UNet;

namespace MyServer
{
    public class ServerTest
    {
        public long Id = 1;
        private Service Service;
        private readonly Dictionary<long, Session> sessions = new Dictionary<long, Session>();

        public void Awake(NetworkProtocol protocol, string host, int port)
        {
            switch (protocol)
            {
                case NetworkProtocol.TCP:
                    this.Service = new TService(host, port);
                    break;
                case NetworkProtocol.UDP:
                    this.Service = new UService(host, port);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.StartAccept();
        }
        private async void StartAccept()
        {
            while (true)
            {
                if (this.Id == 0)
                {
                    return;
                }

                await this.Accept();
            }
        }

        public virtual async Task<Session> Accept()
        {
            Channel channel = await this.Service.AcceptChannel();
            Console.WriteLine("connect client :" + channel.Id);
            Session session = new Session(Id, channel);
            //channel.ErrorCallback += (c, e) => { this.Remove(session.Id); };
            this.sessions.Add(Id, session);
            return session;
        }
        public void Update()
        {
            if (this.Service == null)
            {
                return;
            }
            this.Service.Update();
        }
    }
}
