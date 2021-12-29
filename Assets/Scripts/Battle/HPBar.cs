using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    // HPの増減の描画をする
    [SerializeField] GameObject health;

    //HP初期設定
    public void SetHP(float hp)
    {
        // ポイント　Pivot = 0 にしてScale調節
        health.transform.localScale = new Vector3(hp, 1, 1);
    }

    //徐々にHPを減らす
    public IEnumerator SetHPSmooth(float newHP)
    {
        /*  currentHP -> newHP  */
        float currentHP = health.transform.localScale.x;
        float changeAmount = currentHP - newHP;

        while (currentHP - newHP > Mathf.Epsilon)
        {
            //1秒かけて changeAmount　分減らす
            currentHP -= changeAmount * Time.deltaTime;
            //減らしたら描画
            health.transform.localScale = new Vector3(currentHP, 1, 1);
            yield return null;
        }

        health.transform.localScale = new Vector3(newHP, 1, 1);
    }
}
