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
using System.Threading.Tasks;

namespace L.Network.UNet
{
    internal sealed class UPoller : IDisposable
    {
        static UPoller()
        {
            Library.Initialize();
        }

        public USocketManager USocketManager { get; private set; }
        private readonly EQueue<IntPtr> connQueue = new EQueue<IntPtr>();

        private IntPtr host;

        // 线程同步队列,发送接收socket回调都放到该队列,由poll线程统一执行
        private EQueue<Action> concurrentQueue = new EQueue<Action>();

        private EQueue<Action> localQueue;
        private readonly object lockObject = new object();

        private ENetEvent eNetEventCache;

        private TaskCompletionSource<USocket> AcceptTcs { get; set; }

        public UPoller(string hostName, ushort port)
        {
            try
            {
                USocketManager = new USocketManager();

                UAddress address = new UAddress(hostName, port);
                ENetAddress nativeAddress = address.Struct;
                host = NativeMethods.enet_host_create(ref nativeAddress,
                        NativeMethods.ENET_PROTOCOL_MAXIMUM_PEER_ID, 0, 0, 0);

                if (host == IntPtr.Zero)
                {
                    throw new Exception("Host creation call failed.");
                }

                NativeMethods.enet_host_compress_with_range_coder(host);
            }
            catch (Exception e)
            {
                throw new Exception("UPoll construct error, address: {hostName}:{port}", e);
            }
        }

        public UPoller()
        {
            USocketManager = new USocketManager();

            host = NativeMethods.enet_host_create(IntPtr.Zero, NativeMethods.ENET_PROTOCOL_MAXIMUM_PEER_ID, 0, 0, 0);

            if (host == IntPtr.Zero)
            {
                throw new Exception("Host creation call failed.");
            }

            NativeMethods.enet_host_compress_with_range_coder(host);
        }

        public void Dispose()
        {
            if (host == IntPtr.Zero)
            {
                return;
            }

            NativeMethods.enet_host_destroy(host);

            host = IntPtr.Zero;
        }

        public IntPtr Host
        {
            get
            {
                return host;
            }
        }

        public void Flush()
        {
            NativeMethods.enet_host_flush(host);
        }

        public void Add(Action action)
        {
            lock (lockObject)
            {
                concurrentQueue.Enqueue(action);
            }
        }

        public Task<USocket> AcceptAsync()
        {
            if (AcceptTcs != null)
            {
                throw new Exception("do not accept twice!");
            }

            // 如果有请求连接缓存的包,从缓存中取
            if (connQueue.Count > 0)
            {
                IntPtr ptr = connQueue.Dequeue();

                USocket socket = new USocket(ptr, this);
                USocketManager.Add(ptr, socket);
                return Task.FromResult(socket);
            }

            AcceptTcs = new TaskCompletionSource<USocket>();
            return AcceptTcs.Task;
        }

        private void OnAccepted(ENetEvent eEvent)
        {
            if (eEvent.Type == EventType.Disconnect)
            {
                AcceptTcs.TrySetException(new Exception("socket disconnected in accpet"));
            }

            USocket socket = new USocket(eEvent.Peer, this);
            USocketManager.Add(socket.PeerPtr, socket);
            socket.OnAccepted();

            var tcs = AcceptTcs;
            AcceptTcs = null;
            tcs.SetResult(socket);
        }

        private void OnEvents()
        {
            lock (lockObject)
            {
                localQueue = concurrentQueue;
                concurrentQueue = new EQueue<Action>();
            }

            while (localQueue.Count > 0)
            {
                Action a = localQueue.Dequeue();
                a();
            }
        }

        private int Service()
        {
            int ret = NativeMethods.enet_host_service(host, IntPtr.Zero, 0);
            return ret;
        }

        public void Update()
        {
            OnEvents();

            if (Service() < 0)
            {
                return;
            }

            while (true)
            {
                if (NativeMethods.enet_host_check_events(host, ref eNetEventCache) <= 0)
                {
                    return;
                }

                switch (eNetEventCache.Type)
                {
                    case EventType.Connect:
                        {
                            // 这是一个connect peer
                            if (USocketManager.ContainsKey(eNetEventCache.Peer))
                            {
                                USocket uSocket = USocketManager[eNetEventCache.Peer];
                                uSocket.OnConnected();
                                break;
                            }

                            // 这是accept peer
                            if (AcceptTcs != null)
                            {
                                OnAccepted(eNetEventCache);
                                break;
                            }

                            // 如果server端没有acceptasync,则请求放入队列
                            connQueue.Enqueue(eNetEventCache.Peer);
                            break;
                        }
                    case EventType.Receive:
                        {
                            USocket uSocket = USocketManager[eNetEventCache.Peer];
                            uSocket.OnReceived(eNetEventCache);
                            break;
                        }
                    case EventType.Disconnect:
                        {
                            USocket uSocket = USocketManager[eNetEventCache.Peer];
                            USocketManager.Remove(uSocket.PeerPtr);
                            uSocket.PeerPtr = IntPtr.Zero;
                            uSocket.OnDisconnect(eNetEventCache);
                            break;
                        }
                }
            }
        }
    }
}