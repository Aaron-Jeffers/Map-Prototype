using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Sticker : MonoBehaviour
{
    //[SerializeField]
    SOSticker soSticker;
    StickerManager stickerManager;
    public SOSticker SetStickerScriptableObject
    {
        set { soSticker = value; }
    }

    Image buttonSprite;
    Button button;
    GameObject highlight;
    string highlightTag;
    GameObject dragSprite;
    GameObject placeSprite;
    AudioSource audioSource;

    bool buttonPressed;
    bool done;

    private void Start()
    {
        InitialiseRuntime();
    }

    private void OnValidate()
    {
        InitialiseEditor();
    }
    private void Update()
    {
        if (done && !audioSource.isPlaying) { stickerManager.CanContinue = true; }
        if(done || !buttonPressed) { return; }

        MoveObject(dragSprite);

        if (!Mouse.current.leftButton.wasReleasedThisFrame) { return; }
        
        if (Raycast())
        {
            SetObjectActive(placeSprite, true);
            MoveObject(placeSprite);
            done = true;
            button.interactable = false;
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

    void InitialiseEditor()
    {
        if (!soSticker) { return; }
        name = soSticker.NewName;
    }

    void InitialiseRuntime()
    {
        stickerManager = GetComponentInParent<StickerManager>();

        tag = soSticker.NewTag;
        highlightTag = soSticker.HighlightTag;

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soSticker.Clip;

        button = GetComponent<Button>();
        buttonSprite = GetComponent<Image>();
        buttonSprite.sprite = soSticker.ButtonSprite;

        highlight = Instantiate(soSticker.Highlight);
        dragSprite = Instantiate(soSticker.DragSprite);
        placeSprite = Instantiate(soSticker.PlaceSprite);

        highlight.tag = highlightTag;
        
        SetObjectActive(highlight, false);
        SetObjectActive(dragSprite, false);
        SetObjectActive(placeSprite, false);

        audioSource.Play();
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