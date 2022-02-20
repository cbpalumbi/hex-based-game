using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour, ISelectable, IHoverable
{
    public int xIndex;
    public int zIndex;
    public Material defaultMat;
    public Material hoverMat;
    public Material invalidOutlineMat;
    public Material defaultOutlineMat;
    public Material selectedMat;
    public Material highlightMat;
    private MeshRenderer meshRenderer;
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
        meshRenderer = GetComponent<MeshRenderer>();
        
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        outlineTransform = transform.GetChild(0);
        outlineMeshRenderer = outlineTransform.gameObject.GetComponent<MeshRenderer>();
        SetToDefaultMaterial();
    }

    public void OnMouseEnter() 
    {
        // if(meshRenderer != null) {
        //     meshRenderer.material = hoverMat;
        // }
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
        { //if clicked already selected tile, deselect
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
                SetToSelectedMaterial();
            }
        }
    }

    public void SetToDefaultMaterial()
    {
        if (meshRenderer != null)
        {
            outlineMeshRenderer.material = defaultMat;
        }
    }

    //unused as far as i know
    public void SetToSelectedMaterial()
    {
        if (meshRenderer != null)
        {
            outlineMeshRenderer.material = selectedMat;
        }
    }

    public void SetToHighlightMaterial()
    {
        if (meshRenderer != null)
        {
            outlineMeshRenderer.material = highlightMat;
        }
    }

    public void TurnOnOutline()
    {
        outlineTransform.gameObject.SetActive(true);
    }

    public void TurnOffOutline()
    {
        outlineTransform.gameObject.SetActive(true);
        outlineMeshRenderer.material = defaultOutlineMat;
    }

    public void TurnOnInvalidOutline() {
        outlineTransform.gameObject.SetActive(true);
        outlineMeshRenderer.material = invalidOutlineMat;
    }
}
