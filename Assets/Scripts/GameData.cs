using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameData
{
    public string playerName = "Maria";
    [SerializeField]
    private List<string> defaultFlags = new List<string>();
    private Dictionary<GlobalFlag, bool> globalFlags = new Dictionary<GlobalFlag, bool>();

    public string PlayerName
    {
        get { return playerName; }
        set { playerName = value; }
    }

    public GameData()
    {
        foreach (GlobalFlag flag in Enum.GetValues(typeof(GlobalFlag)))
        {
            globalFlags.Add(flag, false);
        }
    }

    public bool GetFlag(GlobalFlag flag) { return globalFlags[flag]; }
    public void SetFlag(GlobalFlag flag, bool value) { globalFlags[flag] = value; }
    public enum GlobalFlag
    {
        HasInteractedWithCrate = 0
    }
}
