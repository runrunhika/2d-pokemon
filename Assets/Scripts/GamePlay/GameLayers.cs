using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayers : MonoBehaviour
{
    // 壁判定のLayer
    [SerializeField] LayerMask solidObjectsLayer;
    [SerializeField] LayerMask interactableLayer;
    // 草むら判定のLayer
    [SerializeField] LayerMask longGrassLayer;

    [SerializeField] LayerMask playerLayer;
    // どこからでも利用可能
    public static GameLayers Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public LayerMask SolidObjectsLayer { get => solidObjectsLayer; }
    public LayerMask InteractableLayer { get => interactableLayer; }
    public LayerMask LongGrassLayer { get => longGrassLayer; }
    public LayerMask PlayerLayer { get => playerLayer; }
}
