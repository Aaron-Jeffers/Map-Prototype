using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

/// <summary>
/// Script that handles line drawing and snapping the line to a path
/// </summary>
public class LineDrawer : MonoBehaviour
{
    #region Variables
    /// <summary List of line variables>
    /// Variables
    /// </summary>
    [Tooltip("Distance between each vertex of the line renderer")]
    public float vertexThreshold;
    [Tooltip("Maximum length of the line being drawn")]
    public float maxLength;
    [Tooltip("Maximum vertices based off maximum line length and vertex threshold")]
    protected int maxVertices;
    [Tooltip("Width of the line renderer")]
    public float thickness;
    [Tooltip("Wether to snap the line renderer to the path once a player has let go of the mouse button")]
    public bool snapToPath;
    [Tooltip("Determines if player is drawing a line")]
    protected bool drawing;
    [Tooltip("Determines if player can draw more than one line")]
    protected bool multiLine;
    [Tooltip("Determines if the player has drawn a line before")]
    protected bool lineDrawn;
    [Tooltip("Determines if the cursor position should be set to the previous line renderers position on instantiation of a new line")]
    public bool snapToLastLine;
    #endregion

    #region References
    /// <summary List of line references>
    /// References
    /// </summary>
    [Tooltip("Main Camera")]
    protected Camera cam;  
    [Tooltip("The line prefab")]
    public GameObject line;
    [Tooltip("The line renderer component")]
    protected LineRenderer lineRenderer;
    [Tooltip("Vertex positions of the line renderer")]
    protected List<Vector3> vertexPos = new List<Vector3>();
    [Tooltip("Positions of all the nodes of the path")]
    public List<Transform> pathPoints = new List<Transform>();
    [Tooltip("List of drawn line renderers")]
    public List<GameObject> lines = new List<GameObject>();
    [Tooltip("Reference to mouse")]
    protected Mouse mouse;
    #endregion

    void Start()
    {       
        mouse = Mouse.current;
        lineRenderer = GetComponent<LineRenderer>();
        cam = Camera.main;
        maxVertices = Mathf.RoundToInt(maxLength / vertexThreshold);

        InitialisePathList ();
    }

    /// <summary>
    /// Initialises the path list that the player can move along
    /// </summary>
    void InitialisePathList()
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
    }

    /// <summary>
    /// Instantiates new line and deletes old one if applicable
    /// </summary>
    protected void InstantiateLine()
    {       
        vertexPos.Clear();  //Clears list of vertex points

        //Instantiates new line and sets parameters
        line = Instantiate(Resources.Load<GameObject>("LineDrawer"), Vector3.zero, Quaternion.identity);
        lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.startWidth = thickness;
        lineRenderer.endWidth = thickness;

        //Sets first two line positions as mouse position
        for (int i = 0; i < 2; i++)
        {
            vertexPos.Add(cam.ScreenToWorldPoint(mouse.position.ReadValue()));
            vertexPos[i] = new Vector3(vertexPos[i].x, vertexPos[i].y, 0);
            lineRenderer.SetPosition(i, vertexPos[i]);
        }

        lineDrawn = true;
    }

    /// <summary>
    /// Adds vertices to the current line being drawn
    /// </summary>
    /// <param name="vertex"></param>
    protected void AddVertex(Vector3 vertex)
    {
        vertexPos.Add(vertex);
        lineRenderer.positionCount = vertexPos.Count;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, vertex);
    }

    /// <summary>
    /// Returns closest path point to the line renderer vertex
    /// </summary>
    /// <param name="vertex"></param>
    /// <returns></returns>
    protected Vector3 GetClosestPoint(Vector3 vertex)
    {
        Vector3 closestPoint = new Vector3(100,100,100);    //Closest point initialised with arbitrarily large number so will never be selected

        float closestDistanceSquared = Mathf.Infinity;  //Closest distance set to infinity for initialisation
        for (int i = 0; i < pathPoints.Count; i++)   //Checks all the points on the path 
        {
            Vector3 vectorToNextPoint = vertex - pathPoints[i].position;  //Gets the vector between vertex and path point
            float squaredDistance = vectorToNextPoint.sqrMagnitude;      //Gets the square distance between the two points, computationally cheap and as each value is scaled up the same amount it is accurate for this purpose

            if (squaredDistance < closestDistanceSquared)     //If distance checked is less than the last closest distance
            {
                closestDistanceSquared = squaredDistance;     //Becomes new closest distance
                closestPoint = pathPoints[i].position;        //Closest point is the path point being checked
            }
        }
        return closestPoint;   //Return the path point closest to the vertex on the lien renderer
    }
}
