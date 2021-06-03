using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutUnitData : UnitData
{
    public float scoutTileMovementSpeed = 5;


    void Start()
    {
        tileSpeed = scoutTileMovementSpeed;
    }

}
