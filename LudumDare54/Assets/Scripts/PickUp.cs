using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] public KeyCode pickUpInput;

    private void Start()
    {
        InvokeRepeating("UpdateUI", 0.1f, 0.1f);
    }

    private void UpdateUI()
    {
        UIManager.Instance.UpdateCurrentPickUpText(pickUpInput.ToString());
    }

}
