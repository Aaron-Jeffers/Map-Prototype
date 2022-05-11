using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that retuns the sprite of what the camera is rendering
/// </summary>
public class ScreenCapture : MonoBehaviour
{
    #region References

    Camera thisCamera;      //Camera that renders the lines drawn and the effect on the layer mask

    #endregion 

    #region Variables

    int screenHeight, screenWidth;  
    int depth = 24;

    #endregion

    private void Awake()
    {
        thisCamera = GetComponent<Camera>();
        screenHeight = Screen.height;
        screenWidth = Screen.width;
    }

    /// <summary>
    /// Returns a new sprite based off what the camera is rendering
    /// </summary>
    /// <returns></returns>
    public Sprite ReturnSpriteMask()
    {
        RenderTexture rendTexture = new RenderTexture(screenWidth, screenHeight, depth);            //Creates a new texture based off the screen width/height/depth
        Rect rect = new Rect(0, 0, screenWidth, screenHeight);                                      //Creates a new rectangle 
        Texture2D texture = new Texture2D(screenWidth, screenHeight, TextureFormat.RGBA32, false);  //Creates a new texture the size of the screen

        thisCamera.targetTexture = rendTexture;     //Sets the new render texture 
        thisCamera.Render();                        

        RenderTexture currentRenderTexture = RenderTexture.active;      //Stores a temporary texture    
        RenderTexture.active = rendTexture;                             //Sets the current texture
        texture.ReadPixels(rect, 0, 0);                                 //Reads pixels and applies to texture
        texture.Apply();

        //Removes the old texture 
        thisCamera.targetTexture = null;                               
        RenderTexture.active = currentRenderTexture;
        Destroy(rendTexture);

        Sprite newSprite = Sprite.Create(texture, rect, new Vector2(.5f, .5f));     //Creates a new sprite on with the texture on the rectangle and pivots this

        return newSprite;
    }
}
