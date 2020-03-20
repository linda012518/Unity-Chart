using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class Histogram : MonoBehaviour
    {
        public enum AnimationType
        {
            Onebyone,//依次增长
            SameTime,//同时增长
            InverseOnebyone,//反向依次增长
        }
        public AnimationType aType;
        [Header("是否显示文字")]
        public bool useLabel;
        public Vector2 dataLabelsOffset;
        public Vector3 dataLabelsRotation;

        [Header("是否开启颜色修改")]
        public bool useColor;
        public Color[] Colors;

        [Space(10)]
        [Header("柱状图")]
        [SerializeField]
        private Image[] barItems;
        [SerializeField]
        private Sprite itemSp;
        [SerializeField]
        private float barWidth;
        [Tooltip("数据最大值")]
        [Header("数据最大值")]
        public float MaxDataV;
        [Tooltip("图表高度最大值")]
        [Header("图表高度最大值")]
        public float MaxHeight;

        private int onebyoneindex;
        private int Ionebyoneindex;
        public float animSpeed;

        List<Tween> tweeners = new List<Tween>();

        void Awake()
        {
            Init();
        }

        void Init()
        {
            onebyoneindex = 0;
            Ionebyoneindex = barItems.Length - 1;

            if (useColor)
            {
                SetItemColor();

            }
            SetItemImage();
            SetItemLabel(useLabel);
        }
        public void SetItemColor()
        {
            for (int i = 0; i < barItems.Length; i++)
            {
                barItems[i].color = Colors[i];
            }
        }

        public void SetItemImage()
        {
            for (int i = 0; i < barItems.Length; i++)
            {
                barItems[i].sprite = itemSp;
            }
        }

        public void SetItemLabel(bool flag)
        {
            if (flag)
            {
                for (int i = 0; i < barItems.Length; i++)
                {
                    barItems[i].transform.Find("num").GetComponent<TextMeshProUGUI>().text = "";
                    barItems[i].transform.Find("num").GetComponent<RectTransform>().anchoredPosition = dataLabelsOffset;
                    barItems[i].transform.Find("num").GetComponent<RectTransform>().localEulerAngles = dataLabelsRotation;

                }
            }
            else
            {
                for (int i = 0; i < barItems.Length; i++)
                {
                    barItems[i].transform.Find("num").GetComponent<TextMeshProUGUI>().text = "";
                    barItems[i].transform.Find("num").gameObject.SetActive(false);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="barValues"==数据></param>
        /// <param name="unit"==单位></param>
        /// <param name="isFloat" ==参数是否为小数></param>
        /// <param name="isReset"==是否重新加载></param>
        /// <param name="style"==小数位 ></param>
        public void OnPlayBarAnimator( List<float> barValues,   string unit = "", bool isFloat = false, bool isReset = false,string style = "")
        {
            float multiple = barItems.Length;
            float dhight = 0;
            if (isReset)
                ResetState();
            List<float> Height = new  List<float>();
            for (int i = 0; i < barValues.Count; i++)
            {
                Height.Add(CalculateCon(barValues[i], MaxDataV, MaxHeight));
            }
            switch (aType)
            {
                case AnimationType.InverseOnebyone:
                    if (Ionebyoneindex >= 0)
                    {
                        if (Height[Ionebyoneindex] > MaxHeight)
                            dhight = MaxHeight;
                        else
                            dhight = Height[Ionebyoneindex];
                        Tweener tw1 = barItems[Ionebyoneindex].GetComponent<RectTransform>().DOSizeDelta(new Vector2(barWidth, dhight), animSpeed / multiple).OnComplete(delegate { Ionebyoneindex--; OnPlayBarAnimator( barValues,   unit, isFloat); });
                        barItems[onebyoneindex].transform.Find("num").GetComponent<TextMeshProUGUI>().text = "";
                        barItems[Ionebyoneindex].transform.Find("num").GetComponent<TextMeshProUGUI>().UpdateText(barValues[Ionebyoneindex], animSpeed / multiple, unit, isFloat, style);
                        tweeners.Add(tw1);
                    }
                    break;
                case AnimationType.Onebyone:
                    if (onebyoneindex < barItems.Length)
                    {
                        if (Height[onebyoneindex] > MaxHeight)
                            dhight = MaxHeight;
                        else
                            dhight = Height[onebyoneindex];
                        Tweener tw1 = barItems[onebyoneindex].GetComponent<RectTransform>().DOSizeDelta(new Vector2(barWidth, dhight), animSpeed / multiple).OnComplete(delegate { onebyoneindex++; OnPlayBarAnimator( barValues,   unit, isFloat); });
                        barItems[onebyoneindex].transform.Find("num").GetComponent<TextMeshProUGUI>().text = "";
                        barItems[onebyoneindex].transform.Find("num").GetComponent<TextMeshProUGUI>().UpdateText(barValues[onebyoneindex], animSpeed / multiple, unit, isFloat, style);
                        tweeners.Add(tw1);
                    }
                    break;
                case AnimationType.SameTime:
                    for (int i = 0; i < barItems.Length; i++)
                    {
                        if (Height[i] > MaxHeight)
                            dhight = MaxHeight;
                        else
                            dhight = Height[i];
                        barItems[onebyoneindex].transform.Find("num").GetComponent<TextMeshProUGUI>().text = "";
                        barItems[i].transform.Find("num").GetComponent<TextMeshProUGUI>().UpdateText(barValues[i], animSpeed, unit, isFloat, style);
                        Tweener tw1 = null;
                        if (barValues[i] < 0)
                        {
                            barItems[i].rectTransform.localEulerAngles = new Vector3(0, 0, 180);
                            tw1 = barItems[i].GetComponent<RectTransform>().DOSizeDelta(new Vector2(barWidth, -dhight), animSpeed);
                        }
                        else
                        {
                            barItems[i].rectTransform.localEulerAngles = new Vector3(0, 0, 0);
                            tw1 = barItems[i].GetComponent<RectTransform>().DOSizeDelta(new Vector2(barWidth, dhight), animSpeed);
                        }
                        tweeners.Add(tw1);
                    }
                    break;
            }
        }

        /// <summary>
        /// 计算图形占比（柱状图）
        /// </summary>
        /// <param name="maxD">数据最大值</param>
        /// <param name="maxH">图形最大高度</param>
        /// <returns></returns>
        public static float CalculateCon(float data, float maxD, float maxH)
        {
            float aa = data * maxH / maxD;
            return aa;
        }
        public void ResetState()
        {
            onebyoneindex = 0;
            Ionebyoneindex = barItems.Length - 1;

            SetItemLabel(useLabel);

            for (int i = 0; i < barItems.Length; i++)
            {
                barItems[i].GetComponent<RectTransform>().sizeDelta = new Vector2(barWidth, 0);
                //barItems[i].transform.GetChild<TextMeshProUGUI>("num").text = "";
            }                   
        }     
    }
}
