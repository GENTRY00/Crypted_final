using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockStompActivator : MonoBehaviour
{
    public GameObject bigfoot; // Assign Bigfoot in the Inspector

    private string previousTag;

    private void Start()
    {
        // Save the initial tag of the rock
        previousTag = gameObject.tag;
    }

    private void Update()
    {
        // Check if the tag has changed to "Snapped"
        if (previousTag != gameObject.tag && gameObject.tag == "Snapped")
        {
            Debug.Log("Rock tag changed to Snapped. Enabling stomp on Bigfoot...");
            EnableStomp();
        }

        // Update the previous tag to the current tag
        previousTag = gameObject.tag;
    }

    private void EnableStomp()
    {
        if (bigfoot != null)
        {
            var stompController = bigfoot.GetComponent<StompController>();
            if (stompController != null)
            {
                stompController.enabled = true; // Enable the StompController
                Debug.Log("Stomp ability enabled on Bigfoot!");
            }
            else
            {
                Debug.LogError("StompController script is missing from Bigfoot!");
            }
        }
        else
        {
            Debug.LogError("Bigfoot reference is missing!");
        }
    }
}