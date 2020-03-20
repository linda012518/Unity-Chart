using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
///词云组件
/// </summary>
public class WordCloudWidget : MonoBehaviour
{
    private bool isStop;
    List<TextMeshProUGUI> textS = new List<TextMeshProUGUI>();
    public void WordCloud(float maxDelay, List<string> cloudStr)
    {
        StopAnimation();
           isStop = false;
        int count = transform.childCount;
        if (count == 0)
        {
            Debug.Log("没有字体列表,请预制字体列表TextMeshProUGUI");
            return;
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                textS.Add(transform.GetChild(i).GetComponent<TextMeshProUGUI>());
            }
        }
        for (int i = 0; i < count; i++)
        {
            Loop(textS[i], Random.Range(maxDelay/4,maxDelay), cloudStr);
        }
    }
    public void StopAnimation()
    {
        isStop = true;
        for (int i = 0; i < textS.Count; i++)
        {
            DOTween.Kill(textS[i]);
            textS[i].transform.localScale = Vector3.one;
            textS[i].text = "";
        }
    }
    void Loop(TextMeshProUGUI text, float delay, List<string> cloudStr)
    {
        if (isStop)
            return;
        Color color = Color.white;
        text.transform.localScale = Vector3.zero;
        text.text = cloudStr[Random.Range(0, cloudStr.Count)];

        color.r = Random.Range(0.0f, 1.0f);
        color.g = Random.Range(0.0f, 1.0f);
        color.b = Random.Range(0.0f, 1.0f);
        color.a = Random.Range(0.7f, 1);
        text.color = color;
        text.transform.DOScale(Random.Range(1, 2), 2).OnComplete(() => text.transform.DOScale(0, 2).OnComplete(() => Loop(text, delay, cloudStr))).SetDelay(delay);
    }
}
