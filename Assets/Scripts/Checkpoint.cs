using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MouseDetection
{
    private void OnMouseEnter()
    {
        Printy(tag);
        gameObject.SetActive(false);
    }
}
