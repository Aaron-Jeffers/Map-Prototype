using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Instantiates the drop game object
/// </summary>
public class Drop : MonoBehaviour
{
    [Tooltip("Reference to the window check script")]
    [SerializeField] WindowCheck windowCheck;
    [Tooltip("Determines wether or not to delete drops after window check returns true and window is filled in")]
    [SerializeField] bool clearOnWindowCheck;

    [Tooltip("Game object to instantiate")][SerializeField] GameObject drop;
    List<GameObject> drops = new List<GameObject>();
    [SerializeField] Vector3 offsetFromMousePosition;
    [SerializeField][Tooltip("Drops per second")] float inkDropFrequency;
    [SerializeField] int maxNumberOfInkDrops;
    [SerializeField] int numberOfInkDrops;
    [SerializeField] [Range(0, 1)] [Tooltip("Scaling ratio between first and last ink drop instantiated")] float spawnRatio;

    float timer;
    private void Start()
    {
        transform.position = GetMousePos() + offsetFromMousePosition;
    }
    private void Update()
    {
        transform.position = GetMousePos() + offsetFromMousePosition;
        timer += Time.deltaTime;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            foreach(RaycastHit2D ray in Raycast())
            {
                if (ray.collider.GetComponent<InkPot>()) 
                {
                    numberOfInkDrops = 0;
                    timer = 0;
                }
            }
        }

        if (numberOfInkDrops >= maxNumberOfInkDrops) { return; }
        if (timer >= 1 / inkDropFrequency)
        {
            InstantiateDrop();
        }
    }

    public RaycastHit2D[] Raycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        var hit = Physics2D.GetRayIntersectionAll(ray, 100f);
        return hit;
    }

    /// <summary>
    /// Instantiates the drop game object and adds to the list of drops
    /// </summary>
    void InstantiateDrop()
    {
        GameObject newInkDrop = Instantiate(drop, GetMousePos(), drop.transform.rotation);
        var scale = newInkDrop.transform.localScale;
        newInkDrop.transform.localScale = new Vector3(scale.x, scale.y, scale.z);
        drops.Add(newInkDrop);
        timer = 0;
        numberOfInkDrops++;
        CallWindowCheck();
        foreach(RaycastHit2D ray in Raycast())
        {
            if (ray.collider.CompareTag("Outline"))
            {
                newInkDrop.GetComponent<Blotch>().SwitchTime = true;
                return;
            }
        }
        
    }

    /// <summary>
    /// Call the window check function in the window check class. Clears existing drops if configured so and this function returns true.
    /// </summary>
    void CallWindowCheck()
    {
        if (!windowCheck.CheckWindow()) { return; }
        if (!clearOnWindowCheck) { return; }

        for (int i = 0; i < drops.Count; i++)
        {
            Destroy(drops[i]);
        }
        drops.Clear();
    }
    /// <summary>
    /// Returns the mouse position in world space with the z-axis zeroed relative to the camera's z-axis position.
    /// </summary>
    /// <returns></returns>
    Vector3 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - new Vector3(0, 0, Camera.main.transform.position.z);
    }
}
