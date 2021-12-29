using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator 
{
    // Animatorを使わずに画像を切り替えていく
    // ・描画するためのRenderer
    SpriteRenderer spriteRenderer;
    // ・表示する画像達
    List<Sprite> frames;
    // ・フレームレート:どのタイミングで画像を切り替えるのか？
    float frameRate;

    // 現在のフレーム:何枚目の画像を表示する
    int currentFrame;
    float timer;

    public List<Sprite> Frames { get => frames; }

    // 初期化
    public SpriteAnimator(SpriteRenderer spriteRenderer, List<Sprite> frames, float frameRate = 0.16f)
    {
        this.spriteRenderer = spriteRenderer;
        this.frames = frames;
        this.frameRate = frameRate;
    }

    // アニメーション開始
    public void Start()
    {
        currentFrame = 0;
        timer = 0;
        spriteRenderer.sprite = frames[currentFrame];
    }

    // アニメーションを更新する:timerがframeRateを超えたら次の画像
    public void HandleUpdate()
    {
        timer += Time.deltaTime;
        if (timer > frameRate)
        {
            currentFrame = (currentFrame + 1) % frames.Count;
            spriteRenderer.sprite = frames[currentFrame];
            timer -= frameRate;
        }
    }
}
