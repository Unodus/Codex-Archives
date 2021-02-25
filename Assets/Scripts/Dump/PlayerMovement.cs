using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    GameObject MissilePrefab, ExplosionPrefab, ImplosionPrefab, ShipSpriteRef;
    LineMaker LineComp;

    

    void Start() // plays an implosion effect on creation
    {
        LineComp = GameObject.FindGameObjectWithTag("Finish").GetComponent<LineMaker>();

        ShipSpriteRef.SetActive(false);
        GameObject i = Instantiate(ImplosionPrefab, transform);
        Destroy(i, 2.0f);
        StartCoroutine(TeleportIn());
    }

    IEnumerator TeleportIn()
    {
        yield return new WaitForSeconds(2);
        ShipSpriteRef.SetActive(true);
    }

    public void Shoot(Vector3 Target) // Instantiates the missile object 
    {
        GameObject pp = LineComp.GetLine(transform.position, Target);
        GameObject p = Instantiate(MissilePrefab);
        p.transform.SetParent(transform.parent);
        p.transform.localPosition = Vector3.zero;
        pp.transform.SetParent(p.transform);
    }

    public void DestroyMe() // Plays an explosion effect on Death
    {
        if (ShipSpriteRef.activeSelf)
        {
            GameObject i = Instantiate(ExplosionPrefab, transform);
            ShipSpriteRef.SetActive(false);
            Destroy(gameObject, 2.0f);

        }
    }

}
