using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.EventSystems;

/// <summary>
/// Handles mouse over events for the barriers
/// </summary>
public class Barrier : MouseDetection, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// Runs for the first frame the mouse is over an object with mouse detection
    /// </summary>
    /// <param name="eventData"></param>
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        //PrintTag(tag);
        if (drawing)
        {
            var temp = Instantiate(redCross, camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Quaternion.identity);      //Instantiates the red cross
            temp.GetComponent<Fade>().StartTimer();
            scratcher.SetDrawing(false);
        }
    }

    /// <summary>
    /// Runs for the first frame the mouse is no longer over an object with mouse detection
    /// </summary>
    /// <param name="eventData"></param>
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {

    }
}
