using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] Color highlightColor;
    // 役割:dialogのTextを取得して、変更する
    [SerializeField] int letterPerSecond; // 1文字あたりの時間
    [SerializeField] Text dialogText;

    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;

    //Fight or Runを操作するためのList
    [SerializeField] List<Text> actionTexts;
    [SerializeField] List<Text> moveTexts;

    [SerializeField] Text ppText;
    [SerializeField] Text typeText;



    // Textを変更するための関数
    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    //1文字ずつ表示
    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (char letter in dialog)
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / letterPerSecond);
        }

        yield return new WaitForSeconds(0.7f);
    }

    // UIの表示/非表示をする

    // dialogTextの表示管理
    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }

    // actionSelectorの表示管理
    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }

    //moveSelector & moveDetails表示管理
    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

    //選択中のActionTextの色を変える
    public void UpdateActionSelection(int selectAction)
    {
        // selectAction = 0 -> actionTexts[0] の色を変える
        // selectAction = 1 -> actionTexts[1] の色を変える
        // それ以外は黒色

        for (int i=0; i<actionTexts.Count; i++)
        {
            if (selectAction == i)
            {
                actionTexts[i].color = highlightColor;
            }
            else
            {
                actionTexts[i].color = Color.black;
            }
        }
    }

    //選択中のMoveTextsの色を変える & PP Type Text反映
    public void UpdateMoveSelection(int selectMove, Move move)
    {
        // selectMove = 0 -> moveTexts[0] の色を変える
        // selectMove = 1 -> moveTexts[1] の色を変える ....
        // それ以外は黒色

        for (int i = 0; i < moveTexts.Count; i++)
        {
            if (selectMove == i)
            {
                moveTexts[i].color = highlightColor;
            }
            else
            {
                moveTexts[i].color = Color.black;
            }
        }
        ppText.text = $"PP {move.PP}/{move.Base.PP}";
        typeText.text = move.Base.Type.ToString();
        // PPが0なら赤色
        if (move.PP == 0)
        {
            ppText.color = Color.red;
        }
        else
        {
            ppText.color = Color.black;
        }
    }

    // BattleSystemから技名取得・反映
    public void SetMoveNames(List<Move> moves)
    {
        for (int i=0; i<moveTexts.Count; i++)
        {
            // 覚えている数だけ反映
            if (i < moves.Count)
            {
                moveTexts[i].text = moves[i].Base.Name;
            }
            else
            {
                moveTexts[i].text = ".";
            }
        }
    }
}

