using UnityEngine;
using DG.Tweening;

public static class TransformExtend
{
    public static void Rotate2Target(this RectTransform transform,Vector2 target, float startAngle,float totalAngle,float duringTime) {
        totalAngle = totalAngle > 360 ? totalAngle % 360 : totalAngle;
        Quaternion q = Quaternion.identity;
        DOTween.Sequence().Append(DOTween.To (delegate(float value) {
            q = Quaternion.Euler(0, 0, value);
            transform.anchoredPosition = q * target;
            transform.localEulerAngles = q.eulerAngles;
        }, startAngle, totalAngle, duringTime)); 
    }
}
