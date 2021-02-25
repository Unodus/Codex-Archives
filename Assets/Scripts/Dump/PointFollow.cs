using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PointFollow : MonoBehaviour
{

    [SerializeField]GameObject Projector; 


    [SerializeField]  GameObject PlayerPrefab; // Refference to all the Ship prefabs, for instantiation
    [SerializeField]  GameObject SmallEnemyPrefab;
    [SerializeField]  GameObject LargeEnemyPrefab;

    [SerializeField] GameObject PointPrefab; // Refference for each navigable point on the map

    GridMath gridMath;          // Ref to the grid values
    MyProjector projectorMath; // Ref to the generated point values

    [SerializeField]
    GameObject TimerPrefab; // ref to the countdown slider
    Slider TimerObject; 

    int NumOfPoints; // The total amount of points in the grid
    
    public enum NodeMode // Each point can have three states- whether it's empty, able to be selected, or has a ship on it
    {
        Empty,
        Selectable,
        Occupied
    }


    public enum TurnPhase // Turns are divided into different segments, to filter out user inputs.
    {
        EnemyTurnShooting,
        EnemyTurnMoving,
        PlayerTurn_Moving,
        PlayerTurn_Shooting,
        Transition
    }

    public class MyPoint // Each point uses this Struct. The lp-rp points are part of an unimplemented feature due to scope
    {
        public int Column, Row;
        public GameObject p;
        public GameObject lp;
        public GameObject rp;
        public NodeMode Mode;
    }

    public class MyShip // Ship Base - All ships inherit from this
    {
        public GameObject p;

        public MyPoint CurrentNode;
        public float Movement ;
        public float MaxMovement;
        public float Reach;
        public float Shots;
        public int ShipType;

        public void Initiate(MyPoint StartingNode, GameObject prefab) // Creates the Ship and makes sure all variables are set
        {

            p = Instantiate(prefab);

            p.name = "Ship:" + ShipType;
            p.tag = "Player";

            CurrentNode = StartingNode;
            p.transform.SetParent(CurrentNode.p.transform);
            p.transform.localPosition = Vector3.zero;

            p.transform.LookAt( Vector3.zero);

        }
    }

    public class Player : MyShip // The Players Ship. Has long reach and mobility
    {
        public Player(MyPoint StartingNode, GameObject prefab)
        {
            ShipType = 0;
            Movement = 2;
            MaxMovement = 2;
            Reach = 3;
            Shots = 1;

            Initiate(StartingNode, prefab);
        }
    }
    public class SmallEnemy : MyShip // The "smaller" enemy ship
    {
        public SmallEnemy(MyPoint StartingNode, GameObject prefab)
        {
            ShipType = 1;
            Movement = 2;
            MaxMovement = 2;
            Reach = 2;
            Shots = 1;

            Initiate(StartingNode, prefab); 
        }
    }
    public class BigEnemy : MyShip // The larger enemy ship, acts as a carrier for smaller ships
    {
        public BigEnemy(MyPoint StartingNode, GameObject prefab)
        {
            ShipType = 2;
            Movement = 5;
            MaxMovement = 5;
            Reach = 3;
            Shots = 1;

            Initiate(StartingNode, prefab);
        }
    }





    public List<MyPoint> MyPoints = new List<MyPoint>();
    public List<MyShip> MyShips = new List<MyShip>();


    public TurnPhase CurrentTurn;

    //public List<MyPoint> EchoPointsLeft = new List<MyPoint>();
    //public List<MyPoint> EchoPointsRight = new List<MyPoint>();

    Slider InstantiateTimer() // Creates the timer at the start of each turn
    {
        GameObject Canvas = GameObject.FindGameObjectWithTag("Canvas");
        GameObject Timer = Instantiate(TimerPrefab, Canvas.transform);
        return Timer.GetComponent<Slider>();
    }

    void Start()// Instantiate grid and ships in random positions
    {

        CurrentTurn = TurnPhase.PlayerTurn_Shooting;


        projectorMath = Projector.GetComponent<MyProjector>();
        gridMath = projectorMath.gridMath;
        InitiatePoints();
        PositionPoints(MyPoints);


        List<MyPoint> TempList = new List<MyPoint>( MyPoints);
        int RandomNumber;
        RandomNumber = Random.Range(0, TempList.Count);

        MyShips.Add(new Player(MyPoints[RandomNumber], PlayerPrefab));
        TempList.RemoveAt(RandomNumber);

        RandomNumber = Random.Range(0, TempList.Count);
        MyShips.Add(new SmallEnemy(MyPoints[RandomNumber], SmallEnemyPrefab));
        TempList.RemoveAt(RandomNumber);

        
        
        projectorMath.ChangeBool(true);
        ReadyPoints();

    }




    void ChangeNode(MyShip Ship, MyPoint NewPoint)// Moves a ship to a different part of the grid
    { 
        
        if (NewPoint.p == null)
            return;


        Ship.CurrentNode = NewPoint;
        Ship.p.transform.SetParent(NewPoint.p.transform);

        Ship.Movement -= Vector3.Magnitude(Ship.p.transform.localPosition) *0.25f* gridMath.Size;
        if (Ship.Movement <0)
        {
            Ship.Movement = 0;
        }
    }


    void ChangeNodeType(MyPoint Point, NodeMode newMode) // Used to calculate whether a node on the map is selectable
    {
        Point.Mode = newMode;

        if(newMode == NodeMode.Empty)
        {
            Point.p.GetComponent<Renderer>().material.color = new Color(0,0,0,0);
        }
        else if (newMode == NodeMode.Occupied)
        {
            Point.p.GetComponent<Renderer>().material.color = Color.red;
        }
        else if (newMode == NodeMode.Selectable)
        {
            Point.p.GetComponent<Renderer>().material.color = Color.white;
        }

    }

    public void EndTurn() // When the end of turn button is pressed, delete the existing turn timer, reset movement and make the missiles move
    {

        if (CurrentTurn == TurnPhase.EnemyTurnShooting || CurrentTurn == TurnPhase.EnemyTurnMoving)
            return;

        Destroy(GameObject.FindGameObjectWithTag("Timer"));

        MyShips[0].Movement = MyShips[0].MaxMovement;
        CurrentTurn = TurnPhase.Transition;
        LaunchMissiles(MyShips[0], TurnPhase.EnemyTurnMoving);


    }

    void LaunchMissiles(MyShip Sender, TurnPhase NextTurn) // For each unmoved missile, moves them
    {

        GameObject[] Missiles = GameObject.FindGameObjectsWithTag("Missile");
        if (Missiles.Length == 0)
        {
            CurrentTurn = NextTurn;
        }
        else
        {
            foreach (GameObject p in Missiles)
            {
                StartCoroutine(MissileCorourtine(p, Sender, NextTurn));
            }
        }
    }
    IEnumerator MissileCorourtine(GameObject MissileObject, MyShip Sender, TurnPhase NextTurn) // Animates the moving of missiles at the end of each turn
    {
        GameObject Missile = MissileObject.transform.GetChild(0).gameObject;
        LineRenderer Path =  MissileObject.transform.GetComponentInChildren<LineRenderer>();
        float Count = 0;
        while (Count < 1)
        {
            int CurrentPos = (int)Mathf.Lerp(0, Path.positionCount, Count);
             Missile.transform.position = Path.GetPosition(CurrentPos);
            for(int i = 0; i < CurrentPos; i++)
            {
                Path.SetPosition(i, Path.GetPosition(CurrentPos));
            }

            yield return new WaitForSeconds(gridMath.TransitionSpeed * Time.deltaTime); //Count is the amount of time in seconds that you want to wait.
            Count += gridMath.TransitionSpeed * Time.deltaTime;
            foreach(MyShip p in MyShips)
            {
                if (p != Sender)
                {
                    if (Vector3.Distance(p.p.transform.position, Missile.transform.position) < 0.5f)
                    {
                        p.p.GetComponent<PlayerMovement>().DestroyMe();
                    }
                }
            }
        }
        Destroy(MissileObject);

        CurrentTurn = NextTurn;
        yield return null;

    }

    void InitiatePoints() // Instantiate Point gameobjects, based on gridmath data 
    {
        NumOfPoints = gridMath.Columns * gridMath.Rows;

        for (int i = 0; i < gridMath.Columns; i++)
        {

            MyPoint pp;
            for (int j = 0; j < gridMath.Rows; j++)
            {
                pp = new MyPoint();
                pp.Column = i;
                pp.Row = j;
                pp.p = Instantiate(PointPrefab);

                pp.p.transform.SetParent(transform);
                
                pp.p.name = i + ", " + j;
                pp.p.tag = "Point";
                ChangeNodeType(pp, NodeMode.Empty);

                pp.p.transform.position = gridMath.SetPosition(i, j);

                Vector3 Displacement = new Vector3((2 * ((1.25f + gridMath.Columns) / gridMath.Columns)) * gridMath.Size * gridMath.ScreenRatio.x, 0, 0);

                pp.lp = Instantiate(pp.p);
                pp.lp.transform.SetParent(transform);
                pp.lp.transform.position -= Displacement;
                
                pp.rp = Instantiate(pp.p);
                pp.rp.transform.SetParent(transform);
                pp.rp.transform.position += Displacement;

                pp.lp.GetComponent<Renderer>().enabled = false;
                pp.rp.GetComponent<Renderer>().enabled = false;
                MyPoints.Add(pp);
            }
        }
    }
    
    void DeinitialisePoints(List<MyPoint> ThesePoints) // When a grid is resized/deleted, this function clears the gameobjects that have been instantiated
    {
        foreach (MyPoint p in ThesePoints)
        {
            Destroy(p.p);
            Destroy(p.lp);
            Destroy(p.rp);
        }
        ThesePoints.Clear();
        InitiatePoints();
    }

    void FindSelectableNodes(float Range, MyShip Player) // Makes nodes within a certain range selectable
    {
        foreach (MyPoint p in MyPoints)
        {
            
            if (p.Mode == NodeMode.Selectable)
            {
                ChangeNodeType(p, NodeMode.Empty);
            }
            
        }


        Collider[] hitColliders = Physics.OverlapSphere(Player.p.transform.position, Range);




        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag == "Point")
            {
                foreach (MyPoint p in MyPoints)
                {
                    if (p.p == hitCollider.gameObject)
                    {
                        ChangeNodeType(p, NodeMode.Selectable);
                    }
                }

                
            }
        }

    }

    MyPoint GetClosestNode(Vector3 worldPosition, MyShip Player, float CutOff) // Function for finding the closest node, usually for when a node has been clicked
    {
        MyPoint ThisNode = Player.CurrentNode;
        GameObject ClosestPoint = MyPoints[0].p;
        float distance = Vector3.Distance(worldPosition, Player.p.transform.position);
        distance *= 2.1f;
        
        Collider[] hitColliders = Physics.OverlapSphere(Player.p.transform.position, distance);

       
        if (hitColliders.Length == 0)
        {
            return Player.CurrentNode;
        }

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag == "Point")
            {
                
                if(Vector3.Distance(hitCollider.transform.position, worldPosition) < Vector3.Distance(ThisNode.p.transform.position, worldPosition))
                {
                    ClosestPoint = hitCollider.gameObject;

                    foreach (MyPoint p in MyPoints)
                    {
                        if (p.p == ClosestPoint)
                        {
                            if (p.Mode == NodeMode.Selectable)
                            {
                                ThisNode = p;
                            }
                        }
                    }
                }
            }
        }

        if(CutOff == 0)
        {

            return ThisNode;
        }

        if (Vector3.Distance( ThisNode.p.transform.position, worldPosition ) < CutOff)
        {
            return ThisNode;
        }
        else
        {
            return Player.CurrentNode;
        }

    }

    MyPoint GetRandNode() // Function for finding the furest node, usually for ships trying to flee
    {
        int TempNumber = Random.Range(0, MyPoints.Count);
        MyPoint ThisNode = MyPoints[TempNumber];
        return ThisNode;
    }


    void PositionShips(List<MyShip> TheseShips) // moves ships to the point they should be on, smoothly
    {
        foreach (MyShip p in TheseShips)
        {

            p.p.transform.LookAt(gridMath.SetPosition(p.CurrentNode.Column, -0.5f)); //Vector3.zero   );

            if (p.p.transform.localPosition == Vector3.zero)
            {
            }
            else if(Vector3.Distance(p.p.transform.localPosition, Vector3.zero) < 1.0f)
            {
                FindSelectableNodes(p.Movement * gridMath.Size, p);
                float MovementTime = gridMath.TransitionSpeed * Time.deltaTime;
                p.p.transform.localPosition = Vector3.MoveTowards(p.p.transform.localPosition, Vector3.zero, MovementTime);


            }
            else
            {
                float MovementTime = Vector3.Distance(p.p.transform.localPosition, Vector3.zero) * gridMath.TransitionSpeed * Time.deltaTime;
                p.p.transform.localPosition = Vector3.MoveTowards(p.p.transform.localPosition, Vector3.zero, MovementTime);
            }


        }
    }


    void PositionPoints(List<MyPoint> ThesePoints) // moves points to positions they should be on, smoothly
    {
        foreach (MyPoint p in ThesePoints)
        {
            float MovementTime = Vector3.Distance(p.p.transform.position, gridMath.SetPosition(p.Column, p.Row)) * gridMath.TransitionSpeed * Time.deltaTime;

            Vector3 TargetPosition = Vector3.MoveTowards(p.p.transform.position, gridMath.SetPosition(p.Column, p.Row), MovementTime);
            Vector3 TargetSize = Vector3.one * gridMath.Size * 0.1f;
            p.p.transform.localScale = TargetSize;
            p.p.transform.position = TargetPosition;

            p.lp.transform.localScale = TargetSize;
            p.rp.transform.localScale = TargetSize;

            if (gridMath.PolarActive)
            {
                p.lp.transform.position = Vector3.MoveTowards(p.lp.transform.position, TargetPosition, Vector3.Distance(p.lp.transform.position, TargetPosition) * gridMath.TransitionSpeed * Time.deltaTime);
                p.rp.transform.position = Vector3.MoveTowards(p.rp.transform.position, TargetPosition, Vector3.Distance(p.rp.transform.position, TargetPosition) * gridMath.TransitionSpeed * Time.deltaTime);
            }
            else
            {
                Vector3 Displacement = new Vector3((2 * ((1.25f+gridMath.Columns)/gridMath.Columns))* gridMath.Size * gridMath.ScreenRatio.x, 0, 0);
                p.lp.transform.position = Vector3.MoveTowards(p.lp.transform.position, TargetPosition - Displacement, Vector3.Distance(p.lp.transform.position, TargetPosition - Displacement) * gridMath.TransitionSpeed * Time.deltaTime);
                p.rp.transform.position = Vector3.MoveTowards(p.rp.transform.position, TargetPosition + Displacement, Vector3.Distance(p.rp.transform.position, TargetPosition + Displacement) * gridMath.TransitionSpeed * Time.deltaTime);
            }
        }
    }

    public void ReadyPoints()// When transitioning between modes, the points need time to settle before calculating where the active nodes should be
    {
        if(CurrentTurn == TurnPhase.EnemyTurnMoving)
        {
            CurrentTurn = TurnPhase.Transition;
            StartCoroutine("Reset", (0));
        }
        else
        {
            CurrentTurn = TurnPhase.Transition;
            StartCoroutine("Reset", (gridMath.TransitionSpeed * 1.0f));
        }
    }
    IEnumerator Reset(float Count) // This function waits for the appropriate time before allowing the selection to happen
    {
        yield return new WaitForSeconds(Count); //Count is the amount of time in seconds that you want to wait.
     
        if(Count == 0)
        {
            yield return new WaitForSeconds(gridMath.TransitionSpeed * 2.0f);
            CurrentTurn = TurnPhase.EnemyTurnShooting;
        }
        else
        {
            if (gridMath.PolarActive)
            {
                CurrentTurn = TurnPhase.PlayerTurn_Moving;


                FindSelectableNodes(MyShips[0].Movement * gridMath.Size, MyShips[0]);


            }
            else
            {
                CurrentTurn = TurnPhase.PlayerTurn_Shooting;
                FindSelectableNodes(MyShips[0].Reach * gridMath.Size, MyShips[0]);
            }


        }

        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (MyShip p in MyShips) // if there is only one ship remaining (ie, player has won), return to the menu
        {
            if (p.p == null)
            {
                MyShips.Remove(p);

                if (MyShips[0].ShipType != 0)
                {
                    SceneManager.LoadScene(0);
                }

                return;
            }
        }

        if (NumOfPoints == gridMath.Columns * gridMath.Rows)
        {
            PositionPoints(MyPoints);
        }
        else
        {
            DeinitialisePoints(MyPoints);
        }

        PositionShips(MyShips);



        //handles the turn-changing logic

        if (CurrentTurn == TurnPhase.PlayerTurn_Moving && !gridMath.PolarActive)
        {
            CurrentTurn = TurnPhase.PlayerTurn_Shooting;
        }
        if (CurrentTurn == TurnPhase.PlayerTurn_Shooting && gridMath.PolarActive)
        {
            CurrentTurn = TurnPhase.PlayerTurn_Moving;
        }

        if (CurrentTurn == TurnPhase.EnemyTurnMoving )
        {
            //do enemy stuff

            foreach (MyShip p in MyShips)
            {
                if (p.ShipType == 1)
                {
                    FindSelectableNodes(p.Movement * gridMath.Size, p);
                    ChangeNode(p, GetClosestNode(MyShips[0].p.transform.position, p, 0));
                    p.Movement = p.MaxMovement;
                }

                if (p.ShipType == 2)
                {
                    FindSelectableNodes(p.Movement * gridMath.Size, p);
                    ChangeNode(p, GetClosestNode(GetRandNode().p.transform.position, p, 0));
                    p.Movement = p.MaxMovement;
                }

            }
            projectorMath.ChangeBool(false);
            ReadyPoints();
        }
        if (CurrentTurn == TurnPhase.EnemyTurnShooting)
        {
            //do enemy stuff

            List<MyPoint> ShipsToMake = new List<MyPoint>();

            foreach (MyShip p in MyShips)
            {
                if (p.ShipType == 1)
                {
                    FindSelectableNodes(p.Reach * gridMath.Size, p);    
                    p.p.GetComponent<PlayerMovement>().Shoot(GetClosestNode(MyShips[0].p.transform.position, p, 0).p.transform.position);
                    LaunchMissiles(p, TurnPhase.PlayerTurn_Moving);

                }
                if (p.ShipType == 2)
                {
                    FindSelectableNodes(p.Reach * gridMath.Size, p);
                    ShipsToMake.Add(p.CurrentNode);
                    
                }
            }

            foreach (MyPoint p in ShipsToMake)
            {
                MyShips.Add(new SmallEnemy(p, SmallEnemyPrefab));

            }


            projectorMath.ChangeBool(true);
            ReadyPoints();

            FindSelectableNodes(MyShips[0].Reach * 100, MyShips[0]);

            MyPoint SpawnZone = GetRandNode();
            int RandomNumber = Random.Range(0, 2);
            if (RandomNumber == 0)
            {
                MyShips.Add(new SmallEnemy(SpawnZone, SmallEnemyPrefab));
            }
            else
            {
                MyShips.Add(new BigEnemy(SpawnZone, LargeEnemyPrefab));
            }


        }


        else if (CurrentTurn != TurnPhase.Transition)
        {
            if (TimerObject == null)
            {
                TimerObject = InstantiateTimer();
            }
            else
            {
                TimerObject.value -= Time.deltaTime;
                if (TimerObject.value <= 0 )
                {
                    projectorMath.ChangeBool(true);

                    EndTurn();
                    Destroy(TimerObject.gameObject);
                }
            }
        }


        // Handles inputs
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0;

            if (CurrentTurn == TurnPhase.PlayerTurn_Moving)
            {
                ChangeNode(MyShips[0], GetClosestNode(worldPosition, MyShips[0], 1));

            }


            else if (CurrentTurn == TurnPhase.PlayerTurn_Shooting)
            {
                
                    //worldPosition = GetClosestNode(worldPosition, MyShips[0]).p.transform.position;
                    if (Vector3.Distance(MyShips[0].p.transform.position, worldPosition) < MyShips[0].Reach)
                    {
                        MyPoint FirePoint = GetClosestNode(worldPosition, MyShips[0], 1);
                        if (FirePoint.p != null)
                    {
                        MyShips[0].p.GetComponent<PlayerMovement>().Shoot(FirePoint.p.transform.position);
                        projectorMath.ChangeBool(true);
                        ReadyPoints();

                    }


                }

            }
            


        }



    }
}   
