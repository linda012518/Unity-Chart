using UnityEngine;
using DG.Tweening;

namespace Utils
{
    public class PolygonSlider : MonoBehaviour
    {
        /// <summary>几边形</summary>
        public byte _polygonCount;
        RectTransform[] lines;
        Vector2 rect;

        T[] GetChild<T>(Transform transform)
        {
            T[] temp = new T[transform.childCount];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = transform.GetChild(i).GetComponent<T>();
            }
            return temp;
        }

        void Awake()
        {
            lines = GetChild<RectTransform>(transform);
            //lines = transform.GetChild<RectTransform>();

            rect = lines[0].sizeDelta;
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].sizeDelta = new Vector2(0, rect.y);
            }
        }

        void InitElement()
        {
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].sizeDelta = new Vector2(0, rect.y);
            }
        }

        void KillAnim()
        {
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].DOKill();
            }
        }

        public void EnterAnim(float rate)
        {
            InitElement();
            if (rate == 1)
            {
                for (int i = 0; i < _polygonCount; i++)
                {
                    lines[i].DOSizeDelta(rect, 1.0f / _polygonCount).SetDelay(1.0f / _polygonCount * i);
                }
            }
            else
            {
                int count = Mathf.CeilToInt(rate / (1.0f / _polygonCount));

                for (int i = 0; i < count; i++)
                {
                    if (i == count - 1)
                    {
                        float temp = rate / (1.0f / _polygonCount) - Mathf.FloorToInt(rate / (1.0f / _polygonCount));
                        lines[i].DOSizeDelta(new Vector2(temp * rect.x, rect.y), 1.0f / count).SetDelay(1.0f / count * i);
                        break;
                    }
                    lines[i].DOSizeDelta(rect, 1.0f / count).SetDelay(1.0f / count * i);
                }
            }

        }

    }
}