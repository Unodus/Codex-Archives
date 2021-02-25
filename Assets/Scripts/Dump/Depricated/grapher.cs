using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class grapher : MonoBehaviour// don't use this script!
{

    float clock = 10.0f;
    float updateddif = 0.2f;




 //   int gridX = 10;
   // int gridY = 10;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        clock += Time.deltaTime;
        if (clock >= updateddif)
        {
        //    DrawGraph(gridX, gridY);
            clock = 0;
        }
    }

    void DrawGraph(int X, int Y)
    {
        DrawLine(new Vector3(0, 0, 0), new Vector3(1, 0, 0), Color.white, (updateddif + (updateddif * 0.1f)));
    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Mobile/Particles/Additive"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }
}
