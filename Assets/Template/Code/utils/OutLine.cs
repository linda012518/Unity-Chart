using UnityEngine;

public class Line
{
    public float a;
    public float b;
    public float d;
    public Vector2 normal;
}

public class OutLine : MonoBehaviour
{

    public Transform p1, p2, p3;

    public float distance = 1;

    public bool isClose = false;

    public bool isCenter = false;

    void Update()
    {
        if (isCenter)
        {
            if (isClose)
            {
                Line line1 = GetVertical(p1.position, p2.position);
                Line line2 = GetVertical(p2.position, p3.position);
                Line line3 = GetVertical(p3.position, p1.position);
                Vector2 go1 = GetPoint(line1, line2);
                Vector2 go2 = GetPoint(line3, line2);
                Vector2 go3 = GetPoint(line3, line1);
                Debug.DrawLine(new Vector3(go1.x, go1.y, 0), new Vector3(go2.x, go2.y, 0), Color.yellow);
                Debug.DrawLine(new Vector3(go1.x, go1.y, 0), new Vector3(go3.x, go3.y, 0), Color.yellow);
                Debug.DrawLine(new Vector3(go3.x, go3.y, 0), new Vector3(go2.x, go2.y, 0), Color.yellow);

                Line line11 = GetVertical(p2.position, p1.position);
                Line line22 = GetVertical(p3.position, p2.position);
                Line line33 = GetVertical(p1.position, p3.position);
                Vector2 go11 = GetPoint(line11, line22);
                Vector2 go22 = GetPoint(line33, line22);
                Vector2 go33 = GetPoint(line33, line11);
                Debug.DrawLine(new Vector3(go11.x, go11.y, 0), new Vector3(go22.x, go22.y, 0), Color.yellow);
                Debug.DrawLine(new Vector3(go11.x, go11.y, 0), new Vector3(go33.x, go33.y, 0), Color.yellow);
                Debug.DrawLine(new Vector3(go33.x, go33.y, 0), new Vector3(go22.x, go22.y, 0), Color.yellow);

                Debug.DrawLine(p1.position, p2.position, Color.green);
                Debug.DrawLine(p3.position, p2.position, Color.green);
                Debug.DrawLine(p3.position, p1.position, Color.green);
            }
            else
            {
                Line line1 = GetVertical(p1.position, p2.position);
                Line line2 = GetVertical(p2.position, p3.position);
                Vector2 go1 = GetPoint(line1, line2);
                Debug.DrawLine(p3.position + (Vector3)line2.normal * distance, new Vector3(go1.x, go1.y, 0), Color.yellow);
                Debug.DrawLine(p1.position + (Vector3)line1.normal * distance, new Vector3(go1.x, go1.y, 0), Color.yellow);

                Line line11 = GetVertical(p2.position, p1.position);
                Line line22 = GetVertical(p3.position, p2.position);
                Vector2 go2 = GetPoint(line11, line22);
                Debug.DrawLine(p3.position + (Vector3)line22.normal * distance, new Vector3(go2.x, go2.y, 0), Color.yellow);
                Debug.DrawLine(p1.position + (Vector3)line11.normal * distance, new Vector3(go2.x, go2.y, 0), Color.yellow);

                Debug.DrawLine(p1.position, p2.position, Color.green);
                Debug.DrawLine(p3.position, p2.position, Color.green);
            }
        }
        else
        {
            if (isClose)
            {
                Line line1 = GetVertical(p1.position, p2.position);
                Line line2 = GetVertical(p2.position, p3.position);
                Line line3 = GetVertical(p3.position, p1.position);

                Vector2 go1 = GetPoint(line1, line2);
                Vector2 go2 = GetPoint(line3, line2);
                Vector2 go3 = GetPoint(line3, line1);

                Debug.DrawLine(p1.position, p2.position, Color.green);
                Debug.DrawLine(p3.position, p2.position, Color.green);
                Debug.DrawLine(p3.position, p1.position, Color.green);

                Debug.DrawLine(new Vector3(go1.x, go1.y, 0), new Vector3(go2.x, go2.y, 0), Color.yellow);
                Debug.DrawLine(new Vector3(go1.x, go1.y, 0), new Vector3(go3.x, go3.y, 0), Color.yellow);
                Debug.DrawLine(new Vector3(go3.x, go3.y, 0), new Vector3(go2.x, go2.y, 0), Color.yellow);
            }
            else
            {
                Line line1 = GetVertical(p1.position, p2.position);
                Line line2 = GetVertical(p2.position, p3.position);

                Vector2 go1 = GetPoint(line1, line2);

                Debug.DrawLine(p1.position, p2.position, Color.green);
                Debug.DrawLine(p3.position, p2.position, Color.green);

                Debug.DrawLine(p3.position + (Vector3)line2.normal * distance, new Vector3(go1.x, go1.y, 0), Color.yellow);
                Debug.DrawLine(p1.position + (Vector3)line1.normal * distance, new Vector3(go1.x, go1.y, 0), Color.yellow);
            }
        }

    }

    Vector2 GetPoint(Line one, Line two)
    {
        Vector2 go = Vector2.zero;

        go.x = (two.b * one.d - one.b * two.d) / (one.a * two.b - two.a * one.b);
        go.y = (one.a * two.d - two.a * one.d) / (one.a * two.b - two.a * one.b);

        return go;
    }


    Line GetVertical(Vector2 start, Vector2 end)
    {
        Line line = new Line();

        Vector2 go = start - end;
        go = Quaternion.Euler(0, 0, 90) * go;
        line.normal = go.normalized;

        line.d = Vector3.Dot(go, end + line.normal * distance);
        line.a = go.x;
        line.b = go.y;

        return line;
    }

}
