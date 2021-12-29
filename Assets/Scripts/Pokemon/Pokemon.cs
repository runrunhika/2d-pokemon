using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// レベルに応じたステータスの違うモンスターを生成するクラス
// 注意:データのみ扱う:純粋C#のクラス
[System.Serializable]
public class Pokemon
{
    // インスペクターからデータを設定できるようにする
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;

    // ベースとなるデータ
    public PokemonBase Base { get => _base; }
    public int Level { get => level; }

    public int HP { get; set; }
    // 使える技
    public List<Move> Moves { get; set; }
    // 現在の技
    public Move CurrentMove { get; set; }

    // ステータスと追加ステータス
    public Dictionary<Stat, int> Stats { get; set; }
    public Dictionary<Stat, int> StatBoosts { get; set; }
    // ログを溜めておく変数を作る：出し入れが簡単なリスト
    public Queue<string> StatusChanges { get; private set; }

    // 状態異常
    public Condition Status { get; private set; }
    public int StatusTime { get; set; }

    // バトル終了時に回復する:Statusと同じように設定する
    public Condition VolatileStatus { get; private set; }
    public int VolatileStatusTime { get; set; }


    public bool HpChange { get; set; }

    Dictionary<Stat, string> statDic = new Dictionary<Stat, string>()
    {
        {Stat.Attack, "攻撃"},
        {Stat.Defense, "防御"},
        {Stat.SpAttack, "特殊攻撃"},
        {Stat.SpDefense, "特殊防御"},
        {Stat.Speed, "スピード"},
        {Stat.Accuracy, "命中率"},
        {Stat.Evasion, "回避率"},
    };

    // ステータス変化が怒った時に実行したいことを登録しておく関数を作る
    public System.Action OnStatusChanged;

    // コンストラクター：生成時の初期設定 => Init関数に変更
    public void Init()
    {
        StatusChanges = new Queue<string>();
        Moves = new List<Move>();
        // 使える技の設定:覚える技のレベル以上なら、Movesに追加
        foreach (LearnableMove learnableMove in Base.LearnableMoves)
        {
            if (Level >= learnableMove.Level)
            {
                // 技を覚える
                Moves.Add(new Move(learnableMove.Base));
            }

            // 4つい上の技は使えない
            if (Moves.Count >=4)
            {
                break;
            }
        }

        CalculateStats();
        HP = MaxHP;

        ResetStatBoost();
        Status = null;
        VolatileStatus = null;
    }

    void ResetStatBoost()
    {
        StatBoosts = new Dictionary<Stat, int>()
        {
            {Stat.Attack, 0},
            {Stat.Defense, 0},
            {Stat.SpAttack, 0},
            {Stat.SpDefense, 0},
            {Stat.Speed, 0},
            {Stat.Accuracy, 0},
            {Stat.Evasion, 0},
        };
    }

    // ゲーム終了時に実行したいこと
    public void OnBattleOver()
    {
        ResetStatBoost();
        VolatileStatus = null;
    }

    void CalculateStats()
    {
        Stats = new Dictionary<Stat, int>();
        // ステータス変化してない初期値を入れていく
        Stats.Add(Stat.Attack, Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5);
        Stats.Add(Stat.Defense, Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5);
        Stats.Add(Stat.SpAttack, Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5);
        Stats.Add(Stat.SpDefense, Mathf.FloorToInt((Base.SpDefense * Level) / 100f) + 5);
        Stats.Add(Stat.Speed, Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5);
        MaxHP = Mathf.FloorToInt((Base.MaxHP * Level) / 100f) + 10 + Level;
    }

    int GetStat(Stat stat)
    {
        int statValue = Stats[stat];
        // boostの分を計算してやる
        int boost = StatBoosts[stat];
        float[] boostValues = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };

        if (boost >= 0)
        {
            // 強化なら
            statValue = Mathf.FloorToInt(statValue * boostValues[boost]);
        }
        else
        {
            // 弱体化なら
            statValue = Mathf.FloorToInt(statValue / boostValues[-boost]);
        }

        return statValue;
    }

    public void ApplyBoosts(List<StatBoost> statBoosts)
    {
        // ステータス変化を反映：StatBoostsを変更する
        foreach (StatBoost statBoost in statBoosts)
        {
            // どのステートを
            Stat stat = statBoost.stat;
            // 何段階
            int boost = statBoost.boost;
            StatBoosts[stat] = Mathf.Clamp(StatBoosts[stat] + boost, -6, 6);
            if (boost > 0)
            {
                StatusChanges.Enqueue($"{Base.Name}の{statDic[stat]}が上がった");
            }
            else
            {
                StatusChanges.Enqueue($"{Base.Name}の{statDic[stat]}が下がった");
            }
        }

    }
    // levelに応じたステータスを返すもの:プロパティ(+処理を加えることができる)
    public int Attack
    {
        get { return GetStat(Stat.Attack); }
    }
    public int Defense
    {
        get { return GetStat(Stat.Defense); }
    }
    public int SpAttack
    {
        get { return GetStat(Stat.SpAttack); }
    }
    public int SpDefense
    {
        get { return GetStat(Stat.SpDefense); }
    }
    public int Speed
    {
        get { return GetStat(Stat.Speed); }
    }
    public int MaxHP { get; private set; }

    // 修正:3つの情報を渡す
    // ・戦闘不能
    // ・クリティカル
    // ・相性
    public DamageDetails TakeDamage(Move move, Pokemon attacker)
    {
        // クリティカル
        float critical = 1f;
        //6.25%でクリティカル
        if (Random.value * 100 <= 6.25f)
        {
            critical = 2f;
        }
        // 相性
        float type = TypeChart.GetEffectivenss(move.Base.Type, Base.Type1) * TypeChart.GetEffectivenss(move.Base.Type, Base.Type2);
        DamageDetails damageDetails = new DamageDetails
        {
            Fainted = false,
            Critical = critical,
            TypeEffectiveness = type
        };

        // 特殊技の場合の修正
        float attack = attacker.Attack;
        float defense = Defense;
        if (move.Base.Category == MoveCategory.Special)
        {
            attack = attacker.SpAttack;
            defense = SpDefense;
        }

        float modifiers = Random.Range(0.85f, 1f) * type * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attack / defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        UpdateHP(damage);
        return damageDetails;
    }

    public void UpdateHP(int damage)
    {
        HP = Mathf.Clamp(HP - damage, 0, MaxHP);
        HpChange = true;
    }

    public Move GetRandomMove()
    {
        // PPが0なら排除する:Where(条件):条件にあったものを集めることができる
        List<Move> movesWithPP = Moves.Where(x => x.PP > 0).ToList();

        int r = Random.Range(0, movesWithPP.Count);
        return movesWithPP[r];
    }

    // 状態異常を受けたときに呼び出す関数
    public void SetStatus(ConditionID conditionID)
    {
        if (Status != null)
        {
            return;
        }
        Status = ConditionDB.Conditions[conditionID];
        Status?.OnStart?.Invoke(this);
        StatusChanges.Enqueue($"{Base.Name}{Status.StartMessage}");
        OnStatusChanged?.Invoke();
    }

    // 状態異常から回復
    public void CureStatus()
    {
        Status = null;
        OnStatusChanged?.Invoke();
    }

    // 状態異常を受けたときに呼び出す関数
    public void SetVolatileStatus(ConditionID conditionID)
    {
        if (VolatileStatus != null)
        {
            return;
        }
        VolatileStatus = ConditionDB.Conditions[conditionID];
        VolatileStatus?.OnStart?.Invoke(this);
        StatusChanges.Enqueue($"{Base.Name}{VolatileStatus.StartMessage}");
        // OnStatusChanged?.Invoke(); // UIの変更はいらない
    } 

    // 状態異常から回復
    public void CureVolatileStatus()
    {
        VolatileStatus = null;
        // OnStatusChanged?.Invoke(); // UI変更いらない
    }

    // 技の前に実行
    public bool OnBeforeMove()
    {
        bool canRunMove = true;
        // 状態異常について
        if (Status?.OnBeforeMove != null)
        {
            if (Status.OnBeforeMove(this) == false)
            {
                canRunMove = false;
            }
        }
        // 混乱系の状態異常について
        if (VolatileStatus?.OnBeforeMove != null)
        {
            if (VolatileStatus.OnBeforeMove(this) == false)
            {
                canRunMove = false;
            }
        }
        return canRunMove;
    }

    // ターン終了時にやりたいこと：状態異常
    public void OnAfterTurn()
    {
        Status?.OnAfterTurn?.Invoke(this);
        VolatileStatus?.OnAfterTurn?.Invoke(this);
    }
}

public class DamageDetails
{
    public bool Fainted { get; set; } // 戦闘不能かどうか
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}
