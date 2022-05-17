using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class used to interpolate values evaluated as a function of time of an animation curve, i.e. Curve(t)
/// </summary>
class Interpolation : MonoBehaviour
{
    [Tooltip("Animation curve used to set scale interpolation over time. Advise limiting y-value between the range (-1,1) and using scaleOffset to set magnitude. There is currently no way to automaticallly limit this in unity. X-axis or time can be set from (0,infinity)")][SerializeField]
    AnimationCurve interpolationCurve;
    AnimationCurve interpolationCurveFlipped;
    
    [Range(1f,10f)][Tooltip("Magnitude of interpolation value at time(t) of animation curve")][SerializeField]
    float scaleOffset;
    float xMax, xMin;
    int length;
    bool flipped, scaling;

    private void Start()
    {
        FlipCurve();
        
    }
    private void Update()
    {
   
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (scaling) { return; }
            AnimationCurve curve2follow = new AnimationCurve();

            if(flipped)
            {
                curve2follow = interpolationCurveFlipped;
            }
            else if (!flipped)
            {
                curve2follow = interpolationCurve;
            }

            StartCoroutine(ScaleCircle(curve2follow, xMax));
        }
            
      
    }

    /// <summary>
    /// Creates a duplicated but mirrored instance of the animation curve to handle reverse interpolation to the original value
    /// </summary>
    void FlipCurve()
    {
        length = interpolationCurve.length;          //Number of key frames on the curve
        xMin = interpolationCurve.keys[0].time;      //Time value of first key
        xMax = interpolationCurve[length - 1].time;  //Time value of last key
        
        interpolationCurveFlipped = new AnimationCurve();
        interpolationCurveFlipped.preWrapMode = WrapMode.Clamp;
        interpolationCurveFlipped.postWrapMode = WrapMode.Clamp;

        //Reverses time values of the animation curve and adds them to the new flipped animation curve
        for (int i = 0; i < length; i++)
        {
            float time = interpolationCurve.keys[i].time;
            float value = interpolationCurve.keys[i].value - 1;
            
            float newTime = (xMax - time);
            Keyframe key = new Keyframe(newTime, value);
            
            interpolationCurveFlipped.AddKey(key);
        }
    }

    //Finds the maximum keyframe value in an animation curve
    float GetMaxValue(AnimationCurve curve)
    {
        float maxValue = 0;
        for(int i = 0; i < curve.length; i++)
        {
            if (curve.keys[i].value > maxValue)
            {
                curve.keys[i].value = maxValue;
            }
        }
        return maxValue;
    }

    IEnumerator ScaleCircle(AnimationCurve curve,float time)
    {
        flipped = !flipped;
        scaling = true;

        
        Vector3 originalScale = transform.localScale;
        
        for (float t = 0f; t < time; t += Time.deltaTime)
        {
            float curveValueAtT = curve.Evaluate(t) * scaleOffset;
            Vector3 newScale = new Vector3(curveValueAtT, curveValueAtT, curveValueAtT);
            transform.localScale = originalScale + newScale;
            yield return null;
        }
       
        scaling = false;
    }
}
