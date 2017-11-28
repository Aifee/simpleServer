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

namespace L.Network.UNet
{
    internal class USocketManager
    {
        private readonly Dictionary<IntPtr, USocket> sockets = new Dictionary<IntPtr, USocket>();

        public void Add(IntPtr peerPtr, USocket uSocket)
        {
            sockets.Add(peerPtr, uSocket);
        }

        public void Remove(IntPtr peerPtr)
        {
            sockets.Remove(peerPtr);
        }

        public bool ContainsKey(IntPtr peerPtr)
        {
            if (sockets.ContainsKey(peerPtr))
            {
                return true;
            }
            return false;
        }

        public USocket this[IntPtr peerPtr]
        {
            get
            {
                if (!sockets.ContainsKey(peerPtr))
                {
                    throw new KeyNotFoundException("No Peer Key");
                }
                return sockets[peerPtr];
            }
        }
    }
}