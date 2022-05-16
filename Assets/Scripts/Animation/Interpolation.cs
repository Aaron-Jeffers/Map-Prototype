using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Interpolation : MonoBehaviour
{
    public AnimationCurve interpolationCurve;
    public bool flip;
    private void Update()
    {
        //if (flip) { FlipCurve(); }
    }

    void FlipCurve()
    {
        float xMax, xMin;
        int length;
        length = interpolationCurve.length;
        xMin = interpolationCurve.keys[1].time;
        
        xMax = interpolationCurve[length - 1].time;

        for(int i = 0; i < length; i++)
        {
            float time = interpolationCurve.keys[i].time;
            float value = interpolationCurve.keys[i].value;

            float newTime = (xMax - time) + xMin;

            //interpolationCurve.keys[i].time = newTime;
            Keyframe key = new Keyframe(newTime, value);
            interpolationCurve.MoveKey(i, key);
                       
        }
        flip = !flip;
    }
}
