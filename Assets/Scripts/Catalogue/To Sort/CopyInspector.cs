/*
// tool i am making to help the artists  merge items across
// So they not have to do it by hand 
*/
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CopyInspector : EditorWindow
{
    private GameObject m_Source;
    private GameObject m_Target;


    [MenuItem("Helpers/CopyComponents")]
    public static void ShowWindow()
    {
        GetWindow<CopyInspector>("CopyInspector");
    }

    void OnGUI()
    {
        GUILayout.Label("Copy Scripts accross ", EditorStyles.boldLabel);


        m_Source = (GameObject)EditorGUILayout.ObjectField("Source", m_Source, typeof(GameObject), true);
        m_Target = (GameObject)EditorGUILayout.ObjectField("m_Target", m_Target, typeof(GameObject), true);


        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Transform"))
        {
            Transform(m_Source, m_Target);
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("CopySpecialComponents Not Meshes"))
        {
            CopySpecialComponents(m_Source, m_Target);
        }


        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Transform     ALL Children"))
        {
            TransformSubList(m_Source, m_Target);
        }
        if (GUILayout.Button("CopySpecialComponents Not Meshes   ALL Children"))
        {
            CopySpecialComponentsSubList(m_Source, m_Target);
        }
        if (GUILayout.Button("CopySpecialComponents Not Meshes   ALL Children Overwrite "))
        {
            CopySpecialComponentsSubList(m_Source, m_Target, true);
        }


        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        string lToolTip = "Drag the one you want to Source, then select all the targets and press the button";

        if (GUILayout.Button(new GUIContent("Copy Source to Selected Items Transforms Only", lToolTip)))
        {
            GameObject[] lItems = Selection.gameObjects;

            Undo.RecordObjects(lItems, "Copy Source Start");
            for (int i = 0; i < lItems.Length; i++)
            {
                Transform(m_Source, lItems[i]);
            }
            Undo.RecordObjects(lItems, "Copy Source End");
        }


        if (GUILayout.Button(new GUIContent("Copy Source to Selected Items CopySpecialComponents Not Meshes", lToolTip)))
        {
            
            GameObject[] lItems = Selection.gameObjects;

            Undo.RecordObjects(lItems, "Copy Source Start");
            for (int i = 0; i < lItems.Length; i++)
            {
                CopySpecialComponents(m_Source, lItems[i], true);               
            }
            Undo.RecordObjects(lItems, "Copy Source End");
        }

    }


    private void Transform(GameObject lSource, GameObject lTarget)
    {
        lTarget.transform.localPosition = lSource.transform.localPosition;
        lTarget.transform.localRotation = lSource.transform.localRotation;
        lTarget.transform.localScale    = lSource.transform.localScale;
    }

    private void CopySpecialComponents(GameObject lSource, GameObject lTarget, bool lOverwrite = false)
    {
        Component[] lSourceArray = lSource.GetComponents<Component>();
        Component[] lTargetArray = lTarget.GetComponents<Component>();
        for (int i = 0; i < lSourceArray.Length; i++)
        {
            var componentType = lSourceArray[i].GetType();
            if (componentType != typeof(MeshFilter) &&
                componentType != typeof(MeshRenderer)
                )
            {
                Debug.Log("Found a component of type " + lSourceArray[i].GetType());

                UnityEditorInternal.ComponentUtility.CopyComponent(lSourceArray[i]);
                if (true == lOverwrite)
                {
                    if (lSourceArray[i].GetType() == lTargetArray.GetType())
                    {
                        UnityEditorInternal.ComponentUtility.PasteComponentValues(lTargetArray[i]);
                    }
                }
                else
                {
                     UnityEditorInternal.ComponentUtility.PasteComponentAsNew(lTarget);
                }


                Debug.Log("Copied " + lSourceArray[i].GetType() + " from " + lSource.name + " to " + lTarget.name);
            }
        }
    }

    private void GetDictionary(GameObject lSource, GameObject lTarget, out Dictionary<string,GameObject> lSourceDictionary, out Dictionary<string, GameObject> lTargetDictionary)
    {
        string lSourceRemoveString = Helpers.General.GetGameObjectPath(lSource);
        string lTargetRemoveString = Helpers.General.GetGameObjectPath(lTarget);



        Transform[] lSourceGameObjects = (Transform[])lSource.GetComponentsInChildren<Transform>(true);
        lSourceDictionary = new Dictionary<string, GameObject>();
        int iLength = lSourceGameObjects.Length;
        for (int i = 0; i < iLength; i++)
        {
            string lName = Helpers.General.GetGameObjectPath(lSourceGameObjects[i]);
            lName = lName.Replace(lSourceRemoveString, "");
            lSourceDictionary.Add(lName, lSourceGameObjects[i].gameObject);
        }

        Transform[] lTargetGameObjects = (Transform[])lTarget.GetComponentsInChildren<Transform>(true);
        lTargetDictionary = new Dictionary<string, GameObject>();
        for (int i = 0; i < lTargetGameObjects.Length; i++)
        {
            string lName = Helpers.General.GetGameObjectPath(lTargetGameObjects[i]);
            lName = lName.Replace(lTargetRemoveString, "");
            lTargetDictionary.Add(lName, lTargetGameObjects[i].gameObject);
        }
    }

    private void CopySpecialComponentsSubList(GameObject lSource, GameObject lTarget, bool lOverwrite = false)
    {
        Dictionary<string, GameObject> lSourceDictionary;
        Dictionary<string, GameObject> lTargetDictionary;
        GetDictionary(lSource, lTarget, out lSourceDictionary, out lTargetDictionary);
        List<string> lSourcekeyList = new List<string>(lSourceDictionary.Keys);
        List<string> lTargetkeyList = new List<string>(lTargetDictionary.Keys);
        for (int i = 0; i < lSourcekeyList.Count; i++)
        {
            for (int j = 0; j < lTargetkeyList.Count; j++)
            {
                if(lSourcekeyList[i] == lTargetkeyList[j])
                {
                    CopySpecialComponents(lSourceDictionary[lSourcekeyList[i]], lTargetDictionary[lTargetkeyList[i]], lOverwrite);
                    
                }
            }
        }
    }


    private void TransformSubList(GameObject lSource, GameObject lTarget)
    {
        Dictionary<string, GameObject> lSourceDictionary;
        Dictionary<string, GameObject> lTargetDictionary;
        GetDictionary(lSource, lTarget, out lSourceDictionary, out lTargetDictionary);
        List<string> lSourcekeyList = new List<string>(lSourceDictionary.Keys);
        List<string> lTargetkeyList = new List<string>(lTargetDictionary.Keys);
        for (int i = 0; i < lSourcekeyList.Count; i++)
        {
            for (int j = 0; j < lTargetkeyList.Count; j++)
            {
                if (lSourcekeyList[i] == lTargetkeyList[j])
                {
                    Transform(lSourceDictionary[lSourcekeyList[i]], lTargetDictionary[lTargetkeyList[i]]);
                }
            }
        }
    }

}

#endif