using UnityEngine;
using UnityEngine.EventSystems;

namespace LeeFramework.Console
{
    public class BottomExt : MonoBehaviour, IPointerClickHandler,IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public RectTransform rect;
        public RectTransform extRect;
        public RectTransform moveRect;

        private bool _IsDrag = false;
        private Vector2 _StartPos = Vector2.zero;


        public void OnBeginDrag(PointerEventData eventData)
        {
            _IsDrag = true;
            _StartPos = extRect.localPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, eventData.pressEventCamera, out pos))
            {
                Vector2 offset = pos - _StartPos;
                rect.sizeDelta -= new Vector2(0, offset.y);
                _StartPos = pos;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _IsDrag = false;
            _StartPos = Vector2.zero;
            RuntimeConsole.instance.consoleAll.UpdateData();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_IsDrag)
            {
                return;
            }
            moveRect.anchorMin = Vector2.zero;
            moveRect.anchorMax = Vector2.one;
            moveRect.offsetMin = new Vector2(moveRect.offsetMin.x, 0);
            moveRect.offsetMax = new Vector2(moveRect.offsetMax.x, 0);
            RuntimeConsole.instance.consoleAll.UpdateData();
        }
    } 
}
