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

namespace L.Network.TNet
{
    /// <summary>
    /// TCP 解析包状态
    /// </summary>
    internal enum TParserState
    {
        /// <summary>
        /// 数据包大小
        /// </summary>
        PacketSize,
        /// <summary>
        /// 数据包体
        /// </summary>
        PacketBody
    }
}
