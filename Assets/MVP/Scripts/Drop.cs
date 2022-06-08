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
    [SerializeField] List<WindowCheck> windowChecks;
    [Tooltip("Determines wether or not to delete drops after window check returns true and window is filled in")]
    [SerializeField] bool clearOnWindowCheck;

    [Tooltip("List of drop game objects to instantiate")][SerializeField] List<GameObject> drop;
    List<GameObject> drops = new List<GameObject>();
    [SerializeField] Vector3 offsetFromMousePosition;
    [SerializeField][Tooltip("Drops per second")] float inkDropFrequency;
    [SerializeField] Slider inkPotSlider;
    [SerializeField] Slider pipetteSlider;
    [SerializeField] int maxNumberOfInkDrops;
    int numberOfInkDrops;
    int inkPotLevel;
    int pipetteLevel;
    [SerializeField] [Range(0, 1)] float finalInkDropScaleRatio;
    GameObject dropHolder;
    float timer;
    Vector2 lastMousePos;

    private void Start()
    {
        SetWindowChecks();
        transform.position = GetMousePos() + offsetFromMousePosition;
        pipetteSlider.maxValue = maxNumberOfInkDrops;
        pipetteLevel = 0;
        pipetteSlider.value = pipetteLevel;
        inkPotSlider.maxValue = maxNumberOfInkDrops;
        inkPotLevel = maxNumberOfInkDrops;
        inkPotSlider.value = inkPotLevel;
        dropHolder = GameObject.FindGameObjectWithTag("DropHolder");
        lastMousePos = GetMousePos();
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
                    pipetteLevel += inkPotLevel;
                    pipetteSlider.value = pipetteLevel;
                    inkPotLevel = 0;
                    inkPotSlider.value = inkPotLevel;
                    timer = 0;
                    break;
                }
            }
        }
        
        if (pipetteLevel <= 0) { return; }
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
        int i = GetMouseDirection();
        GameObject newInkDrop = Instantiate(drop[i], GetMousePos(), drop[i].transform.rotation, dropHolder.transform);

        var scale = newInkDrop.transform.localScale;
        _ = (pipetteLevel == 1) ? scale *= finalInkDropScaleRatio : scale;
        newInkDrop.transform.localScale = new Vector3(scale.x, scale.y, scale.z);
        drops.Add(newInkDrop);
        newInkDrop.GetComponent<Blotch>().Drop = this;
        timer = 0;
        //numberOfInkDrops++;
        pipetteLevel--;
        pipetteSlider.value = pipetteLevel;
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
        for(int i = 0; i < windowChecks.Count; i++)
        {
            if (!windowChecks[i].CheckWindow()) { continue; }
        }
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
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - new Vector3(0, 0, Camera.main.transform.position.z);

        return mousePos;
    }

    int GetMouseDirection()
    {
        Vector2 newMousePos = GetMousePos();

        if(newMousePos == lastMousePos)
        {
            return 0;
        }

        float mouseDirection = Mathf.Atan2(newMousePos.y - lastMousePos.y,newMousePos.x-lastMousePos.x) * Mathf.Rad2Deg;

        lastMousePos = newMousePos;

        if (mouseDirection > -22.5 && mouseDirection <= 22.5)
        {
            return 1;
        }
        if (mouseDirection > 22.5 && mouseDirection <= 67.5)
        {
            return 2;
        }
        if (mouseDirection > 67.5 && mouseDirection <= 112.5)
        {
            return 3;
        }
        if (mouseDirection > 112.5 && mouseDirection <= 157.5)
        {
            return 4;
        }
        if (mouseDirection > 157.5 || mouseDirection <= -157.5)
        {
            return 5;
        }
        if (mouseDirection > -157.5 && mouseDirection <= -112.5)
        {
            return 6;
        }
        if (mouseDirection > -112.5 && mouseDirection <= -67.5)
        {
            return 7;
        }
        if (mouseDirection > -67.5 && mouseDirection <= -22.5)
        {
            return 8;
        }

        return -1;
    }
    public void ReturnInk(GameObject obj)
    {
        if (!drops.Contains(obj))  return; 
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

    public void ResetInk()
    {
        pipetteLevel = 0;
        pipetteSlider.value = pipetteLevel;
        inkPotLevel = maxNumberOfInkDrops;
        inkPotSlider.value = inkPotLevel;
        for(int i = 0; i < drops.Count; i++)
        {
            Destroy(drops[i]);
        }
        drops.Clear();
    }

    public void SetWindowChecks()
    {
        windowChecks.Clear();
        WindowCheck[] windows = FindObjectsOfType<WindowCheck>();
        windowChecks.AddRange(windows);
    }
}