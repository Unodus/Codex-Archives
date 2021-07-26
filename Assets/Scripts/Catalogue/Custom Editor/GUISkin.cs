#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUISkin
{

    public enum GUISkinName
    {
        GUISkinTick,
        GUISkinNormal,
    }

    static List<GUISkinName> m_GUISkinErrorMessage = new List<GUISkinName>();
    static Dictionary<GUISkinName, UnityEngine.GUISkin> m_GUISkin = new Dictionary<GUISkinName, UnityEngine.GUISkin>();
    public static void SetButton(GUISkinName lGUISkinName, UnityEngine.GUISkin lSkin)
    {
        if (GUISkinExists(lGUISkinName) == true)
        {
            lSkin.button = m_GUISkin[lGUISkinName].button;
        }
    }
    public static void SetButtonMiddle(GUISkinName lGUISkinName, UnityEngine.GUISkin lSkin)
    {
        if (GUISkinExists(lGUISkinName) == true)
        {
            lSkin = m_GUISkin[lGUISkinName];
        }
    }
    public static Texture GetButtonTexture(GUISkinName lGUISkinName)
    {
        if (GUISkinExists(lGUISkinName) == true)
        {
            return m_GUISkin[lGUISkinName].button.active.background;
        }
        return null;
    }
    public static Texture GetBoxTexture(GUISkinName lGUISkinName)
    {
        if (GUISkinExists(lGUISkinName) == true)
        {
            return m_GUISkin[lGUISkinName].box.active.background;
        }
        return null;
    }
    public static bool GUISkinExists(GUISkinName lGUISkinName)
    {
        if (m_GUISkin.ContainsKey(lGUISkinName) == false)
        {
            m_GUISkin.Add(lGUISkinName, Resources.Load(lGUISkinName.ToString()) as UnityEngine.GUISkin);
        }
        if (m_GUISkin[lGUISkinName] == null)
        {
            if (m_GUISkinErrorMessage.Contains(lGUISkinName) == false)
            {
                m_GUISkinErrorMessage.Add(lGUISkinName);
            }

        }
        return m_GUISkin[lGUISkinName] != null;
    }
}

#endif