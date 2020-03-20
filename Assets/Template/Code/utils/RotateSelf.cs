using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{   
    public class RotateSelf : MonoBehaviour
    {
        public enum Axial
        {
             X, Y, Z
        }

        public enum LoopMode
        {
            Always,PingPong
        }
        public Axial axis = Axial.Z;
        public LoopMode loopType = LoopMode.Always;
        public Vector3 StartVec;
        public Vector3 EndVec;
       
        public float RotateSpeed = 1;

        private Vector3 axiss;
        // Use this for initialization
        void Start()
        {
            switch (axis)
            {
                case Axial.X:
                    axiss = Vector3.right;
                    RotateSlef();
                    break;
                case Axial.Y:
                    axiss = Vector3.up;
                    RotateSlef();
                    break;
                case Axial.Z:
                    axiss = Vector3.forward;                
                    RotateSlef();
                    break;
            }
        }
        void RotateSlef( )
        {          
            if (loopType == LoopMode.PingPong)
            {
                transform.DORotate(EndVec, RotateSpeed, RotateMode.Fast).OnComplete(() => transform.DORotate(StartVec, RotateSpeed, RotateMode.Fast).OnComplete(() => RotateSlef()));
            }
        }
        void Update()
        {
            if (loopType == LoopMode.Always)
            transform.Rotate(axiss, RotateSpeed * Time.deltaTime);
        }

    }
}