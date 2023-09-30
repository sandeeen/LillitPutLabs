using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySwapManager : MonoBehaviour
{
    public static KeySwapManager Instance;

    [SerializeField] List<KeyCode> allAvaiableInputs;
    [SerializeField] List<KeyCode> startingInputsAvailable;
    [SerializeField] public List<KeyCode> currentInputs;

    PlayerController playerController;
    [SerializeField] bool playerDecidesInputs;
    [SerializeField] int inputsLeftToDecide;
    [SerializeField] KeyCode nextKeyToAddToStartingInput;

    [SerializeField] List<string> availableMovementOptions;
    [SerializeField] int numberOfInputSlots;

    public bool isWaitingForKeyPress = false;
    public KeyCode nextPressedKey = KeyCode.None;

    int indexToAddUiTextTo = 0;

    [SerializeField] KeyCode inputToSwap;


    public bool isWaitingForKeyToRemove = false;

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

    void Start()
    {

        //copy all available inputs to new list
        foreach (var item in allAvaiableInputs)
        {
            startingInputsAvailable.Add(item);
        }

        numberOfInputSlots = UIManager.Instance.panels.Count;
        inputsLeftToDecide = numberOfInputSlots;
        availableMovementOptions = null;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

    }

    void Update()
    {
        if (isWaitingForKeyPress || inputsLeftToDecide > 0)
        {
            NextKeyFunction();
        }

        if (isWaitingForKeyToRemove)
        {
            GetKeyToSwap();
        }

    }

    private void NextKeyFunction()
    {
        foreach (KeyCode keyCode in allAvaiableInputs)
        {
            if (Input.GetKeyDown(keyCode))
            {
                nextPressedKey = keyCode;
                isWaitingForKeyPress = false;

                if (inputsLeftToDecide > 0)
                {
                    inputsLeftToDecide--;
                    nextKeyToAddToStartingInput = keyCode;
                    AddInput(keyCode);
                    StartUI(keyCode);
                }
                break;
            }
        }
    }

    public void GetKeyToSwap()
    {
        foreach (KeyCode keyCode in currentInputs)
        {
            if (Input.GetKeyDown(keyCode))
            {

                inputToSwap = keyCode;
                isWaitingForKeyToRemove = false;
                playerController.InputToSwapDecided(inputToSwap);
                playerController.swapMode = false;

            }
        }
    }

    public void WaitForNextKeyPress()
    {
        isWaitingForKeyPress = true;
    }

    public KeyCode GetNextPressedKey()
    {
        return nextPressedKey;
    }

    //add input to list of usable inputs that PlayerController checks to see if player can use certain inpuc
    public void AddInput(KeyCode keycodeToAddToCurrent)
    {
        if (currentInputs.Count <= numberOfInputSlots)
        {
            currentInputs.Add(keycodeToAddToCurrent);

        }
        else
        {
            Debug.Log("No more Movement slots are available");
        }
    }

    public void SwapInput(KeyCode inputToRemove, KeyCode inputToAdd)
    {
        currentInputs.Remove(inputToRemove);
        currentInputs.Add(inputToAdd);

        UIManager.Instance.SwapCurrentInputUI(inputToRemove, inputToAdd);

    }

    //start UI to show current inputs in the beginning when player choses inputs
    private void StartUI(KeyCode keycode)
    {
        UIManager.Instance.UpdateCurrentInputUI(indexToAddUiTextTo, keycode);
        indexToAddUiTextTo++;
    }

    public int CountKeycodeOccurrences(List<KeyCode> keycodes, KeyCode targetKeycode)
    {
        int count = 0;

        foreach (KeyCode keycode in keycodes)
        {
            if (keycode == targetKeycode)
            {
                count++;
            }
        }

        return count;
    }
}
