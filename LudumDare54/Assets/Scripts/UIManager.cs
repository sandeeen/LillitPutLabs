using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] public List<GameObject> panels;
    [SerializeField] TextMeshProUGUI pressKeyYouWantToSwapText;

    [SerializeField] Sprite wSprite;
    [SerializeField] Sprite aSprite;
    [SerializeField] Sprite dSprite;
    [SerializeField] Sprite cSprite;

    Sprite spriteToChangeTo;

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
    }

    public void HideSwapText()
    {
        pressKeyYouWantToSwapText.enabled = false;
    }

    public void UpdateCurrentPickUpText(string text)
    {
        pressKeyYouWantToSwapText.text = "PRESS KEY YOU WANT TO SWAP FOR '" + text +"'";
    }

    public void UpdateCurrentInputUI(int panelIndex, KeyCode input)
    {
        panels[panelIndex].GetComponentInChildren<TextMeshProUGUI>().text = input.ToString();

        if (input.ToString() == "W")
        {
            spriteToChangeTo = wSprite;
            Debug.Log("IMAGE SWITCH TO W");
        }

        if (input.ToString() == "D")
        {
            spriteToChangeTo = dSprite;
            Debug.Log("IMAGE SWITCH TO W");
        }

        if (input.ToString() == "A")
        {
            spriteToChangeTo = aSprite;
            Debug.Log("IMAGE SWITCH TO W");
        }

        if (input.ToString() == "C")
        {
            spriteToChangeTo = cSprite;
            Debug.Log("IMAGE SWITCH TO W");
        }

        panels[panelIndex].GetComponentInChildren<Image>().sprite = spriteToChangeTo;
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

                    if (newInput.ToString() == "W")
                    {
                        spriteToChangeTo = wSprite;
                        Debug.Log("IMAGE SWITCH TO W");
                    }

                    if (newInput.ToString() == "D")
                    {
                        spriteToChangeTo = dSprite;
                        Debug.Log("IMAGE SWITCH TO W");
                    }

                    if (newInput.ToString() == "A")
                    {
                        spriteToChangeTo = aSprite;
                        Debug.Log("IMAGE SWITCH TO W");
                    }

                    if (newInput.ToString() == "C")
                    {
                        spriteToChangeTo = cSprite;
                        Debug.Log("IMAGE SWITCH TO W");
                    }

                    panel.GetComponentInChildren<Image>().sprite = spriteToChangeTo;

                    return;
                }
            }
        }

    }


}
