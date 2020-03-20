using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
namespace Utils {
    /// <summary>
    /// 雷达图
    /// </summary>
    public class RadarChart : BaseImage
    {
        private void UpdateState()
        {
            SetAllDirty();
        }
        [SerializeField, Range(0, 1), Header("多边形顶点到中心的距离")] private float[] m_values = new float[] { 1, 1, 1, 1, 1, 1 };
        private float[] values
        {
            get { return m_values; }
            set
            {
                for (int i = 0; i < value.Length; i++)
                {
                    value[i] = Mathf.Clamp01(value[i]);
                }
                m_values = value;
                if (HaveChange(m_values))
                {

                    SetVerticesDirty();
                }
            }
        }
        [SerializeField, Range(0, 360), Header("顶点的偏转角度")] private int m_offsetAngle = 0;
        private int offsetAngle
        {
            get { return m_offsetAngle; }
            set
            {
                m_offsetAngle = Get360Angles(value);
                if (HaveChange(m_offsetAngle))
                {
                    SetVerticesDirty();
                }
            }
        }
        public void ShowAnimation(List<float> groups,float during,AnimType at=AnimType.none,int offsetAngle = 0) {
            List<float> temps = new List<float>();
            for (int i = 0; i < groups.Count; i++)
            {
                temps.Add(0);
            }
            switch (at)
            {
                case AnimType.none:
                    Paint(groups, offsetAngle);
                    break;
                default:
                    DOTween.Sequence().Append(DOTween.To(delegate (float value) {
                        for (int i = 0; i < groups.Count; i++)
                        {
                            temps[i] = groups[i] * value;
                        }
                        Paint(temps, offsetAngle);
                    }, 0, 1, during));
                    break;
            }
        }
        //画图
        public void Paint(List <float> groups,int offsetAngle = 0) {
            SetPolygon(groups.ToArray(), offsetAngle);
        }
        /// <summary>
        /// 当前多边形的顶点距离
        /// </summary>
        private float[] m_nowValues;
        /// <summary>
        /// 当前多边形的顶点偏转角
        /// </summary>
        private int m_nowOffsetAngle;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (HaveChange(values, offsetAngle))
            {
                SetVerticesDirty();
            }
        }
#endif
        /// <summary>
        /// 设置多边形
        /// </summary>
        /// <param name="values">顶点到中心点的距离比例</param>
        /// <param name="offsetAngle">所有点顺时针旋转角度</param>
        private void SetPolygon(float[] values, int offsetAngle = 0)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = Mathf.Clamp01(values[i]);
            }
            offsetAngle = Mathf.Clamp(offsetAngle, -180, 180);

            if (!HaveChange(values, offsetAngle))
            {
                return;
            }

            this.m_values = values;
            this.m_offsetAngle = offsetAngle;
            // 作用：重新绘制
            SetVerticesDirty();
        }

        #region ChangeCheck
        private bool HaveChange(float[] values, int offsetAngle)
        {
            return HaveChange(values) || HaveChange(offsetAngle);
        }

        private bool HaveChange(float[] values)
        {
            if (m_nowValues == null)
            {
                return true;
            }
            if (m_nowValues.Length != values.Length)
            {
                return true;
            }
            for (int i = 0; i < m_nowValues.Length; i++)
            {
                if (m_nowValues[i] != values[i])
                {
                    return true;
                }
            }
            return false;
        }

        private bool HaveChange(int offsetAngle)
        {
            return m_nowOffsetAngle != offsetAngle;
        }
        #endregion

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            Color32 color32 = color;

            // 创建多边形网格
            Mesh mesh = Create(values, offsetAngle);
            toFill.Clear();
            // 顶点和uv
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                toFill.AddVert(mesh.vertices[i], color32, mesh.uv[i]);
            }
            // 三角
            for (int i = 0; i < mesh.triangles.Length / 3; i++)
            {
                toFill.AddTriangle(mesh.triangles[3 * i], mesh.triangles[3 * i + 1], mesh.triangles[3 * i + 2]);
            }


            // 记录当前多边形的值
            m_nowValues = new float[values.Length];
            Array.Copy(values, m_nowValues, values.Length);
            m_nowOffsetAngle = offsetAngle;
        }

        private Mesh Create(float[] values, int offsetAngle)
        {
            Mesh mesh = new Mesh();

            // 多边形单元长度
            float polygonUnit = rectTransform.rect.width < rectTransform.rect.height ? rectTransform.rect.width / 2 : rectTransform.rect.height / 2;

            // 顶点
            Vector3[] vertices = new Vector3[values.Length + 1];
            float unitAngle = 360f / values.Length;
            float angle;
            float distance;
            vertices[0] = Vector3.zero;
            for (int i = 1; i < vertices.Length; i++)
            {
                angle = unitAngle * (i - 1) + offsetAngle;
                distance = values[i - 1];
                vertices[i] = GetPosition(angle, distance);
                vertices[i] = new Vector2(vertices[i].x * rectTransform.rect.width / 2, vertices[i].y * rectTransform.rect.height / 2);
            }
            // 三角形构造方式：中心向四周发散([0,1,2],[0,2,3],[0,3,4]...)
            int[] triangles = new int[3 * (values.Length)];
            int triangleIndex = -1;
            for (int i = 0; i < triangles.Length; i++)
            {
                if (i % 3 == 0)
                {
                    triangles[i] = 0;
                    triangleIndex++;
                }
                else
                {
                    triangles[i] = i - triangleIndex * 2;
                    if (triangles[i] == vertices.Length)
                    {
                        triangles[i] = 1;
                    }
                }
            }
            mesh.vertices = vertices;
            mesh.triangles = triangles;


            // uv
            Vector2[] uvs = new Vector2[vertices.Length];
            for (int i = 0; i < uvs.Length; i++)
            {
                uvs[i] = new Vector2((vertices[i].x + polygonUnit) / (2 * polygonUnit), (vertices[i].y + polygonUnit) / (2 * polygonUnit));
            }
            mesh.uv = uvs;

            return mesh;
        }

        /// <summary>
        /// 已知与y轴所成顺时针角度和距原点距离，得到坐标
        /// </summary>
        /// <param name="angles"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        private Vector2 GetPosition(float angles, float distance)
        {
            angles = Get360Angles(angles);
            float radian = Mathf.Deg2Rad * angles;
            float cosA = Mathf.Cos(radian);
            float sinA = Mathf.Sin(radian);
            return new Vector2(distance * sinA, distance * cosA);
        }

        /// <summary>
        /// 得到角度对应的0-360角度
        /// </summary>
        /// <param name="angles"></param>
        /// <returns></returns>
        private float Get360Angles(float angles)
        {
            if (angles < 0)
            {
                angles += 360;
                return Get360Angles(angles);
            }
            else if (angles >= 360)
            {
                angles -= 360;
                return Get360Angles(angles);
            }
            else
            {
                return angles;
            }
        }
        private int Get360Angles(int angles)
        {
            if (angles < 0)
            {
                angles += 360;
                return Get360Angles(angles);
            }
            else if (angles >= 360)
            {
                angles -= 360;
                return Get360Angles(angles);
            }
            else
            {
                return angles;
            }
        }
    }
}
    