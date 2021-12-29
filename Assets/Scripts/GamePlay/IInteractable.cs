using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IInteractableを継承しているクラスは、必ずInteract関数を持っている
public interface IInteractable
{
    // 関数を宣言する
    public void Interact(Vector3 initiator);
}
