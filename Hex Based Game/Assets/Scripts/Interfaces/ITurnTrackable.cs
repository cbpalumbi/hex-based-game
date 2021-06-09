using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurnTrackable
{
    public bool GetIsTurnTrackingOn();
    public void ProcessTurn();
}
