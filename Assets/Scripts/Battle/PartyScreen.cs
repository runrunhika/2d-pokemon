using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{
    [SerializeField] Text messageText;
    // ポケモン選択画面(PartyMenberUI.cs)の管理
    PartyMemberUI[] memberSlots;

    List<Pokemon> pokemons;

    // PartyMemberUIの取得
    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>();
    }

    //BattleSystemから手持ちポケモンデータ取得 -> 各々にデータをセット
    public void SetPartyData(List<Pokemon> pokemons)
    {
        this.pokemons = pokemons;
        for (int i=0; i<memberSlots.Length; i++)
        {
            if (i<pokemons.Count)
            {
                memberSlots[i].SetData(pokemons[i]);
            }
            else
            {
                memberSlots[i].gameObject.SetActive(false);
            }
        }
        messageText.text = "ポケモンを選択してください.";
    }

    //選択されたポケモンの名前の色を変える
    public void UpdateMemberSelection(int selectedMember)
    {
        //selectedMember と一致するなら名前の色を変える
        for (int i=0; i< pokemons.Count; i++)
        {
            if (i == selectedMember)
            {
                //色を変える
                memberSlots[i].SetSelected(true);
            }
            else
            {
                memberSlots[i].SetSelected(false);
            }
        }
    }

    public void SetMessage(string message)
    {
        messageText.text = message;
    }

}
