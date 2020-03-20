using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using DG.Tweening;
public class GraphSimple : MonoBehaviour {
    RawImage qrcodeRaw;
    SectorPieChart sectorPieChart;
    RadarChart  radarChart;
    PointWidget pointChart;
    BrokenLineFillChart brokenLineFillChart;
    CircleWidget circleWidget;
    RectTransform rect;
    WordCloudWidget wordCloudWidget;
    InputField InputField;
    float animSpeed=2;
    InputField text;
    InputField point;
    // Use this for initialization
    void Start () {
        text = transform.Find("Text").GetComponent<InputField>();
        point = transform.Find("point").GetComponent<InputField>();
        qrcodeRaw = transform.Find("QrCode").GetComponent<RawImage>();
        qrcodeRaw.texture = QRCodeInterface.GenerateQrCode("QWER", QRCoder.QRCodeGenerator.ECCLevel.H);
        sectorPieChart = transform.Find("SectorPieChart").GetComponent<SectorPieChart>();
        radarChart = transform.Find("RadarChart").GetComponent<RadarChart>();
        pointChart= transform.Find("PointChart").GetComponent<PointWidget>();
        brokenLineFillChart = transform.Find("BrokenLineFillChart").GetComponent<BrokenLineFillChart>();
        circleWidget = transform.Find("CircleWidget").GetComponent<CircleWidget>();
        rect = transform.Find("image/Rotate2Target").GetComponent<RectTransform>();
        wordCloudWidget = transform.Find("WordCloudWidget").GetComponent<WordCloudWidget>();
        InputField = transform.Find("InputField").GetComponent<InputField>();      
        point.text = 74 + "";
        pointChart.ShowProgress(74,2);
        PaintPie();
        PaintRadar();
        BrokenLineFillChart();
        CircleWidget();
        WordCloudWidget();
    }	
    void PaintPie() {
        List<int> sectorAngles = new List<int>() { 60, 80, 70, 50, 100 };
        List<float> sectorHeight = new List<float>() { 30, 120, 60, 40, 90 };
        List<Color> colos = new List<Color>() { Color.red, Color.green, Color.blue, Color.yellow, Color.gray };
        sectorPieChart.Paint(sectorAngles, sectorHeight, colos);
    }
    void PaintRadar()
    {
        List<float> sectorHeight = new List<float>() { 0.5f, 0.8f, 0.7f, 0.6f, 0.5f, 0.4f };
        radarChart.Paint(sectorHeight);
    }
    void BrokenLineFillChart()
    {
        List<float> data = new List<float>() { 30,60,70,75,80,64,46,50};
        brokenLineFillChart.Paint(data,0,100);
    }
    void CircleWidget()
    {
        text.text = 0.23f + "";
        Vector2 v2 = new Vector2(0, 30);
        rect.Rotate2Target(v2, 0, 0.23f*360, 0.5f);
        circleWidget.ShowAnimation(0.5f, 0.23f);
    }
    void WordCloudWidget() {
        List<string> strs = new List<string>() { "巨州云", "大数据", "AI", "VR", "AR", "MR", "ERP", "OA", "SAAS", "PAAS", "物业", "鑫苑" };
        wordCloudWidget.WordCloud(2,strs);
    }
    #region 动画
    public void QrCode() {
        qrcodeRaw.texture = QRCodeInterface.GenerateQrCode(InputField.text, QRCoder.QRCodeGenerator.ECCLevel.H);
    }
    int index = 0;
    public void PaintPieAnimator() {
        List<int> sectorAngles = new List<int>() { 60, 85, 70, 70, 75 };
        List<float> sectorHeight = new List<float>() { 30, 120, 160, 40, 90 };
        List<Color> colos = new List<Color>() { Color.red, Color.green, Color.blue, Color.yellow, Color.gray };
        AnimType at = AnimType.none;
        index++;
        if (index > 3)
            index = 0;
        switch (index) {
            case 0:
                at = AnimType.none;
                break;
            case 1:
                at = AnimType.one;
                break;
            case 2:
                at = AnimType.two;
                break;
            case 3:
                at = AnimType.three;
                break;
        }
        sectorPieChart.ShowAnimation(sectorAngles, sectorHeight, colos, animSpeed, at);
    }
    public void PaintRadarAnimator() {
        List<float> sectorHeight = new List<float>() { 0.5f, 0.8f, 0.7f, 0.6f, 0.5f, 0.4f};
        radarChart.ShowAnimation(sectorHeight,animSpeed,AnimType.one);
    }
    public void BrokenLineFillChartAnimator()
    {
        List<float> data = new List<float>() { 30, 60, 70, 75, 80, 64, 46, 50 };
        brokenLineFillChart.ShowAnimation(data, 0, 100,animSpeed,AnimType.one);
    }
    public void CircleWidgetAnimator()
    {

        float value = float.Parse(text.text);
        Vector2 v2 = new Vector2(0, 30);
        rect.Rotate2Target(v2, 0, value * 360, 0.5f);
        circleWidget.ShowAnimation(0.5f, value);
    }
    public void PointWidgetAnimator() {
        float value= float.Parse(point.text);
        pointChart.ShowProgress(value, 0.5f);
    }
    public void WordCloudWidgetAnimator()
    {
        List<string> strs = new List<string>() { "巨州云", "大数据", "AI", "VR", "AR", "MR", "ERP", "OA", "SAAS", "PAAS", "物业", "鑫苑" };
        wordCloudWidget.WordCloud(2, strs);
    }
    #endregion
}
