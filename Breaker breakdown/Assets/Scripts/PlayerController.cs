using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float interactionRange = 2f; // Range within which the player can interact with appliances

    private void Update()
    {
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
                    appliance.ToggleState();
                }
            }
        }
    }
}