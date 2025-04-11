using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFLevelAltar : MonoBehaviour
{
    public GameObject bigfoot; // Assign Bigfoot in the Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Object entered altar trigger: {other.name}, Tag: {other.tag}");

        // Check if the object is tagged "Snapped"
        if (other.CompareTag("Snapped"))
        {
            Debug.Log("Snapped rock detected. Enabling stomp...");
            EnableStomp();
        }
        else
        {
            Debug.Log("Entered object is not a snapped rock.");
        }
    }

    private void EnableStomp()
    {
        if (bigfoot != null)
        {
            var stompController = bigfoot.GetComponent<StompController>();
            if (stompController != null)
            {
                stompController.EnableStomp(); // Enable the stomp ability on Bigfoot
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