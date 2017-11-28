/*
 *************************************************************
 * Copyright (c) 2017 - 2019 liuaf
 * Create time：2017/11/8 14:35:32
 * Created by：liuaf
 * Contact information：329737941@qq.com
 **************************************************************
 */

using System;

namespace L.Utility
{
    public static class EnumExtension
    {
        public static int EnumIndex<T>(int value)
        {
            int i = 0;
            foreach (object v in Enum.GetValues(typeof(T)))
            {
                if ((int)v == value)
                {
                    return i;
                }
                ++i;
            }
            return -1;
        }

        public static T FromString<T>(this string str)
        {
            if (!Enum.IsDefined(typeof(T), str))
            {
                return default(T);
            }
            return (T)Enum.Parse(typeof(T), str);
        }
    }
}
