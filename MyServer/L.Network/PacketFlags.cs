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
    /// 通讯包标记
    /// </summary>
    [Flags]
    public enum PacketFlags
    {
        /// <summary>
        /// 没有标记
        /// </summary>
        None = 0,
        /// <summary>
        /// 可靠的
        /// </summary>
        Reliable = 1 << 0,
        /// <summary>
        /// 乱序
        /// </summary>
        Unsequenced = 1 << 1,
        /// <summary>
        /// 没有指定的
        /// </summary>
        NoAllocate = 1 << 2
    }
}
