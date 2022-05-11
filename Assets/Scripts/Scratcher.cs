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
    /// <summary>
    /// Reference
    /// </summary>
    public SpriteMask spriteMask;
    public Camera scratchRenderCamera;
    ScreenCapture screenCapture;

    private void Awake()
    {
        screenCapture = scratchRenderCamera.GetComponent<ScreenCapture>();
    }

    private void Update()
    {
        if(drawing)
        {
            //spriteMask.sprite = screenCapture.ReturnSpriteMask();
        }

        InputHandler();
    }

    void InputHandler()
    {
        //When mouse 1 is pressed
        if (mouse.leftButton.wasPressedThisFrame)
        {
            InstantiateLine();
            drawing = true;
        }

        //When mouse 1 is held
        if (mouse.leftButton.isPressed)
        {
            Scratch();
        }

        //When mouse 1 is released
        if (mouse.leftButton.wasReleasedThisFrame)
        {
            EndScratch();
            //Vector2 warpPosition = Screen.safeArea.center;  // never let it move
            //mouse.WarpCursorPosition(warpPosition);
            //InputState.Change(mouse.position, warpPosition);
        }
    }

    void Scratch()
    {
        if (vertexPos.Count >= maxVertices) { drawing = false; return; }
        Vector2 mousePosition = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());       
        if (Vector2.Distance(mousePosition, vertexPos[vertexPos.Count - 1]) >= vertexThreshold)
        {
            AddVertex(mousePosition);
            spriteMask.sprite = screenCapture.ReturnSpriteMask();
        }
    }

    void EndScratch()
    {
        drawing = false;

        if (snapToPath)
        {
            for (int i = 0; i < vertexPos.Count; i++)
            {
                vertexPos[i] = GetClosestPoint(vertexPos[i]);       //Gets closest path point to each line renderer vertex
                vertexPos[i] = new Vector3(vertexPos[i].x, vertexPos[i].y, 0);  //Zeros the z-axis
                lineRenderer.SetPosition(i, vertexPos[i]);   //Sets new vertex position 
            }
        }
        spriteMask.sprite = screenCapture.ReturnSpriteMask();
        //Destroy(line);
    }

    public bool IsDrawing()
    {
        return drawing;
    }
}
