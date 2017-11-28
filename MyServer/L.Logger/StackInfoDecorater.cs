/*
 *************************************************************
 * Copyright (c) 2017 - 2019 liuaf
 * Create time：2017/11/8 14:35:32
 * Created by：liuaf
 * Contact information：329737941@qq.com
 **************************************************************
 */

using System.Diagnostics;
using System.IO;

namespace L.Logger
{
    public class StackInfoDecorater : LogDecorater
    {
        public StackInfoDecorater(LogDecorater decorater = null)
            : base(decorater)
        {
            FileName = true;
            FileLineNumber = true;
        }

        public bool FileName { get; set; }

        public bool FileLineNumber { get; set; }

        public override string Decorate(string message)
        {
            if (_decorater != null)
            {
                message = _decorater.Decorate(message);
            }

            if (!this.FileLineNumber && !this.FileName)
            {
                return message;
            }

            string extraInfo = "";
            StackTrace stackTrace = new StackTrace(true);
            StackFrame frame = stackTrace.GetFrame(this.Level + 3);

            if (FileName)
            {
                string fileName = Path.GetFileName(frame.GetFileName());
                extraInfo += fileName + " ";
            }
            if (FileLineNumber)
            {
                int fileLineNumber = frame.GetFileLineNumber();
                extraInfo += fileLineNumber + " ";
            }
            return extraInfo + message;
        }
    }
}