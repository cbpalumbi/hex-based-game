using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutUnitData : UnitData
{
    public float scoutTileMovementSpeed = 5;
    void Start()
    {
        SetupScout();
    }

    private void SetupScout()
    {
        tileSpeed = scoutTileMovementSpeed;
        unitName = "SCOUT";
        List<Stat> stats = new List<Stat>();

        stats.Add(new FloatStat("Movement Speed", scoutTileMovementSpeed));
        stats.Add(new FloatStat("stat2", 15.7f));
        
        playerFacingStats = stats;
    }
}
