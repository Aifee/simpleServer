/*
 *************************************************************
 * Copyright (c) 2017 - 2019 liuaf
 * Create time：2017/11/8 14:35:32
 * Created by：liuaf
 * Contact information：329737941@qq.com
 **************************************************************
 */

using ProtoBuf;
using System;
using System.ComponentModel;
using System.IO;

namespace L.Utility
{
    public class ProtobufExtension
    {
        public static byte[] ToBytes(object message)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(ms, message);
                return ms.ToArray();
            }
        }

        public static T FromBytes<T>(byte[] bytes)
        {
            T t;
            using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                t = Serializer.Deserialize<T>(ms);
            }
            ISupportInitialize iSupportInitialize = t as ISupportInitialize;
            if (iSupportInitialize == null)
            {
                return t;
            }
            iSupportInitialize.EndInit();
            return t;
        }

        public static T FromBytes<T>(byte[] bytes, int index, int length)
        {
            T t;
            using (MemoryStream ms = new MemoryStream(bytes, index, length))
            {
                t = Serializer.Deserialize<T>(ms);
            }
            ISupportInitialize iSupportInitialize = t as ISupportInitialize;
            if (iSupportInitialize == null)
            {
                return t;
            }
            iSupportInitialize.EndInit();
            return t;
        }

        public static object FromBytes(Type type, byte[] bytes)
        {
            object t;
            using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                t = Serializer.NonGeneric.Deserialize(type, ms);
            }
            ISupportInitialize iSupportInitialize = t as ISupportInitialize;
            if (iSupportInitialize == null)
            {
                return t;
            }
            iSupportInitialize.EndInit();
            return t;
        }

        public static object FromBytes(Type type, byte[] bytes, int index, int length)
        {
            object t;
            using (MemoryStream ms = new MemoryStream(bytes, index, length))
            {
                t = Serializer.NonGeneric.Deserialize(type, ms);
            }
            ISupportInitialize iSupportInitialize = t as ISupportInitialize;
            if (iSupportInitialize == null)
            {
                return t;
            }
            iSupportInitialize.EndInit();
            return t;
        }
    }
}