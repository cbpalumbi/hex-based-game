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
    private List<Vector2> tempPreviewPath;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        tileManager = GameObject.Find("HexTileManager").GetComponent<HexTileManager>();
        tempPreviewPath = new List<Vector2>();
    }
    
    void Update()
    {
        //
        
        if(gameManager.SelectedUnit != null)
        { //only check if there is a selected unit
            if(Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if(Physics.Raycast(ray, out hit))
                {
                    if(hit.transform.gameObject.GetComponent<Hex>())
                    {
                        startHexPosRMB = hit.transform.position;
                        //Debug.Log("start RMB on hex: " + hit.transform.gameObject.name);

                        Hex hitHex = hit.transform.gameObject.GetComponent<Hex>();
                        startHexIndex = new Vector2(hitHex.xIndex, hitHex.zIndex);
                        tempPreviewPath = tileManager.PreviewPathFromUnitToDestination(gameManager.SelectedUnit, startHexIndex);
                    
                        //turn on hex outline
                        hitHex.TurnOnOutline();
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
            if(Input.GetMouseButton(1))
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
                            
                            Hex hitHex = hit.transform.gameObject.GetComponent<Hex>();
                            Vector2 hitHexIndex = new Vector2(hitHex.xIndex, hitHex.zIndex);
                            
                            //set currently previewing destination to be the new target hex instead of the original one we clicked
                            startHexPosRMB = currentHexPosRMB;

                            //update startHexIndex too
                            startHexIndex = hitHexIndex;
                            
                            //rerun preview with new target hex
                            tempPreviewPath = tileManager.PreviewPathFromUnitToDestination(gameManager.SelectedUnit, hitHexIndex);
                            
                            if(tempPreviewPath.Count <= gameManager.SelectedUnit.unitData.tileSpeed)
                            {
                                //turn on new tile outline
                                hitHex.TurnOnOutline();
                            }
                            else 
                            {
                                hitHex.TurnOnInvalidOutline();
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
            {
                //if lifted up on RMB, starts unit movement
                if(gameManager.SelectedUnit)
                {
                    gameManager.SelectedUnit.MoveAlongPath(tempPreviewPath);
                }
                tempPreviewPath.Clear();
            }
        }
    }
}
