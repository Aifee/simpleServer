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
    public class LogAdapter : LogDecorater, ILog
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetLogger("Logger");

        public LogAdapter(LogDecorater decorater = null)
            : base(decorater)
        {
        }

        public void Warning(string message)
        {
            _logger.Warn(this.Decorate(message));
        }

        public void Info(string message)
        {
            _logger.Info(this.Decorate(message));
        }

        public void Debug(string message)
        {
            _logger.Debug(this.Decorate(message));
        }

        public void Error(string message)
        {
            _logger.Error(this.Decorate(message));
        }
    }
}