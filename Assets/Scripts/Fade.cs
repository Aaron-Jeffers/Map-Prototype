using System.Collections;
using UnityEngine;

public class Fade : MonoBehaviour
{
    float timer;
    readonly float fadeAmount = 0f;
    Vector2 fadeTimeRange = new Vector2(2f, 7f);
    bool fade;

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer >= fadeTimeRange.x && !fade)
        {
            StartCoroutine(FadeTo(fadeAmount, fadeTimeRange.y - fadeTimeRange.x));
            fade = true;
        }
        if(timer > fadeTimeRange.y + 0.05f)
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator FadeTo(float value, float time)
    {
        Color color = GetComponent<SpriteRenderer>().material.color;
        float alpha = color.a;

        for (float t = 0f; t < time; t += Time.deltaTime)
        {
            Color newColor = new Color(color.r, color.g, color.b, Mathf.Lerp(alpha, value, t));
            GetComponent<SpriteRenderer>().material.color = newColor;
            yield return null;
        }
    }
}
