using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public bool activateFogOnPlay = false;
    private Unit selectedUnit;
    private Vector2 selectedHexIndex;
    private HexTileManager tileManager;
    private UnitManager unitManager;
    private UIManager uiManager;

    [HideInInspector] public Queue<TurnTask> turnTasks;

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
        
        turnTasks = new Queue<TurnTask>();
        SetupGame();
    }

    public void ProcessTurnIndicatorClick()
    {
        switch(turnTasks.Dequeue().taskType) 
        {
            case TaskType.UnitMovement:
                //zoom to unit
                break;
            case TaskType.NextTurn:
            default:
                ProcessNextTurn();
                break;
        }
    }

    private void ProcessNextTurn()
    {
        //progress all player-independent changes
            //reset unit movement speed
            //deposit yields
            //process completed items (eg: research)

        //clear list of tasks
        turnTasks.Clear();
        //re-add 'next turn' task
        turnTasks.Enqueue(new NextTurnTurnTask());

        List<Unit> unitsAwaitingInstruction = unitManager.ProcessAllUnitTurns();

        foreach(Unit unit in unitsAwaitingInstruction)
        {
            turnTasks.Enqueue(new UnitMovementTurnTask(unit));
        }
        Debug.Log("tasks.Count: " + turnTasks.Count);
        //generate new queue of tasks
        
        //update all UI
        uiManager.UpdateSelectedInfoPanel();
    }

    public void SetupGame() {

        // char[,] myTiles = new char[,] {
        //     {'B', 'B', 'B'},
        //     {'B', 'B', 'B'},
        //     {'B', 'B', 'B'},
        //     {'B', 'B', 'A'}
        // };

        // TileMapData myData = new TileMapData(3, 4, myTiles);

        // tileManager.CreateHexTileMap(myData.GetWidth(), myData.GetHeight(), myData);

        TileMapData myData = new TileMapData();
        tileManager.CreateHexTileMap(myData);

        unitManager.GenerateTestUnits();

        selectedHexIndex = new Vector2(-1, -1);

        //add original turn task to go to next turn
        turnTasks.Enqueue(new NextTurnTurnTask());
        
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
            selectedUnit.DebugMoveToDestinationAlongPath(selectedHexIndex);
        }
    }
}
