using System.Collections;
using UnityEngine;

public class Blotch : MonoBehaviour
{
    public float timer;
    [SerializeField] float defaultStartFadeTime;
    [SerializeField] float delayedStartFadeTime;
    [SerializeField] float fadeRate;
    [SerializeField] float fadeLimit;
    bool switchTime;

    public bool SwitchTime { set { switchTime = value; } }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= (switchTime ? delayedStartFadeTime : defaultStartFadeTime))
        {
            float newScale = fadeRate * Time.deltaTime;
            transform.localScale = new Vector2(transform.localScale.x - newScale, transform.localScale.y - newScale);
            if (transform.localScale.x <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
