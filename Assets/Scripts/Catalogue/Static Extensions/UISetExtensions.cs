using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public static class UISetExtensions
{
    private static readonly MethodInfo toggleSetMethod;
    private static readonly MethodInfo sliderSetMethod;
    private static readonly MethodInfo scrollbarSetMethod;

    private static readonly FieldInfo dropdownValueField;
    //private static readonly MethodInfo dropdownRefreshMethod;  // Unity 5.2 <= only

    static UISetExtensions()
    {
        // Find the Toggle's set method
        toggleSetMethod = FindSetMethod(typeof(Toggle));

        // Find the Slider's set method
        sliderSetMethod = FindSetMethod(typeof(Slider));

        // Find the Scrollbar's set method
        scrollbarSetMethod = FindSetMethod(typeof(Scrollbar));

        // Find the Dropdown's value field and its' Refresh method
        dropdownValueField = (typeof(Dropdown)).GetField("m_Value", BindingFlags.NonPublic | BindingFlags.Instance);
        //dropdownRefreshMethod = (typeof(Dropdown)).GetMethod("Refresh", BindingFlags.NonPublic | BindingFlags.Instance);  // Unity 5.2 <= only
    }

    public static void Set(this Toggle instance, bool value, bool sendCallback = false)
    {
        toggleSetMethod.Invoke(instance, new object[] { value, sendCallback });
    }

    public static void Set(this Slider instance, float value, bool sendCallback = false)
    {
        sliderSetMethod.Invoke(instance, new object[] { value, sendCallback });
    }

    public static void Set(this Scrollbar instance, float value, bool sendCallback = false)
    {
        scrollbarSetMethod.Invoke(instance, new object[] { value, sendCallback });
    }

    public static void Set(this Dropdown instance, int value)
    {
        dropdownValueField.SetValue(instance, value);
        //dropdownRefreshMethod.Invoke(instance, new object[] { }); // Unity 5.2 <= only

        /* In Unity v. 5.3 and above, they removed the private "Refresh" method and now instead you need to call instance.RefreshShownValue(); instead. */
        instance.RefreshShownValue(); // Unity 5.3 >= only
    }

    private static MethodInfo FindSetMethod(System.Type objectType)
    {
        var methods = objectType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
        for (var i = 0; i < methods.Length; i++)
        {
            if (methods[i].Name == "Set" && methods[i].GetParameters().Length == 2)
            {
                return methods[i];
            }
        }
        return null;
    }


    static Slider.SliderEvent emptySliderEvent = new Slider.SliderEvent();
    public static void SetValue(this Slider instance, float value)
    {
        var originalEvent = instance.onValueChanged;
        instance.onValueChanged = emptySliderEvent;
        instance.value = value;
        instance.onValueChanged = originalEvent;
    }

    static Toggle.ToggleEvent emptyToggleEvent = new Toggle.ToggleEvent();
    public static void SetValue(this Toggle instance, bool value)
    {
        var originalEvent = instance.onValueChanged;
        instance.onValueChanged = emptyToggleEvent;
        instance.isOn = value;
        instance.onValueChanged = originalEvent;
    }

    static InputField.OnChangeEvent emptyInputFieldEvent = new InputField.OnChangeEvent();
    public static void SetValue(this InputField instance, string value)
    {
        var originalEvent = instance.onValueChanged;
        instance.onValueChanged = emptyInputFieldEvent;
        instance.text = value;
        instance.onValueChanged = originalEvent;
    }

    // TODO: Add more UI types here.
}

