using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteStretch : MonoBehaviour
{
    public bool callOnceStretch;

    private void Update()
    {
        if (callOnceStretch)
        {
            Stretch();
            callOnceStretch = false;
        }
    }

    void Stretch()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite sprite = spriteRenderer.sprite;

        float worldHeight = Camera.main.orthographicSize * 2f;
        float worldWidth = worldHeight * Camera.main.aspect;

        float spriteWidth = sprite.bounds.size.x;
        float spriteHeight = sprite.bounds.size.y;

        float scaleFactorX = worldWidth / spriteWidth;
        float scaleFactorY = worldHeight / spriteHeight;

        gameObject.transform.localScale = new Vector3(scaleFactorX, scaleFactorY, 1);
    }
}



