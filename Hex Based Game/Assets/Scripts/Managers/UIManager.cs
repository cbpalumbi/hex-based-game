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
    public GameObject stat1Panel;
    public TextMeshProUGUI stat1Text;
    public GameObject stat2Panel;
    public TextMeshProUGUI stat2Text;
    public GameObject stat3Panel;
    public TextMeshProUGUI stat3Text;
    public GameObject stat4Panel;
    public TextMeshProUGUI stat4Text;
    public TextMeshProUGUI turnTaskText;
    private List<GameObject> statPanels;
    private List<TextMeshProUGUI> statTexts;
    private GameManagerScript gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        statPanels = new List<GameObject>();
        statPanels.Add(stat1Panel);
        statPanels.Add(stat2Panel);
        statPanels.Add(stat3Panel);
        statPanels.Add(stat4Panel);
        statTexts = new List<TextMeshProUGUI>();
        statTexts.Add(stat1Text);
        statTexts.Add(stat2Text);
        statTexts.Add(stat3Text);
        statTexts.Add(stat4Text);
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
        ClearStatText();
        HideStatPanels();

        if(gameManager.SelectedUnit == null)
        {
            return;
        }

        UnitData selectedUnitData = gameManager.SelectedUnit.unitData;

        selectedInfoName.text = selectedUnitData.unitName;

        for(int i = 0; i < selectedUnitData.playerFacingStats.Count; i++)
        {
            statPanels[i].SetActive(true);

            Stat stat = selectedUnitData.playerFacingStats[i];
            statTexts[i].text = stat.statName + " : " + stat.statValue;
        }
    }

    private void HideStatPanels() 
    {
        foreach(GameObject panel in statPanels)
        {
            panel.SetActive(false);
        }
    }

    private void ClearStatText()
    {
        foreach(TextMeshProUGUI textField in statTexts)
        {
            textField.text = "";
        }
    }

}
