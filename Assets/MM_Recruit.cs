using UnityEngine;

public class Recruit : MonoBehaviour
{
    private Companion_Follow companionFollow;
    private Collider2D recruitCollider;  // Reference to the collider
    private SwitchController switchController;  // Reference to SwitchController
    private UIController uiController; // Reference to the UIController

    void Start()
    {
        companionFollow = GetComponent<Companion_Follow>();  // Cache the Companion_Follow component
        switchController = FindObjectOfType<SwitchController>();  // Find the SwitchController in the scene
        uiController = FindObjectOfType<UIController>(); // Find the UIController in the scene


        // Find the specific trigger collider
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (var collider in colliders)
        {
            if (collider.isTrigger)  // Check if the collider is a trigger
            {
                recruitCollider = collider;
                break;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = other.gameObject;

            // Get the necessary components from the recruit and the player
            PlayerController recruitPlayerController = GetComponent<PlayerController>();
            Companion_Follow playerCompanionFollow = player.GetComponent<Companion_Follow>();
            PlayerController playerController = player.GetComponent<PlayerController>();

            // Ensure all necessary components are found
            if (companionFollow != null && recruitPlayerController != null && playerCompanionFollow != null && playerController != null && switchController != null)
            {
                // Add the recruit to the SwitchController's players array
                bool addedToPlayers = false;
                if (switchController.players[1] == null)
                {
                    switchController.players[1] = this.gameObject;
                    addedToPlayers = true;
                    if (uiController != null)
                    {
                        uiController.ShowCharacterHead(1);
                    }
                }
                else if (switchController.players[2] == null)
                {
                    switchController.players[2] = this.gameObject;
                    addedToPlayers = true;
                    if (uiController != null)
                    {
                        uiController.ShowCharacterHead(2);
                    }
                }

                if (!addedToPlayers)
                {
                    Debug.LogWarning("No available slot to add the recruit in SwitchController.");
                    return;
                }

                // Set the recruited companion to follow the player
                companionFollow.SetTarget(player);  // Set the player as the target for the companion to follow
                companionFollow.enabled = true;

                // Update who follows whom if needed
                // (Optional logic depending on how you want companions to interact)

                // Disable the recruitment trigger to prevent multiple recruitments
                if (recruitCollider != null)
                    recruitCollider.enabled = false;

                

                this.enabled = false;
            }
            else
            {
                Debug.LogWarning("Missing required components on player or recruit!");
            }
        }
    }
}
