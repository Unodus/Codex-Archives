using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class PanelMenu : MonoBehaviour
{
    private static PanelMenu instance = null;

    [SerializeField]
    Vector3 Onscreen, Offscreen;

    [SerializeField]
    GameObject MyTile;

    RectTransform MyTileTransform;

    [SerializeField]
    float TransitionTime;


    [SerializeField]
    AudioMixer masterMixer;

    static Dictionary<string, KeyCode> keyMapping;
    static string[] keyMaps = new string[5]
    {
        "Forward",
        "Backward",
        "Left",
        "Right",
        "Pause"
    };
    static KeyCode[] defaults = new KeyCode[5]
    {
        KeyCode.W,
        KeyCode.S,
        KeyCode.A,
        KeyCode.D,
        KeyCode.P
    };
    private static void InitializeDictionary()
    {
        keyMapping = new Dictionary<string, KeyCode>();
        for (int i = 0; i < keyMaps.Length; ++i)
        {
            keyMapping.Add(keyMaps[i], defaults[i]);
        }
    }

    private bool Visible, Locked;

    public static PanelMenu Instance
    {
        get
        {
            return instance;
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

     }
    private void Start()
    {
        MyTileTransform = MyTile.GetComponent<RectTransform>();
        InitializeDictionary();
        MoveMenu(false);
    }

    public static void SetKeyMap(string keyMap, KeyCode key)
    {
        if (!keyMapping.ContainsKey(keyMap))
            throw new ArgumentException("Invalid KeyMap in SetKeyMap: " + keyMap);
        keyMapping[keyMap] = key;
    }

    public static bool GetKeyDown(string keyMap)
    {
        return Input.GetKeyDown(keyMapping[keyMap]);
    }


    // Update is called once per frame
    void Update()
    {
        if (GetKeyDown("Pause") && !Locked)
        {
            Locked = true;
            MoveMenu(!Visible);
        }
    }

    public void MoveMenu(bool Displayed )
    {
        Visible = Displayed;
        if (!Visible)
            Time.timeScale = 1;
        StartCoroutine(Lerper(0.0f, (1.0f / TransitionTime)));
    }

    private IEnumerator Lerper(float Counter, float Speed)
    {

        if (Visible)
            MyTileTransform.anchoredPosition = Vector3.Lerp(Offscreen, Onscreen,  Tween.EaseInOut(Counter));
        else
            MyTileTransform.anchoredPosition = Vector3.Lerp(Onscreen, Offscreen, Tween.EaseInOut(Counter));


        yield return new WaitForSeconds(Time.deltaTime * Speed);
        
        if (Counter < 1)
        {
            StartCoroutine(Lerper(Counter + (Speed * Time.deltaTime), Speed));
        }
        else
        {
            Locked = false;
            if (Visible)
                Time.timeScale = 0;
        }

    }

    public void SetSfxLevel(float sfxLvl)
    {
        masterMixer.SetFloat("volSfx", sfxLvl);
    }

    public void SetMusicLevel(float musicLvl)
    { 
        masterMixer.SetFloat("volMusic", musicLvl);
    }


    public void SetButton()
    {

    }

}
