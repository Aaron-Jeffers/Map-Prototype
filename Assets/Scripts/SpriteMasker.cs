using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class SpriteMasker : Drawer
{
    #region References
    [Tooltip("The sprite mask used for the scratch effect")]
    public SpriteMask spriteMask;
    [Tooltip("The camera used to render the sprite mask used for the scratch effect")]
    public Camera scratchRenderCamera;
    ScreenCapture screenCapture;    //Screen capture script
    #endregion

    private void Awake()
    {
        mouse = Mouse.current;
        cam = Camera.main;
        screenCapture = scratchRenderCamera.GetComponent<ScreenCapture>();
        brushPrefab = Resources.Load<GameObject>("Brush").gameObject;
        circlePrefab = Resources.Load<GameObject>("PlayerCircle").gameObject;
        
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
        NewSpriteMask();
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

    void Draw()
    {
        DrawBrush(cam.ScreenToWorldPoint(mouse.position.ReadValue()));
        NewSpriteMask();
    }
    void Erase()
    {
        EraseBrush(cam.ScreenToWorldPoint(mouse.position.ReadValue()));
        NewSpriteMask();
    }

    /// <summary>
    /// Generates a new sprite mask
    /// </summary>
    void NewSpriteMask()
    {
        spriteMask.sprite = screenCapture.ReturnSpriteMask();
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
