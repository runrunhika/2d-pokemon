using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// 会話の表示をする
public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] float letterPerSecond;

    public UnityAction OnShowDialog;
    public UnityAction OnCloseDialog;
    public UnityAction OnDialogFinished;

    public static DialogManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    Dialog dialog;
    int currentLine = 0;
    bool isTyping;

    public bool IsShowing { get; private set; }

    // 与えられた文章を表示する
    public IEnumerator ShowDialog(Dialog dialog, UnityAction onFinishied)
    {
        // フレーム終わりまでまつ
        yield return new WaitForEndOfFrame();
        IsShowing = true;
        OnShowDialog?.Invoke();
        OnDialogFinished = onFinishied;
        this.dialog = dialog;
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
    }

    // タイプ形式で文字を表示する
    IEnumerator TypeDialog(string line)
    {
        isTyping = true;
        dialogText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / letterPerSecond);
        }
        isTyping = false;
    }


    public void HandleUpdate()
    {
        // Zボタンを押したら、次を表示する.最後だったら閉じる
        if (Input.GetKeyDown(KeyCode.Z) && isTyping == false)
        {
            currentLine++;
            if (currentLine < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }
            else
            {
                // 会話終了
                IsShowing = false;
                currentLine = 0;
                dialogBox.SetActive(false);
                OnDialogFinished?.Invoke();
                OnCloseDialog?.Invoke();
            }
        }
    }
}
