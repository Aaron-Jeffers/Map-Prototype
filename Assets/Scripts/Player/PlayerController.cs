using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variables
    [Tooltip("Positions of all the nodes of the path")]
    [SerializeField] private float moveSpeed;
    [Tooltip("NextTarget")]
    private int nextTarget = 0;
    private int next = 1;
    private int targetCount;
    bool shouldMove;

    //References
    [Tooltip("Positions of all the nodes of the path")]
    [SerializeField] private List<Transform> pathPoints = new List<Transform>();  

    public void InitialisePlayer(List<Transform> transforms)
    {
        shouldMove = true;
        pathPoints = transforms;
        transform.position = pathPoints[0].position;
        targetCount = pathPoints.Count - 1;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (!shouldMove) { return; }
        Vector3 target = (pathPoints[nextTarget].position - transform.position);
        if(target.sqrMagnitude <= 0.01f)
        {
            nextTarget += next;
            if (nextTarget == pathPoints.Count) { shouldMove = false; }
        }

        //if(nextTarget == 0 || nextTarget >= targetCount -1)
        //{
        //    next *= -1;
        //}
        transform.position += target.normalized * moveSpeed * Time.deltaTime;
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public List<Transform> GetPathPoints()
    {
        return pathPoints;
    }
}
