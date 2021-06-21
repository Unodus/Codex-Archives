using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Walter 's tip of the day - 04/24/2021

//Did you know Unity has a way of entering Play-Mode extremely quickly in the editor?
//The reason it takes a while is because Unity has to reload and reset some things to ensure it'd start the same way as in a build.

//It does a couple things but there are two very significant processes you have control over, Reload Scene and Reload Domain
//Reload Scene: Destroys all GameObjects in the editor and reloads the scene from disk, like it would when loading a scene in a build.
//Reload Domain: Resets the entire C# State;  all static fields, properties, and event handlers are reset each time you enter Play Mode.

//Both of these things can take an tremendous amount of time, especially as your scene grows.
//Luckily you can disable these for quick development:

//Go to Edit/Project Settings/Editor, then scroll down to Enter Play Mode Settings -Toggle it on, and configure the way you like.
//I recommend only disabling Reload Scene, you're less likely to run into issues with that.

//But beware that now your loading times are not the as they would be in a build, and stops you from debugging scene loading, though I personally think you should debug that in a build anyway (if you can).
//If you do choose to disable Reload Domain as well, you'll need to edit your scripts to handle these resets themselves:


public class FastPlayMode : MonoBehaviour
{
    //This counter won't reset to zero when Domain Reloading is disabled.
    private static int killCounter = 0;
    private void OnKill() => killCounter++;

    //By Adding a method that resets all your static Fields/Properties with the following Attribute, it will handle these reloads for you.

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStaticMembers()
    {
        killCounter = default;
    }


    private void Awake()
    {
        //Usually you wouldn't unsubscribe to a method like Application.quitting.
        //But right now we have to, because these aren't reset and then the Quit method would be called twice, maybe causing some nasty bugs.
        Application.quitting += Quit;
    }
    private static void Quit() { }

    //Just unsubscribe it in the same way:

    [RuntimeInitializeOnLoadMethod]
    static void ResetStaticEventHandlers()
    {
        Application.quitting -= Quit;
    }


//There's also some issues with OnEnable() OnDisable() and OnDestroy() not being called for scripts using the [ExecuteInEditMode] attribute, read more on the docs.
//Check out the Docs over here, I can't go over everything 
//I oversimplified the processes a lot and there's much I missed, so do check it out if you're planning on using this.
//https://docs.unity3d.com/2021.1/Documentation/Manual/ConfigurableEnterPlayMode.html
//https://docs.unity3d.com/2021.1/Documentation/Manual/DomainReloading.html
//https://docs.unity3d.com/2021.1/Documentation/Manual/SceneReloading.html
//https://docs.unity3d.com/2021.1/Documentation/Manual/ConfigurableEnterPlayModeDetails.html

//If you make Assets for the Asset Store, or Open-Source tools, Both Unity and I stongly recommend you build these explicit reloads into your code.

}
