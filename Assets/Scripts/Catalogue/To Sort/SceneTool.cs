#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Owner: Kyle Hatch
/// Created: 24/02/2017
/// 
/// Descirption: For switching scene inside the editor, saves on switching
/// </summary>


public class SceneTool : EditorWindow
{
   

    private int m_iActiveScenesInBuild = 0;
    private List<string> m_ListOfScenes = new List<string>();
    private List<string> m_ListOfScenePaths = new List<string>();

    private static string s_PathToTestScenes;
    private List<string> m_TestScenes = new List<string>();
    private List<string> m_TestScenePaths = new List<string>();

    private static string s_PathToArtScenes;
    private List<string> m_ArtScenes = new List<string>();
    private List<string> m_ArtScenePaths = new List<string>();

    private int m_iTestScenesInBuild = 0;
    private int m_iArtScenesInBuild = 0;
    private Color m_OldColor;

    private Vector2 m_vSbarValue;

    [MenuItem("Tools/Scene Tool")]
    private static void ShowWindow()
    {
        SceneTool tool = EditorWindow.GetWindow<SceneTool>();
        tool.titleContent = new GUIContent("Scene Tool");
    }

    private void OnEnable()
    {
        s_PathToTestScenes = Application.dataPath + Path.DirectorySeparatorChar + @"Scenes\Test";
        s_PathToArtScenes = Application.dataPath + Path.DirectorySeparatorChar + @"Art\Scenes";
    }

    private void OnGUI()
    {
        //dont' allow change when running
        if (true == EditorApplication.isPlaying)
        {
            return;
        }

        m_OldColor = GUI.color;

        if (true == GUILayout.Button("Refresh"))
        {
            GatherSceneInfo();
            GatherTestScenes();
            GatherArtScenes();
        }


        EditorGUILayout.BeginVertical();
        m_vSbarValue = EditorGUILayout.BeginScrollView(m_vSbarValue, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height - 50));

        EditorGUILayout.Space();
        GUI.color = Color.cyan;
        GUILayout.Box("Game Scenes", GUILayout.MaxWidth(Screen.width));
        EditorGUILayout.Space();
        GUI.color = m_OldColor;

        for (int i = 0; i < m_iActiveScenesInBuild; i++)
        {
            if (true == GUILayout.Button(m_ListOfScenes[i]))
            {
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                EditorSceneManager.OpenScene(m_ListOfScenePaths[i]);
            }
        }

        EditorGUILayout.Space();
        GUI.color = Color.green;
        GUILayout.Box("Test scenes", GUILayout.MaxWidth(Screen.width));
        EditorGUILayout.Space();
        GUI.color = m_OldColor;

        for (int i = 0; i < m_iTestScenesInBuild; i++)
        {
            if (true == GUILayout.Button(m_TestScenes[i]))
            {
                try
                {
                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                    EditorSceneManager.OpenScene(m_TestScenePaths[i]);
                }
                catch (System.Exception e)
                {
                    Debug.Log("Error: " + e.Message);
                }
            }
        }

        EditorGUILayout.Space();
        GUI.color = Color.green;
        GUILayout.Box("Art scenes", GUILayout.MaxWidth(Screen.width));
        EditorGUILayout.Space();
        GUI.color = m_OldColor;

        for (int i = 0; i < m_iArtScenesInBuild; i++)
        {
            if (true == GUILayout.Button(m_ArtScenes[i]))
            {
                try
                {
                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                    EditorSceneManager.OpenScene(m_ArtScenePaths[i]);
                }
                catch (System.Exception e)
                {
                    Debug.Log("Error: " + e.Message);
                }
            }
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void GatherSceneInfo()
    {
        m_ListOfScenes.Clear();
        m_ListOfScenePaths.Clear();
        m_iActiveScenesInBuild = 0;
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        int iLevelCount = scenes.Length;
        for (int iLevels = 0; iLevels < iLevelCount; iLevels++)
        {
            EditorBuildSettingsScene scene = scenes[iLevels];
            if (true == scene.enabled)
            {
                string sPath = scene.path;
                m_ListOfScenePaths.Add(sPath);

                string sName = scene.path.Substring(scene.path.LastIndexOf('/') + 1);
                sName = sName.Substring(0, sName.Length - 6);
                m_ListOfScenes.Add(sName);
                m_iActiveScenesInBuild++;
            }
        }
    }

    private void GatherTestScenes()
    {
        m_TestScenes.Clear();
        m_TestScenePaths.Clear();
        m_iTestScenesInBuild = 0;
        string[] sScenes = Directory.GetFiles(s_PathToTestScenes, "*.unity", SearchOption.TopDirectoryOnly);
        m_iTestScenesInBuild = sScenes.Length;

        for (int i = 0; i < m_iTestScenesInBuild; i++)
        {
            string sFile = Path.GetFileNameWithoutExtension(sScenes[i]);

            //Make sure it's not a Scene Path
            if (false == m_ListOfScenes.Contains(sFile))
            {
                m_TestScenePaths.Add(sScenes[i]);
                m_TestScenes.Add(sFile);
            }
        }
        m_iTestScenesInBuild = m_TestScenes.Count;
    }

    private void GatherArtScenes()
    {
        m_ArtScenes.Clear();
        m_ArtScenePaths.Clear();
        m_iArtScenesInBuild = 0;
        string[] sScenes = Directory.GetFiles(s_PathToArtScenes, "*.unity", SearchOption.AllDirectories);
        m_iArtScenesInBuild = sScenes.Length;

        for (int i = 0; i < m_iArtScenesInBuild; i++)
        {
            string sFile = Path.GetFileNameWithoutExtension(sScenes[i]);
            if (false == m_ListOfScenes.Contains(sFile))
            {
                m_ArtScenePaths.Add(sScenes[i]);
                m_ArtScenes.Add(sFile);
            }
        }
        m_iArtScenesInBuild = m_ArtScenes.Count;
    }
}
#endif