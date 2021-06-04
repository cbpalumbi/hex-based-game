using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatStat : Stat
{
    public FloatStat(string name, float value)
    {
        statName = name;
        statValue = value.ToString();
    }
}
