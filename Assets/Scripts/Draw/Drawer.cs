using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

/// <summary>
/// Script that handles drawing function
/// </summary>
public class Drawer : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Variables
    /// </summary>
    [Header("Variables")]
    [Tooltip("Bool to determine if drawing is limited to within the radius of the circle around the player")][SerializeField]
    protected bool limitDrawing;
    [Tooltip("Minimum distance between each brush instantiation")][Range(0.001f, 10f)][SerializeField] 
    protected float drawThreshold;
    //[Tooltip("Maximum distance between brush instantiation and player")]
    //protected float drawThresholdToPlayer;
    [Tooltip("Minimum distance between each mouse and brush stroke to erase")] [Range(0.1f, 10f)][SerializeField] 
    protected float eraseRadius;
    [Tooltip("Size of the brush")][Range(0.1f,10f)][SerializeField] 
    protected float brushRadius;
    [Tooltip("Size of the circle")][Range(0.1f, 10f)][SerializeField] 
    protected float circleRadius;
    [Tooltip("Maximum number of brush strokes the player can take")][SerializeField][Range(0, 10000)]
    protected int maxBrushStroke;
    [Tooltip("Number of brush strokes taken")][SerializeField] 
    protected int brushStrokesTaken;           //Serialized for debugging
    [Tooltip("Determines if player is drawing")]
    protected bool drawing;
    [Tooltip("Determines if player is erasing")]
    protected bool erasing;
    #endregion

    #region References
    /// <summary>
    /// References
    /// </summary>
    [Header("References")]
    [Tooltip("Main Camera")]
    protected Camera cam;
    [Tooltip("The player component")][SerializeField]
    protected GameObject player;
    [Tooltip("The circle")]
    protected GameObject circle;
    [Tooltip("The circle prefab")]
    public GameObject circlePrefab;    
    [Tooltip("The brush")]
    protected GameObject brush;
    [Tooltip("The brush prefab")]
    public GameObject brushPrefab;
    [Tooltip("The brush sprite")]
    protected GameObject brushSprite;
    [Tooltip("The brush sprite")]
    public GameObject brushSpritePrefab;
    [Tooltip("List of all brush strokes instantiated and active")]
    public List<GameObject> brushesDrawn = new List<GameObject>();
    [Tooltip("Positions of all the nodes of the path")]
    public List<Transform> pathPoints = new List<Transform>();
    [Tooltip("Reference to mouse")]
    protected Mouse mouse;
    #endregion

    /// <summary>
    /// Initialises the circle by spawning on the player and setting the correct scale
    /// </summary>
    protected void InitialiseCircle()
    {
        circle = Instantiate(circlePrefab, player.transform.position, Quaternion.identity);
        circle.transform.localScale = new Vector3(circleRadius, circleRadius, circleRadius);
    }

    /// <summary>
    /// Updates player circle to track the player
    /// </summary>
    protected void UpdateCircle()
    {
        circle.transform.position = player.transform.position;
        //circle.transform.localScale = new Vector3(circleRadius, circleRadius, circleRadius); 
    }

    /// <summary>
    /// Draws brush strokes at an input position
    /// </summary>
    /// <param name="position"></param>
    protected void DrawBrush(Vector2 position)
    {
        //Draw brush, instantiate
        //Add to list 
        if (brushStrokesTaken >= maxBrushStroke) { UpdateBrushSprite(position, false); return; }  
        if (GetBrushesInRange(position, drawThreshold).Count > 0) { return; }
        if (limitDrawing) { if (Vector2.Distance(position, player.transform.position) > (circle.transform.localScale.x / 2) - (brushRadius / 2)) { UpdateBrushSprite(position, false); return; } }   //Limits drawing within radius of player circle
        var mousePos = mouse.position.ReadValue();
        if (mousePos.x < 0 || mousePos.y < 0 || mousePos.x > Screen.width || mousePos.y > Screen.height) { return; }    //Prevents drawing if mouse is off screen

        brush = Instantiate(brushPrefab, position, Quaternion.identity);
        brush.transform.localScale *= brushRadius;
        brushesDrawn.Add(brush);
        brushStrokesTaken++;
        UpdateBrushSprite(position, true);
    }

    void UpdateBrushSprite(Vector2 position, bool show)
    {
        brushSprite.transform.position = position;
        brushSprite.GetComponent<SpriteRenderer>().enabled = show;
    }

    /// <summary>
    /// Erases brush strokes 
    /// </summary>
    /// <param name="position"></param>
    protected void EraseBrush(Vector2 position)
    {
        List<GameObject> brushes2Erase = GetBrushesInRange(position, eraseRadius);  //List off all brush strokes in range of cursor position to be erased

        for (int i = 0; i < brushes2Erase.Count; i++)
        {
            int j = brushesDrawn.IndexOf(brushes2Erase[i]);     //Gets the index of a brush being erased from the list of drawn brushes, int j

            GameObject brush2bErased = brushesDrawn[j];         //Reference to this brush stroke

            brushesDrawn.RemoveAt(j);     //Removes this brush from the list of brushes drawn                      
            Destroy(brush2bErased);       //Destroys this brush
            brushStrokesTaken--;          //Removes 1 from brush strokes taken
        }
    }

    /// <summary>
    /// Initialises the path list that the player can move along
    /// </summary>
    protected void InitialisePathList()
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }

        for (int i = 0; i < children.Count; i++)
        {
            if (children[i].CompareTag("pathPoints"))
            {
                pathPoints.Add(children[i].transform);
            }
        }

        player.GetComponent<PlayerController>().InitialisePlayer(pathPoints);
    }

    /// <summary>
    /// Returns a list of drawn brushes in within a radius of the mouse position
    /// </summary>
    /// <param name="mousePosition"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    private List<GameObject> GetBrushesInRange(Vector3 mousePosition, float radius)
    {
        List<GameObject> brushesInRange = new List<GameObject>();   

        for (int i = 0; i < brushesDrawn.Count;i++)
        {
            Vector2 dist2Brush = brushesDrawn[i].transform.position - mousePosition;    //Vector between the mouse position and indexed drawn brush position
            float distance = dist2Brush.sqrMagnitude;       //Squared magnitude of this vector

            if (distance <= Mathf.Pow(radius,2))      //Comparing the square of the radius to the square magnitude of the vector, computationally cheaper than the square root approach specifically for large numbers of calculations
            {
                brushesInRange.Add(brushesDrawn[i]);    //Add the brush to the list of brushes in range
            }
        }
        return brushesInRange;  //Return the list of all brushes in range
    }

    /// <summary>
    /// Returns list of transforms of all the path points
    /// </summary>
    /// <returns></returns>
    public List<Transform> GetPathPoints()
    {
        return pathPoints;
    }
}
