using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*  名前・レベル・HPを管理    */
public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] Text statusText;
    [SerializeField] HPBar hpBar;

    // ????????
    [SerializeField] Color poisonColor;
    [SerializeField] Color burnColor;
    [SerializeField] Color sleepColor;
    [SerializeField] Color paralysisColor;
    [SerializeField] Color freezeColor;

    Pokemon _pokemon;
    // ConditionID??????????
    Dictionary<ConditionID, Color> statusColors;


    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;
        nameText.text = pokemon.Base.Name;
        levelText.text = "LV "+pokemon.Level;
        //最大HPからの割合を入れる
        hpBar.SetHP((float)pokemon.HP / pokemon.MaxHP);

        statusColors = new Dictionary<ConditionID, Color>()
        {
            {ConditionID.Poison, poisonColor },
            {ConditionID.Burn, burnColor },
            {ConditionID.Paralysis, paralysisColor },
            {ConditionID.Sleep, sleepColor },
            {ConditionID.Freeze, freezeColor },
        };
        SetStatusText();
        _pokemon.OnStatusChanged += SetStatusText;
    }

    // ??????????????????
    void SetStatusText()
    {
        // _pokemon????????
        if (_pokemon.Status == null)
        {
            statusText.text = "";
        }
        else
        {
            statusText.text = _pokemon.Status.Name;
            // ????
            statusText.color = statusColors[_pokemon.Status.Id];
        }
    }

    public IEnumerator UpdateHP()
    {
        if (_pokemon.HpChange)
        {
            yield return hpBar.SetHPSmooth((float)_pokemon.HP / _pokemon.MaxHP);
            _pokemon.HpChange = false;
        }
    }
}
