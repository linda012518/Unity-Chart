using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Utils
{
    [RequireComponent(typeof(RawImage))]
    public class RawImageUvMove : MonoBehaviour
    {
        RawImage ri;
        public float speed = 0.5f;
        public Vector2 initialSize;
        public bool isMove;
        public enum Direction
        {
            left,
            right
        }
        public Direction direction = Direction.left;
        void Start()
        {
            ri = GetComponent<RawImage>();

        }
        float x = 0;
        // Update is called once per frame
        void Update()
        {
            if (isMove)
                UvMove();
        }
        void UvMove()
        {
            if (JudgeMoveDirection(ri.GetComponent<RectTransform>().localEulerAngles))
            {
                Move(-1);
            }
            else
            {
                Move(1);
            }
        }
        bool JudgeMoveDirection(Vector3 angle)
        {
            if (angle.z >= 0)
                return true;
            else
                return false;
        }
        private void Move(float fuhao)
        {

            float multipleX = ri.GetComponent<RectTransform>().sizeDelta.x / initialSize.x;
            float multipleY = ri.GetComponent<RectTransform>().sizeDelta.y / initialSize.y;
            switch (direction)
            {
                case Direction.left:
                    x += Time.deltaTime;
                    break;
                case Direction.right:
                    x -= Time.deltaTime;
                    break;
            }
            ri.uvRect = new Rect(fuhao * x * speed, 0, multipleX, multipleY);
        }
    }
}
