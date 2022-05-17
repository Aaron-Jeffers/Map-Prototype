using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class Masker : Drawer
{
    private void Awake()
    {
        mouse = Mouse.current;
        cam = Camera.main;
        //brushPrefab = Resources.Load<GameObject>("Brush").gameObject;
        //circlePrefab = Resources.Load<GameObject>("PlayerCircle").gameObject;
        
        InitialisePathList();
    }
    private void Start()
    {
        InitialiseCircle();
    }
    private void Update()
    {
        InputHandler();
        UpdateCircle();
        //NewSpriteMask();
        
    }

    /// <summary>
    /// Handles player inputs
    /// </summary>
    void InputHandler()
    {
        //When mouse 0 is pressed
        if (mouse.leftButton.wasPressedThisFrame)
        {
            Draw();
            drawing = true;
        }

        //When mouse 0 is held
        if (mouse.leftButton.isPressed && drawing)
        {
            Draw();
        }

        //When mouse 0 is released
        if (mouse.leftButton.wasReleasedThisFrame)
        {
            //NewSpriteMask();
            drawing = false;
        }

        if (drawing) { return; }

        //When mouse 1 is pressed
        if (mouse.rightButton.wasPressedThisFrame)
        {
            Erase();
            erasing = true;
        }
        //When mouse 1 is held
        if (mouse.rightButton.isPressed && erasing)
        {
            Erase();
        }
        //When mouse 1 is released
        if (mouse.rightButton.wasReleasedThisFrame)
        {
            //NewSpriteMask();
            erasing = false;
        }
    }

    /// <summary>
    /// Calls the draw brush method. Potentially deprecated from earlier versions with more functionality, might change to a direct call rather than this reference method.
    /// </summary>
    void Draw()
    {
        DrawBrush(cam.ScreenToWorldPoint(mouse.position.ReadValue()));
    }

    /// <summary>
    /// Calls the erase brush method. Potentially deprecatedfrom earlier versions with more functionality, might change to a direct call rather than this reference method.
    /// </summary>
    void Erase()
    {
        EraseBrush(cam.ScreenToWorldPoint(mouse.position.ReadValue()));
    }

    /// <summary>
    /// Simple boolean return for whether or not teh player is currently drawing
    /// </summary>
    /// <returns></returns>
    public bool IsDrawing()
    {
        return drawing;
    }

    /// <summary>
    /// Simple boolean to set drawing state
    /// </summary>
    /// <returns></returns>
    public void SetDrawing(bool draw)
    {
        drawing = draw;
    }
}
