using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blotch : MonoBehaviour
{
    float timer;
    [SerializeField] float startFadeTime;
    [SerializeField] float fadeRate;
    [SerializeField] float fadeLimit;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= startFadeTime)
        {
            transform.position += new Vector3(0, 0, fadeRate) * Time.deltaTime;
            if(transform.position.z >= fadeLimit)
            {
                Destroy(gameObject);
            }
        }
    }
}
