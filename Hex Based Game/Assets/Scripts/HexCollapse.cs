using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCollapse : MonoBehaviour
{
    
    HexData data;
    HexTileManager tileManager;
    Hex hexScript;

    void Start() {
        data = gameObject.GetComponent<HexData>();
        tileManager = GameObject.Find("HexTileManager").GetComponent<HexTileManager>();
        hexScript = gameObject.GetComponent<Hex>();
    }
    
    public void ResetCollapseTurns() {
        data.collapseTurnsCurrent = data.collapseTurnsMax;
    }

    public void CollapseByOne(Vector2 hexIndex) {
        if (data.collapseTurnsCurrent > 0) {
            data.collapseTurnsCurrent -= 1;
        }
        
        if (data.collapseTurnsCurrent == 0) {
            CollapseHex(hexIndex);
        }
    }

    public void CollapseHex(Vector2 hexIndex) {
        //tileManager.hexes.Remove(hexIndex);
        tileManager.hexes[hexIndex].HideHex();
        gameObject.GetComponent<HexData>().isTraversable = false;

    }
}
