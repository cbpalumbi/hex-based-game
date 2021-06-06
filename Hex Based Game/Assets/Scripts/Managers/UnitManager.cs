using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    
    public List<Unit> units;
    public GameObject scoutUnitPrefab;
    public GameObject engineerUnitPrefab;
    public float unitHeightOffsetFloat = 0.25f;  //to sit on board, units must be raised up on y axis
    private GameManagerScript gameManager;
    private HexTileManager tileManager;
    private int uniqueUnitCount = 0;

    void Start() 
    {
        units = new List<Unit>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        tileManager = GameObject.Find("HexTileManager").GetComponent<HexTileManager>();
    }

    public void GenerateTestUnits() 
    {
        GenerateUnit(UnitType.Scout, new Vector2(10, 10));
        GenerateUnit(UnitType.Engineer, new Vector2(8, 8));
    }

    public void GenerateUnit(UnitType unitType, Vector2 hexIndex) 
    {
        if (tileManager.hexes[hexIndex].OccupyingUnit != null)
        {
            Debug.Log("Cannot generate unit because tile is already occupied by another unit");
            return;
        }

        if (hexIndex.x < 0 || hexIndex.x > tileManager.mapWidth || hexIndex.y < 0 || hexIndex.y > tileManager.mapHeight)
        {
            Debug.Log("Cannot generate unit at invalid hex index");
            return;
        }

        if (!System.Enum.IsDefined(typeof(UnitType), unitType))
        {
            Debug.Log("Cannot generate a unit of invalid unit type");
            return;
        }

        GameObject unitPrefab = null;

        switch(unitType)
        {
            case UnitType.Engineer:
                unitPrefab = engineerUnitPrefab;
            break;
            case UnitType.Scout:
            default:
                unitPrefab = scoutUnitPrefab;
            break;
        }

        if (unitPrefab == null) 
        {
            Debug.Log("Cannot generate unit from null prefab");
            return;
        }

        Vector3 worldPos = tileManager.GetUnitWorldPosFromHexIndex(hexIndex);

        if(worldPos != new Vector3(-1,-1,-1))
        {
            //instantiates new unit
            GameObject unitObj = GameObject.Instantiate(unitPrefab);
            //moves unit to proper position
            unitObj.transform.position = worldPos;

            Unit unit = unitObj.GetComponent<Unit>();

            //sets unit's current hex to location hex
            unit.CurrentHexIndex = hexIndex;
            AssignNewUnitId(unit);

            //set unit reference scripts
            unit.unitData = unitObj.GetComponent<UnitData>();

            //adds unit to units list
            units.Add(unit);
            //sets location's occupying unit to this unit
            tileManager.hexes[hexIndex].OccupyingUnit = unit;
        }
    }

    private void AssignNewUnitId(Unit unit) 
    {
        unit.unitId = uniqueUnitCount;
        uniqueUnitCount++;
    }

    public void DeselectAllUnits() {
        gameManager.SelectedUnit = null;
        foreach(Unit unit in units)
        {
            unit.SetShadowToDefaultMaterial();
        }
    }

    public void MoveUnitToDestinationIndex(Unit unit, Vector2 destinationIndex) 
    {
        if(!tileManager.hexes.ContainsKey(destinationIndex))
        {
            Debug.Log("Destination hex is not in hexes dictionary");
            return;
        }

        if (unit == null)
        {
            Debug.Log("Unit to move was null!");
            return;
        }

        unit.DebugMoveToDestination(destinationIndex);
    }

    public enum UnitType {
        Scout,
        Engineer
    }
}