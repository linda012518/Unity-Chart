using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Utils
{
    public class RefreshBar : MonoBehaviour
    {

        RectTransform[] rects;
        Image[] images;
        public float interval;//两张图片间隔距离
        public Direction direction;
        public float moveSpeed = 0.2f;//更新间隔
        public int count;//可显示图片数量，空块的个数
        int length;//数组长度
        float farPoint;//可显示区域最远点
        int moveIndex;//当前移动图片索引

        Color[] arrColor;
        Vector2[] arrPositionX;
        Vector2[] arrPositionY;

        void Start()
        {
            rects = transform.GetChild<RectTransform>();
            images = transform.GetChild<Image>();
            length = rects.Length;
            arrColor = new Color[length];
            arrPositionX = new Vector2[length];
            arrPositionY = new Vector2[length];
            farPoint = (count - 1) * interval;

            InitData();
            Init();
            InvokeRepeating("FadeMove", 0, moveSpeed);
        }

        void InitData()
        {
            //有一个是纯透明的，用来移动缓冲，减1
            int middle = (length - 1) / 2;
            //计算递减透明差值
            float a = 1f / (middle + 1);
            int index = 0;

            //Color c = Color.white;
            Color c = transform.GetChild(0).GetComponent<Image>().color;
            float point = 0;

            for (int i = 0; i < rects.Length - 1; i++)
            {
                if ((length - 1) % 2 == 0)
                {
                    if (i < middle) c.a = a * (i + 1) + a;
                    else c.a = (middle - index++) * a + a;
                }
                else
                {
                    if (i < middle) c.a = a * (i + 1);
                    else if (i > middle) c.a = (middle - index++) * a;
                    else c.a = 1;
                }
                arrColor[i] = c;
                point = interval * i;
                arrPositionX[i].x = point;
                arrPositionY[i].y = point;
            }
            c.a = 0;
            arrColor[length - 1] = c;
            point = interval * (length - 1);
            arrPositionX[length - 1].x = point;
            arrPositionY[length - 1].y = point;
        }

        void Init()
        {
            for (int i = 0; i < rects.Length - 1; i++)
            {
                ResetPosition(i);
                images[i].gameObject.SetActive(false);
                images[i].color = arrColor[i];
            }
            ResetPosition(length - 1);
            images[length - 1].color = arrColor[length - 1];
            images[length - 1].gameObject.SetActive(false);
            images[0].gameObject.SetActive(true);
            moveIndex = length - 1;
        }

        void ResetPosition(int index)
        {
            switch (direction)
            {
                case Direction.Left:
                    rects[index].anchoredPosition = arrPositionX[index];
                    break;
                case Direction.Right:
                    rects[index].anchoredPosition = -arrPositionX[index];

                    break;
                case Direction.Top:
                    rects[index].anchoredPosition = -arrPositionY[index];

                    break;
                case Direction.Down:
                    rects[index].anchoredPosition = arrPositionY[index];
                    break;
            }
        }

        void ResetBar(bool visible)
        {
            if (visible)
            {
                for (int i = 0; i < length; i++)
                {
                    images[i].DOKill();
                }
                Init();
            }
        }

        void SetVisible(bool visible, GameObject obj)
        {
            if (obj.activeSelf != visible) return;
            obj.SetActive(!visible);
        }

        void UpdateColor()
        {
            //for (int i = 0; i < length - 1; i++)
            //{
            //    images[i].DOColor(images[i + 1].color, moveSpeed);
            //}
            //images[length - 1].DOColor(images[0].color, moveSpeed);
            for (int i = 0; i < length - 1; i++)
            {
                images[i].DOFade(images[i + 1].color.a, moveSpeed);
            }
            images[length - 1].DOFade(images[0].color.a, moveSpeed);
        }

        public enum Direction { Top, Down, Left, Right }

        void FadeMove()
        {
            /*
            6543210     0=6
            5432106     6=5
            4321065     5=4
            3210654     4-3
            2106543     3-2
            1065432     2-1
            0654321     1-0
            6543210     0-6
            */
            UpdateColor();
            switch (direction)
            {
                case Direction.Top:
                    rects[moveIndex].SetAnchoredPosition(null, rects[moveIndex].anchoredPosition.y + interval * length);

                    SetVisible(rects[moveIndex].anchoredPosition.y > farPoint, rects[moveIndex].gameObject);
                    //检查对头位置，是否全部移走
                    ResetBar(rects[moveIndex = moveIndex == 0 ? length - 1 : --moveIndex].anchoredPosition.y > farPoint);
                    break;
                case Direction.Down:
                    rects[moveIndex].SetAnchoredPosition(null, rects[moveIndex].anchoredPosition.y - interval * length);

                    SetVisible(rects[moveIndex].anchoredPosition.y < -farPoint, rects[moveIndex].gameObject);
                    ResetBar(rects[moveIndex = moveIndex == 0 ? length - 1 : --moveIndex].anchoredPosition.y < -farPoint);
                    break;
                case Direction.Left:
                    rects[moveIndex].SetAnchoredPosition(rects[moveIndex].anchoredPosition.x - interval * length, null);

                    SetVisible(rects[moveIndex].anchoredPosition.x < -farPoint, rects[moveIndex].gameObject);
                    ResetBar(rects[moveIndex = moveIndex == 0 ? length - 1 : --moveIndex].anchoredPosition.x < -farPoint);
                    break;
                case Direction.Right:
                    rects[moveIndex].SetAnchoredPosition(rects[moveIndex].anchoredPosition.x + interval * length, null);

                    SetVisible(rects[moveIndex].anchoredPosition.x > farPoint, rects[moveIndex].gameObject);
                    ResetBar(rects[moveIndex = moveIndex == 0 ? length - 1 : --moveIndex].anchoredPosition.x > farPoint);
                    break;
            }
        }
    }
}