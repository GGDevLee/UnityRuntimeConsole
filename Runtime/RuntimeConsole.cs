using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace LeeFramework.Console
{
    public class RuntimeConsole : MonoBehaviour
    {
        public static RuntimeConsole instance;
        public ConsoleMini consoleMini;
        public ConsoleAll consoleAll;

        private LogMgr _LogMgr;


        private bool _IsFirst = true;
        private float _FrameTime = 0.25f;
        private int _Fps = 0;
        private float _LastUpdate = 0;
        private int _Frames = 0;
        private float _MemorySize = 1024.0f * 1024.0f;

        private void Awake()
        {
            instance = this;
            _LogMgr = new LogMgr();
            Debug.Log(123);
            Debug.LogWarning(123);
            Debug.LogError(123);
            
        }

        private void Update()
        {
            UpdateMini();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Input.GetKeyDown(KeyCode.Space)Input.GetKeyDown(KeyCode.Space)Input.GetKeyDown(KeyCode.Space)Input.GetKeyDown(KeyCode.Space)Input.GetKeyDown(KeyCode.Space)Input.GetKeyDown(KeyCode.Space)Input.GetKeyDown(KeyCode.Space)Input.GetKeyDown(KeyCode.Space)Input.GetKeyDown(KeyCode.Space)Input.GetKeyDown(KeyCode.Space)Input.GetKeyDown(KeyCode.Space)Input.GetKeyDown(KeyCode.Space)Input.GetKeyDown(KeyCode.Space)Input.GetKeyDown(KeyCode.Space)Input.GetKeyDown(KeyCode.Space)Input.GetKeyDown(KeyCode.Space)Input.GetKeyDown(KeyCode.Space)Input.GetKeyDown(KeyCode.Space)Input.GetKeyDown(KeyCode.Space)Input.GetKeyDown(KeyCode.Space)Input.GetKeyDown(KeyCode.Space)Input.GetKeyDown(KeyCode.Space)Input.GetKeyDown(KeyCode.Space)");
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
                consoleMini.SetMono(Profiler.GetMonoUsedSizeLong() / _MemorySize);
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

        public void ShowConsole()
        {
            consoleMini.SetActive(false);
            consoleAll.SetActive(true);
        }

        public void HideConsole()
        {
            consoleMini.SetActive(true);
            consoleAll.SetActive(false);
        }

    }
}