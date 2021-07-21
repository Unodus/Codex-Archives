using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ToggleGroupExtensions
{

    private static System.Reflection.FieldInfo m_ToggleListMember;

    /// <summary>
    /// Gets the list of toggles. Do NOT add to the list, only read from it.
    /// </summary>
    /// <param name="grp"></param>
    /// <returns></returns>
    public static IList<Toggle> GetToggles(this ToggleGroup grp)
    {
        if (m_ToggleListMember == null)
        {
            m_ToggleListMember = typeof(ToggleGroup).GetField("m_Toggles", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (m_ToggleListMember == null)
                throw new System.Exception("UnityEngine.UI.ToggleGroup source code must have changed in latest version and is no longer compatible with this version of code.");
        }
        return m_ToggleListMember.GetValue(grp) as IList<Toggle>;
    }

    public static int Count(this ToggleGroup grp)
    {
        return GetToggles(grp).Count;
    }

    public static Toggle Get(this ToggleGroup grp, int index)
    {
        return GetToggles(grp)[index];
    }

    public static bool Active(this ToggleGroup grp)
    {
        bool bActive = false;
        foreach (Toggle t in GetToggles(grp))
        {
            if (true == t.isOn)
            {
                bActive = true;
                break;
            }
        }

        return bActive;
    }

}
