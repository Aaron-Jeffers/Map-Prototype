using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    [SerializeField] List<GameObject> drops = new List<GameObject>();
    [SerializeField] Vector3 offsetFromMousePosition;
    [SerializeField][Tooltip("Drops per second")] float inkDropFrequency;
    [SerializeField] Slider inkPotSlider;
    [SerializeField] int maxNumberOfInkDrops;
    [SerializeField] int numberOfInkDrops;
    [SerializeField] int remainingInkDrops;
    [SerializeField] int inkPotLevel;
    [SerializeField] [Range(0, 1)] float finalInkDropScaleRatio;
    [SerializeField] [Range(0, 1)] [Tooltip("Scaling ratio between first and last ink drop instantiated")] float spawnRatio;

    float timer;
    private void Start()
    {
        transform.position = GetMousePos() + offsetFromMousePosition;
        inkPotSlider.maxValue = maxNumberOfInkDrops;
        inkPotLevel = maxNumberOfInkDrops;
        inkPotSlider.value = inkPotLevel;
    }
    private void Update()
    {
        transform.position = GetMousePos() + offsetFromMousePosition;
        timer += Time.deltaTime;

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            foreach(RaycastHit2D ray in Raycast())
            {
                if (ray.collider.GetComponent<InkPot>()) 
                {
                    remainingInkDrops += inkPotLevel;
                    inkPotLevel = 0;
                    inkPotSlider.value = inkPotLevel;
                    timer = 0;
                    break;
                }
            }
        }

        if (remainingInkDrops <= 0) { return; }
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
        _ = (remainingInkDrops == 1) ? scale *= finalInkDropScaleRatio : scale;
        newInkDrop.transform.localScale = new Vector3(scale.x, scale.y, scale.z);
        drops.Add(newInkDrop);
        newInkDrop.GetComponent<Blotch>().Drop = this;
        timer = 0;
        //numberOfInkDrops++;
        remainingInkDrops--;
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
        //List<GameObject> obj = new List<GameObject>();
        for (int i = 0; i < drops.Count; i++)
        {
            //if (!drops[i]) { drops.RemoveAt(i); }
            //if (drops[i].GetComponent<Blotch>().Fading == false)
            //{
            //    drops[i].GetComponent<Blotch>().Fading = true;
            //    StartCoroutine(drops[i].GetComponent<Blotch>().Fade(/*callback1 => windowCheck.EnableFill(callback1), callback2 => { obj.Add(callback2); }*/));
            //}
        }
        //RemoveDrops(obj);
        //drops.Clear();
    }
    /// <summary>
    /// Returns the mouse position in world space with the z-axis zeroed relative to the camera's z-axis position.
    /// </summary>
    /// <returns></returns>
    Vector3 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - new Vector3(0, 0, Camera.main.transform.position.z);
    }

    public void ReturnInk(GameObject obj)
    {
        inkPotLevel++;
        inkPotSlider.value = inkPotLevel;
        drops.Remove(obj);
    }
    void RemoveDrops(List<GameObject> obj)
    {
        for (int i = 0; i < obj.Count; i++)
        {
            drops.RemoveAt(drops.IndexOf(obj[i]));
            //Destroy(obj[i]);
        }
    }
}
