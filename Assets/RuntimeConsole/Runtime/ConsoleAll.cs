using LeeFramework.UILoopListMini;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LeeFramework.Console
{
    public class ConsoleAll : MonoBehaviour
    {
        public LoopListMini loopListMini;
        public Button btnDelete;
        public InputField iptSearch;
        public Button btnInfo;
        public Toggle togLog;
        public Toggle togWarring;
        public Toggle togError;
        public Button btnClose;
        public ScrollRectMini scrollRect;
        public Text txtLogNum;
        public Text txtWarringNum;
        public Text txtErrorNum;
        public Button btnSelect;

        private bool _IsLog = true;
        private bool _IsWarring = true;
        private bool _IsError = true;
        private string _SearchStr = string.Empty;

        /// <summary>
        /// 是否显示帧率
        /// </summary>
        private bool _IsFps = false;

        /// <summary>
        /// 是否显示内存
        /// </summary>
        private bool _IsMono = false;

        private int _LogIndex = -1;
        private int _SelectLogIndex = -1;
        private const float _ScrollTime = 1;
        private RectTransform _ViewsRect;
        private List<LogCache> _AllLogCache = new List<LogCache>();
        private List<LogCache> _AllShowLogCache = new List<LogCache>();
        private Dictionary<int, LogView> _AllLoopItem = new Dictionary<int, LogView>();
        private Color _ColTxtWhite = new Color(1, 1, 1);
        private Color _ColTxtGray = new Color(125 / 255f, 125 / 255f, 125 / 255f);


        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
            if (loopListMini.isInit)
            {
                loopListMini.UpdateData(_AllShowLogCache.Count);
            }
        }

        public void AddLog(Log log, int fps, float mono)
        {
            _LogIndex++;
            LogCache cache = new LogCache(log, _LogIndex, fps, mono);
            _AllLogCache.Add(cache);
            AddShowLog(cache);
            UpdateLogNum();
            if (loopListMini.isInit)
            {
                loopListMini.UpdateData(_AllShowLogCache.Count);
            }
        }

        public void ShowFps(bool value)
        {
            _IsFps = value;
            loopListMini.UpdateData(_AllShowLogCache.Count);
        }

        public void ShowMono(bool value)
        {
            _IsMono = value;
            loopListMini.UpdateData(_AllShowLogCache.Count);
        }

        /// <summary>
        /// 显示完全日志
        /// </summary>
        public void ShowFullLog(LogView view, int index)
        {
            //先重置之前的
            foreach (LogCache cache in _AllLogCache)
            {
                if (cache.isFull && cache.index != index)
                {
                    cache.isFull = false;
                    foreach (LogView item in _AllLoopItem.Values)
                    {
                        if (item.logCache != null && item.logCache.index == cache.index)
                        {
                            item.SetBgColor(cache.index);
                        }
                    }
                }
            }

            LogCache log = _AllLogCache.Find(x => x.index == index);
            if (log.isFull)
            {
                log.isFull = false;
                _SelectLogIndex = -1;
                RuntimeConsole.instance.consoleBottom.HideFullLog();
                ResetView();
                view.SetBgColor(index);
                loopListMini.UpdateData(_AllShowLogCache.Count);
            }
            else
            {
                log.isFull = true;
                _SelectLogIndex = index;
                RuntimeConsole.instance.consoleBottom.ShowFullLog(log);
                float h = RuntimeConsole.instance.consoleBottom.rect.rect.height + RuntimeConsole.instance.consoleBottom.rectLogFull.rect.height;
                _ViewsRect.offsetMin = new Vector2(0, h);
                view.SetFullBgColor();
                loopListMini.UpdateData(_AllShowLogCache.Count);
            }

        }

        public void ResetView()
        {
            _ViewsRect.offsetMin = new Vector2(0, RuntimeConsole.instance.consoleBottom.rect.rect.height);
        }

        public void UpdateData()
        {
            loopListMini.UpdateData(_AllShowLogCache.Count);
        }

        /// <summary>
        /// 重置全部Log背景
        /// </summary>
        public void ResetAllLogViewBg()
        {
            LogCache log = _AllLogCache.Find(x => x.isFull);
            if (log != null)
            {
                log.isFull = false;

                LogView view = null;
                foreach (LogView item in _AllLoopItem.Values)
                {
                    if (item.logCache.index == log.index)
                    {
                        view = item;
                        break;
                    }
                }
                if (view != null)
                {
                    view.SetBgColor(log.index);
                }

                _SelectLogIndex = -1;
                ResetView();
                loopListMini.UpdateData(_AllShowLogCache.Count);
            }
        }


        private void Start()
        {
            _ViewsRect = scrollRect.GetComponent<RectTransform>();
            loopListMini.Init(OnInitDone, OnRefresh, OnRecycle);
            btnDelete.onClick.AddListener(OnClickDelete);
            iptSearch.onValueChanged.AddListener(OnInputSearch);
            btnInfo.onClick.AddListener(OnClickInfo);
            togLog.onValueChanged.AddListener(OnTogLog);
            togLog.isOn = true;
            togWarring.onValueChanged.AddListener(OnTogWarring);
            togWarring.isOn = true;
            togError.onValueChanged.AddListener(OnTogError);
            togError.isOn = true;
            btnClose.onClick.AddListener(OnClickClose);
            btnSelect.onClick.AddListener(OnClickSelect);
            if (loopListMini.isInit)
            {
                loopListMini.UpdateData(_AllShowLogCache.Count);
            }
        }

        private void OnInitDone(List<LoopItem> list)
        {
            _AllLoopItem.Clear();
            foreach (LoopItem item in list)
            {
                _AllLoopItem[item.instanceId] = item.gameObject.GetComponent<LogView>();
            }
        }

        private void OnRefresh(LoopItem item)
        {
            if (_AllLoopItem.ContainsKey(item.instanceId) && item.id <= _AllShowLogCache.Count - 1)
            {
                LogCache data = _AllShowLogCache[item.id];
                LogView view = _AllLoopItem[item.instanceId];
                view.SetLog(data);
                view.ShowFps(_IsFps);
                view.ShowMono(_IsMono);
                if (data.isFull)
                {
                    view.SetFullBgColor();
                }
                else
                {
                    view.SetBgColor(item.id);
                }
                if (_SelectLogIndex != -1 && data.index == _SelectLogIndex)
                {
                    if (btnSelect.gameObject.activeSelf)
                    {
                        //btnSelect.gameObject.SetActive(false);
                    }
                }
            }
        }

        private void OnRecycle(LoopItem item)
        {

        }

        private void AddShowLog(LogCache cache)
        {
            if (IsSearch())
            {
                if (!IsSearchLog(cache.log.condition))
                {
                    return;
                }
            }
            switch (cache.log.type)
            {
                case LogType.Log:
                    if (_IsLog)
                    {
                        _AllShowLogCache.Add(cache);
                    }
                    break;
                case LogType.Warning:
                    if (_IsWarring)
                    {
                        _AllShowLogCache.Add(cache);
                    }
                    break;
                case LogType.Error:
                case LogType.Assert:
                case LogType.Exception:
                    if (_IsError)
                    {
                        _AllShowLogCache.Add(cache);
                    }
                    break;
            }
        }

        private void OnClickDelete()
        {
            _LogIndex = -1;
            _AllLogCache.Clear();
            _AllShowLogCache.Clear();
            RuntimeConsole.instance.logMgr.ClearLog();
            RuntimeConsole.instance.consoleBottom.HideFullLog();
            ResetView();
            loopListMini.UpdateData(-1);
            UpdateLogNum();
        }

        private void OnInputSearch(string txt)
        {
            if (!string.IsNullOrEmpty(txt))
            {
                _SearchStr = txt;
                SelectSearchLog(txt);
                loopListMini.UpdateData(_AllShowLogCache.Count);
            }
            else
            {
                _SearchStr = string.Empty;
                OnTogLog(_IsLog);
                OnTogWarring(_IsWarring);
                OnTogError(_IsError);
            }
        }

        /// <summary>
        /// 打开信息
        /// </summary>
        private void OnClickInfo()
        {
            RuntimeConsole.instance.consoleInfo.SetActive(true);
        }

        /// <summary>
        /// 开启关闭Log
        /// </summary>
        private void OnTogLog(bool value)
        {
            _IsLog = value;
            SelectAllCacheLog();
            loopListMini.UpdateData(_AllShowLogCache.Count);

            txtLogNum.color = value ? _ColTxtWhite : _ColTxtGray;
        }

        /// <summary>
        /// 开启关闭Warring
        /// </summary>
        private void OnTogWarring(bool value)
        {
            _IsWarring = value;
            SelectAllCacheLog();
            loopListMini.UpdateData(_AllShowLogCache.Count);

            txtWarringNum.color = value ? _ColTxtWhite : _ColTxtGray;
        }

        /// <summary>
        /// 开启关闭Error
        /// </summary>
        private void OnTogError(bool value)
        {
            _IsError = value;
            SelectAllCacheLog();
            loopListMini.UpdateData(_AllShowLogCache.Count);

            txtErrorNum.color = value ? _ColTxtWhite : _ColTxtGray;
        }

        /// <summary>
        /// 关闭
        /// </summary>
        private void OnClickClose()
        {
            RuntimeConsole.instance.HideConsoleAll();
        }

        /// <summary>
        /// 跳到选中的Item
        /// </summary>
        private void OnClickSelect()
        {
            if (_SelectLogIndex == -1)
            {
                return;
            }

            float progress = _SelectLogIndex * 1.0f / _AllShowLogCache.Count;
            loopListMini.ScrollToTarget(1 - progress, _ScrollTime);
        }

        /// <summary>
        /// 筛选日志
        /// </summary>
        private void SelectAllCacheLog()
        {
            _AllShowLogCache.Clear();

            foreach (LogCache cache in _AllLogCache)
            {
                if (cache == null)
                {
                    continue;
                }

                if (IsSearch())
                {
                    if (!IsSearchLog(cache.log.condition))
                    {
                        continue;
                    }
                }

                switch (cache.log.type)
                {
                    case LogType.Log:
                        if (_IsLog)
                        {
                            _AllShowLogCache.Add(cache);
                        }
                        break;
                    case LogType.Warning:
                        if (_IsWarring)
                        {
                            _AllShowLogCache.Add(cache);
                        }
                        break;
                    case LogType.Error:
                    case LogType.Assert:
                    case LogType.Exception:
                        if (_IsError)
                        {
                            _AllShowLogCache.Add(cache);
                        }
                        break;
                }

            }

            _AllShowLogCache.Sort((item1, item2) =>
            {
                if (item1.log.index < item2.log.index)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            });
        }

        /// <summary>
        /// 筛选搜索日志
        /// </summary>
        private void SelectSearchLog(string txt)
        {
            _AllShowLogCache.Clear();
            foreach (LogCache cache in _AllLogCache)
            {
                if (cache == null)
                {
                    continue;
                }
                if (IsSearchLog(cache.log.condition))
                {
                    switch (cache.log.type)
                    {
                        case LogType.Log:
                            if (_IsLog)
                            {
                                _AllShowLogCache.Add(cache);
                            }
                            break;
                        case LogType.Warning:
                            if (_IsWarring)
                            {
                                _AllShowLogCache.Add(cache);
                            }
                            break;
                        case LogType.Error:
                        case LogType.Assert:
                        case LogType.Exception:
                            if (_IsError)
                            {
                                _AllShowLogCache.Add(cache);
                            }
                            break;
                    }

                }
            }

            _AllShowLogCache.Sort((item1, item2) =>
            {
                if (item1.log.index < item2.log.index)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            });
        }

        /// <summary>
        /// 是否搜索状态
        /// </summary>
        private bool IsSearch()
        {
            return !string.IsNullOrEmpty(_SearchStr);
        }

        /// <summary>
        /// 是否搜索的日志
        /// </summary>
        private bool IsSearchLog(string condition)
        {
            if (IsSearch() && condition.ToLower().Contains(_SearchStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 刷新日志数量
        /// </summary>
        private void UpdateLogNum()
        {
            txtLogNum.text = RuntimeConsole.instance.logMgr.allLog.ToString();
            txtWarringNum.text = RuntimeConsole.instance.logMgr.allWarring.ToString();
            txtErrorNum.text = RuntimeConsole.instance.logMgr.allError.ToString();
        }
    }
}