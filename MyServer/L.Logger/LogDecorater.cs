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
    public abstract class LogDecorater
    {
        protected const string SEP = " ";
        private int _level;
        protected readonly LogDecorater _decorater;

        protected LogDecorater(LogDecorater decorater = null)
        {
            _decorater = decorater;
            _level = 0;
        }

        protected int Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
                if (_decorater != null)
                {
                    _decorater.Level = value + 1;
                }
            }
        }

        public virtual string Decorate(string message)
        {
            if (_decorater == null)
            {
                return message;
            }
            return _decorater.Decorate(message);
        }
    }
}