using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HBCharacterBase))]
public class HBCharacterBaseEditor : Editor
{
    //public override void OnInspectorGUI()
    //{
    //    var snapmonIMG = (SnapmonBase) target;

    //    //Draw whatever we already have in SO definition
    //    base.OnInspectorGUI();
    //    //Guard clause
    //    if (snapmonIMG.iconSprite == null)
    //        return;

    //    //Convert the iconSprite (see SO script) to Texture
    //    Texture2D texture = AssetPreview.GetAssetPreview(snapmonIMG.iconSprite);
    //    //We crate empty space 80x80 (you may need to tweak it to scale better your sprite
    //    //This allows us to place the image JUST UNDER our default inspector
    //    GUILayout.Label("", GUILayout.Height(80), GUILayout.Width(80));
    //    //Draws the texture where we have defined our Label (empty space)
    //    GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
    //}

    public override Texture2D RenderStaticPreview(
    string assetPath, Object[] subAssets, int width, int height)
    {
        var iconAssets = (HBCharacterBase)target;

        if (iconAssets == null || iconAssets.iconSprite == null)
        {
            return null;
        }

        var texture = new Texture2D(width, height);
        EditorUtility.CopySerialized(source: iconAssets.iconSprite.texture, dest: texture);
        return texture;
    }
}