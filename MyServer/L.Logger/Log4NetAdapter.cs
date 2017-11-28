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
    public class Log4NetAdapter : LogDecorater, ILog
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger("Logger");

        public Log4NetAdapter(LogDecorater decorater = null)
            : base(decorater)
        {
        }

        public void Warning(string message)
        {
            _logger.Warn(Decorate(message));
        }

        public void Info(string message)
        {
            _logger.Info(Decorate(message));
        }

        public void Debug(string message)
        {
            _logger.Debug(Decorate(message));
        }

        public void Error(string message)
        {
            _logger.Error(Decorate(message));
        }
    }
}