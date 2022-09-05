using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexData : MonoBehaviour
{
    public bool isTraversable = true;
    
    public bool doesCollapse = false;

    public int collapseTurnsMax = 3;

    [HideInInspector]
    public int collapseTurnsCurrent;

}
