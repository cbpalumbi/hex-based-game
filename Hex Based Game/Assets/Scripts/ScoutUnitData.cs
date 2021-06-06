using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutUnitData : UnitData
{
    public float scoutTileMovementSpeed = 5;

    private Unit unitScript;

    void Start()
    {
        Setup();
    }

    private void Setup()
    {
        playerFacingStats = new List<Stat>();

        tileSpeed = scoutTileMovementSpeed;

        unitName = "SCOUT";

        ConstructRemainingMovementStat(scoutTileMovementSpeed, scoutTileMovementSpeed);
        playerFacingStats.Add(new FloatStat("stat2", 15.7f));

        unitScript = gameObject.GetComponent<Unit>();
        unitScript.SetRemainingMovement(tileSpeed);
    }
}
