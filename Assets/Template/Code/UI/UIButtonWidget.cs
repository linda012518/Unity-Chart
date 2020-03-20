using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace UI
{
    public class UIButtonWidget : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler,
        IPointerDownHandler, IPointerUpHandler,
        IUpdateSelectedHandler, ISelectHandler,
        IPointerClickHandler
    {
        float clickTime = 0.5f;
        bool isSignleClick;
        DateTime OnClickTime;


        float doubleClickTime = 0.5f;
        ushort clickCount = 0;
        bool isDoubleClick;
        DateTime OnDoubleClickTime;


        float intervalTime = 2.0f;

        public delegate void VoidDelegate(GameObject go);
        public VoidDelegate onClick;
        public VoidDelegate onDoubleClick;
        public VoidDelegate onDown;
        public VoidDelegate onUp;
        public VoidDelegate onEnter;
        public VoidDelegate onExit;
        public VoidDelegate onSelect;
        public VoidDelegate onUpdateSelect;

        public static UIButtonWidget Get(GameObject go)
        {
            UIButtonWidget listener = go.GetComponent<UIButtonWidget>();
            if (listener == null) listener = go.AddComponent<UIButtonWidget>();
            return listener;
        }
        public static UIButtonWidget Get(Transform go)
        {
            UIButtonWidget listener = go.GetComponent<UIButtonWidget>();
            if (listener == null) listener = go.gameObject.AddComponent<UIButtonWidget>();
            return listener;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            SignleClick();
            DoubleClick();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            UpdateTime();
            if (onDown != null) onDown(gameObject);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (onEnter != null) onEnter(gameObject);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (onExit != null) onExit(gameObject);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (onUp != null) onUp(gameObject);
        }
        public void OnSelect(BaseEventData eventData)
        {
            if (onSelect != null) onSelect(gameObject);
        }
        public void OnUpdateSelected(BaseEventData eventData)
        {
            if (onUpdateSelect != null) onUpdateSelect(gameObject);
        }

        void UpdateTime()
        {
            SignleClickTimeCtrl();
            DoubleClickTimeCtrl();
        }

        void SignleClickTimeCtrl()
        {
            if (onClick == null) return;

            if (isSignleClick)
            {
                if ((float)(DateTime.Now - OnClickTime).TotalSeconds > intervalTime)
                {
                    isSignleClick = false;
                }
                else
                {
                    return;
                }
            }
            OnClickTime = DateTime.Now;
        }

        void SignleClick()
        {
            if (onClick != null)
            {
                if (clickTime < (float)(DateTime.Now - OnClickTime).TotalSeconds)
                {
                    return;
                }
                if (isSignleClick) return;
                isSignleClick = true;
                onClick(gameObject);
            }
        }

        void DoubleClickTimeCtrl()
        {
            if (onDoubleClick == null) return;
            if (isDoubleClick)
            {
                if ((float)(DateTime.Now - OnDoubleClickTime).TotalSeconds > intervalTime)
                {
                    isDoubleClick = false;
                    OnDoubleClickTime = DateTime.Now;
                }
            }
            else
            {
                if (clickCount == 0)
                {
                    OnDoubleClickTime = DateTime.Now;
                }
                else if (clickCount == 1)
                {
                    if (doubleClickTime < (float)(DateTime.Now - OnDoubleClickTime).TotalSeconds)
                    {
                        clickCount = 0;
                        OnDoubleClickTime = DateTime.Now;
                        return;
                    }

                }
                else if (clickCount == 2)
                {
                    if (doubleClickTime < (float)(DateTime.Now - OnDoubleClickTime).TotalSeconds)
                    {
                        clickCount = 0;
                        OnDoubleClickTime = DateTime.Now;
                        return;
                    }
                }
                else
                {
                    Debug.LogError("double click events error...");
                }
            }
        }

        void DoubleClick()
        {
            if (onDoubleClick == null) return;
            if (doubleClickTime < (float)(DateTime.Now - OnDoubleClickTime).TotalSeconds)
            {
                clickCount = 0;
                return;
            }
            if (isDoubleClick) return;
            ++clickCount;
            if (clickCount == 2)
            {
                clickCount = 0;
                isDoubleClick = true;
                onDoubleClick(gameObject);
            }
        }

    }
}