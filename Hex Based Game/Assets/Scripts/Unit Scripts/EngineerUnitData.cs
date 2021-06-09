using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineerUnitData : UnitData
{
    public float engineerTileMovementSpeed = 5;
    private Unit unitScript;

    void Start()
    {
        Setup();
    }

    private void Setup()
    {
        playerFacingStats = new List<Stat>();

        tileSpeed = engineerTileMovementSpeed;

        unitName = "ENGINEER";

        ConstructRemainingMovementStat(engineerTileMovementSpeed, engineerTileMovementSpeed);
        playerFacingStats.Add(new StringStat("Engineery-ness", "a lot"));

        unitScript = gameObject.GetComponent<Unit>();
        unitScript.SetRemainingMovement(tileSpeed);
    }
}
