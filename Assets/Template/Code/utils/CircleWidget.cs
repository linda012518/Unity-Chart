using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
namespace Utils {
    /// <summary>
    /// 
    /// </summary>
    public class CircleWidget : MonoBehaviour
    {
        public int imageCount;
        public List<Image> imageList = new List<Image>();
        public Vector2 startPoint;
        public Color startColor;
        public Color endColor;
        private float animSpeed;
        int currentint;
        private void Awake()
        {
            InitCircle();
        }
        private void InitCircle()
        {
            Quaternion q = Quaternion.identity;
            Color color = Color.white;
            if (transform.childCount == 0)
            {
                Debug.Log("容器中缺少小块，自动创建默认小块");
                for (int i = 0; i < imageCount; i++)
                {
                    color = Color.Lerp(startColor, endColor, (float)i / imageCount);
                    q = Quaternion.Euler(0, 0, i * 360f / imageCount);
                    Image img = Tool.CreateImage(transform, new Vector2(5, 8));
                    img.color = color;
                    img.rectTransform.localEulerAngles = q.eulerAngles;
                    img.rectTransform.anchoredPosition = q * startPoint;
                    img.enabled = false;
                    imageList.Add(img);
                }
            }
            else {
                Debug.Log("加载容器中的小块");
                for (int i=0;i<transform.childCount;i++)
                {
                    imageList.Add(transform.GetChild(i).GetComponent<Image>());
                }
            } 
        }
        public void ShowAnimationDoTween(float speed,float current) {
            current = current > 1 ? 1 : current;
            int  currentint = Mathf.CeilToInt(imageCount * current);
            Debug.Log(currentint+":"+ imageCount * current+":"+ Mathf.FloorToInt(imageCount * current));
            int bb = 0;
            DOTween.Sequence().Append(DOTween.To (()=>bb,delegate(int value) {
                Debug.Log(value);
                imageList[value].enabled = true;
            },currentint-1,speed));
        }
        /// <summary>
        /// dotween 实现不是很精确
        /// </summary>
        /// <param name="speed">动画速度</param>
        /// <param name="current">百分比</param>
        public void ShowAnimation(float speed, float current)
        {
            ResetState();
               current = current > 1 ? 1 : current;
            currentint = Mathf.CeilToInt(imageCount * current);
            animSpeed = speed / currentint;
            ResetState();
            IEnumerator coroutine = Progress(currentint);
            StartCoroutine(coroutine);
        }
        public void ResetState()
        {
            IEnumerator coroutine = Progress(currentint);
            StopCoroutine(coroutine);
            if (imageList.Count > 0)
                for (int i = 0; i < imageList.Count; i++)
                {
                    imageList[i].enabled = false;
                }
        }
        IEnumerator Progress(float num)
        {
            for (int i = 0; i < num; i++)
            {
                imageList[i].enabled = true;
                yield return new WaitForSeconds(animSpeed);
            }
        }
    }

    public class Tool {

        public static Image CreateImage(Transform parent,Vector2 size) {
            GameObject img = new GameObject();
            Image imge= img.AddComponent<Image>();
            imge.rectTransform.sizeDelta = size;
            img.transform.SetParent(parent);
            return imge;
        }
    }
}
