/*
 *************************************************************
 * Copyright (c) 2017 - 2019 liuaf
 * Create time：2017/11/8 14:35:32
 * Created by：liuaf
 * Contact information：329737941@qq.com
 **************************************************************
 */
 
using System.Reflection;

namespace L.Utility
{
    public static class MethodInfoExtension
    {
        public static void Run(this MethodInfo methodInfo, object obj, params object[] param)
        {

            if (methodInfo.IsStatic)
            {
                object[] p = new object[param.Length + 1];
                p[0] = obj;
                for (int i = 0; i < param.Length; ++i)
                {
                    p[i + 1] = param[i];
                }
                methodInfo.Invoke(null, p);
            }
            else
            {
                methodInfo.Invoke(obj, param);
            }
        }
    }
}
