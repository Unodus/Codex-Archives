/* Got from the internet
 * 
 * goes though and removes all missing scripts from gameobjects 
 */


#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;


/// <summary>
/// 
/// </summary>
public class FindMissingScriptsRecursively : EditorWindow
{
    private static int _goCount;
    private static int _componentsCount;
    private static int _missingCount;

    private static bool _bHaveRun;

//    [MenuItem("Helpers/FindMissingScriptsRecursively")]
    public static void ShowWindow()
    {
        GetWindow(typeof(FindMissingScriptsRecursively));
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Find Missing Scripts in selected GameObjects"))
        {
            FindInSelected();
        }

        if (!_bHaveRun) return;

        EditorGUILayout.TextField(_goCount + " GameObjects Selected");
        if (_goCount > 0) EditorGUILayout.TextField(_componentsCount + " _componentsCount");
        if (_goCount > 0) EditorGUILayout.TextField(_missingCount + "   _missingCount");
    }

    private static void FindInSelected()
    {
        var go = Selection.gameObjects;
        _goCount = 0;
        _componentsCount = 0;
        _missingCount = 0;
        foreach (var g in go)
        {
            FindInGo(g);
        }

        _bHaveRun = true;
        Debug.Log("Searched " + _goCount +  " GameObjects, " +_componentsCount +  " components, found " + _missingCount + " missing");
    }

    private static void FindInGo(GameObject g)
    {
        _goCount++;
        var components = g.GetComponents<Component>();

        var r = 0;

        for (var i = 0; i < components.Length; i++)
        {
            _componentsCount++;
            if (components[i] != null) continue;
            _missingCount++;
            var s = g.name;
            var t = g.transform;
            while (t.parent != null)
            {
                s = t.parent.name + "/" + s;
                t = t.parent;
            }

            Debug.LogError(s + " has a missing script at " + i);

            var serializedObject = new SerializedObject(g);
            r++;
        }

        foreach (Transform childT in g.transform)
        {
            FindInGo(childT.gameObject);
        }
    }
}
#endif