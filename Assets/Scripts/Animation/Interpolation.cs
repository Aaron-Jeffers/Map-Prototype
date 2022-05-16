using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Interpolation : MonoBehaviour
{
    public AnimationCurve interpolationCurve;
    public AnimationCurve interpolationCurveFlipped;
    
    [Range(1f,10f)]
    public float scaleOffset;
    float xMax, xMin;
    int length;

    public bool flipped;
    public bool scaling;

    private void Start()
    {
        FlipCurve();
        
    }
    private void Update()
    {
   
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            //scaling = true;
            AnimationCurve curve2follow = new AnimationCurve();

            if (flipped) { curve2follow = interpolationCurve; }
            if(!flipped) { curve2follow = interpolationCurveFlipped; }
            StartCoroutine(ScaleCircle(curve2follow, xMax));
        }
            
      
    }

    void FlipCurve()
    {
        length = interpolationCurve.length;
        xMin = interpolationCurve.keys[0].time;
        
        xMax = interpolationCurve[length - 1].time;
        
        Debug.Log("Length: " + length + " xMax: " + xMax + " xMin: " + xMin);
        interpolationCurveFlipped = new AnimationCurve();
        interpolationCurveFlipped.preWrapMode = WrapMode.Clamp;
        interpolationCurveFlipped.postWrapMode = WrapMode.Clamp;

        for (int i = 0; i < length; i++)
        {
            Debug.Log(i);
            float time = interpolationCurve.keys[i].time;
            float value = interpolationCurve.keys[i].value - 1;
            
            float newTime = (xMax - time);
            Keyframe key = new Keyframe(newTime, value);
            
            interpolationCurveFlipped.AddKey(key);
            Debug.Log(interpolationCurveFlipped.keys[i].value);
        }
        
        
    }
    IEnumerator ScaleCircle(AnimationCurve curve,float time)
    {
        Vector3 originalScale = transform.localScale;
        for (float t = 0f; t < time-0.02; t += Time.deltaTime)
        {
            float scaleAmount = curve.Evaluate(t) * scaleOffset;
            Vector3 scale = new Vector3(scaleAmount, scaleAmount, scaleAmount);
            transform.localScale = originalScale + scale;
            yield return null;
        }
        //scaling = false;
    }
}
