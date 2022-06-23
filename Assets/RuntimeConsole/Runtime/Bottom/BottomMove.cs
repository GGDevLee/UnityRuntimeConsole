using UnityEngine;
using UnityEngine.EventSystems;

namespace LeeFramework.Console
{
    public class BottomMove : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public RectTransform rect;
        public RectTransform startRect;
        public RectTransform moveRect;

        private bool _IsDrag = false;
        private Vector2 _StartPos = Vector2.zero;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _IsDrag = true;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, startRect.position, eventData.pressEventCamera, out _StartPos);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, eventData.pressEventCamera, out pos))
            {
                Vector2 offset = pos - _StartPos;
                moveRect.localPosition += new Vector3(offset.x, offset.y, 0);
                _StartPos = pos;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _IsDrag = false;
            _StartPos = Vector2.zero;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_IsDrag)
            {
                return;
            }
            moveRect.anchorMin = Vector2.zero;
            moveRect.anchorMax = Vector2.one;
            moveRect.offsetMin = Vector2.zero;
            moveRect.offsetMax = Vector2.zero;
            RuntimeConsole.instance.consoleAll.UpdateData();
        }
    }
}
