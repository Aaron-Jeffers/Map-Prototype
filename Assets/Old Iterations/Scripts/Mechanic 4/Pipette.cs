using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pipette : MonoBehaviour
{
    [SerializeField]
    Vector3 offsetFromMousePosition;
    [SerializeField]
    GameObject inkDrop;
    [SerializeField]
    [Range(0.1f, 5f)]
    float inkDropSize;
    [SerializeField]
    [Tooltip("Drops per second")]
    float inkDropFrequency;
    [SerializeField]
    int maxNumberOfInkDrops;
    [SerializeField]
    int numberOfInkDrops;
    float timer;
    [SerializeField]
    float inkFadeAmount;
    [SerializeField]
    Vector2 inkFadeTime;
    public int highlightThreshold;
    int highlightDrops;
    public GameObject highlight, highlightReplacement;
    List<GameObject> inkDrops = new List<GameObject>();


    // Update is called once per frame
    void Update()
    {
        transform.position =  GetMousePos() + offsetFromMousePosition;
        timer += Time.deltaTime;

        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            if(!Raycast().collider.GetComponent<InkPot>())
            {
                return;
            }
            numberOfInkDrops = 0;
        }
        if (numberOfInkDrops >= maxNumberOfInkDrops) { return; }
        if (timer >= 1 / inkDropFrequency)
        {
            DropInk();
        }
    }

    void DropInk()
    {
        timer = 0;
        numberOfInkDrops++;
        GameObject newInkDrop = Instantiate(inkDrop, GetMousePos(), Quaternion.identity);
        newInkDrop.transform.localScale *= inkDropSize;
        var inkDropScript = newInkDrop.GetComponent<InkDrop>();
        if(!Raycast() || Raycast().collider.GetComponent<InkPot>())
        {
            inkDropScript.FadeAmount = inkFadeAmount;
            inkDropScript.FadeTime = inkFadeTime;
            inkDropScript.Fade = true;
        }
        if(Raycast().collider.GetComponent<Highlights>())
        {
            inkDrops.Add(newInkDrop);
            highlightDrops++;
            if(highlightDrops >= highlightThreshold)
            {
                highlight.SetActive(false);
                highlightReplacement.SetActive(true);
                for(int i = 0; i < inkDrops.Count; i++)
                {
                    var script = inkDrops[i].GetComponent<InkDrop>();
                    script.FadeAmount = inkFadeAmount;
                    script.FadeTime = inkFadeTime;
                    script.Fade = true;
                }
                inkDrops.Clear();
            }
        }
    }

    public RaycastHit2D Raycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        var hit = Physics2D.GetRayIntersection(ray, 100f);
        return hit;
    }
    public 
    Vector3 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - new Vector3(0, 0, Camera.main.transform.position.z);
    }
}
