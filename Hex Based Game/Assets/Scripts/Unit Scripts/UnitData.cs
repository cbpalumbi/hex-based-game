using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData : MonoBehaviour
{
    [HideInInspector] public float tileSpeed;
    [HideInInspector] public string unitName;
    [HideInInspector] public List<Stat> playerFacingStats;

    public void ConstructRemainingMovementStat(float remaining, float max)
    {
        StringStat remainingMovementStat = new StringStat("Mvmt Speed", (remaining.ToString() + " / " + max.ToString()));
        
        if(playerFacingStats.Count == 0)
        {
            playerFacingStats.Add(remainingMovementStat);
        }
        else
        {
            for(int i = 0; i < playerFacingStats.Count; i++)
            {
                if(playerFacingStats[i].statName == "Mvmt Speed")
                {
                    playerFacingStats[i] = remainingMovementStat;
                    break;
                }
            }
        }
    }
}
