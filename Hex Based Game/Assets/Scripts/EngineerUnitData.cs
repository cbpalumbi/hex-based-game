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
        tileSpeed = engineerTileMovementSpeed;
        unitName = "ENGINEER";
        List<Stat> stats = new List<Stat>();

        stats.Add(new FloatStat("Mvmt Speed", engineerTileMovementSpeed));
        
        playerFacingStats = stats;

        unitScript = gameObject.GetComponent<Unit>();
        unitScript.SetRemainingMovement(tileSpeed);
    }
}
