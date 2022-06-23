using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeeFramework.Console
{
    public class LogMgr
    {
        public int allLog => _AllLog;
        public int allWarring => _AllWarring;
        public int allError => _AllError;

        private int _AllLogIndex = -1;
        private List<Log> _AllLogs = new List<Log>();
        private int _AllLog = 0;
        private int _AllWarring = 0;
        private int _AllError = 0;

        private Queue<Log>_TmpLogs = new Queue<Log>();

        public LogMgr()
        {
            Application.logMessageReceivedThreaded -= LogCallback;
            Application.logMessageReceivedThreaded += LogCallback;
        }

        public void LogCallback(string condition, string stackTrace, LogType type)
        {
#if UNITY_ANDROID || UNITY_IOS
            if (string.IsNullOrEmpty(stackTrace))
            {
                stackTrace = new System.Diagnostics.StackTrace(true).ToString();
            } 
#endif
            _AllLogIndex++;
            Log log = new Log(condition, stackTrace, type, _AllLogIndex);
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
                    case LogType.Exception:
                        _AllError++;
                        break;
                }

                lock (_TmpLogs)
                {
                    _TmpLogs.Enqueue(log);
                }
            }
        }

        public void Update()
        {
            lock (_TmpLogs)
            {
                if (_TmpLogs.Count > 0)
                {
                    while (_TmpLogs.Count > 0)
                    {
                        RuntimeConsole.instance.AddLog(_TmpLogs.Dequeue());
                    }
                }
            }
        }

        public void ClearLog()
        { 
            _AllLogs.Clear();
            _AllLogIndex = -1;
            _AllLog = 0;
            _AllWarring = 0;
            _AllError = 0;
            RuntimeConsole.instance.consoleMini.ClearLog();
        }

    } 
}

