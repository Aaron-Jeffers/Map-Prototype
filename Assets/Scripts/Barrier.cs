using UnityEngine;

public class Barrier : MouseDetection
{
    private void OnMouseEnter()
    {
        PrintTag(tag);
        if(drawing)
        {
            Instantiate(redCross, camera.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
        }
    }
        
}
