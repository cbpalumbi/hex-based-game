using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI hoverHexText;
    public TextMeshProUGUI selectedHexText;
    public GameObject selectedInfoPanel;
    public TextMeshProUGUI selectedInfoName;
    private GameManagerScript gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
    }

    public void UpdateHoverText(float x, float z)
    {
        hoverHexText.text = "hover: " + x + ", " + z;
    }

    public void UpdateSelectedText(float x, float z)
    {
        selectedHexText.text = "selected: " + x + ", " + z;
    }

    public void TurnOnSelectedInfoPanel()
    {
        UpdateSelectedInfoPanel();
        selectedInfoPanel.SetActive(true);
    }

    public void TurnOffSelectedInfoPanel()
    {
        selectedInfoPanel.SetActive(false);
    }

    public void UpdateSelectedInfoPanel()
    {
        selectedInfoName.text = gameManager.SelectedUnit.gameObject.name;
    }

}
