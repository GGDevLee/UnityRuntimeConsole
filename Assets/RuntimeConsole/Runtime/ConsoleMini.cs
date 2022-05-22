using System.Collections;
using System.Collections.Generic;
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

        private Vector2 _HalfSize = Vector2.zero;
        private RectTransform _CanvasRect;
        private Vector2 _CanvasSize;
        private RectTransform _Rect;
        private bool _IsDrag = false;

        private void Start()
        {
            _CanvasRect = canvas.GetComponent<RectTransform>();
            _Rect = GetComponent<RectTransform>();
            _HalfSize = _Rect.sizeDelta * 0.5f;
            _CanvasSize = _CanvasRect.sizeDelta;

            txtVersion.text = string.Format("Version: {0}", Application.version);
            MoveToPos();
        }

        public void SetActive(bool value)
        { 
            gameObject.SetActive(value);
        }

        public void SetFsp(int frame)
        {
            txtFps.text = string.Format("FPS: {0}", frame);
        }

        public void SetMono(float mono)
        {
            txtMono.text = string.Format("Mono: {0:f1}", mono);
        }

        public void SetTime(float seconds)
        {
            txtTime.text = string.Format("Time: {0:f1}", seconds);
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            _IsDrag = true;
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
            RuntimeConsole.instance.ShowConsole();
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

            StartCoroutine(MoveTween(tarPos));
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
                progress += Time.unscaledTime * 0.003f;
                _Rect.localPosition = Vector2.Lerp(initPos, pos, progress);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
