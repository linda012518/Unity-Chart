using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace Utils
{
    public class BrokenGraph : MonoBehaviour
    {
        public enum AnimationType
        {
            onebyone,
            all,
        }
        public AnimationType aType;
        public bool useColor;
        public bool useLabel;
        public Vector2 dataLabelsOffset;
        [Space(10)]
        [Header("线")]
        [SerializeField]
        private List<RectTransform> lineItems = new List<RectTransform>();
        [SerializeField]
        private Color lineColor;
        [Header("点")]
        [SerializeField]
        private List<RectTransform> pointItems = new List<RectTransform>();
      [SerializeField]
        private Sprite pointImg;
        private float animSpeed = 1;

        private int onebyoneindex;
        private int Ionebyoneindex;
        private float _multiple;
        List<Tween> tweeners = new List<Tween>();
        void Awake()
        {
            InitBrokenGraph();

        }
        #region funcation
        private void InitBrokenGraph()
        {
            Transform pointF = transform.Find("point");
            Transform lineF = transform.Find("line");
            for (int i = 0; i < pointF.childCount; i++)
            {
                pointItems.Add(pointF.GetChild(i).GetComponent<RectTransform>());
            }
            for (int i = 0; i < lineF.childCount; i++)
            {
                lineItems.Add(lineF.GetChild(i).GetComponent<RectTransform>());
                lineF.GetChild(i).GetComponent<RawImage>().color = lineColor;
            }

            SetItemLabel(useLabel);
        }
        #endregion

        public void ResetState()
        {
            onebyoneindex = 0;
            //lines = 0;

            for (int i = 0; i < tweeners.Count; i++)
            {
                DOTween.Kill(tweeners[i], false);
            }

            tweeners.Clear();
            for (int i = 0; i < pointItems.Count; i++)
            {
                pointItems[i].anchoredPosition = new Vector2(pointItems[i].anchoredPosition.x, 0);
            }
            for (int i = 0; i < lineItems.Count; i++)
            {
                lineItems[i].sizeDelta = Vector2.zero;
            }

        }

        public void SetItemLabel(bool flag)
        {
            if (flag)
            {
                for (int i = 0; i < pointItems.Count; i++)
                {
                    pointItems[i].transform.Find("num").gameObject.SetActive(flag);
                    pointItems[i].transform.Find("num").GetComponent<RectTransform>().anchoredPosition = dataLabelsOffset;
                }
            }
            else
            {
                for (int i = 0; i < pointItems.Count; i++)
                {
                    pointItems[i].transform.Find("num").GetComponent<TextMeshProUGUI>().text = "";
                    pointItems[i].transform.Find("num").gameObject.SetActive(flag);
                }
            }
        }
        public void OnPlayBarAnimator(List<float> height, float maxHightData, float maxHight, bool isReset = false)
        {
            switch (aType)
            {               
                case AnimationType.onebyone:

                    if (onebyoneindex < pointItems.Count)
                    {
                        float ss = moveHigh(height[onebyoneindex], maxHightData, maxHight);
                        pointItems[onebyoneindex].DOAnchorPosY(ss, animSpeed);
                        DOTween.Sequence().Append(DOTween.To(delegate (float value)
                        {
                            if (onebyoneindex != 0 && onebyoneindex != pointItems.Count )                              
                                {
                                    GetLinePosition(lineItems[onebyoneindex-1], pointItems[onebyoneindex].localPosition, pointItems[onebyoneindex  -1].localPosition);
                                }
                        }, 0, 1, animSpeed)).OnComplete(()=> {

                            onebyoneindex++;
                            OnPlayBarAnimator(height, maxHightData, maxHight);
                        });
                    }
                    break;
                case AnimationType.all:
                    DOTween.Sequence().Append(DOTween.To(delegate (float value) {
                        for (int i = 0; i < pointItems.Count; i++)
                        {
                            float hh = moveHigh(height[i], maxHightData, maxHight);
                            pointItems[i].anchoredPosition = new Vector2(pointItems[i].anchoredPosition.x, hh * value);
                            if (i != pointItems.Count - 1)
                                GetLinePosition(lineItems[i], pointItems[i].localPosition, pointItems[i + 1].localPosition);
                        }
                    }, 0, 1, animSpeed));
                    break;
            }
        }
        private float moveHigh(float data, float maxD, float maxH)
        {
            return data * maxH / maxD;
        }

        public static Vector3 GetBetweenPoint(Vector3 start, Vector3 end, float percent)
        {
            Vector3 normal = (end - start).normalized;
            float distance = Vector3.Distance(start, end);
            return normal * (distance * percent) + start;
        }
        public static void GetLinePosition(Transform trans, Vector3 v1, Vector3 v2, bool isWorld = false, float lineW = 2, float mul = 1 / 0.00462963f)
        {
#if UNITY_STANDALONE_OSX||UNITY_IOS
            mul = 1 / 0.003473917f;
#endif
            if (!isWorld)
            {
                trans.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(v1.y - v2.y, v1.x - v2.x) * Mathf.Rad2Deg);
                trans.localPosition = GetBetweenPoint(v1, v2, 0.5f);
                float distance = Vector3.Distance(v1, v2);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(distance, lineW);
            }
            else
            {
                trans.eulerAngles = new Vector3(0, 0, Mathf.Atan2(v1.y - v2.y, v1.x - v2.x) * Mathf.Rad2Deg);
                trans.position = GetBetweenPoint(v1, v2, 0.5f);
                float distance = Vector3.Distance(v1, v2) * mul;
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(distance, lineW);
                Debug.Log(new Vector2(distance, lineW));

            }

        }
    }
}

