using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace LeeFramework.Console
{
    public class ConsoleInfo : MonoBehaviour
    {
        public Button btnClose;
        public Button btnFresh;
        public Text txtAppDes;
        public Text txtApp;
        public Text txtSysDes;
        public Text txtSys;
        public Slider sldSize;

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
            if (value)
            {
                RefreshInfo();
            }
        }

        private void Start()
        {
            txtApp.fontSize = 12;
            txtSys.fontSize = 12;
            sldSize.minValue = 6;
            sldSize.maxValue = 24;
            sldSize.value = 12;
            btnClose.onClick.AddListener(OnClickClose);
            btnFresh.onClick.AddListener(RefreshInfo);
            sldSize.onValueChanged.AddListener(OnSldFontSize);
            RefreshInfo();
        }

        private void RefreshInfo()
        {
            RefreshApp();
            RefreshSys();
        }

        /// <summary>
        /// 更新App信息
        /// </summary>
        private void RefreshApp()
        {
            txtApp.text = string.Empty;
            Application app = new Application();
            Type type = typeof(Application);
            PropertyInfo[] pros = type.GetProperties();
            foreach (PropertyInfo item in pros)
            {
                txtApp.text += string.Format("{0} : <color='#00A4FF'>{1}</color>\n", item.Name, item.GetValue(app));
                //Debug.Log("PropertyInfo : " + item.Name + "  Value : " + item.GetValue(app));
            }
        }

        /// <summary>
        /// 更新系统信息
        /// </summary>
        private void RefreshSys()
        {
            txtSys.text = string.Empty;
            SystemInfo app = new SystemInfo();
            Type type = typeof(SystemInfo);
            PropertyInfo[] pros = type.GetProperties();
            foreach (PropertyInfo item in pros)
            {
                txtSys.text += string.Format("{0} : <color='#00A4FF'>{1}</color>\n", item.Name, item.GetValue(app));
                //Debug.Log("PropertyInfo : " + item.Name + "  Value : " + item.GetValue(app));
            }
        }

        private void OnSldFontSize(float value)
        {
            txtAppDes.fontSize = (int)value;
            txtSysDes.fontSize = (int)value;
            txtApp.fontSize = (int)value;
            txtSys.fontSize = (int)value;
        }

        private void OnClickClose()
        {
            txtApp.text = string.Empty;
            txtSys.text = string.Empty;
            gameObject.SetActive(false);
        }
    } 
}

