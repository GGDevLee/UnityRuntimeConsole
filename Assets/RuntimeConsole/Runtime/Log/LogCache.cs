using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LeeFramework.Console
{
    public class LogCache
    {
        public int index;
        public Log log;
        public string scene;
        public int fps;
        public float mono;
        public float time;
        public bool isFull = false;//是否显示全部日志

        public LogCache(Log l, int i, int fps, float mono)
        {
            log = l;
            index = i;
            this.fps = fps;
            this.mono = mono;
            scene = SceneManager.GetActiveScene().name;
            time = Time.realtimeSinceStartup;
            isFull = false;
        }
    }
}