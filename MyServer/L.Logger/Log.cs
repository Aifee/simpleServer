/*
 *************************************************************
 * Copyright (c) 2017 - 2019 liuaf
 * Create time：2017/11/8 14:35:32
 * Created by：liuaf
 * Contact information：329737941@qq.com
 **************************************************************
 */

namespace L.Logger
{
    public static class Log
    {
        private static readonly ILog globalLog = new LogAdapter();

        public static void Warning(string message)
        {
            globalLog.Warning(message);
        }

        public static void Info(string message)
        {
            globalLog.Info(message);
        }

        public static void Debug(string message)
        {
            globalLog.Debug(message);
        }

        public static void Error(string message)
        {
            globalLog.Error(message);
        }
    }
}