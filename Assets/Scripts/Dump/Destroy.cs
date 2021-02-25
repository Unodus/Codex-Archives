using System.Collections;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    [SerializeField]
    float decay;

    float clock =0;


    [SerializeField]
    Vector3 Displacement;

    [SerializeField]
    int Mode;

    Vector3 StartPosition;
    Vector3 EndPosition;


    // Start is called before the first frame update
    void Start()
    {
        if(Mode == 0)
        {
            StartPosition = transform.position;
            EndPosition = StartPosition + Displacement;

            StartCoroutine(Lerper(0.0f, (1.0f / decay)));
        }
        else if(Mode ==1 )
        {
            StartPosition = transform.localScale- Displacement;
            EndPosition = transform.localScale + Displacement;
        }
    }
    void Update()
    {
        if (Mode== 1)
        {
            if (clock <1)
            {
                clock += Time.deltaTime * decay;
                transform.localScale = Vector3.Lerp(StartPosition, EndPosition, Tween.SinLerp(clock));
            }
            else
            {
                clock = 0;
            }

        }

        if (Mode == 2)
        {
            if (clock < 1)
            {
                clock += Time.deltaTime * decay;
                transform.position = Vector3.Lerp(StartPosition, EndPosition, Tween.EaseInOut(clock));


            }
            else
            {
                StartPosition = EndPosition;
                EndPosition = new Vector3(Displacement.x * UnityEngine.Random.Range(-1.0f, 1.0f), Displacement.y * UnityEngine.Random.Range(-1.0f, 1.0f), Displacement.z * UnityEngine.Random.Range(-1.0f, 1.0f));
      
                clock = 0;
            }

        }
    }


    private IEnumerator Lerper(float Counter, float Speed)
    {
        transform.position = Vector3.Lerp(StartPosition, EndPosition, Tween.EaseIn(Counter));
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, Tween.EaseIn(Counter));

        yield return new WaitForSeconds(Time.deltaTime * Speed);
        if(Counter < 1)
        {
    //        Debug.Log(Counter + ": " +Speed);
            StartCoroutine(Lerper(Counter + (Speed*Time.deltaTime), Speed));
        }
        else
        {
            Destroy(transform.gameObject);
        }

    }

}
