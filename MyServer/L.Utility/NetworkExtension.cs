/*
 *************************************************************
 * Copyright (c) 2017 - 2019 liuaf
 * Create time：2017/11/8 14:35:32
 * Created by：liuaf
 * Contact information：329737941@qq.com
 **************************************************************
 */

using System.Net;

namespace L.Utility
{
    public static class NetworkExtension
    {
        public static uint NetworkToHostOrder(uint a)
        {
            return (uint)IPAddress.NetworkToHostOrder((int)a);
        }

        public static ushort NetworkToHostOrder(ushort a)
        {
            return (ushort)IPAddress.NetworkToHostOrder((short)a);
        }

        public static ulong NetworkToHostOrder(ulong a)
        {
            return (ushort)IPAddress.NetworkToHostOrder((long)a);
        }

        public static uint HostToNetworkOrder(uint a)
        {
            return (uint)IPAddress.HostToNetworkOrder((int)a);
        }

        public static ushort HostToNetworkOrder(ushort a)
        {
            return (ushort)IPAddress.HostToNetworkOrder((short)a);
        }

        public static ulong HostToNetworkOrder(ulong a)
        {
            return (ushort)IPAddress.HostToNetworkOrder((long)a);
        }
    }
}