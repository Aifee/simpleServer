/*
 *************************************************************
 * Copyright (c) 2017 - 2019 liuaf
 * Create time：2017/11/8 14:35:32
 * Created by：liuaf
 * Contact information：329737941@qq.com
 **************************************************************
 */

using System;
using System.Net;

namespace L.Network.UNet
{
    internal struct UAddress
    {
        private readonly uint ip;
        private readonly ushort port;

        public UAddress(string _host, int _port)
        {
            IPAddress address = IPAddress.Parse(_host);
            ip = BitConverter.ToUInt32(address.GetAddressBytes(), 0);
            port = (ushort)_port;
        }

        public ENetAddress Struct
        {
            get
            {
                ENetAddress address = new ENetAddress { Host = ip, Port = port };
                return address;
            }
        }
    }
}