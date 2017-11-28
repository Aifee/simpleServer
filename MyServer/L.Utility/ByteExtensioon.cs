﻿/*
 *************************************************************
 * Copyright (c) 2017 - 2019 liuaf
 * Create time：2017/11/8 14:35:32
 * Created by：liuaf
 * Contact information：329737941@qq.com
 **************************************************************
 */
 
using System.Text;

namespace L.Utility
{
    public static class ByteExtensioon
    {
        public static string ToHex(this byte b)
        {
            return b.ToString("X2");
        }

        public static string ToHex(this byte[] bytes)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in bytes)
            {
                stringBuilder.Append(b.ToString("X2"));
            }
            return stringBuilder.ToString();
        }

        public static string ToHex(this byte[] bytes, string format)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in bytes)
            {
                stringBuilder.Append(b.ToString(format));
            }
            return stringBuilder.ToString();
        }

        public static string ToHex(this byte[] bytes, int offset, int count)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = offset; i < offset + count; ++i)
            {
                stringBuilder.Append(bytes[i].ToString("X2"));
            }
            return stringBuilder.ToString();
        }

        public static string ToStr(this byte[] bytes)
        {
            return Encoding.Default.GetString(bytes);
        }

        public static string ToStr(this byte[] bytes, int index, int count)
        {
            return Encoding.Default.GetString(bytes, index, count);
        }

        public static string Utf8ToStr(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static string Utf8ToStr(this byte[] bytes, int index, int count)
        {
            return Encoding.UTF8.GetString(bytes, index, count);
        }
    }
}
