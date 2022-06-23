using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LeeFramework.Console
{
    public class ConsoleMini : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Canvas canvas;
        public Text txtFps;
        public Text txtMono;
        public Text txtTime;
        public Text txtVersion;
        public Image imgLogBg;
        public Text txtLog;
        public Text txtWarring;
        public Text txtError;
        public Color colNormal;
        public Color colLog;
        public Color colWarring;
        public Color colError;

        private CanvasGroup _CanvasGroup;
        private int _Log, _Warring, _Error = 0;
        private Vector2 _HalfSize = Vector2.zero;
        private RectTransform _CanvasRect;
        private Vector2 _CanvasSize;
        private RectTransform _Rect;
        private bool _IsDrag = false;
        private Coroutine _CorMove = null;

        private void Start()
        {
            _CanvasRect = canvas.GetComponent<RectTransform>();
            _CanvasGroup = GetComponent<CanvasGroup>();
            _Rect = GetComponent<RectTransform>();
            _HalfSize = _Rect.sizeDelta * 0.5f;
            _CanvasSize = _CanvasRect.sizeDelta;


            txtVersion.text = string.Format("Ver: {0}", Application.version);
            MoveToPos();
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
            if (value)
            {
                imgLogBg.color = colNormal;
            }
        }

        public void AddLog(LogType logType)
        {
            switch (logType)
            {
                case LogType.Log:
                    _Log++;
                    UpdateLog();
                    break;
                case LogType.Warning:
                    _Warring++;
                    UpdateWarring();
                    break;
                case LogType.Error:
                case LogType.Exception:
                case LogType.Assert:
                    _Error++;
                    UpdateError();
                    break;
            }

            if (_Error > 0)
            {
                imgLogBg.color = colError;
            }
            else if (_Warring > 0)
            {
                imgLogBg.color = colWarring;
            }
            else
            {
                imgLogBg.color = colLog;
            }
        }

        public void SetFsp(int fps)
        {
            txtFps.text = string.Format("FPS: {0}", fps);
        }

        public void SetMono(float mono)
        {
            txtMono.text = string.Format("Mono: {0:f1}", mono);
        }

        public void SetTime(float seconds)
        {
            txtTime.text = string.Format("Time: {0:f1}", seconds);
        }

        /// <summary>
        /// 屏幕旋转
        /// </summary>
        public void OnScreenDimensionsChange()
        {
            if (_CanvasRect != null)
            {
                _CanvasSize = _CanvasRect.sizeDelta;
                if (gameObject.activeSelf)
                {
                    MoveToPos();
                }
            }
        }

        private void UpdateLog()
        {
            txtLog.text = _Log >= 999 ? "999" : _Log.ToString();
        }

        private void UpdateWarring()
        {
            txtWarring.text = _Warring >= 999 ? "999" : _Warring.ToString();
        }

        private void UpdateError()
        {
            txtError.text = _Error >= 999 ? "999" : _Error.ToString();
        }

        public void ClearLog()
        {
            txtLog.text = "0";
            txtWarring.text = "0";
            txtError.text = "0";
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _IsDrag = true;
            _CanvasGroup.alpha = 1;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_CanvasRect, eventData.position, eventData.pressEventCamera, out pos))
            {
                _Rect.localPosition = pos;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _IsDrag = false;
            _CanvasSize = _CanvasRect.sizeDelta;

            MoveToPos();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_IsDrag)
            {
                return;
            }
            RuntimeConsole.instance.ShowConsoleAll();
        }

        private void MoveToPos(bool imm = false)
        {
            float toLeft = _CanvasSize.x * 0.5f + _Rect.localPosition.x;
            float toRight = _CanvasSize.x - toLeft;

            float toButtom = _CanvasSize.y * 0.5f + _Rect.localPosition.y;
            float toUp = _CanvasSize.y - toButtom;

            float minX = Mathf.Min(toLeft, toRight);
            float minY = Mathf.Min(toButtom, toUp);
            Vector2 tarPos = Vector2.zero;
            //左右
            if (minX < minY)
            {
                if (toLeft < toRight)
                {
                    tarPos = new Vector2(_CanvasSize.x * -0.5f + _HalfSize.x, _Rect.localPosition.y);
                }
                else
                {
                    tarPos = new Vector2(_CanvasSize.x * 0.5f - _HalfSize.x, _Rect.localPosition.y);
                }
            }
            //上下
            else
            {
                if (toButtom < toUp)
                {
                    tarPos = new Vector2(_Rect.localPosition.x, _CanvasSize.y * -0.5f + _HalfSize.y);
                }
                else
                {
                    tarPos = new Vector2(_Rect.localPosition.x, _CanvasSize.y * 0.5f - _HalfSize.y);
                }
            }
            if (_CorMove != null)
            {
                StopCoroutine(_CorMove);
            }
            _CorMove = StartCoroutine(MoveTween(tarPos));
        }

        private IEnumerator MoveTween(Vector2 pos)
        {
            if (Vector2.Distance(_Rect.localPosition, pos) < 0.1f)
            {
                yield return null;
            }
            float progress = 0;
            Vector2 initPos = _Rect.localPosition;
            while (progress < 1)
            {
                progress += Time.unscaledDeltaTime * 5f;
                _Rect.localPosition = Vector2.Lerp(initPos, pos, progress);
                yield return null;
            }

            yield return new WaitForSeconds(3);

            while (_CanvasGroup.alpha > 0.4f)
            {
                _CanvasGroup.alpha -= 0.01f;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
