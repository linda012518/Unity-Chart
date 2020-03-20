using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Utils
{
    public class PieGraphI : MonoBehaviour
    {
        [Header("饼图")] public List<Image> rings = new List<Image>();
        [Tooltip("圆角高宽")] public Vector2 maxCircleAngle = new Vector2(20, 20);
        [Tooltip("间隔值 小于minValue")] public float interval;
        public float minValue = 0.03f;
        [Tooltip("圆角区域对应的圆环fillamount(半角)")] public float angle = 0.015f; //当前圆环的实测值

        public void ResetState()
        {
            for (int i = 0; i < rings.Count; i++)
            {
                rings[i].transform.Find("start").GetChild(0).GetComponent<RectTransform>().sizeDelta = maxCircleAngle;
                rings[i].transform.Find("end").GetChild(0).GetComponent<RectTransform>().sizeDelta = maxCircleAngle;
                rings[i].transform.Find("start").GetComponent<RectTransform>().localEulerAngles = Vector3.zero;
                rings[i].transform.Find("end").GetComponent<RectTransform>().localEulerAngles = Vector3.zero;
                rings[i].fillAmount = 0;
                rings[i].transform.localEulerAngles = Vector3.zero;
            }
        }

        public void PieAreaControll(List<float> list, float duration)
        {
            if (JudgeInterval(list))
            {
                interval = 0;
            }

            if (interval > minValue)
            {
                interval = minValue;
            }

            float last = 0;
            for (int i = 0; i < rings.Count; i++)
            {
                rings[i].transform.Find("start").localEulerAngles = new Vector3(0, 0, 0);
                rings[i].transform.Find("end").localEulerAngles = new Vector3(0, 0, 0);
                float temp = list[i] - angle * 2 - interval;
                rings[i].DOFillAmount(temp, duration);
                //0.1f 实际测试值0.01对应10,
                if (list[i] < minValue)
                {
                    rings[i].transform.Find("end").GetChild(0).GetComponent<RectTransform>().sizeDelta =
                        maxCircleAngle * list[i] / minValue;
                    rings[i].transform.Find("start").GetChild(0).GetComponent<RectTransform>().sizeDelta =
                        maxCircleAngle * list[i] / minValue;
                    if (i == 0)
                    {
                        rings[i].transform.localEulerAngles = new Vector3(0, 0, 0);
                    }
                    else if (i > 0)
                    {
                        if (list[i - 1] > minValue)
                            rings[i].transform.localEulerAngles = new Vector3(0, 0,
                                rings[i - 1].transform.localEulerAngles.z +
                                (last - angle * (1 - list[i] / minValue)) * 360);
                        else
                            rings[i].transform.localEulerAngles = new Vector3(0, 0,
                                rings[i - 1].transform.localEulerAngles.z +
                                (last + angle * (list[i] / minValue)) * 360);
                    }

                    if (i < list.Count - 1)
                    {
                        if (list[i + 1] > minValue)
                            last = angle * (list[i] / minValue) + angle;
                        else
                            last = angle * (list[i] / minValue);
                    }

                }
                else
                {
                    #region 处理角度大于180的旋转

                    Transform end = rings[i].transform.Find("end");
                    float tempZ = end.localEulerAngles.z;
                    Sequence se = DOTween.Sequence();
                    se.SetAutoKill(false);
                    se.Join(DOTween.To(delegate(float value) { end.localEulerAngles = new Vector3(0, 0, value); },
                        tempZ, -temp * 360, duration));

                    #endregion

                    //   rings[i].transform.Find("end").DOLocalRotate(-new Vector3(0, 0,(ddd * 360)), 2);//只适用于角度小鱼180的旋转
                    if (i == 0)
                    {
                        rings[i].transform.localEulerAngles = new Vector3(0, 0, angle * 360);
                    }
                    else if (i > 0)
                    {
                        rings[i].transform.localEulerAngles = new Vector3(0, 0,
                            rings[i - 1].transform.localEulerAngles.z + last * 360);

                    }

                    last = list[i];
                }

            }
        }

        //判断是否启用间隔值
        bool JudgeInterval(List<float> list)
        {
            bool flag = false;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] < minValue)
                {
                    flag = true;
                    break;
                }
            }

            return flag;
        }
    }
}
