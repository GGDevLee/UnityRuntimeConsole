using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace LeeFramework.Console
{
    public class ConsoleAll : MonoBehaviour
    {
        public Button btnDelete;
        public Toggle togLog;
        public Toggle togWarring;
        public Toggle togError;
        public Button btnClose;

        private void Start()
        {
            btnClose.onClick.AddListener(OnClickClose);
        }


        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }




        private void OnClickDelete()
        { 
        
        }

        private void OnTogLog(bool value)
        { 
        
        }

        private void OnTogWarring(bool value)
        { 
        
        }

        private void OnTogError(bool value)
        { 
            
        }

        private void OnClickClose()
        {
            RuntimeConsole.instance.HideConsole();
        }
    }
}