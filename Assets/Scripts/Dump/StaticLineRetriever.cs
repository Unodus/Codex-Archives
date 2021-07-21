/// <summary>
/// Owner: James Bradbury
/// Created: 21/07/2021
/// Description: A script that, when assigned a list of objects and positions, moves the objects up the list of positions. (Ie: "Winching" a set of lines up a Static Line)
/// </summary>

using UnityEngine;
public class StaticLineRetriever : MonoBehaviour
{
    #region Internal Variables
    [SerializeField] GameObject[] m_StaticLineObjects;
    [SerializeField] Transform[] m_StatePositions;
    [SerializeField] int WinchCount;
    [SerializeField] bool Initialized = false;
    #endregion
    public static StaticLineRetriever Attach(in GameObject go, in GameObject[] StaticLineObjects, in Transform[] StatePositions) // This is a factory pattern initializer. Use this function to add the function to an object, rather than getcomponent or addcomponent
    {
        if (go == null) return null;
        StaticLineRetriever Component = go.AddComponent<StaticLineRetriever>();
        Component.Init(StaticLineObjects, StatePositions);
        return Component;
    }
    private void Init(in GameObject[] StaticLineObjects, in Transform[] StatePositions) // Assigns variables internally. Component will not continue until init has been called
    {
        if (StaticLineObjects.Length == 0) return;
        if (StatePositions.Length == 0) return;
        m_StaticLineObjects = StaticLineObjects;
        m_StatePositions = StatePositions;
        Initialized = true;
    }
    public void WinchPositions(in int displacement) // Moves all objects in script up/down by the value in displacement. Call this function whenever the winch is used, inputting the amount to move by
    {
        if (!Initialized) return;
        if (WinchCount +displacement  >= m_StatePositions.Length ) return;
        WinchCount += displacement;
        if (WinchCount < 0) WinchCount = 0;
        for (int i = 0; i < m_StaticLineObjects.Length; i++)
        {
            int WinchIndex = WinchCount - i;
            if (WinchIndex < 0) WinchIndex = 0;
            m_StaticLineObjects[i].transform.position = m_StatePositions[WinchIndex].position;
        }
    }
}
