using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexData : MonoBehaviour
{
    public bool isTraversable = true;
    
    public bool doesCollapse = false;

    public int collapseTurnsMax = 5;
    public int collapseTurnsCurrent;

    void Start() {
        collapseTurnsCurrent = collapseTurnsMax;
    }

}
