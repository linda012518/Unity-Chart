using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class MoveInRange : MonoBehaviour
    {      
        /// <summary>
        /// 图片位移
        /// </summary>
        public enum LoopMode
        {
            Once, PingPong
        }
        public LoopMode loopType = LoopMode.PingPong;
        public Vector3 StartVec;
        public Vector3 EndVec;
        public float Duration = 1;
        // Use this for initialization
        void Start()
        {         
            Move();
        }

        void Move()
        {
            if (loopType == LoopMode.Once)
            {
                transform.DOLocalMove(EndVec, Duration);
            }
            if (loopType == LoopMode.PingPong)
            {
                transform.DOLocalMove(EndVec, Duration).OnComplete(() => transform.DOLocalMove(StartVec, Duration).OnComplete(() => Move()));
            }

        }      
    }
}
