using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LeeFramework.Console
{
    public class ConsoleBottom : MonoBehaviour
    {
        public RectTransform rect;
        public ConsoleAll consoleAll;
        public Toggle togMono;
        public Toggle togFps;
        public GameObject goLogFull;
        public RectTransform rectLogFull;
        public Image imgLogType;
        public Text txtContent;
        public Text txtTime;
        public Text txtMono;
        public Text txtFps;
        public Text txtScene;
        public Button btnCopy;
        public Button btnClose;

        private LogCache _LogCache;

        private void Awake()
        {
            togMono.onValueChanged.AddListener(OnTogMono);
            togFps.onValueChanged.AddListener(OnTogFps);
            btnCopy.onClick.AddListener(OnClickCopy);
            btnClose.onClick.AddListener(OnClickClose);
        }

        private void OnClickCopy()
        {
            if (_LogCache != null)
            {
                string content = string.Format("{0}\n{1}", _LogCache.log.condition, _LogCache.log.stackTrace);
                GUIUtility.systemCopyBuffer = content;
            }
        }

        private void OnClickClose()
        {
            RuntimeConsole.instance.consoleAll.ResetAllLogViewBg();
            HideFullLog();
        }

        private void OnTogFps(bool value)
        {
            consoleAll.ShowFps(value);
        }

        private void OnTogMono(bool value)
        {
            consoleAll.ShowMono(value);
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public void ShowFullLog(LogCache log)
        {
            _LogCache = log;
            goLogFull.SetActive(true);
            txtContent.text = string.Format("<b>{0}</b>\n<size=12>{1}</size>", log.log.condition, log.log.stackTrace);
            txtScene.text = log.scene;
            txtTime.text = log.time.ToString("0.00");
            txtMono.text = log.mono.ToString("0.0");
            txtFps.text = log.fps.ToString();
            SetLogType(log.log.type);
        }


        private void SetLogType(LogType type)
        {
            switch (type)
            {
                case LogType.Log:
                    imgLogType.sprite = RuntimeConsole.instance.spriteLog;
                    break;
                case LogType.Warning:
                    imgLogType.sprite = RuntimeConsole.instance.spriteWarring;
                    break;
                case LogType.Error:
                case LogType.Exception:
                case LogType.Assert:
                    imgLogType.sprite = RuntimeConsole.instance.spriteError;
                    break;
            }
        }

        public void HideFullLog()
        {
            goLogFull.SetActive(false);
        }

    }
}

