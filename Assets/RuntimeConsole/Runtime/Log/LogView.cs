using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LeeFramework.Console
{
    public class LogView : MonoBehaviour
    {
        public Button btnContent;
        public Text txtContent;

        public Image imgType;
        public Image imgFrame;
        public Image imgMono;
        public Text txtFps;
        public Text txtMono;

        public LogCache logCache => _Cache;

        private string _Tmp;
        private int _Fps = 0;
        private float _Mono = 0;
        private LogCache _Cache = null;
        private Color _ColItemBlack = new Color(56 / 255f, 56 / 255f, 56 / 255f);
        private Color _ColItemWhite = new Color(75 / 255f, 75 / 255f, 75 / 255f);
        private Color _ColItemBlue = new Color(44 / 255f, 93 / 255f, 135 / 255f);

        private void Start()
        {
            btnContent.onClick.AddListener(OnClickLog);
        }

        private void OnClickLog()
        {
            RuntimeConsole.instance.consoleAll.ShowFullLog(this, _Cache.index);
        }

        public void SetLog(LogCache cache)
        {
            _Cache = cache;

            txtContent.text = cache.log.condition.ToString();
            SetLogType(cache.log.type);

            _Tmp = cache.log.condition.ToString().ToLower();
            SetFps(cache.fps);
            SetMono(cache.mono);
        }

        public bool Check(string str)
        {
            return _Tmp.Contains(str.ToLower());
        }

        public void ShowFps(bool value)
        {
            imgFrame.gameObject.SetActive(value);
        }

        public void ShowMono(bool value)
        {
            imgMono.gameObject.SetActive(value);
        }

        public void SetBgColor(int index)
        {
            //是否复数
            bool isDouble = index % 2 == 0 || index == 0 ? true : false;

            btnContent.GetComponent<Image>().color = isDouble ? _ColItemBlack : _ColItemWhite;
        }

        public void SetFullBgColor()
        {
            btnContent.GetComponent<Image>().color = _ColItemBlue;
        }

        public void Dispose()
        {
            gameObject.SetActive(false);
            _Fps = 0;
            _Mono = 0;
            txtFps.text = string.Empty;
            txtMono.text = string.Empty;
        }



        private void SetLogType(LogType type)
        {
            switch (type)
            {
                case LogType.Log:
                    imgType.sprite = RuntimeConsole.instance.spriteLog;
                    break;
                case LogType.Warning:
                    imgType.sprite = RuntimeConsole.instance.spriteWarring;
                    break;
                case LogType.Error:
                case LogType.Exception:
                case LogType.Assert:
                    imgType.sprite = RuntimeConsole.instance.spriteError;
                    break;
            }
        }

        private void SetFps(int fps)
        {
            _Fps = fps;
            txtFps.text = fps.ToString();
        }

        private void SetMono(float mono)
        {
            _Mono = mono;
            txtMono.text = mono.ToString("0.0");
        }
    }
}

