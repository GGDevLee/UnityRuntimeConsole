using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

namespace LeeFramework.Console
{
    public class RuntimeConsole : MonoBehaviour
    {
        public static RuntimeConsole instance;
        public ConsoleMini consoleMini;
        public ConsoleAll consoleAll;
        public ConsoleInfo consoleInfo;
        public ConsoleBottom consoleBottom;

        public Sprite spriteLog;
        public Sprite spriteWarring;
        public Sprite spriteError;

        private LogMgr _LogMgr;
        public LogMgr logMgr
        {
            get
            {
                return _LogMgr;
            }
        }


        private bool _IsFirst = true;
        private float _FrameTime = 0.25f;
        private int _Fps = 0;
        private float _Mono = 0;
        private float _LastUpdate = 0;
        private int _Frames = 0;
        private float _MemorySize = 1024.0f * 1024.0f;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            instance = this;
            DontDestroyOnLoad(this);
            _LogMgr = new LogMgr();
        }

        private void Update()
        {
            _LogMgr.Update();
            UpdateMini();
        }

        private void OnRectTransformDimensionsChange()
        {
            if (consoleMini != null && consoleMini.gameObject.activeInHierarchy)
            {
                consoleMini.OnScreenDimensionsChange();
            }
        }

        #region MiniConsole
        private void UpdateMini()
        {
            UpdateTime();
            UpdateMono();
            UpdateFps();
        }

        private void UpdateTime()
        {
            if (consoleMini != null)
            {
                consoleMini.SetTime(Time.realtimeSinceStartup);
            }
        }

        private void UpdateMono()
        {
            if (consoleMini != null)
            {
                _Mono = Profiler.GetMonoUsedSizeLong() / _MemorySize;
                consoleMini.SetMono(_Mono);
            }
        }

        private void UpdateFps()
        {
            if (_IsFirst)
            {
                _IsFirst = false;
                _Fps = 0;
                _Frames = 0;
                _LastUpdate = Time.realtimeSinceStartup;
                return;
            }

            _Frames++;
            float during = Time.realtimeSinceStartup - _LastUpdate;

            if (during > _FrameTime)
            {
                _Fps = (int)(_Frames / during);
                if (consoleMini != null)
                {
                    consoleMini.SetFsp(_Fps);
                }
                _LastUpdate = Time.realtimeSinceStartup;
                _Frames = 0;
            }
        }
        #endregion

        public void AddLog(Log log)
        {
            consoleMini.AddLog(log.type);
            consoleAll.AddLog(log, _Fps, _Mono);
        }

        public void ShowConsoleAll()
        {
            consoleMini.SetActive(false);
            consoleAll.SetActive(true);
            consoleBottom.SetActive(true);
        }

        public void HideConsoleAll()
        {
            consoleMini.SetActive(true);
            consoleAll.SetActive(false);
            consoleBottom.SetActive(false);
        }

    }
}