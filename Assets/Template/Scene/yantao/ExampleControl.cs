using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class ExampleControl : MonoBehaviour
    {
        public Button reset;

        public PieGraphI pie;
        public Histogram histogram,histogram2;
        public BrokenGraph broken;
        [SerializeField]List<float> PieData = new  List<float>(){0.26f,0.15f,0.16f,0.43f};
        [SerializeField]List<float> LineData = new List<float>() { 15, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75 };
        [SerializeField]List<float> histogramData = new List<float>() { 49, 29, 95, 48, 56, 71 };
        [SerializeField]List<float> histogramData2 = new List<float>() { 0.49f, 0.229f, 0.95f, 0.48f, 0.56f, 0.71f };
        [SerializeField] float animspeed;
        // Use this for initialization
        void Start()
        {
            //饼图
            pie.PieAreaControll(PieData, animspeed);
            //柱状图
            histogram.OnPlayBarAnimator(histogramData);
            histogram2.OnPlayBarAnimator(histogramData2, "", true, true, "0.00");
            //折线图
            broken.OnPlayBarAnimator(LineData, 100, 800);
            reset.onClick.AddListener(Reload);
        }

        void Reload()
        {
            pie.ResetState();
            histogram.ResetState();
            broken.ResetState();
            Invoke("EnterAnimation", 0.5f);
        }

        void EnterAnimation()
        {
            pie.PieAreaControll(PieData, animspeed);
            histogram.OnPlayBarAnimator(histogramData);
            histogram2.OnPlayBarAnimator(histogramData2,"",true,true,"0.00");
            broken.OnPlayBarAnimator(LineData, 100, 800);
        }
       
    }
}
