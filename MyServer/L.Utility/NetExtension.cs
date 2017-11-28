/*
 *************************************************************
 * Copyright (c) 2017 - 2019 liuaf
 * Create time：2017/11/8 14:35:32
 * Created by：liuaf
 * Contact information：329737941@qq.com
 **************************************************************
 */

using System.Collections.Generic;
using System.Net;

namespace L.Utility
{
    public class NetExtension
    {
        public static string[] GetAddressIPs()
        {
            //获取本地的IP地址
            List<string> addressIPs = new List<string>();
            foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (address.AddressFamily.ToString() == "InterNetwork")
                {
                    addressIPs.Add(address.ToString());
                }
            }
            return addressIPs.ToArray();
        }
    }
}