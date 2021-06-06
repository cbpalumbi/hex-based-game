using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, ISelectable
{
    private Vector2 currentHexIndex;

    public float speed;
    private MeshRenderer shadowMeshRenderer;
    public Material selectedMat;
    public Material defaultMat;
    private GameManagerScript gameManager;
    private UnitManager unitManager;
    private HexTileManager tileManager;
    private UIManager uIManager;
    public float wayPointRadius = 0.01f;
    
    [HideInInspector] public int unitId;
    private int currentStepInPathIndex = 0;
    private bool shouldMove = false;
    [HideInInspector] public float movementRemaining;
    
    private List<Vector3> globalPathToFollowInWorldPos;
    [HideInInspector] public UnitData unitData;


    public Vector2 CurrentHexIndex {
        get { return currentHexIndex; }
        set {
            currentHexIndex = value;
        }
    }
    
    void Start() 
    {
        tileManager = GameObject.Find("HexTileManager").GetComponent<HexTileManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        SetupUnit();

    } 

    private void SetupUnit()
    {
        if (gameObject.transform.GetChild(0))
        {
            shadowMeshRenderer = gameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
        }
        else 
        {
            Debug.Log("Unit has no shadow gameobject");
        }

        //set initial movement remaining to movement speed
        SetMovementSpeedToMax();
    }

    public void SetMovementSpeedToMax()
    {
        movementRemaining = unitData.tileSpeed;
    }

    public void DebugMoveToDestination(Vector2 destinationIndex) 
    {
        if(tileManager.hexes.ContainsKey(destinationIndex)) 
        {
            Unit destinationOccupyingUnit = tileManager.hexes[destinationIndex].OccupyingUnit;
            if(destinationOccupyingUnit == null) //makes sure there is no unit already there
            {
                //get tile's position in Unity space
                Vector3 destinationPos = tileManager.GetUnitWorldPosFromHexIndex(destinationIndex);
                //move unit to new position
                transform.position = destinationPos;
                //null out occupying unit of vacated tile
                tileManager.hexes[currentHexIndex].OccupyingUnit = null;
                //set unit's currentHexIndex to index of destination tile
                currentHexIndex = destinationIndex;
                //set destination tile's occupying unit to this unit
                tileManager.hexes[destinationIndex].OccupyingUnit = this;
                
                //Debug.Log("Moving to hex (" + destinationIndex.x + ", " + destinationIndex.y + ") which is at position " + destinationPos);
            }
            else
            {
                Debug.Log("There is another unit occupying the destination hex");
                return;    
            }
        }
        else 
        {
            Debug.Log("Invalid destination hex");
            return;
        }
    }

    public void DebugMoveToDestinationAlongPath(Vector2 destinationIndex)
    {
        Dictionary<Vector2, Vector2> searchResults = tileManager.Search(currentHexIndex, destinationIndex);
        List<Vector2> path = tileManager.ReconstructPathFromSearch(currentHexIndex, destinationIndex, searchResults);

        List<Vector3> pathInWorldPos = tileManager.ConvertTilePathToWorldPosPath(path);

        // foreach(Vector2 step in path)
        // {
            
        //     tileManager.hexes[step].SetToSelectedMaterial();
        // }

        //provides movement system a new destination and activates movement
        TurnOnMovement(pathInWorldPos);
        //set unit's current index to destination's index
        currentHexIndex = path[path.Count - 1];
    }

    public void MoveAlongPath(List<Vector2> hexPath)
    {
        List<Vector3> pathInWorldPos = tileManager.ConvertTilePathToWorldPosPath(hexPath);
        //tileManager.PreviewPath(hexPath);
        TurnOnMovement(pathInWorldPos);
        //set unit's current index to destination's index
        currentHexIndex = hexPath[hexPath.Count - 1];
    }

    void Update()
    {
        if (shouldMove) //if movement system is on
        {
            float distance = Vector3.Distance(globalPathToFollowInWorldPos[currentStepInPathIndex], transform.position);

            transform.position = Vector3.MoveTowards(transform.position, globalPathToFollowInWorldPos[currentStepInPathIndex], Time.deltaTime * speed);
        
            if (distance < wayPointRadius) //if reached *current step* destination
            {
                //decrease movement remaining by one, will replace with tile cost later
                movementRemaining--;

                //set new destination to next step in path
                currentStepInPathIndex++; 

                if (currentStepInPathIndex >= globalPathToFollowInWorldPos.Count) //if we've just reached  the *final step* destination
                {
                    TurnOffMovement();
                }
            }
        }
    }

    private void TurnOnMovement(List<Vector3> pathInWorldPos)
    {
        //sets global to be path produced by the search passed in
        globalPathToFollowInWorldPos = pathInWorldPos;
        //switches unit to movement mode
        shouldMove = true;
    }

    private void TurnOffMovement()
    {
        //takes unit out of movement mode
        shouldMove = false;
        //resets path stepper
        currentStepInPathIndex = 0;
        //clears just-completed path
        globalPathToFollowInWorldPos.Clear();
        //destroys all existing preview path lines
        tileManager.ClearPreviewPath();
        //remove outline of destination hex, now current hex
        tileManager.TryGetHexFromIndex(currentHexIndex).TurnOffOutline();
    }

    public void OnMouseDown() 
    {
        Debug.Log("Unit clicked");
        if (gameManager.SelectedUnit == this)
        {
            gameManager.SelectedUnit = null;
            SetShadowToDefaultMaterial();
            return;
        }
        else
        {
            unitManager.DeselectAllUnits();

            gameManager.SelectedUnit = this;
            uIManager.TurnOnSelectedInfoPanel();
            uIManager.UpdateSelectedInfoPanel();

            if(shadowMeshRenderer != null) 
            {
                shadowMeshRenderer.material = selectedMat;
            }
        }
    }

    public void SetShadowToDefaultMaterial() {

        if(shadowMeshRenderer != null && defaultMat != null) {
            shadowMeshRenderer.material = defaultMat;
        }
        else 
        {
            Debug.Log("Unit shadow mesh renderer or default mat is null");
        }
    }

}
