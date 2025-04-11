using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public GameObject[] players = new GameObject[3]; // Array to hold the player GameObjects
    public int currentPlayerIndex = 0; // Track the current player
    private UIController uiController; // Reference to the UIController


    public Camera cam; // Reference to the camera

    public void SwitchPlayers()
    {
        uiController = FindObjectOfType<UIController>(); // Find the UIController in the scene

        // Early exit if no players are defined in the array
        if (players == null || players.Length == 0)
        {
            Debug.LogWarning("Players array is empty.");
            return;
        }

        // Get the current player
        GameObject currentPlayer = players[currentPlayerIndex];
        if (currentPlayer != null)
        {
            // Disable the current player's controller and enable its companion follow
            PlayerController currentPlayerController = currentPlayer.GetComponent<PlayerController>();
            Companion_Follow currentCompanionFollow = currentPlayer.GetComponent<Companion_Follow>();

            if (currentPlayerController != null)
                currentPlayerController.enabled = false; // Disable movement for the current player

            if (currentCompanionFollow != null)
                currentCompanionFollow.enabled = true; // Enable companion follow
        }

        int nextPlayerIndex;

        // Find the next player in the array
        if (currentPlayerIndex == 2 || players[currentPlayerIndex + 1] == null)
        {
            nextPlayerIndex = 0;
        } 
        else
        {
            nextPlayerIndex = currentPlayerIndex + 1;
        }

        //// Ensure the next player exists
        //while (players[nextPlayerIndex] == null)
        //{
        //    nextPlayerIndex = (nextPlayerIndex + 1) % players.Length;

        //    // If we've looped back to the original index, exit to prevent infinite loopfe
        //    if (nextPlayerIndex == currentPlayerIndex)
        //    {
        //        Debug.LogWarning("No other players to switch to.");
        //        return;
        //    }
        //}

        GameObject nextPlayer = players[nextPlayerIndex];
        Debug.Log(nextPlayerIndex);
        if (nextPlayer != null)
        {
            // Enable the next player's controller and disable its companion follow
            PlayerController nextPlayerController = nextPlayer.GetComponent<PlayerController>();
            Companion_Follow nextCompanionFollow = nextPlayer.GetComponent<Companion_Follow>();

            if (nextPlayerController != null)
                nextPlayerController.enabled = true; // Enable movement for the next player

            if (nextCompanionFollow != null)
            {
                nextCompanionFollow.enabled = false; // Disable companion follow
            }
        }

        // Update the camera to follow the new player
        if (cam != null)
        {
            CameraController cameraController = cam.GetComponent<CameraController>();
            if (cameraController != null && nextPlayer != null)
            {
                cameraController.SetTarget(nextPlayer.transform);
            }
            else
            {
                Debug.LogWarning("CameraController component not found on camera.");
            }
        }

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
            {
                if (i != nextPlayerIndex)
                {
                    Companion_Follow companionFollow = players[i].GetComponent<Companion_Follow>();
                    companionFollow.SetTarget(players[nextPlayerIndex]);
                }
            }
        }
        uiController.ShowSelectedCharacter(nextPlayerIndex);


        // Update the current player index to the next one
        currentPlayerIndex = nextPlayerIndex;

        Debug.Log("Switched to player: " + nextPlayer.name);
    }
}
