using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float lastXPosition;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastXPosition = transform.position.x;
    }

    private void Update()
    {
        float currentXPosition = transform.position.x;
        float directionX = currentXPosition - lastXPosition;

        if (Input.GetKeyDown(KeyCode.D) && KeySwapManager.Instance.currentInputs.Contains(KeyCode.D))
        {
            spriteRenderer.flipX = false;
        }
        if (Input.GetKeyDown(KeyCode.A) && KeySwapManager.Instance.currentInputs.Contains(KeyCode.A))
        {
            spriteRenderer.flipX = true;
        }

        //lastXPosition = currentXPosition;
    }
}
