using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InkPot : MonoBehaviour
{
    Camera camera;
    Vector3 offset = new Vector3(10, 5, 10);

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        transform.position = camera.transform.position + offset;
    }
}
