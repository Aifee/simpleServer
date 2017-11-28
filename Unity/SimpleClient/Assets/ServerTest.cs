using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L.Network;
using L.Network.TNet;
using L.Network.UNet;
using System;
using System.Threading.Tasks;

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
                this.Service = new TService();
                
                break;
            case NetworkProtocol.UDP:
                this.Service = new UService();
                
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        this.StartConnect(host, port);
    }
    private void StartConnect(string host, int port)
    {
        Channel channel = Service.ConnectChannel(host, port);
        Session session = new Session(Id, channel);
        //channel.ErrorCallback += (c, e) => { this.Remove(session.Id); };
        this.sessions.Add(Id, session);
    }

    public virtual async Task<Session> Accept()
    {
        Channel channel = await this.Service.AcceptChannel();
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
