using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class MyProjector : MonoBehaviour

{
    public GridMath gridMath = new GridMath();

    private bool gridVisible = true;

    Camera MyCamera; // Scene Camera

    [SerializeField]
    GameObject LineRenderer; //linerender prefab

    [SerializeField]
    LineMaker LineManager; //linerender prefab

    [SerializeField]
    int Rows, Columns, Displacement; //linerender prefab
   
    

    [SerializeField]
    Sprite sprite; //point image

    [SerializeField]
    Vector3[,] Grid; //grid of vectors

    // Start is called before the first frame update
    void Awake() //Instatiates line creator, and grabs variable values from the grid values file 
    {
        LineManager = LineRenderer.GetComponent<LineMaker>();
        gridMath.Rows = Rows + Random.Range(-Displacement, Displacement);
        gridMath.Columns = Columns + Random.Range(-Displacement, Displacement);
        gridMath.Size = 1;

    }
    void Start() //instantiates the grid, after the linemaker has time to set it's own variables
    {
        Initiate();
    }

    void Initiate() // Instantiates the grid object, and adds the lines in the relevant positions
    { 
        gridMath.SetRatio();

        Grid = new Vector3[gridMath.Columns, gridMath.Rows];

        for (int i = 0; i < gridMath.Columns; i++)
        {
            for (int j = 0; j < gridMath.Rows; j++)
            {
                SetPosition(i, j);
            }
        }
        UpdateSize(gridMath.Size);


        if (gridVisible)
        { DrawLines(); }
        
    }

    void Deinitiate() // every time the grid changes size, it needs to get rid of the previous grid
    {
        LineManager.DeSpawnGrid();
    }


    public void Bump() // Called if the grid needs a "Soft Reset" (ie, if the grid size changes, etc)
    {
        Deinitiate();
        Initiate();
    }

    public void ChangeX(string newX)//update width of grid
    {
        if (String.IsNullOrEmpty(newX) )
            return;
        if (int.Parse(newX) <= 1)
            return;
        gridMath.Columns = int.Parse(newX);
        Bump();
    }
    public void ChangeY(string newY)// update height of grid
    {
        if (String.IsNullOrEmpty(newY))
            return;
        if (int.Parse(newY) <= 1)
            return;
        gridMath.Rows = int.Parse(newY);
        Bump();
    }
    public void ChangeBool(bool newBool)// switch between polar and grid mode
    {
        gridMath.PolarActive = newBool;
        
        
        for (int i = 0; i < gridMath.Columns; i++)
        {
            for (int j = 0; j < gridMath.Rows; j++)
            {
                SetPosition(i, j);
            }
        }


        UpdateSize(gridMath.Size);
    }
    public void ChangeGridVis(bool newBool)// update polar quality
    {
        gridVisible = newBool;


        for (int i = 0; i < gridMath.Columns; i++)
        {
            for (int j = 0; j < gridMath.Rows; j++)
            {
                SetPosition(i, j);
            }
        }
        UpdateSize(gridMath.Size);
        Bump();
    }

    public void UpdateSize(Single newSize) // changes grid zoom size
    {
        gridMath.Size = newSize;

    }



    void DrawLines()// Starts the LineManager
    {
        LineManager.SpawnGrid();
    }


    void SetPosition(int newx, int newy) // Plots a point on the Grid
    {
        if (gridMath.PolarActive)
        {
            float angle = Mathf.Lerp(0, 2*Mathf.PI, ((newx )/ (float)(gridMath.Columns ))); // = x (between 0 and 2*pi)
            float radius = Mathf.Lerp(0, gridMath.Size * gridMath.ScreenRatio.y, ((newy )/ (float)(gridMath.Rows )));  // = y (between 0 and 0.5*vertical)
            Grid[newx, newy] = new Vector3( Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
        }
        else
        {
            float column =   Mathf.Lerp(-gridMath.Size* gridMath.ScreenRatio.x, gridMath.Size * gridMath.ScreenRatio.x, (newx )/ (float)(gridMath.Columns -1.0f));  // x (between +/- 0.5*vertical)
            float row =     Mathf.Lerp(-gridMath.Size * gridMath.ScreenRatio.y, gridMath.Size * gridMath.ScreenRatio.y, (newy )/ (float)(gridMath.Rows -1.0f));   // y (between +/- 0.5*horizontal)
            Grid[newx, newy] = new Vector3(column, row, 0);
        }

    }


   


}
