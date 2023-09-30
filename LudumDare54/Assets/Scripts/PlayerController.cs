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
    public float raycastDistance = 1.5f; 
    public LayerMask groundLayer;
    [SerializeField] bool canJump = true;
    [SerializeField] float jumpCoolDown = 1f;

    [SerializeField] private float coyoteTimer = 0.2f; 
    [SerializeField] private float coyoteTimeRemaining = 0f;

    float horizontalInput;
    Vector2 movement;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        isGrounded = IsGrounded();

        if (!isGrounded && coyoteTimeRemaining > 0)
        {
            coyoteTimeRemaining -= Time.deltaTime;
        }

        if (isGrounded)
        {
            coyoteTimeRemaining = coyoteTimer;
        }



        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded) && !swapMode && KeySwapManager.Instance.currentInputs.Contains(KeyCode.Space) && canJump)
        {
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
    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, groundLayer);

        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, Vector2.down * raycastDistance, Color.green); // Visualize the ray in the Unity editor.
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, Vector2.down * raycastDistance, Color.red); // Visualize the ray in the Unity editor.
            return false;
        }
    } 

    private void Crouch()
    {
        isCrouching = !isCrouching;

        int count = KeySwapManager.Instance.CountKeycodeOccurrences(KeySwapManager.Instance.currentInputs, KeyCode.C);
        float crouchAmount = 0.35f / count;

        if (isCrouching)
        {
            transform.localScale = new Vector3(0.5f, crouchAmount, 0.5f);

        }

        if (!isCrouching)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        }

    }

    private void Jump()
    {

        if (canJump && (isGrounded || coyoteTimeRemaining > 0))
        {
            canJump = false;
            int count = KeySwapManager.Instance.CountKeycodeOccurrences(KeySwapManager.Instance.currentInputs, KeyCode.Space);
            rigidbody.velocity += new Vector2(0, jumpForce + (count * 2));
            StartCoroutine(JumpCoolDown());
        }
    }

    private IEnumerator JumpCoolDown()
    {
        yield return new WaitForSeconds(jumpCoolDown);
        canJump = true;
        yield return null;
    }

    private void RestartLevel()
    {
        SceneChanger.Instance.RestartCurrentLevel();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (!collision.gameObject.CompareTag("NoGrounded"))
        //{
        //    isGrounded = true;

        //}

        if (collision.gameObject.CompareTag("Pickup"))
        {
            isInPickupRange = true;
            currentPickUpObject = collision.gameObject.GetComponent<PickUp>();
            currentPickupInput = currentPickUpObject.pickUpInput;

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //isGrounded = false;

        if (collision.gameObject.CompareTag("Pickup"))
        {
            isInPickupRange = false;
            //isGrounded = true;
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
