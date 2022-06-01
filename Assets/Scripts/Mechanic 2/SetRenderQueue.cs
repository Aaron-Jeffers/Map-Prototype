using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRenderQueue : MonoBehaviour
{
    public int renderQueueNum;
    void Start()
    {
        List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
        spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());

        for(int i = 0; i < spriteRenderers.Count; i++)
        {
            spriteRenderers[i].material.renderQueue = renderQueueNum;
        }

        List<Renderer> renderers = new List<Renderer>();
        renderers.AddRange(GetComponentsInChildren<Renderer>());

        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].material.renderQueue = renderQueueNum;
            renderers[i].material.SetFloat("MyScrMode", 2);
        }
    }
}
