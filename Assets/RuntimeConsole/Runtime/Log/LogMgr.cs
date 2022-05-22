using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LeeFramework.Console
{
    public class LogMgr
    {
        private List<Log> _AllLogs = new List<Log>();
        private int _AllLog = 0;
        private int _AllWarring = 0;
        private int _AllError = 0;

        public LogMgr()
        {
            Application.logMessageReceivedThreaded -= LogCallback;
            Application.logMessageReceivedThreaded += LogCallback;
        }

        public void LogCallback(string condition, string stackTrace, LogType type)
        {
            Log log = new Log(condition,stackTrace,type);
            lock (_AllLogs)
            {
                _AllLogs.Add(log);

                switch (type)
                {
                    case LogType.Log:
                        _AllLog++;
                        break;
                    case LogType.Warning:
                        _AllWarring++;
                        break;
                    case LogType.Error:
                        _AllError++;
                        break;
                }

            }
        }

        public List<Log> GetLogs(LogType type)
        {
            List<Log> list = new List<Log>();

            foreach (Log item in _AllLogs)
            {
                if (item.type == type)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public void ClearLog()
        { 
            _AllLogs.Clear();
            _AllLog = 0;
            _AllWarring = 0;
            _AllError = 0;
        }

    } 
}

