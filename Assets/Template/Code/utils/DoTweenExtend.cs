using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Utils
{
    public static class DoTweenExtend 
    {
        public enum Axial
        {
            None, X, Y, Z
        }

        static Sequence se = DOTween.Sequence();

        /// <summary>往复缩放</summary>
        public static Tweener LoopScale(this Transform transform, Axial axial, float duration, float start = 0, float end = 1, float delay = 0)
        {
            switch (axial)
            {
                case Axial.None:
                    return transform.DOScale(end, duration).SetDelay(delay).OnComplete(delegate ()
                    {
                       // transform.SetLocalScale(start);
                        LoopScale(transform, axial, duration, start, end);
                    });
                case Axial.X:
                    return transform.DOScaleX(end, duration).SetDelay(delay).OnComplete(delegate ()
                    {
                       // transform.SetLocalScale(start, null, null);
                        LoopScale(transform, axial, duration, start, end);
                    });
                case Axial.Y:
                    return transform.DOScaleY(end, duration).SetDelay(delay).OnComplete(delegate ()
                    {
                        //transform.SetLocalScale(null, start, null);
                        LoopScale(transform, axial, duration, start, end);
                    });
                case Axial.Z:
                    return transform.DOScaleZ(end, duration).SetDelay(delay).OnComplete(delegate ()
                    {
                        //transform.SetLocalScale(null, null, start);
                        LoopScale(transform, axial, duration, start, end);
                    });
            }
            return null;
        }


        public static Sequence UpdateText(this TextMeshProUGUI text, float endValue, float during, string unit = "", bool flag = false, string style = "")
        {

            se.SetAutoKill(false);
            if (style == "")
            {
                style = GetStandardStr(endValue + "");
            }
            se.Join(DOTween.To(delegate (float value)
            {
                if (!flag)
                {
                    text.text = value.ToString("0.0").Split('.')[0] + unit;
                }
                else
                {

                    text.text = value.ToString(style) + unit;
                }


            }, 0, endValue, during));
            return se;
        }
        
        public static string GetStandardStr(string s)
        {
            StringBuilder ss = new StringBuilder();
            int index = s.IndexOf('.');
            if (index == -1)
                ss.Append("0");
            else if (index > 0)
            {
                int num = s.Length - index - 1;
                for (int i = 0; i < num + 1; i++)
                {
                    if (i == 0)
                        ss.Append("0.");
                    else
                        ss.Append("0");
                }
            }
            return ss.ToString();
        }
    }
}
