using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Checks if objects are overlapping a given area by a set percentage, if so the area is the fileld in completely.
/// </summary>
public class WindowCheck : MonoBehaviour
{
    public GameObject dot;
    [Tooltip("How many raycasts check the given area. Represented as X * Y in a grid fashion")]
    [SerializeField] Vector2 raycastDensity;
    [Tooltip("Threshold for area coverage check returning true. i.e. if the area that is overlapped is greater than this percentage of the total area, it will return true.")]
    [Range(0,1)][SerializeField] float checkThreshold;
    Vector2[] spriteCorners;    
    [Tooltip("GameObject to fill in outline with after check threshold is met")]
    [SerializeField] GameObject fill;
    [Tooltip("Determines wether or not to keep checking overlap after initial true check and after the outline has been filled")]
    //[SerializeField] bool checkWindowAfterFill;
    bool filled;
    public bool Filled => filled;

    private void Start()
    {
        spriteCorners = GetSpriteCorners(GetComponent<SpriteRenderer>());
    }
    private void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            for (int i = 0; i < RaycastPositions().Count; i++)
            {
               Instantiate(dot, RaycastPositions()[i], Quaternion.identity);
            }
            //Debug.Log(GetMousePos());
        }
    }
    /// <summary>
    /// Function to handle checking the area of overlap of the window
    /// </summary>
    /// <returns></returns>
    public bool CheckWindow()
    {
        if (filled) { return false; }
        if(Raycast("Drop") > RaycastPositions().Count * checkThreshold)
        {
            //check = checkWindowAfterFill;
            EnableFill(true);
            return true;
        }
        return false;
    }

    public void EnableFill(bool value)
    {
        filled = true;
        fill.SetActive(true);
    }

    /// <summary>
    /// Shoots out a series of raycast over the window area. Returns the number of hits with game objects tagged with tag2Check.
    /// </summary>
    /// <returns></returns>
    int Raycast(string tag2Check)
    {
        int hits = 0;
        List<Vector2> targets = RaycastPositions();
        for (int i = 0; i < targets.Count; i++)
        {
            Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(targets[i]));
            var hit = Physics2D.GetRayIntersectionAll(ray, 100f);
            
            foreach(RaycastHit2D newHit in hit)
            {
                if (newHit.collider.tag == tag2Check)
                {
                    hits++;
                    break;
                }
            }
        }

        return hits;
    }

    /// <summary>
    /// Generates a list of positions along the window area to shoot raycast through. These positions are evenly distributed as a grid along the X-Y axis.
    /// </summary>
    /// <returns></returns>
    List<Vector2> RaycastPositions()
    {
        //Gets width and height of window
        float width = spriteCorners[3].x - spriteCorners[1].x;
        float height = spriteCorners[1].y - spriteCorners[0].y;

        //Determines incremental distance based off dimensions and number of raycast per dimension
        float widthIncrements = width / raycastDensity.x;
        float heightIncrements = height / raycastDensity.y;

        List<Vector2> raycastPositions = new List<Vector2>();

        //Works along the grid pattern setting new positions as offsets from the bottom left corner of the sprite 
        for (int i = 0; i <= raycastDensity.x; i++)
        {
            for(int j = 0; j <= raycastDensity.y; j++)
            {
                Vector2 raycastPos = new Vector2(spriteCorners[0].x + (widthIncrements * i), spriteCorners[0].y + (heightIncrements * j));
                Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(raycastPos));
                var hit = Physics2D.GetRayIntersectionAll(ray, 100f);
                foreach (RaycastHit2D newHit in hit)
                {
                    if (newHit.collider.tag == "Outline")
                    {
                        raycastPositions.Add(raycastPos);
                    }
                }
            }
        }

        return raycastPositions;
    }

    /// <summary>
    /// Returns the position of each corner of the sprite being checked. This value is returned in world space regardless of the sprites rotation.
    /// </summary>
    /// <param name="renderer"></param>
    /// <returns></returns>
    Vector2[] GetSpriteCorners(SpriteRenderer renderer)
    {
        Vector2 bottomLeft = renderer.transform.TransformPoint(renderer.sprite.bounds.min);
        Vector2 topLeft = renderer.transform.TransformPoint(renderer.sprite.bounds.min.x, renderer.sprite.bounds.max.y, 0);
        Vector2 topRight = renderer.transform.TransformPoint(renderer.sprite.bounds.max);
        Vector2 bottomRight = renderer.transform.TransformPoint(renderer.sprite.bounds.max.x, renderer.sprite.bounds.min.y, 0);
        return new Vector2[] { bottomLeft , topLeft , topRight , bottomRight };
    }
    Vector3 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - new Vector3(0, 0, Camera.main.transform.position.z);
    }

}
