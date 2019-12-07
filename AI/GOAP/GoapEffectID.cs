using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GoapEffect
{
    public static List<string> IdNames = new List<string>(new string[] { "work", "HasStorage", "Harvesting", "Pickup", "Planting", "Growing" });

    [SerializeField] public int value = 0;
}
