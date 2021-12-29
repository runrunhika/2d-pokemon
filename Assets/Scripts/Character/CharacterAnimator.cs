using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    // 各状態のframes
    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkRightSprites;
    [SerializeField] List<Sprite> walkLeftSprites;

    // Parameters
    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }

    // 状態
    SpriteAnimator walkDownAnim;
    SpriteAnimator walkUpAnim;
    SpriteAnimator walkRightAnim;
    SpriteAnimator walkLeftAnim;

    SpriteAnimator currentAnim;

    SpriteRenderer spriteRenderer;

    bool wasPreviouslyMoving;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkDownAnim = new SpriteAnimator(spriteRenderer, walkDownSprites);
        walkUpAnim = new SpriteAnimator(spriteRenderer, walkUpSprites);
        walkRightAnim = new SpriteAnimator(spriteRenderer, walkRightSprites);
        walkLeftAnim = new SpriteAnimator(spriteRenderer, walkLeftSprites);

        currentAnim = walkDownAnim;
    }

    private void Update()
    {

        SpriteAnimator prevAnim = currentAnim;
        if (MoveX == 1)
        {
            currentAnim = walkRightAnim;
        }
        else if (MoveX == -1)
        {
            currentAnim = walkLeftAnim;
        }
        else if (MoveY == 1)
        {
            currentAnim = walkUpAnim;
        }
        else if (MoveY == -1)
        {
            currentAnim = walkDownAnim;
        }

        // 前のアニメーションと状態が違うなら or 動いてない状態から動いたら Startを実行
        if (prevAnim != currentAnim || wasPreviouslyMoving != IsMoving)
        {
            currentAnim.Start();
        }

        if (IsMoving)
        {
            currentAnim.HandleUpdate();
        }
        else
        {
            // 動いてないなら,最初のフレームを表示
            spriteRenderer.sprite = currentAnim.Frames[0];
        }

        wasPreviouslyMoving = IsMoving;
    }
}
