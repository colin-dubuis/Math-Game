using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement speed of the player
    public float moveSpeed;

    public LayerMask solidObjectLayer;
    public LayerMask grassLayer;

    // Boolean that tracks whether the player is currently moving or not
    private bool isMoving;

    // Vector2 that stores the input from the player
    private Vector2 input;

    private Animator animator;

    public GameObject mainCamera;
    public GameObject battleCamera;

    private void Awake()
    {

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if the player is not currently moving
        if (!isMoving)
        {
            // Get input from the player
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // Remove the diagonal movement
            if(input.x != 0) input.y =0;

            // Check if the player input is not zero
            if (input != Vector2.zero)
            {
                animator.SetFloat("MoveX", input.x);
                animator.SetFloat("MoveY", input.y);

                // Calculate the target position of the player
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if(Iwalkable(targetPos))
                {
                     // Start the Move coroutine to move the player to the target position
                StartCoroutine(Move(targetPos));
                }

               
            }


        }

        animator.SetBool("IsMoving", isMoving);

    }

    // Coroutine that moves the player to the target position
    private IEnumerator Move(Vector3 targetPos)
    {
        // Set isMoving to true to prevent the player from moving again while moving
        isMoving = true;

        // Move the player while the distance to the target position is greater than epsilon
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            // Move the player towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            // Wait for the next frame
            yield return null;
        }

        // Set the player's position to the target position
        transform.position = targetPos;

        // Set isMoving to false to allow the player to move again
        isMoving = false;

        CheckForEncounters();
    }
        private bool Iwalkable(Vector3 targetPos)
        {
          if(  Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectLayer ) != null)
          {
            return false;
          }
          else
          {
            return true;
          }
        }

        private void CheckForEncounters()
        {
            if(Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
            {
                if(Random.Range(1, 101) <= 10)
                {
                    mainCamera.SetActive(false);
                    battleCamera.SetActive(true);
                }
            }
        }

}

