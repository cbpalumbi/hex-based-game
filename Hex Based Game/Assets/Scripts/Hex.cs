using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour, ISelectable, IHoverable
{
    public int xIndex;
    public int zIndex;
    public Material defaultMat;
    public Material hoverMat;
    private Material selectedMat;
    public Material highlightMat;
    private MeshRenderer meshRenderer;
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

    void Start() {
        meshRenderer = GetComponent<MeshRenderer>();
        selectedMat = hoverMat;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    public void OnMouseEnter() 
    {
        // Debug.Log("Mouse entered hex " + (xIndex, zIndex));
        if(meshRenderer != null) {
            meshRenderer.material = hoverMat;
        }
        uiManager.UpdateHoverText(xIndex, zIndex);
    }

    public void OnMouseExit() 
    {
        // Debug.Log("Mouse exited hex " + (xIndex, zIndex));
        if (gameManager.SelectedHexIndex == new Vector2(xIndex, zIndex))
        {
            //if the hex is selected, don't change color back to default on mouse exit
            return;
        }
        else 
        {
            SetToDefaultMaterial();
        }
        
    }

    public void OnMouseDown() 
    {
        if (gameManager.SelectedHexIndex == new Vector2(xIndex, zIndex))
        {
            gameManager.SelectedHexIndex = new Vector2(-1, -1);
            SetToDefaultMaterial();
            return;
        }
        else
        {
            tileManager.DeselectAllHexes();
            gameManager.SelectedHexIndex = new Vector2(xIndex, zIndex);
            if(meshRenderer != null)
            {
                meshRenderer.material = selectedMat;
            }
        }
    }

    public void SetToDefaultMaterial()
    {
        if (meshRenderer != null)
        {
            meshRenderer.material = defaultMat;
        }
    }

    public void SetToSelectedMaterial()
    {
        if (meshRenderer != null)
        {
            meshRenderer.material = selectedMat;
        }
    }

    public void SetToHighlightMaterial()
    {
        if (meshRenderer != null)
        {
            meshRenderer.material = highlightMat;
        }
    }
}
