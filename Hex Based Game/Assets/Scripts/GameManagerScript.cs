using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public bool activateFogOnPlay = true;
    private Unit selectedUnit;
    private Vector2 selectedHexIndex;
    private HexTileManager tileManager;
    private UnitManager unitManager;
    private UIManager uiManager;

    public Vector2 SelectedHexIndex {
        get { return selectedHexIndex; }
        set {
            selectedHexIndex = value;
            uiManager.UpdateSelectedText(value.x, value.y);
        }
    }

    public Unit SelectedUnit {
        get { return selectedUnit; }
        set {
            selectedUnit = value;
        }
    }
    

    void Start()
    {
        tileManager = GameObject.Find("HexTileManager").GetComponent<HexTileManager>();
        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        SetupGame();
    }

    public void SetupGame() {
        tileManager.CreateHexTileMap();
        unitManager.GenerateTestUnits();

        selectedHexIndex = new Vector2(-1, -1);

        if(activateFogOnPlay)
        {
            RenderSettings.fog = true;
        }
    }

    public void MoveSelectedUnitToSelectedDestination() {

        bool selectedHexExists = selectedHexIndex != new Vector2(-1, -1);
        bool selectedUnitExists = selectedUnit != null;

        if(!selectedHexExists && !selectedUnitExists)
        {
            Debug.Log("No destination hex or unit selected");
            return;
        }
        else if (!selectedHexExists)
        {
            Debug.Log("No destination hex selected");
            return;
        }
        else if (!selectedUnitExists)
        {
            Debug.Log("No unit selected");
            return;
        }
        else
        {
            unitManager.MoveUnitToDestinationIndex(selectedUnit, selectedHexIndex);
        }
    }

    public void MoveSelectedUnitToSelectedDestinationAlongPath()
    {
        bool selectedHexExists = selectedHexIndex != new Vector2(-1, -1);
        bool selectedUnitExists = selectedUnit != null;

        if(!selectedHexExists && !selectedUnitExists)
        {
            Debug.Log("No destination hex or unit selected");
            return;
        }
        else if (!selectedHexExists)
        {
            Debug.Log("No destination hex selected");
            return;
        }
        else if (!selectedUnitExists)
        {
            Debug.Log("No unit selected");
            return;
        }
        else
        {
            selectedUnit.MoveToDestinationAlongPath(selectedHexIndex);
        }
    }
}
