using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour
{
    Color[] colorOptions;


    void Start()
    {
        colorOptions = DefineColorOptions();
        if (!TryGetComponent(out Renderer rend)) return;

        rend.material.color = colorOptions[Random.Range(0, colorOptions.Length)];
    }

    Color[] DefineColorOptions()
    {
        List<Color> colors = new List<Color>();

        colors.Add(Color.black);
        colors.Add(Color.blue);
        colors.Add(Color.cyan);
        colors.Add(Color.gray);
        colors.Add(Color.green);
        colors.Add(Color.magenta);
        colors.Add(Color.red);
        colors.Add(Color.white);
        colors.Add(Color.yellow);

        return colors.ToArray();
    }
}
