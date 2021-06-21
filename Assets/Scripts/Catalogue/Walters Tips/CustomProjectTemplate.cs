using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Walter 's tip of the day - 01/03/2021
//Custom Project Templates:

//Are you like me, a pedantic little $hit when it comes to setting up your projects? 
//Do you like having a default folder structure and project settings?
//Do you have that one asset that you need in almost every single project?
//Would you like to have a Package be there by default? i.e. the new Input System?

//Well, fear no more! You can make your own Project Templates for Unity Hub!
//Go to Unity Hub, select a version under Installs and view it in the explorer.
//When you have it open in the explorer, navigate to Data\Resources\PackageManager\ProjectTemplates

//Now just duplicate one of the existing templates as a baseline.
//If you want one you don't have installed yet such as the VR starter project, install it first via Unity Hub.

//Unzip that copied file, open it and unzip the file in there (idk why).
//You now have a folder called package, open that.

//Now you can make changes to whatever you want.
//Do note that you have to change things inside package.json file.
//Here are my changes:

//    {
//    "name": "com.unity.template.custom",
//  "displayName": "Walter's Custom URP Template",
//  "version": "10.2.2",
//  "type": "template",
//  "unity": "2020.2",
//  "host": "hub",
//  "description": "Walter's trimmed down URP Template",
//  "dependencies": {
//        "com.unity.render-pipelines.universal": "10.2.2",
//    "com.unity.inputsystem": "1.0.1"
//  }
//}

//Package dependencies are done in that file as well.

//If you want to change the default project settings I recommend making a project with the original package version of the one you copied, changing the settings and copying the *.asset files under ProjectSettings in the folder with the same name in your new template, it will save you a lot of work. (under ProjectData~)

//When you're done modifying the template just zip the package folder and put it in the folder with the rest of the templates. 
//(You don't have to double zip it like Unity does.)

//Custom Project Templates come with a few caveats:
//-1.You have to re-do them for every single Unity version, so if you install a new patch duplicate your template first.
//- 2. You can't remove the default packages, so Unity Collab is still there by default like a parasite.

//Enjoy your new Project Templates! 

//____________

// 04/22/2021

//Similar to custom project template, you can also define a custom script template that loads into visual studio or visual studio code when a script of that kind is created.

//The script templates are located here : 

// C:\Program Files\Unity\Editor\Data\Resources\ScriptTemplates

//If you go into that directory, edit the NewBehaviourScript.cs.txt as per your liking then, whenever you create a C# Script  inside the editor, it will be loaded as per that template......

//Not extremely important but very useful if you are habitual like me, of removing the start and update functions of the script initially, just remove those functions from the template and you are done

//Detailed process is available here : 

//https://support.unity.com/hc/en-us/articles/210223733-How-to-customize-Unity-script-templates

