using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

/// <summary>
/// Handles scratching out effect and creating a new sprite, inherits from Line Drawer
/// </summary>
public class Scratcher : LineDrawer
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
        screenCapture = scratchRenderCamera.GetComponent<ScreenCapture>();
    }

    private void Update()
    {
        InputHandler();
    }

    /// <summary>
    /// Handles player inputs
    /// </summary>
    void InputHandler()
    {
        //When mouse 0 is pressed
        if (mouse.leftButton.wasPressedThisFrame)
        {
            InstantiateLine();
            drawing = true;
        }

        //When mouse 0 is held
        if (mouse.leftButton.isPressed)
        {
            Scratch();
        }

        //When mouse 0 is released
        if (mouse.leftButton.wasReleasedThisFrame)
        {
            EndScratch();
            
        }
        //When mouse 1 is pressed
        if (mouse.rightButton.wasPressedThisFrame)
        {
            DeleteLastLineRenderer();
        }
        //When mouse 1 is released
        if (mouse.rightButton.wasReleasedThisFrame)
        {
            NewSpriteMask();
        }
    }

    /// <summary>
    /// Manages the scratch effect while the player is drawing
    /// </summary>
    void Scratch()
    {
        //Prevents drawing if the line is past it's max length
        if (vertexPos.Count >= maxVertices) { drawing = false; return; }
        Vector2 mousePosition = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());      //Gets the mouse position and converts to world space coordinates 
        if (Vector2.Distance(mousePosition, vertexPos[vertexPos.Count - 1]) >= vertexThreshold)     //Runs if the mouse has moved far enough away from the last line vertex to set another vertex
        {
            AddVertex(mousePosition);
            NewSpriteMask();
        }
    }

    /// <summary>
    /// Manages the transition out of drawing
    /// </summary>
    void EndScratch()
    {
        drawing = false;

        if (snapToPath)
        {
            SnapLineToPath();
        }

        NewSpriteMask();
        lines.Add(line);  
    }

    /// <summary>
    /// Snaps the line renderer to the path by finding the closest path node to each line renderer vertex
    /// </summary>
    void SnapLineToPath()
    {
        for (int i = 0; i < vertexPos.Count; i++)
        {
            vertexPos[i] = GetClosestPoint(vertexPos[i]);       //Gets closest path point to each line renderer vertex
            vertexPos[i] = new Vector3(vertexPos[i].x, vertexPos[i].y, 0);  //Zeros the z-axis
            lineRenderer.SetPosition(i, vertexPos[i]);   //Sets new vertex position 
        }
    }

    /// <summary>
    /// Deletes the last line rendered and removes it from the list of rendered lines
    /// </summary>
    void DeleteLastLineRenderer()
    {
        Destroy(line);
        lines.RemoveAt(lines.Count - 1);
        if(lines.Count != 0)
        {
            line = lines[lines.Count - 1];  //Sets new line as the new last index in lines as the previous was deleted and removed
        }        
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
}
