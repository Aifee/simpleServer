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
    internal sealed class UPacket : IDisposable
    {
        public UPacket(IntPtr packet)
        {
            PacketPtr = packet;
        }

        public UPacket(byte[] data, PacketFlags flags = PacketFlags.None)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            PacketPtr = NativeMethods.enet_packet_create(data, (uint)data.Length, flags);
            if (PacketPtr == IntPtr.Zero)
            {
                throw new Exception("Packet creation call failed");
            }
        }

        ~UPacket()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (PacketPtr == IntPtr.Zero)
            {
                return;
            }

            NativeMethods.enet_packet_destroy(PacketPtr);
            PacketPtr = IntPtr.Zero;
        }

        private ENetPacket Struct
        {
            get
            {
                return (ENetPacket)Marshal.PtrToStructure(PacketPtr, typeof(ENetPacket));
            }
            set
            {
                Marshal.StructureToPtr(value, PacketPtr, false);
            }
        }

        public IntPtr PacketPtr { get; set; }

        public byte[] Bytes
        {
            get
            {
                ENetPacket enetPacket = Struct;
                var bytes = new byte[(long)enetPacket.DataLength];
                Marshal.Copy(enetPacket.Data, bytes, 0, (int)enetPacket.DataLength);
                return bytes;
            }
        }
    }
}