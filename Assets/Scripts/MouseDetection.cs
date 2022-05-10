using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDetection : MonoBehaviour
{
    Scratcher scratcher;

    private void Awake()
    {
        scratcher = FindObjectOfType<Scratcher>();
    }

    protected void Printy(string tag)
    {
        Debug.Log(tag);
    }
}
