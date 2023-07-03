using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTexture : MonoBehaviour
{
    //public Sprite tex;
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    [Header("Buttons")]
    public bool switchCursor;
    public bool defaultCursor;

    private void Awake()
    {
        if (cursorTexture != null) { Cursor.SetCursor(cursorTexture, hotSpot, cursorMode); }
    }

    void Update()
    {
        if (switchCursor)//basic bool button, fires once after it is clicked in the inspector
        {
            switchCursor = false;
            SwitchCursorTex();
        }

        if (defaultCursor)
        {
            defaultCursor = false;
            DefaultCursorTex();
        }
    }

    //Texture2D ConvertSpriteToTexture(Sprite sprite)
    //{
    //    try
    //    {
    //        if (sprite.rect.width != sprite.texture.width)
    //        {
    //            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
    //            Color[] colors = newText.GetPixels();
    //            Color[] newColors = sprite.texture.GetPixels((int)System.Math.Ceiling(sprite.textureRect.x),
    //                                                         (int)System.Math.Ceiling(sprite.textureRect.y),
    //                                                         (int)System.Math.Ceiling(sprite.textureRect.width),
    //                                                         (int)System.Math.Ceiling(sprite.textureRect.height));
    //            Debug.Log(colors.Length + "_" + newColors.Length);
    //            newText.SetPixels(newColors);
    //            newText.Apply();
    //            return newText;
    //        }
    //        else
    //            return sprite.texture;
    //    }
    //    catch
    //    {
    //        return sprite.texture;
    //    }
    //}

    void SwitchCursorTex()
    {
        //cursorTexture = ConvertSpriteToTexture(tex);
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void DefaultCursorTex()
    {
        // Pass 'null' to the texture parameter to use the default system cursor.
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
