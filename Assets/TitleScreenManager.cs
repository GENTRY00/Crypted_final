using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public GameObject controlsPanel;
    public GameObject creditsPanel;


    void Start()
    {
        // Ensure only the title screen is visible when the game starts
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }


        // Method to start the game
        public void PlayGame()
    {
        // Load the main game scene (replace "MainGame" with your scene name)
        SceneManager.LoadScene("enterGame");
    }

    public void ShowTitle()
    {
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    // Method to show controls
    public void ShowControls()
    {
        controlsPanel.SetActive(true);
    }

    // Method to show credits
    public void ShowCredits()
    {
        creditsPanel.SetActive(true);

    }

    // Method to exit the game
    public void ExitGame()
    {
        // Quit the application
        Debug.Log("Game is exiting..."); // Logs for testing in the editor
        Application.Quit();
    }
}
