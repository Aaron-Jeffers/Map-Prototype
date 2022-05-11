using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;


public class MouseDetection : MonoBehaviour
{ 
    protected Camera camera;
    protected Scratcher scratcher;
    protected GameObject redCross;
    protected bool drawing;

    private void Awake()
    {
        scratcher = FindObjectOfType<Scratcher>();
        redCross = Resources.Load<GameObject>("RedCross");
        camera = Camera.main;
    }

    protected void PrintTag(string tag)
    {
        Debug.Log(tag);
    }

    private void FixedUpdate()
    {
        drawing = scratcher.IsDrawing();
    }

    
}
