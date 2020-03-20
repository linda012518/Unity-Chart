using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public static class TransformTool
    {
        /// <summary>查找子物体</summary>
        public static T GetChild<T>(this Transform transform, string childName) where T : Component
        {
            Transform go = transform.Find(childName);

            if (go != null) return go.GetComponent<T>();

            T target = default(T);

            for (int i = 0; i < transform.childCount; i++)
            {
                target = transform.GetChild(i).GetChild<T>(childName);
                if (target != null)
                    return target;
            }

            return default(T);
        }
        /// <summary>查找子物体</summary>
        public static T[] GetChild<T>(this Transform transform)
        {
            T[] temp = new T[transform.childCount];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = transform.GetChild(i).GetComponent<T>();
            }
            return temp;
        }

        /// <summary>查找子物体</summary>
        public static List<T> GetChildToList<T>(this Transform transform)
        {
            List<T> temp = new List<T>(transform.childCount);
            for (int i = 0; i < transform.childCount; i++)
            {
                temp.Add(transform.GetChild(i).GetComponent<T>());
            }
            return temp;
        }


        private static Vector3 temporaryVector3 = Vector3.zero;
        private static Color temporaryColor = Color.white;

        /// <summary>设置颜色</summary>
        public static void SetColor(this Image image, float? r, float? g, float? b, float? a)
        {
            if (r == null) temporaryColor.r = image.color.r; else temporaryColor.r = r.Value;
            if (g == null) temporaryColor.g = image.color.g; else temporaryColor.g = g.Value;
            if (b == null) temporaryColor.b = image.color.b; else temporaryColor.b = b.Value;
            if (a == null) temporaryColor.a = image.color.a; else temporaryColor.a = a.Value;
            image.color = temporaryColor;
        }

        /// <summary>设置世界坐标</summary>
        public static void SetPosition(this Transform transform, float? x, float? y, float? z)
        {
            if (x == null) temporaryVector3.x = transform.position.x; else temporaryVector3.x = x.Value;
            if (y == null) temporaryVector3.y = transform.position.y; else temporaryVector3.y = y.Value;
            if (z == null) temporaryVector3.z = transform.position.z; else temporaryVector3.z = z.Value;
            transform.position = temporaryVector3;
        }
        /// <summary>设置自身坐标</summary>
        public static void SetLocalPosition(this Transform transform, float? x, float? y, float? z)
        {
            if (x == null) temporaryVector3.x = transform.localPosition.x; else temporaryVector3.x = x.Value;
            if (y == null) temporaryVector3.y = transform.localPosition.y; else temporaryVector3.y = y.Value;
            if (z == null) temporaryVector3.z = transform.localPosition.z; else temporaryVector3.z = z.Value;
            transform.localPosition = temporaryVector3;
        }
        /// <summary>设置世界旋转</summary>
        public static void SetEulerAngles(this Transform transform, float? x, float? y, float? z)
        {
            if (x == null) temporaryVector3.x = transform.eulerAngles.x; else temporaryVector3.x = x.Value;
            if (y == null) temporaryVector3.y = transform.eulerAngles.y; else temporaryVector3.y = y.Value;
            if (z == null) temporaryVector3.z = transform.eulerAngles.z; else temporaryVector3.z = z.Value;
            transform.eulerAngles = temporaryVector3;
        }
        /// <summary>设置自身旋转</summary>
        public static void SetLocalEulerAngles(this Transform transform, float? x, float? y, float? z)
        {
            if (x == null) temporaryVector3.x = transform.localEulerAngles.x; else temporaryVector3.x = x.Value;
            if (y == null) temporaryVector3.y = transform.localEulerAngles.y; else temporaryVector3.y = y.Value;
            if (z == null) temporaryVector3.z = transform.localEulerAngles.z; else temporaryVector3.z = z.Value;
            transform.localEulerAngles = temporaryVector3;
        }
        /// <summary>设置自身缩放</summary>
        public static void SetLocalScale(this Transform transform, float? x, float? y, float? z)
        {
            if (x == null) temporaryVector3.x = transform.localScale.x; else temporaryVector3.x = x.Value;
            if (y == null) temporaryVector3.y = transform.localScale.y; else temporaryVector3.y = y.Value;
            if (z == null) temporaryVector3.z = transform.localScale.z; else temporaryVector3.z = z.Value;
            transform.localScale = temporaryVector3;
        }
        /// <summary>设置自身缩放</summary>
        public static void SetLocalScale(this Transform transform, float value)
        {
            temporaryVector3.x = value;
            temporaryVector3.y = value;
            temporaryVector3.z = value;
            transform.localScale = temporaryVector3;
        }

        private static Vector2 temporaryVector2 = Vector2.zero;
        public static void SetAnchoredPosition(this RectTransform rectTransform, float? x, float? y)
        {
            if (x == null) temporaryVector2.x = rectTransform.anchoredPosition.x; else temporaryVector2.x = x.Value;
            if (y == null) temporaryVector2.y = rectTransform.anchoredPosition.y; else temporaryVector2.y = y.Value;
            rectTransform.anchoredPosition = temporaryVector2;
        }

        public static void Rotate2Target(this RectTransform transform, Vector2 target, float startAngle, float totalAngle, float duringTime)
        {
            totalAngle = totalAngle > 360 ? totalAngle % 360 : totalAngle;
            Quaternion q = Quaternion.identity;
            DOTween.Sequence().Append(DOTween.To(delegate (float value) {
                q = Quaternion.Euler(0, 0, value);
                transform.anchoredPosition = q * target;
                transform.localEulerAngles = q.eulerAngles;
            }, startAngle, totalAngle, duringTime));
        }
    }
}