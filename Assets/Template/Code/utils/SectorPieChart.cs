using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
namespace Utils {
    /// <summary>
    /// 扇形饼图
    /// </summary>
    public class SectorPieChart : BaseImage
    {
        public float minRadius = 60;
        public int startAngle = 0;
#if UNITY_EDITOR
        [SerializeField]
        List<int> sectorAngles = new List<int>();
        [SerializeField]
        List<float> sectorHeight = new List<float>();
        [SerializeField]
        List<Color> colos = new List<Color>();
#endif
        const int circleAngle = 360;
        Vector2[] near;
        Vector2[] far;
        float[] maxRadius;
        Color[] _color;
        int _totalAngle;
        public void ShowAnimation(List<int> sectorAngles, List<float> sectorHeight, List<Color> colos,float during,AnimType at=AnimType.two) {
            List<int> angles = new List<int>();
            for (int i = 0; i < sectorAngles.Count; i++)
            {
                angles.Add(0);
            }
            List<float> heights = new List<float>();
            for (int i = 0; i < sectorHeight.Count; i++)
            {
                heights.Add(0);
            }
            switch (at) {
                case AnimType.none:
                    Paint(sectorAngles, sectorHeight, colos);
                    break;
                case AnimType.one:
                    DOTween.Sequence().Append(DOTween.To(delegate (float value) {
                        for (int i = 0; i < sectorAngles.Count; i++)
                        {                        
                            heights[i] = sectorHeight[i] * value;
                        }
                        Paint(sectorAngles, heights, colos);
                    }, 0, 1, during));
                    break;
                case AnimType.two:
                    DOTween.Sequence().Append(DOTween.To(delegate (float value) {
                        for (int i = 0; i < sectorAngles.Count; i++)
                        {
                            angles[i] = Mathf.CeilToInt(sectorAngles[i] * value);                          
                        }
                        Paint(angles, sectorHeight, colos);
                    }, 0, 1, during));
                    break;
                case AnimType.three:
                    DOTween.Sequence().Append(DOTween.To(delegate (float value) {
                        for (int i = 0; i < sectorAngles.Count; i++)
                        {
                            angles[i] = Mathf.CeilToInt(sectorAngles[i] * value);
                            heights[i] = sectorHeight[i] * value;
                        }
                        Paint(angles, heights, colos);
                    }, 0, 1, during));
                    break;
            }
        }
        /// <summary>
        /// 初始化扇形饼图
        /// </summary>
        /// <param name="sectorAngles">扇形角度集合</param>
        /// <param name="sectorHeight">扇形高度集合</param>
        /// <param name="colos">扇形颜色集合</param>
        public void Paint(List<int> sectorAngles, List<float> sectorHeight, List<Color> colos)
        {
            if (sectorAngles.Count != sectorHeight.Count||sectorAngles.Count != colos.Count|| sectorHeight.Count!= colos.Count)
            {
                UnityEngine.Debug.Log("数据不匹配，请检查数据");
                return;
            }
                int cout = sectorHeight.Count;
                _totalAngle = circleAngle + cout;
                near = new Vector2[_totalAngle];
                far = new Vector2[_totalAngle];
                maxRadius = new float[_totalAngle];
                _color = new Color[_totalAngle];
            try {
                InitPie(sectorAngles, sectorHeight, colos); }
            catch (System.Exception e) {
                Debug.Log("数组越界："+e.Message);
            }
                
                SetAllDirty();  
        }
        void InitPie(List<int> sectorAngles, List<float> sectorHeight, List <Color> colos)
        {          
            int count = sectorAngles.Count;
            Vector2 temp = new Vector2(0, minRadius);
            near[0] = Quaternion.Euler(0, 0, startAngle) * temp;
            Quaternion q = Quaternion.identity;
            int startAngles=0;
            int totalngles = 0;
            for (int i=0;i<count;i++) {
                int ii = i != 0 ? 1 : 0;
                startAngles = totalngles+ ii;
                totalngles += sectorAngles[i]+ii;
                for (int j = startAngles; j <= totalngles; j++)
                {
                    q = Quaternion.Euler(0, 0, j- i);
                    _color[j] = colos[i];
                    maxRadius[j] = sectorHeight[i]+minRadius;
                    near[j] = q * near[0];
                    far[j] = near[j].normalized * maxRadius[j];
                }
            }
        }
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            UIVertex vert = UIVertex.simpleVert;

            for (int i = 0; i < _totalAngle; i++)
            {
                vert.position = near[i];
                vert.color = _color[i];
                vh.AddVert(vert);

                vert.position = far[i];
                vert.color = _color[i];
                vh.AddVert(vert);
            }
            int amount = (_totalAngle - 1) * 2;
            for (int i = 0; i < amount; i++)
            {
                vh.AddTriangle(i, i + 1, i + 2);
            }
            SetNativeSize();
        }
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            Paint(sectorAngles, sectorHeight, colos);
        }

#endif
    }
}

