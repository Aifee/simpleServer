/*
 *************************************************************
 * Copyright (c) 2017 - 2019 liuaf
 * Create time：2017/11/8 14:35:32
 * Created by：liuaf
 * Contact information：329737941@qq.com
 **************************************************************
 */

using System;
using System.Runtime.InteropServices;

namespace L.Network.UNet
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ENetEvent
    {
        public EventType Type;
        public IntPtr Peer;
        public byte ChannelID;
        public uint Data;
        public IntPtr Packet;
    }
}