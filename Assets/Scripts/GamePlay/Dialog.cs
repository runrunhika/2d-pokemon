using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 文章のまとまり
[System.Serializable]
public class Dialog
{
    [SerializeField] List<string> lines;

    public List<string> Lines { get => lines; }
}
