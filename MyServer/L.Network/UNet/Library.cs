/*
 *************************************************************
 * Copyright (c) 2017 - 2019 liuaf
 * Create time：2017/11/8 14:35:32
 * Created by：liuaf
 * Contact information：329737941@qq.com
 **************************************************************
 */

using System;

namespace L.Network.UNet
{
    internal static class Library
    {
        public static void Initialize()
        {
            int ret = NativeMethods.enet_initialize();
            if (ret < 0)
            {
                throw new Exception("Initialization failed, ret: {ret}");
            }
        }

        public static void Deinitialize()
        {
            NativeMethods.enet_deinitialize();
        }

        public static uint Time
        {
            get
            {
                return NativeMethods.enet_time_get();
            }
            set
            {
                NativeMethods.enet_time_set(value);
            }
        }
    }
}