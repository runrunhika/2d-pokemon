using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MoveBase : ScriptableObject
{
    // 技のマスターデータ

    // 名前,詳細,タイプ,威力,正確性,PP(技を使うときに消費するポイント)

    [SerializeField] new string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] PokemonType type;
    [SerializeField] int power;
    [SerializeField] int accuracy; // 正確性
    [SerializeField] bool anyHit; // 正確性
    [SerializeField] int pp;
    [SerializeField] int priority;
    // カテゴリー（物理/特殊/ステータス変化）
    // ターゲット
    // ステータス変化のリスト:どのステータスをどの程度変化させるのか？のリスト
    [SerializeField] MoveCategory category;
    [SerializeField] MoveTarget target;
    [SerializeField] MoveEffects effects;
    [SerializeField] List<SecondaryEffects> secondaries;

    // 他のファイル(Move.cs)から参照するためにプロパティを使う
    public string Name { get => name; }
    public string Description { get => description; }
    public PokemonType Type { get => type; }
    public int Power { get => power; }
    public int Accuracy { get => accuracy; }
    public int PP { get => pp; }
    public MoveCategory Category { get => category; set => category = value; }
    public MoveTarget Target { get => target; }
    public MoveEffects Effects { get => effects; }
    public List<SecondaryEffects> Secondaries { get => secondaries; }
    public int Priority { get => priority; }
    public bool AnyHit { get => anyHit; set => anyHit = value; }
}

public enum MoveCategory
{
    Physical,
    Special,
    Stat,
}

public enum MoveTarget
{
    Foe,
    Self,
}

// 下のリスト
[System.Serializable]
public class MoveEffects
{
    [SerializeField] List<StatBoost> boosts;
    [SerializeField] ConditionID status;             // 戦闘終了時に回復しない状態異常
    [SerializeField] ConditionID volatileStatus;     // 戦闘終了時に回復する状態異常


    public List<StatBoost> Boosts { get => boosts; }
    public ConditionID Status { get => status; }
    public ConditionID VolatileStatus { get => volatileStatus; }
}

// 追加効果の実装
[System.Serializable]
public class SecondaryEffects : MoveEffects
{
    [SerializeField] int chance; // 追加効果の命中率
    [SerializeField] MoveTarget target; // 追加効果のターゲット

    public int Chance { get => chance; }
    public MoveTarget Target { get => target; }
}
// どのステータスをどの程度変化させるのか？
[System.Serializable]
public class StatBoost
{
    public Stat stat;
    public int boost;
}
