using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour, ISelectable
{
    public int xIndex;
    public int zIndex;
    public Material defaultMat;
    public Material hoverMat;
    public Material invalidOutlineMat;
    public Material defaultOutlineMat;
    public Material selectedMat;
    public Material highlightMat;
    [HideInInspector]
    public MeshRenderer hexMeshRenderer;
    private MeshRenderer outlineMeshRenderer;
    private GameManagerScript gameManager;
    public HexTileManager tileManager;
    private UIManager uiManager;
    private Unit occupyingUnit;
    public Unit OccupyingUnit {
        get { return occupyingUnit; }
        set {
            occupyingUnit = value;
        }
    }
    
    private Transform outlineTransform;

    void Start() {
        
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        outlineTransform = transform.GetChild(0);
        outlineMeshRenderer = outlineTransform.gameObject.GetComponent<MeshRenderer>();
        //SetHexToDefaultMaterial();
    }

    public void OnMouseEnter() 
    {
        uiManager.UpdateHoverText(xIndex, zIndex);
    }

    public void OnMouseDown() 
    {
        // if (gameManager.SelectedHexIndex == new Vector2(xIndex, zIndex))
        // { //if clicked already selected tile, deselect
        //     gameManager.SelectedHexIndex = new Vector2(-1, -1);
        //     SetOutlineToDefaultMaterial();
        //     return;
        // }
        // else
        // {
        //     tileManager.DeselectAllHexes();
        //     gameManager.SelectedHexIndex = new Vector2(xIndex, zIndex);
        //     if(hexMeshRenderer != null)
        //     {
        //         SetOutlineToSelectedMaterial();
        //     }
        // }
    }

    public void SetHexToDefaultMaterial()
    {
        if (hexMeshRenderer != null)
        {
            hexMeshRenderer.material = defaultMat;
        }
    }

    public void SetOutlineToDefaultMaterial()
    {
        if (outlineMeshRenderer != null)
        {
            outlineMeshRenderer.material = defaultOutlineMat;
        }
    }

    public void SetHexToSelectedMaterial()
    {
        if (hexMeshRenderer != null)
        {
            hexMeshRenderer.material = selectedMat;
        }
    }

    public void SetOutlineToSelectedMaterial()
    {
        if (outlineMeshRenderer != null)
        {
            outlineMeshRenderer.material = selectedMat;
        }
    }

    public void SetHexToHighlightMaterial()
    {
        if (hexMeshRenderer != null)
        {
            hexMeshRenderer.material = highlightMat;
        }
    }

    public void SetOutlineToHighlightMaterial()
    {
        if (outlineMeshRenderer != null)
        {
            outlineMeshRenderer.material = highlightMat;
        }
    }

    public void SetHexToCollapseColor (int turnsToCollapse) {
        //colors list considers most solid color to be at index 0
        int index = tileManager.hexColors.Count - turnsToCollapse;
        if (index < 0) {
            index = 0;
        } else if (index > tileManager.hexColors.Count - 1) {
            index = tileManager.hexColors.Count - 1;
        }
        hexMeshRenderer.material = tileManager.hexColors[index];
    }

    public void TurnOffOutline()
    {
        //outlineTransform.gameObject.SetActive(true);
        outlineMeshRenderer.material = defaultOutlineMat;
    }

    public void TurnOnInvalidOutline() {
        //outlineTransform.gameObject.SetActive(true);
        outlineMeshRenderer.material = invalidOutlineMat;
    }
    
    public void HideHex() {
        outlineMeshRenderer.enabled = false;
        hexMeshRenderer.enabled = false;
    }
}
