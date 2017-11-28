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
using L.Logger;
using L.Utility;

namespace L.Network.TNet
{
    /// <summary>
    /// TCP缓存
    /// </summary>
    public class TBuffer
    {
        /// <summary>
        /// 最大缓存
        /// </summary>
        public const int ChunkSize = 8192;

        /// <summary>
        /// 缓存列表
        /// </summary>
        private readonly LinkedList<byte[]> bufferList = new LinkedList<byte[]>();

        /// <summary>
        /// 上一次读取位置
        /// </summary>
        public int LastIndex { get; set; }

        /// <summary>
        /// 第一次读取位置
        /// </summary>
        public int FirstIndex { get; set; }

        public TBuffer()
        {
            bufferList.AddLast(new byte[ChunkSize]);
        }

        public int Count
        {
            get
            {
                int c = 0;
                if (bufferList.Count == 0)
                {
                    c = 0;
                }
                else
                {
                    c = (bufferList.Count - 1) * ChunkSize + LastIndex - FirstIndex;
                }
                if (c < 0)
                {
                    Log.Error("TBuffer count < 0: {0}, {1}, {2}".Fmt(bufferList.Count, LastIndex, FirstIndex));
                }
                return c;
            }
        }

        public void AddLast()
        {
            bufferList.AddLast(new byte[ChunkSize]);
        }

        public void RemoveFirst()
        {
            bufferList.RemoveFirst();
        }

        public byte[] First
        {
            get
            {
                if (bufferList.First == null)
                {
                    AddLast();
                }
                return bufferList.First.Value;
            }
        }

        public byte[] Last
        {
            get
            {
                if (bufferList.Last == null)
                {
                    AddLast();
                }
                return bufferList.Last.Value;
            }
        }

        public void RecvFrom(byte[] buffer)
        {
            if (Count < buffer.Length || buffer.Length == 0)
            {
                throw new Exception("bufferList size < n, bufferList: {this.Count} buffer length: {buffer.Length}");
            }
            int alreadyCopyCount = 0;
            while (alreadyCopyCount < buffer.Length)
            {
                int n = buffer.Length - alreadyCopyCount;
                if (ChunkSize - FirstIndex > n)
                {
                    Array.Copy(bufferList.First.Value, FirstIndex, buffer, alreadyCopyCount, n);
                    FirstIndex += n;
                    alreadyCopyCount += n;
                }
                else
                {
                    Array.Copy(bufferList.First.Value, FirstIndex, buffer, alreadyCopyCount, ChunkSize - FirstIndex);
                    alreadyCopyCount += ChunkSize - FirstIndex;
                    FirstIndex = 0;
                    bufferList.RemoveFirst();
                }
            }
        }

        public void SendTo(byte[] buffer)
        {
            int alreadyCopyCount = 0;
            while (alreadyCopyCount < buffer.Length)
            {
                if (LastIndex == ChunkSize)
                {
                    bufferList.AddLast(new byte[ChunkSize]);
                    LastIndex = 0;
                }

                int n = buffer.Length - alreadyCopyCount;
                if (ChunkSize - LastIndex > n)
                {
                    Array.Copy(buffer, alreadyCopyCount, bufferList.Last.Value, LastIndex, n);
                    LastIndex += buffer.Length - alreadyCopyCount;
                    alreadyCopyCount += n;
                }
                else
                {
                    Array.Copy(buffer, alreadyCopyCount, bufferList.Last.Value, LastIndex, ChunkSize - LastIndex);
                    alreadyCopyCount += ChunkSize - LastIndex;
                    LastIndex = ChunkSize;
                }
            }
        }
    }
}
