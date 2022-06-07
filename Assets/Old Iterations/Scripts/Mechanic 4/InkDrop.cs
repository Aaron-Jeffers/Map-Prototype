using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkDrop : MonoBehaviour
{
    bool fade = false;

    float fadeAmount;
    Vector2 fadeTime;
    float timer = -0.1f;
    public bool Fade{set { fade = value; }}
    public Vector2 FadeTime{ set { fadeTime = value; }}
    public float FadeAmount { set { fadeAmount = value; } }

    //private void Start()
    //{
    //    Debug.Log(fade);
    //}
    private void Update()
    {
        if (fade)
        { 
            timer += Time.deltaTime;
        }
       
        if(timer >= fadeTime.x)
        {
            fade = false;
            StartCoroutine(FadeTo(fadeAmount, fadeTime.y - fadeTime.x));
        }
    }
    
    IEnumerator FadeTo(float value, float time)
    {
        Color color = GetComponent<SpriteRenderer>().material.color;        //Gets the material color
        float alpha = color.a;      //Gets the alpha channel

        for (float t = 0f; t < time; t += Time.deltaTime)
        {
            Color newColor = new Color(color.r, color.g, color.b, Mathf.Lerp(alpha, value, t));     //Creates a new color with a reduced alpha channel
            GetComponent<SpriteRenderer>().material.color = newColor;           //Sets the new color
            yield return null;
        }
    }
}
