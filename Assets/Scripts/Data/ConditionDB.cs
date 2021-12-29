using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionDB
{
    //辞書型 :　Key, Value
    //宣言と代入を同時に行う
    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
    {
        {
            ConditionID.Poison,
            new Condition()
            {
                Id = ConditionID.Poison,
                Name = "どく",
                StartMessage = "は毒になった",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    // 毒ダメージを与える
                    pokemon.UpdateHP(pokemon.MaxHP/8);
                    // メッセージを表示する
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name}は毒のダメージをうける");
                }
            }
        },
        {
            ConditionID.Burn,
            new Condition()
            {
                Id = ConditionID.Burn,
                Name = "やけど",
                StartMessage = "は火傷をおった",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    // 毒ダメージを与える
                    pokemon.UpdateHP(pokemon.MaxHP/16);
                    // メッセージを表示する
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name}は火傷のダメージをうける");
                }
            }
        },
        {
            ConditionID.Paralysis,
            new Condition()
            {
                Id = ConditionID.Paralysis,
                Name = "マヒ",
                StartMessage = "はマヒになった.",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    // ture:技が出せる, false:マヒで動けない

                    // ・一定確率で
                    // ・技が出せずに自分のターンが終わる

                    // 1,2,3,4が出る中で1が出たら（25%の確率で）
                    if (Random.Range(1,5) == 1)
                    {
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name}はしびれて動けない.");
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            ConditionID.Freeze,
            new Condition()
            {
                Id = ConditionID.Freeze,
                Name = "こおり",
                StartMessage = "はこおった.",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    // ture:技が出せる, false:氷で動けない

                    // ・一定確率で
                    // ・氷が溶けて技が出せる
                    // 1,2,3,4が出る中で1が出たら（25%の確率で）
                    if (Random.Range(1,5) == 1)
                    {
                        // 氷状態を解除
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name}のこおりは溶けた.");
                        return true;
                    }
                    // 技が出せずに自分のターンが終わる
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name}はこおって動けない.");
                    return false;
                }
            }
        },
        {
            ConditionID.Sleep,
            new Condition()
            {
                Id = ConditionID.Sleep,
                Name = "すいみん",
                StartMessage = "は眠った.",
                OnStart = (Pokemon pokemon) =>
                {
                    // 技を受けた時に、何ターン眠るか決める
                    pokemon.StatusTime = Random.Range(1,4); // 1,2,3ターンのどれか
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    // ture:技が出せる, false:眠って技が出せない

                    if (pokemon.StatusTime <= 0)
                    {
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name}は目を覚ました.");
                        return true;
                    }
                    pokemon.StatusTime--;
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name}は眠っている.");
                    return false;
                }
            }
        },

        {
            ConditionID.Confusion,
            new Condition()
            {
                Id = ConditionID.Confusion,
                Name = "こんらん",
                StartMessage = "は混乱した.",
                OnStart = (Pokemon pokemon) =>
                {
                    pokemon.VolatileStatusTime = Random.Range(1,5); // 1,2,3,4ターンのどれか
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    // ture:技が出せる, false:眠って技が出せない

                    // ・もし、カウントが0なら、混乱が解除され行動可能
                    if (pokemon.VolatileStatusTime <= 0)
                    {
                        pokemon.CureVolatileStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name}は混乱がとけた.");
                        return true;
                    }
                    pokemon.VolatileStatusTime--;
                    // ・そうでないなら、一定確率(50%)で通常行動
                    if (Random.Range(1,3) == 1)
                    {
                        return true;
                    }

                    // ・その他の確率で自分を攻撃
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name}は混乱している.");
                    pokemon.UpdateHP(pokemon.MaxHP/8);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name}は自分を攻撃した.");
                    return false;
                }
            }
        },


    };
}

/*  毒   */
//・技の発動後 -> 必ず ダメージを受ける

/*  麻痺  */
//・技の発動前 -> 確率で技が出せずに自分のターン終了

/*  氷   */
//・技発動前　->　確率で溶けて技発動可能 (溶けなければ、技発動不可->相手ターン)

/*  睡眠  */
//・マイターン、眠りのターンカウントを減らす
//・眠りのターンカウント 0 -> 行動可能
//・0以外 -> 行動不可

/*  混乱  */
//・バトル終了時に1回復
//・混乱技を受けたとき -> 何ターン混乱するか決める



public enum ConditionID
{
    None,
    Poison,    // 毒
    Burn,      // 火傷
    Sleep,     // 睡眠
    Paralysis, // 麻痺
    Freeze,    // こおり
    Confusion,
}
