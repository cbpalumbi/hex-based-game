using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovementTurnTask : TurnTask
{
    public Unit unit;

    public UnitMovementTurnTask (Unit unitToMove)
    {
        unit = unitToMove;
        taskType = TaskType.UnitMovement;
    }
}
