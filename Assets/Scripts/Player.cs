using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 7f; // The speed of the player
    [SerializeField] private float rotationSpeed = 10f; // The speed of the player

    private bool isWalking;

    private void Update()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
       
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = movementSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;

        // Check if hit anything with a capsule cast
        bool canMove = !Physics.CapsuleCast (transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance);

        if (!canMove)
        {
            // Wall hugging functionality
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0).normalized;
            Vector3 moveDirectionZ = new Vector3(0, 0, moveDirection.z).normalized;

            // Attempt to only X movement
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionX, moveDistance);
            if (canMove)
            {
                moveDirection = moveDirectionX;
            } else
            {
                // Cannot move on X lets try in Z
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionZ, moveDistance);
                if (canMove)
                {
                    moveDirection = moveDirectionZ;
                }
            }
        }

        if (canMove)
        {
            // Move player
            transform.position += moveDirection * movementSpeed * Time.deltaTime;
        }

        // Check if is walking to switch the animation
        isWalking = moveDirection != Vector3.zero;

        
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);

    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
