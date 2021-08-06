using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDotter : MonoBehaviour
{

    [SerializeField] LineRenderer m_lineRenderer; //Attach the LineRenderer in the Inspector

[SerializeField]    float Distance;
[SerializeField]    float Repetitions;
    public void OnValidate() // Used to update in the Inspector
    {
        UpdateWidths();
    }
    private void LateUpdate() // Update at runtime 
    {
        UpdateWidths();
    }

    private float GetLengthOfLine(LineRenderer line)
    {
        float length = 0;
        for (int i = 1; i < m_lineRenderer.positionCount; i++) length += Vector3.Distance(line.GetPosition(i - 1), line.GetPosition(i));

        return length;
    }
    private void UpdateWidths() // Updates line width based on values
    {
        if (m_lineRenderer == null) if (!TryGetComponent(out m_lineRenderer)) return;
        Distance = GetLengthOfLine(m_lineRenderer);

        float Amount = Distance / Repetitions;

        m_lineRenderer.sharedMaterial.SetTextureScale("_MainTex", new Vector2(Amount, 1.0f));
        //  m_lineRenderer.sharedMaterial. .SetTextureScale("_MainTex", new Vector2(repetition, 1.0f));
        Debug.Log("Set repetition" );

    }
}
