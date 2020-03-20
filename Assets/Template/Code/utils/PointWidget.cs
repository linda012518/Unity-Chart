using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Utils
{
    public class PointWidget : MonoBehaviour
    {
        private float animSpeed;
        private List<Image> objs = new List<Image>();
        private float during;
        private int pointNumber;
        private int index;
        private void Awake()
        {
            index = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                objs.Add(transform.GetChild(i).GetComponent<Image>());
            }
            pointNumber = transform.childCount;
           
        }
        public void ShowProgress(float num, float speed,float max=100)
        {
            during = max / pointNumber;
            ResetState();
            num = num > max ? max : num;
            int index = Mathf.CeilToInt(num / during);
            float qq = num % during;
            if (qq > during / 2)
            {
                index++;
            }
            index = index > pointNumber ? pointNumber : index;
            animSpeed = speed / pointNumber;
            IEnumerator coroutine = Progress(index);
            StartCoroutine(coroutine);
        }
        public void ResetState()
        {
            IEnumerator coroutine = Progress(index);
            StopCoroutine(coroutine);
            if (objs.Count > 0)
                for (int i = 0; i < objs.Count; i++)
                {
                    objs[i].enabled = false;
                }
        }
        IEnumerator Progress(float num)
        {
            for (int i = 0; i < num; i++)
            {
                objs[i].enabled = true;
                yield return new WaitForSeconds(animSpeed);
            }
        }


    }
}

