/*
 *************************************************************
 * Copyright (c) 2017 - 2019 liuaf
 * Create time：2017/11/8 14:35:32
 * Created by：liuaf
 * Contact information：329737941@qq.com
 **************************************************************
 */

using L.Utility;
using System;
using System.Net;
using System.Runtime.InteropServices;

namespace L.Network.UNet
{
    internal sealed class USocket : IDisposable
    {
        private readonly UPoller poller;
        public IntPtr PeerPtr { get; set; }
        private readonly EQueue<byte[]> recvQueue = new EQueue<byte[]>();
        private readonly EQueue<UBuffer> sendQueue = new EQueue<UBuffer>();
        private bool isConnected;
        private Action disconnect;
        private Action received;

        public event Action Received
        {
            add
            {
                received += value;
            }
            remove
            {
                received -= value;
            }
        }

        public event Action Disconnect
        {
            add
            {
                disconnect += value;
            }
            remove
            {
                disconnect -= value;
            }
        }

        public USocket(IntPtr peerPtr, UPoller _poller)
        {
            poller = _poller;
            PeerPtr = peerPtr;
        }

        public USocket(UPoller _poller)
        {
            poller = _poller;
        }

        public void Dispose()
        {
            if (PeerPtr == IntPtr.Zero)
            {
                return;
            }

            poller.USocketManager.Remove(PeerPtr);
            NativeMethods.enet_peer_disconnect_now(PeerPtr, 0);
            PeerPtr = IntPtr.Zero;
        }

        public string RemoteAddress
        {
            get
            {
                ENetPeer peer = Struct;
                IPAddress ipaddr = new IPAddress(peer.Address.Host);
                return "{ipaddr}:{peer.Address.Port}";
            }
        }

        private ENetPeer Struct
        {
            get
            {
                if (PeerPtr == IntPtr.Zero)
                {
                    return new ENetPeer();
                }
                ENetPeer peer = (ENetPeer)Marshal.PtrToStructure(PeerPtr, typeof(ENetPeer));
                return peer;
            }
            set
            {
                Marshal.StructureToPtr(value, PeerPtr, false);
            }
        }

        public EQueue<byte[]> RecvQueue
        {
            get
            {
                return recvQueue;
            }
        }

        public void ConnectAsync(string host, ushort port)
        {
            UAddress address = new UAddress(host, port);
            ENetAddress nativeAddress = address.Struct;

            PeerPtr = NativeMethods.enet_host_connect(poller.Host, ref nativeAddress, 2, 0);
            if (PeerPtr == IntPtr.Zero)
            {
                throw new Exception("host connect call failed, {host}:{port}");
            }
            poller.USocketManager.Add(PeerPtr, this);
        }

        public void SendAsync(byte[] data, byte channelID = 0, PacketFlags flags = PacketFlags.Reliable)
        {
            if (PeerPtr == IntPtr.Zero)
            {
                throw new Exception("USocket 已经被Dispose,不能发送数据!");
            }
            if (!isConnected)
            {
                sendQueue.Enqueue(new UBuffer { Buffer = data, ChannelID = channelID, Flags = flags });
                return;
            }
            UPacket packet = new UPacket(data, flags);
            NativeMethods.enet_peer_send(PeerPtr, channelID, packet.PacketPtr);
            // enet_peer_send函数会自动删除packet,设置为0,防止Dispose或者析构函数再次删除
            packet.PacketPtr = IntPtr.Zero;
        }

        internal void OnConnected()
        {
            isConnected = true;
            while (sendQueue.Count > 0)
            {
                UBuffer info = sendQueue.Dequeue();
                SendAsync(info.Buffer, info.ChannelID, info.Flags);
            }
        }

        internal void OnAccepted()
        {
            isConnected = true;
        }

        internal void OnReceived(ENetEvent eNetEvent)
        {
            // 将包放到缓存队列
            using (UPacket packet = new UPacket(eNetEvent.Packet))
            {
                byte[] bytes = packet.Bytes;
                RecvQueue.Enqueue(bytes);
            }
            received();
        }

        internal void OnDisconnect(ENetEvent eNetEvent)
        {
            disconnect();
        }
    }
}