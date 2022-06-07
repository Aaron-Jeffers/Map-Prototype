using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

/// <summary>
/// Class that any other class requiring a mouse over event can inherit from. Holds required references to game components.
/// </summary>
public class MouseDetection : MonoBehaviour
{ 
    #region References
    protected Camera camera;      //Main game camera
    protected Scratcher scratcher;  //Reference to the scratcher class
    protected GameObject redCross;  //Red cross prefab    ///Note: Should place elsewhere
    protected bool drawing;     //Bool to determine if the player is drawing/scratching a line
    #endregion

    private void Awake()
    {
        scratcher = FindObjectOfType<Scratcher>();
        redCross = Resources.Load<GameObject>("RedCross");
        camera = Camera.main;
    }

    /// <summary>
    /// Used for debugging purposes to print the tag of any object the mouse passes over
    /// </summary>
    /// <param name="tag"></param>
    protected void PrintTag(string tag)
    {
        Debug.Log(tag);
    }

    private void FixedUpdate()
    {
        drawing = scratcher.IsDrawing();
    }  
}
