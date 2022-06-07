using System.Collections;
using UnityEngine;

/// <summary>
/// Class that handles the fading and deletion of an object over time
/// </summary>
public class Fade : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Variables
    /// </summary>
    float timer;                    //Simple timer
    [Range(0f,1f)]
    public float fadeAmount = 0f;   //Final alpha level of object material to be faded to, 1 being opaque, 0 being transparent
    Vector2 fadeTimeRange = new Vector2(2f, 7f);    //Time steps at which fading should begin and end
    bool fading;      //Determines if object is already fading
    bool timerStarted;    //Determines whether or not the timer has started
    #endregion

    /// <summary>
    /// Starts the fade timer
    /// </summary>
    public void StartTimer()
    {
        timerStarted = true;
    }

    private void Update()
    {
        if(timerStarted)
        {
            timer += Time.deltaTime;
        }
        
        //If the initial fade time range step is met
        if(timer >= fadeTimeRange.x && !fading)
        {
            StartCoroutine(FadeTo(fadeAmount, fadeTimeRange.y - fadeTimeRange.x));
            fading = true;
        }

        //If the second fade time range step is met
        if(timer > fadeTimeRange.y + 0.05f)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Coroutine that reduces an object's material alpha channel from it's original value to a set value over a set amount of time
    /// </summary>
    /// <param name="value"></param>
    /// <param name="time"></param>
    /// <returns></returns>
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
