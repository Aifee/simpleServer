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
    public interface ILog
    {
        void Warning(string message);

        void Info(string message);

        void Debug(string message);

        void Error(string message);
    }
}