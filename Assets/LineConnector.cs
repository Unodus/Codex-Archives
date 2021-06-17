using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineConnector : MonoBehaviour
{

    [SerializeField] LineRenderer m_lineRenderer;
    [SerializeField] Transform[] lineObjectPositions;

    public void OnDrawGizmos()
    {
        UpdatePositions();
    }

    private void Update()
    {
        UpdatePositions();
    }

    private void UpdatePositions()
    {
        if (m_lineRenderer == null) return;
        if (m_lineRenderer.positionCount <= 1) return;

        if (lineObjectPositions == null) return;
        if (lineObjectPositions.Length == 0) return;

        for (int i = 0; i < lineObjectPositions.Length; i++)
        {
            if (lineObjectPositions[i] == null) return;
        }

        gameObject.name = "Line: "; 

        for (int i = 0; i < m_lineRenderer.positionCount; i++)
        {
            float j = i / (float)(m_lineRenderer.positionCount - 1.0f);

            float LineI = Mathf.Lerp(0, 1, j);


            float LinePosition = Mathf.Lerp(0, lineObjectPositions.Length - 1, LineI);
            gameObject.name += " " + LinePosition;

            int StartIndex = Mathf.FloorToInt(LinePosition);
            int EndIndex = Mathf.CeilToInt(LinePosition);

            
//            float RelativeLinePosition = Mathf.Lerp(0, lineObjectPositions.Length - 1, i);


            float RelativeLinePosition = Mathf.Lerp(StartIndex, EndIndex, LinePosition);

            Vector3 LineObjectStart = lineObjectPositions[StartIndex  ].position;            
            Vector3 LineObjectEnd = lineObjectPositions[EndIndex].position;

            Vector3 TargetPosition = Vector3.Lerp(LineObjectStart, LineObjectEnd, RelativeLinePosition);

            


            m_lineRenderer.SetPosition(i, TargetPosition);
        }


    }


}
