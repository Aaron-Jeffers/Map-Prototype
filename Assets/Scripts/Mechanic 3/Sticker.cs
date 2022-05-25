using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Sticker : MonoBehaviour
{
    public SOSticker soSticker;

    Image buttonSprite;
    GameObject highlight;
    public string highlightTag;
    GameObject dragSprite;
    GameObject placeSprite;
    GameObject activeObject;

    bool buttonPressed;
    bool done;

    private void Start()
    {
        Initialise();
    }

    private void Update()
    {
        if(done) { return; }
        if (!buttonPressed) { return; }

        MoveObject(dragSprite);

        if (!Mouse.current.leftButton.wasReleasedThisFrame) { return; }

        if (Raycast())
        {
            SetObjectActive(placeSprite, true);
            MoveObject(placeSprite);
            done = true;
        }

        SetObjectActive(highlight, false);
        SetObjectActive(dragSprite, false);
        buttonPressed = false;
    }

    bool Raycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        var hit = Physics2D.GetRayIntersection(ray, 100f);

        if (!hit) { return false; }
        else if (hit.collider.tag == highlightTag) { return true; }
        else { return false; }
    }
        
    public void SetButtonPressed(bool value)
    {
        if (done) { return; }
        buttonPressed = value;
        SetObjectActive(highlight, true);
        SetObjectActive(dragSprite, true);
    }

    void Initialise()
    {
        name = soSticker.name;
        tag = soSticker.tag;
        highlightTag = soSticker.highlightTag;

        buttonSprite = GetComponent<Image>();
        buttonSprite.sprite = soSticker.buttonSprite;
        
        highlight = Instantiate(soSticker.highlight);
        dragSprite = Instantiate(soSticker.dragSprite);
        placeSprite = Instantiate(soSticker.placeSprite);

        highlight.tag = highlightTag;
        
        SetObjectActive(highlight, false);
        SetObjectActive(dragSprite, false);
        SetObjectActive(placeSprite, false);
    }

    void SetObjectActive(GameObject obj, bool active)
    {
        obj.SetActive(active);
    }

    void MoveObject(GameObject obj)
    {
        obj.transform.position = Mouse2WorldPos();
    }

    Vector3 Mouse2WorldPos()
    {
        return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - new Vector3(0, 0, Camera.main.transform.position.z);
    }
}
