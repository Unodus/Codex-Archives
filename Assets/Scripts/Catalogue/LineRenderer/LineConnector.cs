/// <summary>
/// Owner: James Bradbury
/// Created: 21/06/2021
/// 
/// Description: A script that when attached to a Line Renderer, will move its positions to match the positions of any gameobjects transforms placed in the inspector. Will update at runtime, and the inspector
/// Ensure to have a high line count, to guarantee the resolution of the line isn't noteworthy
/// </summary>



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineConnector : MonoBehaviour
{

    [SerializeField] LineRenderer m_lineRenderer; //Attach the LineRenderer in the Inspector
    [SerializeField] Transform[] lineObjectPositions; // Assign the desired transforms for the line to traverse, in the inspector

    public void OnValidate() // Used to update in the Inspector
    {
        UpdatePositions();
    }
    private void Update() // Update at runtime 
    {
        UpdatePositions();
    }

    private void UpdatePositions() // Updates line positions based on transforms
    {
        // Will empty-out early if the script detects that data is missing {
        if (m_lineRenderer == null) return;
        if (m_lineRenderer.positionCount <= 1) return;
        if (lineObjectPositions == null) return;
        if (lineObjectPositions.Length == 0) return;
        for (int i = 0; i < lineObjectPositions.Length; i++)
        {
            if (lineObjectPositions[i] == null) return;
        }
        // } End of null-checks



        /// For every position in the line component, find it's desired position 
        /// First, we calculate where the position is, relative to the line (0 to 1)
        /// Then, we calculate where this relative position is in reference to our transforms (if it is somewhere between 1, 2 or 3. Etc)
        /// We can then calculate the relative position that point is between those two transforms, using Lerps

        for (int i = 0; i < m_lineRenderer.positionCount; i++)
        {
            float j = i / (float)(m_lineRenderer.positionCount - 1.0f);
            float LineI = Mathf.Lerp(0, 1, j);
            float LinePosition = Mathf.Lerp(0, lineObjectPositions.Length - 1, LineI);
          
            int StartIndex = Mathf.FloorToInt(LinePosition);
            int EndIndex = Mathf.CeilToInt(LinePosition);
            float InterValue = Mathf.InverseLerp(StartIndex, EndIndex, LinePosition);
           
            Vector3 LineObjectStart = lineObjectPositions[StartIndex  ].position;            
            Vector3 LineObjectEnd = lineObjectPositions[EndIndex].position;
            Vector3 TargetPosition = Vector3.Lerp(LineObjectStart, LineObjectEnd, InterValue);

            m_lineRenderer.SetPosition(i, TargetPosition);
        }

        // Once position has been calculated and set, the script is complete

    }


}
