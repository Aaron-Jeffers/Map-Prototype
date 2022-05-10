using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MouseDetection
{
    private void OnMouseEnter()
    {
        if(drawing)
        {
            PrintTag(tag);
            gameObject.SetActive(false);
        }        
    }
}
