using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GoapID
{
    public static List<string> LayerNames = new List<string>(new string[] { "ResourcePickup", "Field", "Toolshed", "Storage"});

    [SerializeField] public int value = 0;
}
