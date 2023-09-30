using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rigidbody;

    [SerializeField] float jumpForce = 5f;
    [SerializeField] bool isGrounded = true;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float maxMoveSpeed;
    [SerializeField] bool isInPickupRange;
    [SerializeField] bool hasPressedNextKey = false;

    [SerializeField] string inputPressed;

    [SerializeField] KeyCode inputThatWillBeRemoved;

    [SerializeField] KeyCode currentPickupInput;
    [SerializeField] PickUp currentPickUpObject;

    [Header("Movement Bools")]
    [SerializeField] public bool swapMode = false;
    [SerializeField] bool isCrouching = false;


    float horizontalInput;
    Vector2 movement;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !swapMode && KeySwapManager.Instance.currentInputs.Contains(KeyCode.Space))
        {
            //TODO : Cayote timer or that thing when you jump before you land you jump again
            Jump();
        }

        if (!swapMode)
        {
            Move();
        }

        if (Input.GetKeyDown(KeyCode.E) && isInPickupRange && !swapMode)
        {
            StartSwap();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }

        if (Input.GetKeyDown(KeyCode.C) && !swapMode && KeySwapManager.Instance.currentInputs.Contains(KeyCode.C))
        {
            Crouch();
        }
       


    }
    private void Crouch()
    {
        isCrouching = !isCrouching;

        int count = KeySwapManager.Instance.CountKeycodeOccurrences(KeySwapManager.Instance.currentInputs, KeyCode.C);
        float crouchAmount = 0.7f / count;

        if (isCrouching)
        {
            transform.localScale = new Vector3(1, crouchAmount, 1);

        }

        if (!isCrouching)
        {
            transform.localScale = new Vector3(1, 1, 1);

        }

    }

    private void Jump()
    {
        int count = KeySwapManager.Instance.CountKeycodeOccurrences(KeySwapManager.Instance.currentInputs, KeyCode.Space);
        rigidbody.velocity += new Vector2(0, jumpForce + (count * 2));
    }

    private void RestartLevel()
    {
        SceneChanger.Instance.RestartCurrentLevel();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("NoGrounded"))
        {
            isGrounded = true;

        }

        if (collision.gameObject.CompareTag("Pickup"))
        {
            isInPickupRange = true;
            currentPickUpObject = collision.gameObject.GetComponent<PickUp>();
            currentPickupInput = currentPickUpObject.pickUpInput;

        }


    }

    

    private void OnTriggerExit2D(Collider2D collision)
    {
        isGrounded = false;

        if (collision.gameObject.CompareTag("Pickup"))
        {
            isInPickupRange = false;
            isGrounded = true;
            currentPickupInput = 0;
            currentPickUpObject = null;
        }
    }

  

    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        movement = new Vector2(horizontalInput * moveSpeed, rigidbody.velocity.y);

        rigidbody.velocity = movement;

        int countLeft = KeySwapManager.Instance.CountKeycodeOccurrences(KeySwapManager.Instance.currentInputs, KeyCode.A);
        int countRight = KeySwapManager.Instance.CountKeycodeOccurrences(KeySwapManager.Instance.currentInputs, KeyCode.D);

        float desiredXVelocity = horizontalInput * (moveSpeed + countLeft + countRight);

        // Check if the player can move in the desired direction
        if ((desiredXVelocity < 0) && KeySwapManager.Instance.currentInputs.Contains(KeyCode.A) || (desiredXVelocity > 0) && KeySwapManager.Instance.currentInputs.Contains(KeyCode.D))
        {
            // Apply the velocity to the Rigidbody2D
            rigidbody.velocity = new Vector2(desiredXVelocity, rigidbody.velocity.y);
        }
        else
        {
            // Stop the player if they shouldn't move in this direction
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        }

    }

    public void GetNextKeyPressed()
    {

        KeySwapManager.Instance.WaitForNextKeyPress();
        inputThatWillBeRemoved = KeySwapManager.Instance.GetNextPressedKey();
    }

    private void StartSwap()
    {
        KeySwapManager.Instance.isWaitingForKeyToRemove = true;
        swapMode = true;
        UIManager.Instance.ShowSwapText();
    }

    public void FinishSwap(KeyCode inputToRemove)
    {
        KeySwapManager.Instance.SwapInput(inputToRemove, currentPickupInput);
        currentPickUpObject.pickUpInput = inputToRemove;
        UIManager.Instance.HideSwapText();
    }

    public void InputToSwapDecided(KeyCode inputToRemove)
    {
        inputThatWillBeRemoved = inputToRemove;
        FinishSwap(inputToRemove);
    }




}
