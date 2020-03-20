using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 闪烁组件
/// </summary>
public class TwinkleWidget : MonoBehaviour {
    public enum fanxian
    {
        top,
        bottom
    }
    public fanxian fx;
    public List<Image> imgs = new List<Image>();
    public Color startColor;
    public Color endColor;
    public float speed = 0.08f;
    public float druing = 16;
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            imgs.Add(transform.GetChild(i).GetComponent<Image>());
        }
        for (int i = 0; i < imgs.Count; i++)
        {
            Color col = Color.Lerp(startColor, endColor, (i + 1) / druing);
            imgs[i].color = col;
        }
    }

    void OnEnable()
    {
        if (fx == fanxian.top)
            InvokeRepeating("TopToBottom", 0.1f, speed);
        else if (fx == fanxian.bottom)
            InvokeRepeating("BottomToTop", 0.1f, speed);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
    void TopToBottom()
    {
        for (int i = 0; i < imgs.Count; i++)
        {
            if (i == imgs.Count - 1)
                imgs[i].color = imgs[0].color;
            else
                imgs[i].color = imgs[i + 1].color;
        }
    }
    void BottomToTop()
    {
        for (int i = imgs.Count - 1; i >= 0; i--)
        {
            if (i == 0)
                imgs[i].color = imgs[imgs.Count - 1].color;
            else
                imgs[i].color = imgs[i - 1].color;
        }
    }
}
