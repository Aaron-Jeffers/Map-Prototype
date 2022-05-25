using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ButtonBehaviour : MonoBehaviour, ISelectHandler
{
    //StickerManager stickerManager;
    //EventSystem eventSystem;
    Sticker sticker;

    private void Start()
    {
        //stickerManager = FindObjectOfType<StickerManager>().GetComponent<StickerManager>();
        //eventSystem = FindObjectOfType<EventSystem>().GetComponent<EventSystem>();
        sticker = GetComponent<Sticker>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        //stickerManager.SpawnObject(this.tag);
        //EventSystem.current.SetSelectedGameObject(null);

        //stickerManager.SpawnObject(this.tag);

        sticker.SetButtonPressed(true);

    }

    private void Update()
    {
        if(Mouse.current.leftButton.wasReleasedThisFrame)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
