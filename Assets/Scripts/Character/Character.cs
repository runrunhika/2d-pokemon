using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    // 移動とアニメーションを管理する
    [SerializeField] float moveSpeed;

    CharacterAnimator animator;

    public bool IsMoving { get; set; }
    public CharacterAnimator Animator { get => animator; }

    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
    }


    // コルーチンを使って徐々に目的地に近づける
    public IEnumerator Move(Vector2 moveVec, UnityAction OnMoveOver = null)
    {
        // 向きを変えたい
        animator.MoveX = Mathf.Clamp(moveVec.x, -1f, 1f);
        animator.MoveY = Mathf.Clamp(moveVec.y, -1f, 1f);
        Vector3 targetPos = transform.position;
        targetPos += (Vector3)moveVec;
        if (!IsPathClear(targetPos))
        {
            yield break;
        }

        // 移動中は入力を受け付けたくない
        IsMoving = true;

        // targetPosとの左があるなら繰り返す
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            // targetPosに近づける
            transform.position = Vector3.MoveTowards(
                transform.position, // 現在の場所
                targetPos,          // 目的地
                moveSpeed * Time.deltaTime
                );
            yield return null;
        }

        transform.position = targetPos;
        IsMoving = false;
        OnMoveOver?.Invoke();
    }

    public void HandleUpdate()
    {
        animator.IsMoving = IsMoving;
    }

    bool IsPathClear(Vector3 taregtPos)
    {
        Vector3 diff = taregtPos - transform.position;
        Vector3 dir = diff.normalized; 
        return Physics2D.BoxCast(transform.position+dir, new Vector2(0.2f, 0.2f), 0, dir, diff.magnitude-1, GameLayers.Instance.SolidObjectsLayer | GameLayers.Instance.InteractableLayer | GameLayers.Instance.PlayerLayer) == false;
    }


    // targetPosに移動可能かを調べる関数
    bool IsWalkable(Vector2 targetPos)
    {
        // targetPosに半径0.2fの円のRayを飛ばしてぶつからなかった
        return Physics2D.OverlapCircle(targetPos, 0.2f, GameLayers.Instance.SolidObjectsLayer | GameLayers.Instance.InteractableLayer) == false;
    }

    // targetPosの方を向く
    public void LookTowards(Vector3 targetPos)
    {
        float xDiff = Mathf.Floor(targetPos.x) - Mathf.Floor(transform.position.x);
        float yDiff = Mathf.Floor(targetPos.y) - Mathf.Floor(transform.position.y);

        if (xDiff == 0 || yDiff == 0)
        {
            animator.MoveX = Mathf.Clamp(xDiff, -1f, 1f);
            animator.MoveY = Mathf.Clamp(yDiff, -1f, 1f);
        }
    }
}
