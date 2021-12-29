using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/*   バトルさせるモンスターを保持   */
public class BattleUnit : MonoBehaviour
{
    /*  モンスターはBattleSystemから受け取る    */
    [SerializeField] bool isPlayerUnit;
    [SerializeField] BattleHud hud;

    public Pokemon Pokemon { get; set; }
    public bool IsPlayerUnit { get => isPlayerUnit; }
    public BattleHud Hud { get => hud; }

    //戦闘時の位置
    Vector3 originalPos;
    Color originalColor;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = transform.localPosition;
        originalColor = image.color;
    }

    //BattleSystemからポケモンデータを受け取り生成
    public void Setup(Pokemon pokemon)
    {
        //BattleSystemで呼ぶ
        //_baseからレベルに応じたモンスターを生成
        Pokemon = pokemon;

        //モンスター画像を反映
        if (isPlayerUnit)
        {
            image.sprite = Pokemon.Base.BackSprite;
        }
        else
        {
            image.sprite = Pokemon.Base.FrontSprite;
        }
        hud.SetData(pokemon);
        //一度戦闘不能になったら、そのまま透過し続けるのを防ぐため
        image.color = originalColor;
        PlayerEnterAnimation();
    }

    // 登場Anim
    public void PlayerEnterAnimation()
    {
        //戦闘開始
        if (isPlayerUnit)
        {
            // 左端に配置
            transform.localPosition = new Vector3(-850, originalPos.y);
        }
        else
        {
            // 右端に配置
            transform.localPosition = new Vector3(850, originalPos.y);
        }
        // 戦闘時の位置までアニメーション
        transform.DOLocalMoveX(originalPos.x, 1f);
    }
    // 攻撃Anim
    public void PlayerAttackAnimation()
    {
        /*  シーケンス   */
        //右に動いた後、元の位置に戻る
        Sequence sequence = DOTween.Sequence();
        if (isPlayerUnit)
        {
            sequence.Append(transform.DOLocalMoveX(originalPos.x + 50f, 0.25f)); // 後ろに追加
        }
        else
        {
            sequence.Append(transform.DOLocalMoveX(originalPos.x - 50f, 0.25f)); // 後ろに追加
        }
        sequence.Append(transform.DOLocalMoveX(originalPos.x, 0.2f)); // 後ろに追加
    }
    // ダメージAnim
    public void PlayerHitAnimation()
    {
        // 色を一度GLAYにしてから戻す
        Sequence sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }
    // 戦闘不能Anim
    public void PlayerFaintAnimation()
    {
        // 下にさがりながら,薄くなる
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMoveY(originalPos.y - 150f, 0.5f));
        sequence.Join(image.DOFade(0, 0.5f));
    }

}
