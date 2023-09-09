using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float interactionRange = 5.0f; // Range within which the player can interact with appliances
    public float moveSpeed = 20.0f; // Speed at which the player moves
    public float rotationSpeed = 700.0f; // Speed at which the player rotates
    private CharacterController controller; // Reference to the Character Controller component
    public GameObject Camera; // Reference to the Scene camera
    private float playerPosX;
    private float playerPosZ;
    public bool playerCaught = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MovePlayer(); // Handles player movement
        UpdateCamera(); // Updates Camera movement to follow player

        // Check for the "K" key press
        if (Input.GetKeyDown(KeyCode.K))
        {
            
            // Cast a ray to detect nearby appliances
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRange);
            foreach (Collider hitCollider in hitColliders)
            {
                Appliance appliance = hitCollider.GetComponent<Appliance>();
                if (appliance != null && appliance.IsOn())
                {
                    // Toggle the state of the appliance if it's on
                    appliance.ToggleState(false);
                }

                PowerFuseBox powerfusebox = hitCollider.GetComponent<PowerFuseBox>();
                if (powerfusebox != null && powerfusebox.IsOn())
                {
                    powerfusebox.powerRestart();
                    GameManager.instance.resetLoad();
                }
            }
        }
        checkCaught();
    }

    private void checkCaught()
    {
        if (playerCaught == true)
        {
            Destroy(this.gameObject);
        }
    }
    
    private void MovePlayer()
    {
        // Get the horizontal and vertical input (WASD keys)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Create a movement direction based on the input
        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);
        moveDirection.Normalize();

        // Move the player using the Character Controller component
        controller.SimpleMove(moveDirection * moveSpeed);

        // Rotate the player to face the movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void UpdateCamera()
    {
        // Get player's current position in the game world
        playerPosX = GameObject.Find("Player").transform.position.x;
        playerPosZ = GameObject.Find("Player").transform.position.z;

        // Update the camera to be at the same positon as the player
        Camera.transform.position = new Vector3(playerPosX, 60, playerPosZ - 3);
    }

    private void OnTriggerEnter(Collider other)
    {
        bool overloadCheck = GameManager.instance.overload;
        if (other.transform.tag == "Ghost" && overloadCheck == true)
        {
            playerCaught = true;
        }
    }
}
