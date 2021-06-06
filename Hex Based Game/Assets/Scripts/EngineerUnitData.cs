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

        SetupEngineer();
    }

    private void SetupEngineer()
    {
        tileSpeed = engineerTileMovementSpeed;
        unitName = "ENGINEER";
        List<Stat> stats = new List<Stat>();

        stats.Add(new FloatStat("Movement Speed", engineerTileMovementSpeed));
        
        playerFacingStats = stats;
    }
}
