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
    [Tooltip("Flipped variation of the animation curve")]
    AnimationCurve interpolationCurveFlipped;
    
    [Range(1f,10f)][Tooltip("Magnitude of interpolation value at time(t) of animation curve")][SerializeField]
    float scaleOffset;
    [Tooltip("Maximum time value of the animation curve")]
    float xMax;
    [Tooltip("Number of key frames in the animation curve")]
    int length;
    [Tooltip("Bool to determine which animation curve to use for scaling")]
    bool flipped;
    [Tooltip("Bool to determine if player circle is currently rescaling")]
    bool scaling;

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

    /// <summary>
    /// Interpolates scaling of an object over time based on the evaluation of the animation curve at each point in time
    /// </summary>
    /// <param name="curve"></param>
    /// <param name="time"></param>
    /// <returns></returns>
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
