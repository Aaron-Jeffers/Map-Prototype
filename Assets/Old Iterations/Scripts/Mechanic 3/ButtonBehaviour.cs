using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ButtonBehaviour : MonoBehaviour, ISelectHandler
{
    Sticker sticker;

    private void Start()
    {
        sticker = GetComponent<Sticker>();
    }

    public void OnSelect(BaseEventData eventData)
    {
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
