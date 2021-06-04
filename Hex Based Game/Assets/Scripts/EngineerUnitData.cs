using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineerUnitData : UnitData
{
    public float engineerTileMovementSpeed = 5;

    void Start()
    {
        tileSpeed = engineerTileMovementSpeed;
        unitName = "ENGINEER";
    }
}
