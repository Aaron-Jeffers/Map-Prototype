using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ButtonBehaviour : MonoBehaviour, ISelectHandler
{
    StickerManager stickerManager;
    //EventSystem eventSystem;

    private void Start()
    {
        stickerManager = FindObjectOfType<StickerManager>().GetComponent<StickerManager>();
        //eventSystem = FindObjectOfType<EventSystem>().GetComponent<EventSystem>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        stickerManager.SpawnObject(this.tag);
        //EventSystem.current.SetSelectedGameObject(null);

    }

    private void Update()
    {
        if(Mouse.current.leftButton.wasReleasedThisFrame)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
