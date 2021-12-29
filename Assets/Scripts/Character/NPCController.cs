using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCController : MonoBehaviour, IInteractable
{
    // 目標
    // ・話しかけても他のNPCはアニメーションする：済
    // ・建物やPlayerを貫通しない：済
    // ・こちらをむく(話しかけたタイミングでNPCの向きを変える)：済

    [SerializeField] Dialog dialog;
    [SerializeField] List<Vector2> movePattern;
    [SerializeField] float timeBetweenPattern;

    Character character;
    NPCState state;
    int currentPattern;

    float idleTimer = 0;//アイドルである時間

    void Awake()
    {
        currentPattern = 0;
        state = NPCState.Idle;
        character = GetComponent<Character>();
    }
    // 話しかけられた時に実行する関数
    public void Interact(Vector3 initiator)
    {
        if (state == NPCState.Idle)
        {
            state = NPCState.Dialog;
            // 話しかけてきた方をむく
            character.LookTowards(initiator);
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog, OnDialogFinished));
        }
    }
    // 会話が終わったら状態をIdleに戻す
    void OnDialogFinished()
    {
        idleTimer = 0;
        state = NPCState.Idle;
    }

    void Update()
    {
        // Idleなら一定間隔で歩く
        if (state == NPCState.Idle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > timeBetweenPattern)
            {
                idleTimer = 0;
                StartCoroutine(Walk());
            }
        }
        character.HandleUpdate();
    }

    IEnumerator Walk()
    {
        state = NPCState.Walk;
        Vector3 oldPosition = transform.position;
        yield return character.Move(movePattern[currentPattern]);
        // 移動してたら次のアニメーションにいく
        if (oldPosition != transform.position)
        {
            currentPattern = (currentPattern + 1) % movePattern.Count;
        }
        state = NPCState.Idle;
    }
}
public enum NPCState
{
    Idle,
    Walk,
    Dialog,
}