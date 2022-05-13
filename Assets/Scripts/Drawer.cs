using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

/// <summary>
/// Script that handles line drawing and snapping the line to a path
/// </summary>
public class Drawer : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Variables
    /// </summary>
    [Header("Variables")]
    [Tooltip("Minimum distance between each brush instantiation")]
    [SerializeField] protected float drawThreshold;
    [Tooltip("Maximum distance between brush instantiation and player")]
    protected float drawThresholdToPlayer;
    [Tooltip("Bool to determine maximum distance between brush instantiation and player")]
    [SerializeField] protected bool limitDrawing;
    [Tooltip("Minimum distance between each mouse and brush stroke to erase")]
    [SerializeField] protected float eraseRadius;
    [Tooltip("Size of the brush")]
    [Range(0.1f,10f)]
    [SerializeField] protected float brushRadius;
    [Tooltip("Size of the circle")]
    [Range(0.1f, 10f)]
    [SerializeField] protected float circleRadius;
    [Tooltip("Maximum number of brush strokes")]
    [SerializeField] protected int maxBrushStroke;
    [Tooltip("Number of brush strokes taken")]
    [SerializeField] protected int brushStrokesTaken;
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
    [Tooltip("The player component")]
    [SerializeField]protected GameObject player;
    [Tooltip("The circle")]
    public GameObject circle;
    [Tooltip("The circle prefab")]
    public GameObject circlePrefab;    
    [Tooltip("The brush")]
    public GameObject brush;
    [Tooltip("The brush prefab")]
    public GameObject brushPrefab;
    [Tooltip("List of all brush strokes instantiated and active")]
    public List<GameObject> brushesDrawn = new List<GameObject>();
    [Tooltip("Positions of all the nodes of the path")]
    public List<Transform> pathPoints = new List<Transform>();
    [Tooltip("Reference to mouse")]
    protected Mouse mouse;
    #endregion

    protected void InitialiseCircle()
    {
        circle = Instantiate(circlePrefab, player.transform.position, Quaternion.identity);
        circle.transform.localScale *= circleRadius;
    }

    protected void UpdateCircle()
    {
        //Move circle along with player

        circle.transform.position = player.transform.position;
    }

    protected void DrawBrush(Vector2 position)
    {
        //Draw brush, instantiate
        //Add to list 
        if (brushStrokesTaken >= maxBrushStroke) { return; }
        if(limitDrawing)
        {
            if (Vector2.Distance(position, player.transform.position) > (circle.transform.localScale.x / 2)) { return; }
        }        
        brush = Instantiate(brushPrefab, position, Quaternion.identity);
        brush.transform.localScale *= brushRadius;
        brushesDrawn.Add(brush);
        brushStrokesTaken++;
    }

    protected void EraseBrush(Vector2 position)
    {
        //Get list of all brushes within radius
        //Remove from list
        //Delete
        List<GameObject> brushes2Erase = GetBrushesInRange(position);

        for (int i = 0; i < brushes2Erase.Count; i++)
        {
            int j = brushesDrawn.IndexOf(brushes2Erase[i]);

            GameObject brush2bErased = brushesDrawn[j];

            brushesDrawn.RemoveAt(j);
            Destroy(brush2bErased);
            brushStrokesTaken--;
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

    private List<GameObject> GetBrushesInRange(Vector3 mousePosition)
    {
        List<GameObject> brushesInRange = new List<GameObject>();

        for (int i = 0; i < brushesDrawn.Count;i++)
        {
            Vector2 dist2Brush = brushesDrawn[i].transform.position - mousePosition;
            float distance = dist2Brush.sqrMagnitude;

            if (distance <= Mathf.Pow(eraseRadius,2))
            {
                brushesInRange.Add(brushesDrawn[i]);
            }
        }
        return brushesInRange;
    }

    public List<Transform> GetPathPoints()
    {
        return pathPoints;
    }
}
