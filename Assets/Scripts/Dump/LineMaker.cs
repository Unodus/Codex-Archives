using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMaker : MonoBehaviour
{

    GridMath gridMath; // Refference to the Grid Variables

    [SerializeField]  // Ref to the line Object
    GameObject LinePrefab;
    

    [SerializeField] // Segments represent the "resolution" of each line. 100 is good enough for this project
    int NumOfSegments;

    [SerializeField] // Multiplier for line width
    float LineWidth;

    [SerializeField] // Colour of the default Line (some lines use other colours to stand out)
    Color LineColor;

    public struct MyLine // used for tracking the object and position of each point
    {
        public Vector2[] Positions;
        public GameObject p;
        public LineRenderer pp;
    }

    public struct MyGrid // used for creating groups of lines, in order to make a grid
    {
        public MyLine[] ColumnLines;
        public MyLine[] RowLines;
        public MyLine[] Outlines;
    }



    MyGrid GridSystem;

    [SerializeField]
    public List<MyLine> MyLines= new List<MyLine>();





    public void SpawnGrid() // Instantiates a grid of lines that go between each point, including 4 lines along the outside that make up the "border" of the game
    {
        GridSystem = new MyGrid();
        Vector2 Start;
        Vector2 End;

        GridSystem.RowLines = new MyLine[gridMath.Rows];
        GridSystem.ColumnLines = new MyLine[gridMath.Columns];

        GridSystem.Outlines = new MyLine[4];




        GridSystem.Outlines[0] = GetLine(new Vector2(-0.5f, -0.5f), End = new Vector2(-0.5f, gridMath.Rows-0.5f));
        GridSystem.Outlines[1] = GetLine(new Vector2(-0.5f, gridMath.Rows - 0.5f), End = new Vector2(gridMath.Columns-0.5f, gridMath.Rows - 0.5f));
        GridSystem.Outlines[2] = GetLine(new Vector2(gridMath.Columns - 0.5f, gridMath.Rows - 0.5f), End = new Vector2(gridMath.Columns - 0.5f, - 0.5f));
        GridSystem.Outlines[3] = GetLine(new Vector2(gridMath.Columns - 0.5f,- 0.5f), End = new Vector2(-0.5f, -0.5f));

        for (int i = 0; i < (4); i++)
        {
            if(i == 3)
            {
                GridSystem.Outlines[i].pp.startColor = Color.yellow;
                GridSystem.Outlines[i].pp.endColor = Color.yellow;

            }
            else
            {
                GridSystem.Outlines[i].pp.startColor = LineColor;
                GridSystem.Outlines[i].pp.endColor = LineColor;

            }
        }

        for (int i = 0; i < (gridMath.Columns -1); i++)
        {
            float j = Mathf.Lerp(i, i+1, 0.5f);
            Start = new Vector2(j, -0.5f);
            End = new Vector2(j, gridMath.Rows -0.5f);
            GridSystem.ColumnLines[i] = GetLine(Start, End);

            GridSystem.ColumnLines[i].pp.startColor = LineColor;
            GridSystem.ColumnLines[i].pp.endColor = LineColor;
        }
        for (int i = 0; i < (gridMath.Rows -1); i++)
        {
            float j = Mathf.Lerp(i, i + 1, 0.5f);
            Start = new Vector2(-0.5f, j);
            End = new Vector2(gridMath.Columns - 0.5f, j);
            GridSystem.RowLines[i] = GetLine(Start, End);

            GridSystem.RowLines[i].pp.startColor = LineColor;
            GridSystem.RowLines[i].pp.endColor = LineColor;

        }
    }
    public void DeSpawnGrid() // Removes lines for if the grid changes size
    {
        for (int i = 0; i < (GridSystem.ColumnLines.Length); i++)
        {
            Destroy(GridSystem.ColumnLines[i].p);
            MyLines.Remove(GridSystem.ColumnLines[i]);
        }
        for (int i = 0; i < (GridSystem.RowLines.Length); i++)
        {
            Destroy(GridSystem.RowLines[i].p);
            MyLines.Remove(GridSystem.RowLines[i]);
        }
        for (int i = 0; i < (GridSystem.Outlines.Length); i++)
        {
            Destroy(GridSystem.Outlines[i].p);
            MyLines.Remove(GridSystem.Outlines[i]);
        }
    }


    public GameObject GetLine(Vector3 cStart, Vector3 cEnd) // Converts two points in world space into a line game object, properly segmented
    {

        Vector2 Start = gridMath.GetPosition(cStart);
        Vector2 End = gridMath.GetPosition(cEnd);

        return GetLine(Start, End).p;

    }
    public MyLine GetLine(Vector2 Start, Vector2 End) // Converts two points in "grid space " into a line
    {
        MyLine lp;
        lp = new MyLine();
        lp.p = Instantiate(LinePrefab);
        lp.p.transform.SetParent(transform);
        lp.p.name = "Line: " + Start + " to " + End;


        lp.pp = lp.p.GetComponent<LineRenderer>();



        lp.pp.widthMultiplier = LineWidth;

        lp.pp.startColor = Color.blue;
        lp.pp.endColor = LineColor;

        lp.pp.positionCount = NumOfSegments;

        lp.Positions = new Vector2[NumOfSegments];
        for (int i = 0; i < NumOfSegments; i++)
        {

            if (gridMath.PolarActive)
            {
                Vector2 NewStart = gridMath.SetPosition(Start.x, Start.y);
                Vector2 NewEnd = gridMath.SetPosition(End.x, End.y);

                Vector3 Interp = new Vector2(Mathf.Lerp(NewStart.x, NewEnd.x, i / (NumOfSegments - 1.0f)), Mathf.Lerp(NewStart.y, NewEnd.y, (i / (NumOfSegments - 1.0f) )));
     
                lp.Positions[i] = gridMath.GetPosition(Interp);
                lp.pp.SetPosition(i, gridMath.SetPosition(lp.Positions[i].x, lp.Positions[i].y));


            }
            else
            {
                lp.Positions[i] = new Vector2(Mathf.Lerp(Start.x, End.x, (i / (NumOfSegments - 1.0f))), Mathf.Lerp(Start.y, End.y, (i / (NumOfSegments - 1.0f))));
                lp.pp.SetPosition(i, gridMath.SetPosition(lp.Positions[i].x, lp.Positions[i].y));

            }
        }
        MyLines.Add(lp);
        return lp;
    }
    public void SpawnLine(Vector2 Start, Vector2 End) // Instantiates the line gameobject 
    {
        MyLine lp;
        lp = new MyLine();
        lp.p = Instantiate(LinePrefab);
        lp.p.transform.SetParent(transform);
        lp.p.name = "Line: " + Start + " to " + End;


        lp.pp = lp.p.GetComponent<LineRenderer>();



        lp.pp.widthMultiplier = LineWidth;

        lp.pp.startColor    = Color.blue;
        lp.pp.endColor      = LineColor;
        
        lp.pp.positionCount = NumOfSegments;

        lp.Positions = new Vector2[NumOfSegments];
        for (int i = 0; i < NumOfSegments; i++)
        {

            if (gridMath.PolarActive)
            {
                Vector2 NewStart = gridMath.SetPosition(Start.x, Start.y);
                Vector2 NewEnd = gridMath.SetPosition(End.x, End.y);

                Vector2 Interp = new Vector2(Mathf.Lerp(NewStart.x, NewEnd.x, (i / (float)NumOfSegments)), Mathf.Lerp(NewStart.y, NewEnd.y, (i / (float)NumOfSegments)));
    
                lp.Positions[i] =gridMath.GetPosition(Interp);
                lp.pp.SetPosition(i, gridMath.SetPosition(Interp.x, Interp.y));

            }
            else
            {
                Vector2 Interp = new Vector2(Mathf.Lerp(Start.x, End.x, (i / (float)NumOfSegments)), Mathf.Lerp(Start.y, End.y, (i / (float)NumOfSegments)));
                lp.Positions[i] = Interp;
                
                lp.pp.SetPosition(i, gridMath.SetPosition(Interp.x, Interp.y));

            }
        }

        MyLines.Add(lp);
                
    }

    public void SpawnLine(Vector3 cStart, Vector3 cEnd) // Instantiates the line object, after converting world space to grid space
    {
        
        Vector2 Start = gridMath.GetPosition(cStart);
        Vector2 End = gridMath.GetPosition(cEnd);

        SpawnLine(Start, End);

    }


    public void SpawnRandomLine() // spawns a random line on the grid, useful for debugging
    {
              Vector3 Start = gridMath.SetPosition(Random.Range(0, gridMath.Columns), Random.Range(0, gridMath.Rows));
              Vector3 End = gridMath.SetPosition(Random.Range(0, gridMath.Columns), Random.Range(0, gridMath.Rows));
        SpawnLine(Start, End);
    }

    public void SpawnRandomCircle() // Spawns a random circle, useful for debugging
    {
        Vector2 StartPoint = new Vector2(Random.Range(1, gridMath.Columns - 1), Random.Range(1, gridMath.Rows - 1));
      
        Vector3 Start = gridMath.SetPosition(StartPoint.x, StartPoint.y);

        StartPoint = gridMath.CalcluateRatio(StartPoint);

        Vector3 End = gridMath.SetPosition(Random.Range(1, gridMath.Columns-1), Random.Range(1, gridMath.Rows-1));


        Debug.Log( StartPoint + " vs " + gridMath.PolartoCoord(Start));
        SpawnCircle(Start, End);
    }
    public void SpawnCircle(Vector3 cStart, Vector3 cEnd) // Spawn circle with world space vector
    {

        Vector2 Start = gridMath.GetPosition(cStart);
        Vector2 End = gridMath.GetPosition(cEnd);

        SpawnCircle(Start, End);

    }


    public void SpawnCircle(Vector2 Start, Vector2 End) // Spawn circle with grid space vector
    {
        float Radius = Vector2.Distance(Start, End);
        Radius *= 0.5f;

        MyLine lp;
        lp = new MyLine();
        lp.p = Instantiate(LinePrefab);
        lp.p.transform.SetParent(transform);
        lp.p.name = "Circle: " + Start + " with radius " + Radius;

        lp.pp = lp.p.GetComponent<LineRenderer>();
        lp.pp.widthMultiplier = LineWidth;

        lp.pp.startColor = Color.blue;
        lp.pp.endColor = LineColor;

        lp.pp.positionCount = NumOfSegments;
        lp.Positions = new Vector2[NumOfSegments];

        for (int i = 0; i < NumOfSegments; i++)
        {

            if (gridMath.PolarActive)
            {

                Vector3 NewStart = gridMath.SetPosition(Start.x, Start.y);
                //  Vector2 NewEnd = gridMath.SetPosition(End.x, End.y);

                float angle = Mathf.Lerp(0, 2 * Mathf.PI, i / (NumOfSegments - 1.0f));
                float x = Mathf.Sin(angle) * Radius;
                float y = Mathf.Cos(angle) * Radius;



                Vector3 Interp = NewStart + gridMath.SetPosition(x, y);

                lp.Positions[i] = gridMath.GetPosition(Interp);

            }
            else
            {

                
                float angle = Mathf.Lerp(0, 2 * Mathf.PI, i / (NumOfSegments -1.0f));
                float x = Mathf.Sin(angle) * Radius;
                float y = Mathf.Cos(angle) * Radius;

                lp.Positions[i] = Start + new Vector2(x, y);
            }
        }

        MyLines.Add(lp);

    }

    

    void Awake()// At the start of the game, make sure there is a ref to all the grid-relevant variables
    {
        gridMath = GameObject.FindGameObjectWithTag("GameController").GetComponent<MyProjector>().gridMath;
 }

    
    void Update() // Every frame, go through the list of Lines and update their positions.
    {
        foreach (var p in MyLines)
        {
            if(p.p == null)
            {
                    MyLines.Remove(p);
                return;
            }
            else
            {
            
            for (int i = 0; i < NumOfSegments; i++)
            {
                p.pp.SetPosition(i, Vector3.MoveTowards(p.pp.GetPosition(i), gridMath.SetPosition(p.Positions[i].x, p.Positions[i].y), Vector3.Distance(p.pp.GetPosition(i), gridMath.SetPosition(p.Positions[i].x, p.Positions[i].y)) * gridMath.TransitionSpeed * Time.deltaTime));
            }
            }
        }


        

    }



}