using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeeFramework.Console
{
    public class Log
    {
        public string condition;
        public string stackTrace;
        public LogType type;
        public int index;

        public Log(string condition, string stackTrace, LogType type, int index)
        {
            this.condition = condition;
            this.stackTrace = stackTrace;
            this.type = type;
            this.index = index;
        }
    }
}