using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.EventSystems;

/// <summary>
/// Handles mouse over events for the checkpoints
/// </summary>
public class Checkpoint : MouseDetection , IPointerEnterHandler , IPointerExitHandler
{
    /// <summary>
    /// Runs for the first frame the mouse is over an object with mouse detection
    /// </summary>
    /// <param name="eventData"></param>
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (drawing)
        {
            //PrintTag(tag);
            GetComponent<Fade>().StartTimer();
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
