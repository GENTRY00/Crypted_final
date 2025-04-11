using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayVictory : MonoBehaviour
{
    public GameObject victoryPanel;  // Reference to the victory UI panel

    // Start is called before the first frame update
    void Start()
    {
        if (victoryPanel != null)
        {
            // Initially hide the victory panel
            victoryPanel.SetActive(false);
        }
    }

    // OnTriggerEnter is called when another collider enters the trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the collider is the player
        {
            // Show the victory panel when the player enters the trigger
            ShowVictory();
        }
    }

    // Method to display the victory panel
    public void ShowVictory()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
    }
}
