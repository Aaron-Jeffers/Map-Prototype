using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    /// <summary List of line variables>
    /// Variables
    /// </summary>
    public float vertexThreshold;
    public float thickness;
    public bool snapToPath;

    /// <summary List of line references>
    /// References
    /// </summary>
    Camera cam;  //Main camera
    public GameObject line;  //Line to be drawn
    LineRenderer lineRenderer;  //Line renderer component
    public List<Vector3> vertexPos = new List<Vector3>();  //List of line renderer positions
    public List<Transform> pathPoints = new List<Transform>();

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        cam = Camera.main;

        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }
        Debug.Log(children.Count);
        //children.AddRange(GetComponentsInChildren<GameObject>());

        for (int i = 0; i < children.Count; i++)
        {
            if (children[i].CompareTag("pathPoints"))
            {
                pathPoints.Add(children[i].transform);
            }
        }
    }

    void Update()
    {
        //When mouse 1 is pressed
        if(Input.GetButtonDown("Fire1"))
        {
            InstantiateLine();
        }

        //When mouse 1 is held
        if(Input.GetButton("Fire1"))
        {
            Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            if(Vector2.Distance(mousePosition, vertexPos[vertexPos.Count - 1]) > vertexThreshold)
            {
                AddVertex(mousePosition);
            }
        }

        //When mouse 1 is released
        if (Input.GetButtonUp("Fire1") && snapToPath)
        {
            for (int i = 0; i < vertexPos.Count; i++)
            {
                vertexPos[i] = GetClosestPoint(vertexPos[i]);       //Gets closest path point to each line renderer vertex
                vertexPos[i] = new Vector3(vertexPos[i].x, vertexPos[i].y, 0);  //Zeros the z-axis
                lineRenderer.SetPosition(i, vertexPos[i]);   //Sets new vertex position 
            }
        }
    }

    /// <summary>
    /// Instantiates new line and deletes old one if applicable
    /// </summary>
    void InstantiateLine()
    {
        if(line) { Destroy(line); } //Destroys old line
        vertexPos.Clear();  //Clears list of vertex points

        //Instantiates new line and sets parameters
        line = Instantiate(Resources.Load<GameObject>("LineDrawer"), Vector3.zero, Quaternion.identity);
        lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.startWidth = thickness;
        lineRenderer.endWidth = thickness;

        //Sets first two line positions as mouse position
        for (int i = 0; i < 2; i++)
        {
            vertexPos.Add(cam.ScreenToWorldPoint(Input.mousePosition));
            vertexPos[i] = new Vector3(vertexPos[i].x, vertexPos[i].y, 0);
            lineRenderer.SetPosition(i, vertexPos[i]);
        }
    }

    /// <summary>
    /// Adds vertices to the current line being drawn
    /// </summary>
    /// <param name="vertex"></param>
    void AddVertex(Vector3 vertex)
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
    Vector3 GetClosestPoint(Vector3 vertex)
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
