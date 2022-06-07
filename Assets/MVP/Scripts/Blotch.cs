using System.Collections;
using System;
using UnityEngine;

public class Blotch : MonoBehaviour
{
    public float timer;
    [SerializeField] float defaultStartFadeTime;
    [SerializeField] float delayedStartFadeTime;
    [SerializeField] float fadeRate;
    [SerializeField] float fadeLimit;
    bool switchTime;
    bool fading;
    bool faded;
    int index;
    Drop drop;

    public bool Fading { get => fading; set { fading = value; } }
    public Drop Drop { set { drop = value; } }
    public int Index { set { index = value; } }
    public bool SwitchTime { set { switchTime = value; } }

    // Update is called once per frame
    void Update()
    {
        //if (fading) { return; }
        timer += Time.deltaTime;
        if(timer >= (switchTime ? delayedStartFadeTime : defaultStartFadeTime))
        {
            fading = true;
            //StartCoroutine(Fade(/*callback1 => { faded = callback1; }, callback2 => { }*/));
            float newScale = fadeRate * Time.deltaTime;
            transform.localScale = new Vector2(transform.localScale.x - newScale, transform.localScale.y - newScale);
            if (transform.localScale.x <= 0)
            {
                drop.ReturnInk(gameObject);
                Destroy(gameObject);
            }
        }
    }

    public IEnumerator Fade(/*Action<bool> callback1, Action<GameObject> callback2*/)
    {
        while (transform.localScale.x >= 0)
        {
            float newScale = fadeRate * Time.deltaTime;
            transform.localScale = new Vector2(transform.localScale.x - newScale, transform.localScale.y - newScale);

            if (transform.localScale.x <= 0)
            {
                //drop.RemoveDrops(gameObject);
                //callback1(true);
                //callback2(gameObject);
                yield return null;
            }
        }
    }
}
