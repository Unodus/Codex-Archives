using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AdPrompter : MonoBehaviour
{
    [SerializeField]
    GameObject DisplayButton;
    [SerializeField]
    float Threshold;
    // Start is called before the first frame update
    void Start()
    {
        DisplayButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Redisplay(Single newSize)
    {
        if ((int)newSize == (int)Threshold)
        {
            DisplayButton.SetActive(true);
        }
    }
}
