using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fill2D : MonoBehaviour
{
    [SerializeField] float scaleFactor;
    [SerializeField] float scaleRate;
    public Vector2 originalScale;
    bool scale;
    private void OnEnable()
    {
        scale = true;
        originalScale = transform.localScale;
        transform.localScale *= scaleFactor;
    }
    private void Update()
    {
        if (!scale) { return; }

        transform.localScale += transform.localScale * scaleRate * Time.deltaTime;

        if(transform.localScale.x >= originalScale.x)
        {
            transform.localScale = originalScale;
            scale = false;
        }
    }
}
