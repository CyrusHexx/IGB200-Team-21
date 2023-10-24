using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float interactionRange = 5.0f; // Range within which the player can interact with appliances
    public float moveSpeed = 20.0f; // Speed at which the player moves
    public float rotationSpeed = 700.0f; // Speed at which the player rotates
    private CharacterController controller; // Reference to the Character Controller component
    public GameObject Camera; // Reference to the Scene camera
    public Animator animator;
    private float playerPosX;
    private float playerPosY;
    private float playerPosZ;

    public bool playerCaught = false;

    public float gravityStr = 9.8f;
    private float gravity = 0;

    public GameObject firstFloor;
    public GameObject secondFloor;
    public GameObject basement;
    public GameObject fuseBoxGamePanel;
    public GameObject finishNodeObject;


    public GameObject firstFloorApps;
    public GameObject secondFloorApps;

    public GameObject GhostOne;
    public GameObject GhostTwo;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        secondFloor.SetActive(false);
        foreach (Renderer renderer in secondFloorApps.GetComponentsInChildren(typeof(Renderer)))
        {
            renderer.enabled = false;
        }

        fuseBoxGamePanel.SetActive(false);

    }

    private void Update()
    {
        MovePlayer(); // Handles player movement
        UpdateCamera(); // Updates Camera movement to follow player

        // Check for the "E" key press
        if (Input.GetKeyDown(KeyCode.E))
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
                if (powerfusebox != null)
                {
                    // If player interacts with the fuse box and the game panel is not active
                    if (!fuseBoxGamePanel.activeSelf)
                    {
                        // Turn off ghosts and activate fuse box game
                        GhostOne.SetActive(false);
                        GhostTwo.SetActive(false);
                        fuseBoxGamePanel.SetActive(true);
                    }
                }
            }
        }

        checkFloor();
        checkCaught();
    }


    private void checkFloor()
    {
        float playerHeight = this.gameObject.transform.position.y;

        if (playerHeight < 2.5f)
        {
            firstFloor.SetActive(false);

            foreach (Renderer renderer in firstFloorApps.GetComponentsInChildren(typeof(Renderer)))
            {
                renderer.enabled = false;
            }
        }
        if (playerHeight > 2.5f && playerHeight < 9)
        {
            secondFloor.SetActive(false);
            foreach (Renderer renderer in secondFloorApps.GetComponentsInChildren(typeof(Renderer)))
            {
                renderer.enabled = false;
            }

            firstFloor.SetActive(true);
            foreach (Renderer renderer in firstFloorApps.GetComponentsInChildren(typeof(Renderer)))
            {
                renderer.enabled = true;
            }
        }
        if (playerHeight > 9)
        {
            secondFloor.SetActive(true);
            foreach (Renderer renderer in secondFloorApps.GetComponentsInChildren(typeof(Renderer)))
            {
                renderer.enabled = true;
            }
        }
    }
    
    
    private void checkCaught()
    {
        if (playerCaught == true)
        {
            SceneManager.LoadScene("Loss");
        }
    }
    
    private void MovePlayer()
    {
        //Gravity System
        if (controller.isGrounded == true)
        {
            gravity = 0;
        }
        if (controller.isGrounded == false)
        {
            gravity -= gravityStr * Time.deltaTime;
        }

        // Create a movement direction based on the input
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), gravity, Input.GetAxis("Vertical")).normalized;
        // Move the player using the Character Controller component
        controller.Move(moveDirection * Time.deltaTime * moveSpeed);
        animator.SetFloat("Walking", controller.velocity.magnitude); // NATHAN was here
        // Rotate the player to face the movement direction
        if (moveDirection.x != 0 || moveDirection.z != 0)
        {
            var rotateDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
            
            Quaternion toRotation = Quaternion.LookRotation(rotateDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void UpdateCamera()
    {
        // Get player's current position in the game world
        playerPosX = GameObject.Find("Player").transform.position.x;
        playerPosZ = GameObject.Find("Player").transform.position.z;
        playerPosY = GameObject.Find("Player").transform.position.y;

        // Update the camera to be at the same positon as the player
        Camera.transform.position = new Vector3(playerPosX, playerPosY + 60, playerPosZ - 3);
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
