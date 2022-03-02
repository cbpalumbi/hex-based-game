using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovementManager : MonoBehaviour
{

    private Vector3 startHexPosRMB; //right mouse button
    private Vector3 currentHexPosRMB;
    private Vector2 startHexIndex;
    private GameManagerScript gameManager;
    private HexTileManager tileManager;
    private UIManager uiManager;
    private List<Vector2> tempPreviewPath;
    private bool isCurrentlySelectedPathValid;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        tileManager = GameObject.Find("HexTileManager").GetComponent<HexTileManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        tempPreviewPath = new List<Vector2>();
    }
    
    void Update()
    {
        //
        
        if(gameManager.SelectedUnit != null)
        { //only check if there is a selected unit
            if(Input.GetMouseButtonDown(1)) //initial click
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if(Physics.Raycast(ray, out hit))
                {
                    if(hit.transform.gameObject.GetComponent<Hex>())
                    {
                        bool isTraversable = hit.transform.gameObject.GetComponent<HexData>().isTraversable;
                        
                        if(isTraversable) {
                            startHexPosRMB = hit.transform.position;
                            //Debug.Log("start RMB on hex: " + hit.transform.gameObject.name);

                            Hex hitHex = hit.transform.gameObject.GetComponent<Hex>();
                            startHexIndex = new Vector2(hitHex.xIndex, hitHex.zIndex);
                            tempPreviewPath = tileManager.PreviewPathFromUnitToDestination(gameManager.SelectedUnit, startHexIndex);

                            ProcessDestinationTile(hitHex);
                        }
                    }
                }
                else
                {
                    startHexPosRMB = new Vector3 (-1,-1,-1);
                }
                
                if (startHexPosRMB != new Vector3 (-1,-1,-1))
                {
                    // Debug.Log("start RMB on hex at pos" + startHexPosRMB);
                }
            }
            if(Input.GetMouseButton(1)) //drag
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if(Physics.Raycast(ray, out hit))
                {
                    if(hit.transform.gameObject.GetComponent<Hex>())
                    {
                        currentHexPosRMB = hit.transform.position;
                        //Debug.Log("current RMB on hex: " + hit.transform.gameObject.name);
                    
                        if(currentHexPosRMB != startHexPosRMB) 
                        {//if we've dragged our RMB to a different hex tile
                            
                            //turn off previous tile outline
                            tileManager.TryGetHexFromIndex(startHexIndex).TurnOffOutline();
                            tileManager.ClearPreviewPath();
                            
                            bool isTraversable = hit.transform.gameObject.GetComponent<HexData>().isTraversable;
                            bool isOccupied = false;
                            if (hit.transform.gameObject.GetComponent<Hex>().OccupyingUnit != null) {
                                isOccupied = true;
                            }
                            
                            if (isTraversable && !isOccupied) {
                                Hex hitHex = hit.transform.gameObject.GetComponent<Hex>();
                                Vector2 hitHexIndex = new Vector2(hitHex.xIndex, hitHex.zIndex);
                                
                                //set currently previewing destination to be the new target hex instead of the original one we clicked
                                startHexPosRMB = currentHexPosRMB;

                                //update startHexIndex too
                                startHexIndex = hitHexIndex;
                                
                                //rerun preview with new target hex
                                tempPreviewPath = tileManager.PreviewPathFromUnitToDestination(gameManager.SelectedUnit, hitHexIndex);                            
                                
                                ProcessDestinationTile(hitHex); //determines tile validity and outlines
                            }
                        }
                    }
                }
                else
                {
                    currentHexPosRMB = new Vector3 (-1,-1,-1);
                }
                
                if (currentHexPosRMB != new Vector3 (-1,-1,-1))
                {
                    // Debug.Log("current RMB on hex at pos" + currentHexPosRMB);
                }
            }
            if(tempPreviewPath.Count > 0 && Input.GetMouseButtonUp(1))
            { //if lifted up on RMB, starts unit movement
                
                if(isCurrentlySelectedPathValid && gameManager.SelectedUnit)
                {
                    Unit selectedUnit = gameManager.SelectedUnit;
                    selectedUnit.MoveAlongPath(tempPreviewPath);
                    
                    //update remaining movement
                    selectedUnit.SetRemainingMovement(selectedUnit.movementRemaining - (tempPreviewPath.Count-1));

                    selectedUnit.unitData.ConstructRemainingMovementStat(selectedUnit.movementRemaining, selectedUnit.unitData.tileSpeed);
                    uiManager.UpdateSelectedInfoPanel();
                }

                //clear lingering UI
                ClearPathAndPathingUI();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            ClearPathAndPathingUI();
        }
    }

    private void ClearPathAndPathingUI() {
        tempPreviewPath.Clear();
        tileManager.ClearPreviewPath();
        tileManager.TryGetHexFromIndex(startHexIndex).TurnOffOutline();
    }

    private void ProcessDestinationTile(Hex hitHex) {
        
        HexData hexData = hitHex.gameObject.GetComponent<HexData>();
        
        if (!hexData.isTraversable) // if non-traversable tile
        { 
            hitHex.TurnOnInvalidOutline();
            isCurrentlySelectedPathValid = false;
            Debug.Log("tile not traversable");
        }
        else if(tempPreviewPath.Count-1 > gameManager.SelectedUnit.movementRemaining) //if destination too far away
        { //minus one because starting hex is in list
            hitHex.TurnOnInvalidOutline();
            isCurrentlySelectedPathValid = false;
        } 
        else //otherwise, valid tile
        { 
            //turn on new tile outline
            hitHex.SetToHighlightMaterial();
            isCurrentlySelectedPathValid = true;
        }
    }
}
