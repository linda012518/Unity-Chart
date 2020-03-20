using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Utils {
    /// <summary>
    /// 折线填充图
    /// </summary>
    public class BrokenLineFillChart : BaseImage
    {     
        [Header("bar宽")]
        public float width;
        [Header("bar高")]
        public float barHeight = 100;
        Color colorB;
        Vector2[] buttom;
        Vector2[] top;
        Color[] _color;
        int totalVector;
        public List<float> groups = new List<float>();
       

        public void ShowAnimation(List<float> groups, float min, float max, float during,AnimType at = AnimType.none) {
            List<float> temps = new List<float>();
            for (int i = 0; i < groups.Count; i++)
            {
                temps.Add(0);
            }
            switch (at)
            {
                case AnimType.none:
                    Paint(groups, min, max);
                    break;
                default:
                    DOTween.Sequence().Append(DOTween.To(delegate (float value) {
                        for (int i = 0; i < groups.Count; i++)
                        {
                            temps[i] = groups[i] * value;
                        }
                        Paint(temps, min, max);
                    }, 0, 1, during));
                    break;
            }
        }
        /// <summary>
        /// 画图
        /// </summary>
        /// <param name="groups">数据</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public void Paint(List<float> groups,float min,float max) {

            this.groups.Clear();
            float temp=0;
            for (int i=0;i< groups.Count;i++) {
                temp = (groups[i]-min) / (max - min)* barHeight;
                this.groups.Add(temp);
            }         
            InitChart();
        }
        private void InitChart() {
            colorB = color;
            colorB.a = 0;
            totalVector = groups.Count;
            buttom =new Vector2[totalVector];
            top = new Vector2[totalVector];
            _color = new Color[totalVector];
            float widthS = width * (totalVector-1);
            rectTransform.sizeDelta = new Vector2(widthS, barHeight);
            for (int i=0;i<totalVector;i++){              
                _color[i] = new Color(colorB.r, colorB.g, colorB.b, groups[i]/ barHeight);
                buttom[i] =  new Vector2(width * i, 0) - new Vector2(widthS, barHeight) / 2;
                top[i] = new Vector2(width * i, groups[i]) - new Vector2(widthS, barHeight) / 2;
            }
            SetAllDirty();
        }
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            UIVertex vert = UIVertex.simpleVert;
            for (int i = 0; i < totalVector; i++)
            {
                vert.position = buttom[i];
                vert.color = colorB;
                vh.AddVert(vert);

                vert.position = top[i];
                vert.color = _color[i];
                vh.AddVert(vert);
            }
            for (int i = 0; i < (totalVector - 1)*2; i++)
            {
                vh.AddTriangle(i, i + 1, i + 2);
            }
        }
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            InitChart();
            SetNativeSize();
        }
#endif
    }
}

