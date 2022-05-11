using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.EventSystems;

public class Checkpoint : MouseDetection , IPointerEnterHandler , IPointerExitHandler
{
    //private void OnMouseEnter()
    //{
    //    if(drawing)
    //    {
    //        PrintTag(tag);
    //        gameObject.SetActive(false);
    //    }        
    //}

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (drawing)
        {
            PrintTag(tag);
            gameObject.SetActive(false);
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {

    }
}
