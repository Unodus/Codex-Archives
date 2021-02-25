using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class UIScript : MonoBehaviour
{
    AudioSource MyAudio;

    [SerializeField]
    GameObject TurnCounterObject;


    int TurnCounter = 0;
    [SerializeField] float PitchVariation;
    float Pitch;
    public void Start()
    {
        MyAudio = GetComponent<AudioSource>();
        Pitch = MyAudio.pitch;
    }
    public void PlaySound(AudioClip PlayMe)
    {
        MyAudio.pitch = Pitch + Random.Range(-PitchVariation, PitchVariation);
        MyAudio.clip = PlayMe;
        MyAudio.Play();
    }

    public void TurnIncrease()
    {
        TurnCounter++;
        string TempString = "Turn: " + TurnCounter;
        TurnCounterObject.GetComponent<TextMeshProUGUI>().text = TempString;
    }

    public void OpenLevel(int Level)
    {

        GameObject Singleton = GameObject.FindGameObjectWithTag("Singleton");
        Singleton.GetComponent<MenuSingleton>().ChangeLevel(Level);
    }
}
