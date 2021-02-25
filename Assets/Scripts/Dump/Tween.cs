using UnityEngine;

public class Tween
{
    public delegate float LerpDelegate(float pct);
    public static float Lerp(float start_value, float end_value, float pct)
    {
        pct = Mathf.Clamp01(pct);
        return (start_value + (end_value - start_value) * pct);
    }
    public static float LerpLerp(LerpDelegate Method1, LerpDelegate Method2, float t)
    {
        return Lerp(Method1(t),Method2(t), t);
    }

    public static float EaseIn(float t)
    {
        return t * t;
    }
    public static float EaseIn(float t, int n) //override for powers of n
    {
        float newT = t;
        for (int i = 0; i < n; i++)
        {
            newT *= t;
        }
        return newT;
    }

    public static float SinLerp(float t)
    {
        return 0.5f * (Mathf.Sin(t * 2 * Mathf.PI) + 1.0f);
    }


    

    public static float EaseOut(float t)
    {
        return Flip(EaseIn(Flip(t)));
    }
    public static float EaseOut(float t, int n) //override for powers of n
    {
        return Flip(EaseIn(Flip(t), n));
    }


    public static float EaseInOut(float t)
    {
        return Lerp(EaseIn(t), EaseOut(t), t);
    }
    public static float EaseInOut(float t, int n)
    {
        return Lerp(EaseIn(t, n), EaseOut(t, n), t);
    }

    static float Flip(float x)
    {
        return 1 - x;
    }

    public static float Spike(float t)// spikes go up and then down
    {
        if (t <= .5f)
            return EaseIn(t / .5f);

        return EaseIn(Flip(t) / .5f);
    }

    public static float OutBounce(float t)
    {
            return (1 - InBounce(1 - t));        
    }
    public static float InBounce(float t)
    {
        return Mathf.Abs(t);
    }

    public static float Clamp(float min, float max, float t) //takes a number between two numbers and returns it between 0 and 1
    {
        t -= min;   
        t /= (max - min); 
        return t;
    }

    public static float Unclamp(float min, float max, float t)// takes a number between 0 and 1 and converts it between two ranges
    {
        return t;
    }



}
