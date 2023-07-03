using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimSingleUse : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;

    SpriteAnimator spriteAnimator;

    private void Start()
    {
        spriteAnimator = new SpriteAnimator(sprites, GetComponent<SpriteRenderer>());
        spriteAnimator.Start();
    }

    private void Update()
    {
        if (spriteAnimator.CurrentFrame != sprites.Count - 1)
        {
            spriteAnimator.HandleUpdate();
        }
    }
}
