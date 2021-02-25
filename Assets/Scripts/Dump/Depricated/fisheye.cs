using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fisheye : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float Displacement;
    Vector3 CurrentPosition;
    Camera MyCamera;
    public float FieldofVisionBound;
    public float DistanceBound;

    float TempDisplacement;
    float CurrentVisionBound;

    public float Undisplacement;

    // Start is called before the first frame update
    void Start()
    {
        MyCamera = GetComponent<Camera>();
        CurrentPosition = transform.localPosition;
        CurrentVisionBound = MyCamera.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        Undisplacement = 1 - Displacement;


        if(Displacement != TempDisplacement)
        {
            MyCamera.fieldOfView = CurrentVisionBound + (Displacement * FieldofVisionBound);
            transform.localPosition = new Vector3(CurrentPosition.x, CurrentPosition.y, CurrentPosition.z - (Displacement * DistanceBound));
        }
        TempDisplacement = Displacement;

        if (Input.GetKey("up") && Displacement <1)
            {
            Displacement += Time.deltaTime;
            }
        else
        {
            if (!Input.GetKey("up") && Displacement > 0)
            {
                Displacement -= Time.deltaTime;

            }
        }

     }
}
