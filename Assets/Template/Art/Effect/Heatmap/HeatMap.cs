
using UnityEngine;
using System.Collections.Generic;

namespace Utils.Effect
{

    public class HeatMap : MonoBehaviour
    {

        private Material _material = null;

        public Material Material
        {
            get
            {
                if (null == _material)
                {
                    var render = this.GetComponent<Renderer>();
                    if (null == render)
                        Debug.LogError("HeatMapComponent can not found Renderer lines 26");
                    _material = render.material;
                }
                return _material;
            }
        }
        
        // 热力影响半径
        public float _influenceRadius = 3.0f;

        // 亮度
        public float _intensity = 3.0f;

        List<Transform> impactFactors;

        List<T> GetChild<T>(Transform transform)
        {
            List<T> temp = new List<T>(transform.childCount);
            for (int i = 0; i < transform.childCount; i++)
            {
                temp.Add(transform.GetChild(i).GetComponent<T>());
            }
            return temp;
        }

        int _childCount;

        private void Start()
        {
            _childCount = transform.childCount;
            impactFactors = GetChild<Transform>(transform);

            _properties = new Vector4();

            Material.SetVectorArray("_Factors", new Vector4[100]);
        }

        private void Update()
        {
            if (_childCount != transform.childCount)
                Start();
            RefreshHeatmap();
        }

        Vector4 _properties;

        private void RefreshHeatmap()
        {
            Material.SetInt("_FactorCount", _childCount);

            Vector4[] ifPosition = new Vector4[_childCount];
            for (int i = 0; i < _childCount; i++)
                ifPosition[i] = impactFactors[i].transform.position;
            Material.SetVectorArray("_Factors", ifPosition);

            _properties.x = _influenceRadius;
            _properties.y = _intensity;
            Material.SetVector("_FactorsProperties", _properties);
        }
    }
}