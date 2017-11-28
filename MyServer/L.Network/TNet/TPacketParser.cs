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

namespace L.Network.TNet
{
    /// <summary>
    /// TCP 包解析
    /// </summary>
    internal class TPacketParser
    {
        private readonly TBuffer buffer;

        private ushort packetSize;
        private readonly byte[] packetSizeBuffer = new byte[2];
        private TParserState state;
        private byte[] packet;
        private bool isOK;

        public TPacketParser(TBuffer _buffer)
        {
            buffer = _buffer;
        }

        private void Parse()
        {
            if (isOK)
            {
                return;
            }

            bool finish = false;
            while (!finish)
            {
                switch (state)
                {
                    case TParserState.PacketSize:
                        if (buffer.Count < 2)
                        {
                            finish = true;
                        }
                        else
                        {
                            buffer.RecvFrom(packetSizeBuffer);
                            packetSize = BitConverter.ToUInt16(packetSizeBuffer, 0);
                            packetSize = NetworkExtension.NetworkToHostOrder(packetSize);
                            if (packetSize > 60000)
                            {
                                throw new Exception("packet too large, size: {this.packetSize}");
                            }
                            state = TParserState.PacketBody;
                        }
                        break;

                    case TParserState.PacketBody:
                        if (buffer.Count < packetSize)
                        {
                            finish = true;
                        }
                        else
                        {
                            packet = new byte[packetSize];
                            buffer.RecvFrom(packet);
                            isOK = true;
                            state = TParserState.PacketSize;
                            finish = true;
                        }
                        break;
                }
            }
        }

        public byte[] GetPacket()
        {
            Parse();
            if (!isOK)
            {
                return null;
            }
            byte[] result = packet;
            isOK = false;
            return result;
        }
    }
}