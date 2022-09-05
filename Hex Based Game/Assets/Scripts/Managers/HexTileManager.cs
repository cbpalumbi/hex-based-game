using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTileManager : MonoBehaviour
{
    public GameObject hexTilePrefab;
    public GameObject nonTraversableHexTilePrefab;
    public GameObject pathSegmentPrefab;
    private GameManagerScript gameManager;
    private UnitManager unitManager;
    public int mapWidth;
    public int mapHeight;

    public float tileXOffset;
    public float tileZOffset;

    public List<Material> hexColors;

    public Dictionary<Vector2, Hex> hexes;
    private List<GameObject> pathSegments;
    private Vector3 unitHeightOffset; 
    public Vector3 pathSegmentHeightOffset = new Vector3 (0, -0.45f, 0); //subtracting from unit height for some reason

    Vector2[,] neighborsOffsetEvenZ  =  { 
                                            { new Vector2 (1, 0 )}, { new Vector2 (0, -1)}, {new Vector2 (-1, -1)}, 
                                            { new Vector2 (-1, 0)}, { new Vector2 (-1, 1)}, { new Vector2 (0, 1)} 
                                        };
    Vector2[,] neighborsOffsetOddZ  =   { 
                                            { new Vector2 (1, 0 )}, { new Vector2 (1, -1)}, {new Vector2 (0, -1)}, 
                                            { new Vector2 (-1, 0)}, { new Vector2 (0, 1)}, { new Vector2 (1, 1)} 
                                        };

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        pathSegments = new List<GameObject>();

        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        unitHeightOffset = new Vector3 (0, unitManager.unitHeightOffsetYFloat, 0);
    }

    public void CreateHexTileMap(TileMapData data) 
    {
        hexes = new Dictionary<Vector2, Hex>();

        if (data != null) {
            for(int x = 0; x < data.GetHeight(); x++) {
                for(int z = 0; z < data.GetWidth(); z++) {
                    
                    GameObject tempGameObject = Instantiate(GetPrefabFromTileCode(data.GetMapData()[x,z]));
                    
                    PositionNewTile(tempGameObject, x, z);

                    SetUpTile(tempGameObject, x, z);
                }
            }

            mapHeight = data.GetHeight();
            mapWidth = data.GetWidth();

        } 
        else { //if no data passed in, make default empty level
            for(int x = 0; x <= 5; x++) {
                for(int z = 0; z <= 5; z++) {
                    GameObject tempGameObject = Instantiate(hexTilePrefab);

                    PositionNewTile(tempGameObject, x, z);

                    SetUpTile(tempGameObject, x, z);
                }
            }
        }
    }

    private void PositionNewTile(GameObject tempTile, int x, int z) {
        if (z % 2 == 0) {
            tempTile.transform.position = new Vector3(x * tileXOffset, -0.15f, z * tileZOffset);
        }
        else 
        {
            tempTile.transform.position = new Vector3(x * tileXOffset + tileXOffset/2, -0.15f, z * tileZOffset);
        }
    }

    public GameObject GetPrefabFromTileCode(char c) {
        GameObject prefab = hexTilePrefab;
        switch (c) {
            case 'A':
                prefab = hexTilePrefab;
                break;
            case 'B':
                prefab = nonTraversableHexTilePrefab;
                break; 
        }
        return prefab;
    }

    public Dictionary<Vector2, Vector2> Search(Vector2 startHex, Vector2 targetHex)
    {
        //set up empty queue of tiles remaining to search through
        Queue<Vector2> frontier = new Queue<Vector2>();
        //add starting hex to queue
        frontier.Enqueue(startHex);
        //create dictionary to track tiles we're at and what tile we came from
        Dictionary<Vector2, Vector2> tileAndPreviousTile = new Dictionary<Vector2, Vector2>();
        //no previous tile for starting hex
        tileAndPreviousTile[startHex] = new Vector2(-1,-1);

        while (frontier.Count > 0)
        {
            //set hex at front of queue to current hex
            Vector2 current = frontier.Dequeue();

            //if we've reached the target, exit out
            if (current == targetHex) { break; }

            //for each of the valid hexes of the current hex's six neighbors
            foreach (Vector2 next in GetNeighbors(current))
            {
                //if the tile we're examining is not one we've ever visited before
                if (!tileAndPreviousTile.ContainsKey(next))
                {
                    Hex nextHex = TryGetHexFromIndex(next);

                    if (nextHex != null) {
                        if (nextHex.gameObject.GetComponent<HexData>().isTraversable) {
                            //add it to the end of the frontier queue
                            frontier.Enqueue(next);
                            //set the previous tile for the next tile to be the current tile
                            tileAndPreviousTile[next] = current;
                        }
                    }

                    
                    
                    // if (hexes.ContainsKey(next))
                    // {
                    //     //highlight each searched hex
                    //     hexes[next].GetComponent<Hex>().SetToHighlightMaterial();
                    // }
                }
            }
        }
        //return all searched hexes
        return tileAndPreviousTile;
    }

    public List<Vector2> ReconstructPathFromSearch(Vector2 startHex, Vector2 targetHex, Dictionary<Vector2, Vector2> searchSpreadTileAndPreviousTile)
    {
        List<Vector2> path = new List<Vector2>();
        //start at the target hex and work our way back to the starting hex
        Vector2 current = targetHex;

        while(current != startHex) //while we have not yet reached the starting hex
        {
            //add current to the path we're building
            path.Add(current);
            //set new current to be the hex that was one earlier in the search than the current hex
            current = searchSpreadTileAndPreviousTile[current];
        }

        //we've reached the start now, so add the starting hex to the end of the path
        //one reason to keep this is because it allows the path segements to start at center of current hex
        path.Add(startHex);

        //reverse the path, now we're going from startHex to targetHex
        path.Reverse();

        DrawLinesOnPath(path);

        return path;
    }

    public List<Vector3> ConvertTilePathToWorldPosPath(List<Vector2> path)
    {
        List<Vector3> pathInWorldPos = new List<Vector3>();

        foreach(Vector2 step in path)
        {
            //Convert hex path list to world pos path list
            pathInWorldPos.Add(GetUnitWorldPosFromHexIndex(step));
        }

        return pathInWorldPos;
    }

    public void DrawLinesOnPath(List<Vector2> pathInHex)
    {
        List<Vector3> pathInWorldPos = ConvertTilePathToWorldPosPath(pathInHex);

        //clear any old segments
        ClearPreviewPath();
        
        for(int i=0; i < pathInWorldPos.Count-1; i++)
        {
            //runs on all list elements except last one

            Vector3 startHexPos = pathInWorldPos[i];
            Vector3 endHexPos = pathInWorldPos[i+1]; //next hex

            Vector3 midPoint = (startHexPos + endHexPos) / 2;

            GameObject pathSegment = GameObject.Instantiate(pathSegmentPrefab, midPoint + pathSegmentHeightOffset, Quaternion.identity);
            pathSegment.transform.Rotate(new Vector3(0, GetYRotationBetweenNeighboringHexes(pathInHex[i], pathInHex[i+1]), 0), Space.Self);
            pathSegments.Add(pathSegment);
        }
    }

    public void ClearPreviewPath()
    {
        foreach(GameObject segment in pathSegments)
        {
            GameObject.Destroy(segment);
        }
        pathSegments.Clear();
    }

    public float GetYRotationBetweenNeighboringHexes(Vector2 hex1, Vector2 hex2)
    {
        float yRotation = 0;
       
        if(hex1.y % 2 != 0) //if the start hex's z-coord is odd
        {
            if((hex1.x == hex2.x && hex1.y > hex2.y) || (hex1.x < hex2.x && hex1.y < hex2.y))
            {
                //(0, +1) or (-1, -1), top right or bottom left
                yRotation = -60;
            }
            else if ((hex1.x < hex2.x && hex1.y > hex2.y) || (hex1.x == hex2.x && hex1.y < hex2.y))
            {
                //(-1, +1) or (0, -1), top left or bottom right
                yRotation = 60;
            }
        }
        else //if the start hex's z-coord is even
        {
            if((hex1.x > hex2.x && hex1.y > hex2.y) || (hex1.x == hex2.x && hex1.y < hex2.y))
            {
                //(+1, +1) or (0, -1), top right or bottom left
                yRotation = -60;
            }
            else if ((hex1.x == hex2.x && hex1.y > hex2.y) || (hex1.x > hex2.x && hex1.y < hex2.y))
            {
                //(0, +1) or (+1, -1), top left or bottom right
                yRotation = 60;
            }
        }

        return yRotation;
    }

    public void SearchFromSelectedUnitToSelectedHex()
    {
        if (gameManager.SelectedUnit == null)
        {
            Debug.Log("Select a unit to path from");
            return;
        }

        Vector2 startHex = gameManager.SelectedUnit.CurrentHexIndex;
        Vector2 targetHex = gameManager.SelectedHexIndex;

        if (targetHex == new Vector2(-1, -1))
        {
            Debug.Log("Select a destination tile to search for");
            return;
        }

        List<Vector2> path = ReconstructPathFromSearch(startHex, targetHex, Search(startHex, targetHex));

        // foreach(Vector2 hexIndex in path)
        // {
        //     hexes[hexIndex].GetComponent<Hex>().SetToSelectedMaterial();
        // }
    }

    public void HighlightTile(Vector2 hexIndex)
    {
        Hex hexToHighlight = TryGetHexFromIndex(hexIndex);
        if(hexToHighlight)
        {
            hexToHighlight.SetHexToHighlightMaterial();
        }
    }
    
    public Hex TryGetHexFromIndex(Vector2 hexIndex)
    {
        if(hexes.ContainsKey(hexIndex))
        {
            return hexes[hexIndex];
        }
        else
        {
            Debug.Log("Hex at index " + hexIndex + " is not in hexes dictionary");
            return null;
        }
    }

    public Vector2[,] GetNeighbors(Vector2 hexIndex)
    {
        Vector2[,] neighbors = {{}};
        //adds pre-defined offsets to find neighbor tiles
        //different based on whether zIndex is odd or even
        if (hexes.ContainsKey(hexIndex))
        {
            var ifIsZeroIsEvenX = (int)hexIndex.x & 1;
            var ifIsZeroIsEvenZ = (int)hexIndex.y & 1;

            Vector2[,] offsetVectors = neighborsOffsetEvenZ;;
            
            if (ifIsZeroIsEvenZ == 0) 
            {
                //z is even
                offsetVectors = neighborsOffsetEvenZ;
            }
            else
            {
                //z is odd
                offsetVectors = neighborsOffsetOddZ;
            }

            neighbors =  new Vector2[,] { 
                                            { hexIndex + offsetVectors[0,0]}, { hexIndex + offsetVectors[1,0] }, { hexIndex + offsetVectors[2,0] }, 
                                            { hexIndex + offsetVectors[3,0] }, { hexIndex + offsetVectors[4,0] }, { hexIndex + offsetVectors[5,0] } 
                                        };
   
        }
        else
        {
            //Debug.Log("Cannot retrieve neighbors for hex because hexIndex is invalid");
        }
        return neighbors;
    }

    public void GetAndHighlightSelectedTileNeighbors()
    {
        DeselectAllHexes();

        foreach(var neighborIndex in GetNeighbors(gameManager.SelectedHexIndex))
        {
            HighlightTile(neighborIndex);
        }
    }

    private void SetUpTile(GameObject go, int x, int z)
    {
        go.transform.parent = transform;
        go.name= x.ToString() + ", " + z.ToString();

        if (go.GetComponent<Hex>())
        {
            Hex hex = go.GetComponent<Hex>();
            hex.xIndex = x;
            hex.zIndex = z;
            hex.tileManager = this;
            hexes.Add(new Vector2(x, z), hex);
            // hex.SetToDefaultMaterial();
        }
        else 
        {
            Debug.Log("Could not retrieve Hex component from instantiated prefab");
        }   
    } 

    public void DeselectAllHexes()
    {
        foreach(Hex hex in hexes.Values) 
        {
            hex.SetHexToDefaultMaterial();
        }
    } 

    public Vector3 GetUnitWorldPosFromHexIndex(Vector2 hexIndex)
    {
        if (hexes.ContainsKey(hexIndex))
        {
            //Debug.Log("adding height offset of: " + unitHeightOffset + " to pos of : " + hexes[hexIndex].transform.position);
            return hexes[hexIndex].transform.position + unitHeightOffset;
        }
        else
        {
            Debug.Log("Hex index not found in dictionary");
        }
        return new Vector3(-1,-1,-1);
    }

    public List<Vector2> PreviewPathFromUnitToDestination(Unit unit, Vector2 targetHex)
    {
        List<Vector2> pathFromSearch = ReconstructPathFromSearch(unit.CurrentHexIndex, targetHex, Search(unit.CurrentHexIndex, targetHex));

        return pathFromSearch;
    }

}
