using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.EventSystems;

public class Barrier : MouseDetection, IPointerEnterHandler, IPointerExitHandler
{
    //private void OnMouseEnter()
    //{
    //    PrintTag(tag);
    //    if(drawing)
    //    {
    //        Instantiate(redCross, camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Quaternion.identity);
    //    }
    //}

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        PrintTag(tag);
        if (drawing)
        {
            Instantiate(redCross, camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Quaternion.identity);
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {

    }
}
