using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData : MonoBehaviour
{
    [HideInInspector] public float tileSpeed;
    [HideInInspector] public string unitName;
    [HideInInspector] public List<Stat> playerFacingStats;

    void Start()
    {
        playerFacingStats = new List<Stat>();
    }
}
