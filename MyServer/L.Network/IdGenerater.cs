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

namespace L.Network
{
    /// <summary>
    /// ID 生成器，进行侦同步中的侦校验工作
    /// </summary>
    public class IdGenerater
    {
        public static long AppId { private get; set; }

        private static ushort value;

        public static long GenerateId()
        {
            long time = L.Utility.TimeExtension.ClientNowSeconds();

            return (AppId << 48) + (time << 16) + ++value;
        }
    }
}
