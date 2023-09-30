using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] public List<GameObject> panels;
    [SerializeField] TextMeshProUGUI pickupText;
    [SerializeField] TextMeshProUGUI pressKeyYouWantToSwapText;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void ShowSwapText()
    {
        pressKeyYouWantToSwapText.enabled = true;
        pressKeyYouWantToSwapText.text = "PRESS KEY YOU WANT TO SWAP FOR '" + pickupText.text + "'";
    }

    public void HideSwapText()
    {
        pressKeyYouWantToSwapText.enabled = false;
    }

    public void UpdateCurrentPickUpText(string text)
    {
        pickupText.text = text;
        
    }

    public void UpdateCurrentInputUI(int panelIndex, KeyCode input)
    {
        panels[panelIndex].GetComponentInChildren<TextMeshProUGUI>().text = input.ToString();
    }

    public void SwapCurrentInputUI(KeyCode oldInput, KeyCode newInput)
    {
        foreach (GameObject panel in panels)
        {
            TextMeshProUGUI textComponent = panel.GetComponentInChildren<TextMeshProUGUI>();

            if (textComponent != null)
            {
                string textInPanel = textComponent.text;

                //compare string to keycode
                if (textInPanel.Equals(oldInput.ToString(), System.StringComparison.OrdinalIgnoreCase))
                {
                    //match found - swap to new match
                    textComponent.text = newInput.ToString();

                    return;
                }
            }
        }

    }


}
