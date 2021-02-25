using UnityEngine;

public class GridMath 
{
    public Vector2 ScreenRatio; // Vertical/Horizontal = actual screen size. Columns/Rows = grid space


    public bool PolarActive; //Polar quality on/off



    [Range(2, 100)]
    public int Columns, Rows;

    [Range(0.0f, 2.0f)]
    public float Size; //Size of cell dots

    public float TransitionSpeed = 2;



    public Vector3 SetPosition(float x, float y)
    {
        Vector2 Pos = new Vector2(x, y);
        Pos = CalcluateRatio(Pos);


        if (PolarActive)
        {
            
            return CoordtoPolar(Pos);
        }
        else
        {

            return CoordtoGrid(Pos) * 0.5f;
        }

    }
    public Vector2 GetPosition(Vector3 CurrentPosition)
    {
        Vector2 Pos ;
        if (PolarActive)
        {
            Pos = PolartoCoord(CurrentPosition);

            Pos = UnCalcluatePolarRatio(Pos);
        }
        else
        {
            CurrentPosition *= 2;
            Pos = GridtoCoord(CurrentPosition);
            Pos = UnCalcluateRatio(Pos);

        }



        return Pos;

    }


    public void SetRatio()
    {

        float Ratio;

        Ratio = Columns / (float)Rows; 
       
 
        ScreenRatio.y = Camera.main.orthographicSize;
        ScreenRatio.x = ScreenRatio.y * Ratio; //* (Screen.width / Screen.height);
    }



    public Vector2 CalcluateRatio(Vector2 Value)// x must be expressed as (x) / (float)(Columns - 1.0f))
    {
        Vector2 MaxValue = new Vector2(Columns, Rows);

        if(PolarActive)
        {
            Value.x += 1;
            Value.y += 1;
        }
        else
        {
            MaxValue.x -= 1;
            MaxValue.y -= 1;
        }

        Vector2 Ratio;
        Ratio = Value / MaxValue;  // x (between +/- 0.5*vertical)
        return Ratio;
    }

    public Vector2 UnCalcluateRatio(Vector2 Ratio)// x must be expressed as (x) / (float)(Columns - 1.0f))
    {
        Vector2 MaxValue = new Vector2(Columns, Rows);
        
            MaxValue.x -= 1;
            MaxValue.y -= 1;
        


        return (Ratio* MaxValue);
    }
    public Vector2 UnCalcluatePolarRatio(Vector2 Ratio)// x must be expressed as (x) / (float)(Columns - 1.0f))
    {

        Vector2 MaxValue = new Vector2(Columns, Rows);



        Ratio = Ratio * MaxValue;
        Ratio.x += 1;
        Ratio.y += 1;
        return (Ratio );
    }



    public Vector3 CoordtoGrid(Vector2 Position)// x must be expressed as (x) / (float)(Columns - 1.0f))
    {
;

        float column =  Mathf.LerpUnclamped(-Size * ScreenRatio.x, Size * ScreenRatio.x, Position.x);  // x (between +/- 0.5*vertical)
        float row =     Mathf.LerpUnclamped(-Size * ScreenRatio.y, Size * ScreenRatio.y , Position.y);   // y (between +/- 0.5*horizontal)
        return new Vector3(column, row, 0);
    }

    public Vector3 CoordtoPolar(Vector2 Position)
    {
        //if(Position.x <0)
        //{ Position.x = 1 - Position.x; }
        float angle = Mathf.LerpUnclamped(0, 2 * Mathf.PI, Position.x); // = x (between 0 and 2*pi)
        float radius = Mathf.LerpUnclamped(0, Size * ScreenRatio.y, Position.y);  // = y (between 0 and 0.5*vertical)


        return new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
    }

    public Vector2 PolartoCoord(Vector3 Position)// This doesn't work
    {
        Vector2 CoordPosition;// =         CoordtoPolar (GridtoCoord(Position));        
        float radius = Vector3.Magnitude(Position);
        CoordPosition.y = Mathf.InverseLerp(0, Size * ScreenRatio.y, radius);

        CoordPosition.x = Position.x / radius;
        CoordPosition.x =  Mathf.Acos(CoordPosition.x);
        
        CoordPosition.x = Mathf.InverseLerp(0, 2 * Mathf.PI, CoordPosition.x);  // = x (between 0 and 2*pi)

        return CoordPosition; // new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
    }




    public Vector2 GridtoCoord(Vector3 Position)// x must be expressed as (x) / (float)(Columns - 1.0f))
    {
        Vector2 CoordPosition;

        CoordPosition.x= Mathf.InverseLerp(-Size * ScreenRatio.x, Size * ScreenRatio.x, Position.x);  // x (between +/- 0.5*vertical)
        CoordPosition.y= Mathf.InverseLerp(-Size * ScreenRatio.y, Size * ScreenRatio.y, Position.y);   // y (between +/- 0.5*horizontal)

        return CoordPosition;
    }

  
}
