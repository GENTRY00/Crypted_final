using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel;  // Assign the Panel from the Canvas here
    public GameObject controlsPanel; // Assign the Controls Panel from the Canvas here
    public GameObject warningPanel;

    public GameObject failCanvas;
    private bool isMenuActive = false;
    public bool isFail;

    void Start()
    {
        // Make sure the menu and controls start hidden
        menuPanel.SetActive(false);
        controlsPanel.SetActive(false);
        warningPanel.SetActive(false);
        failCanvas.SetActive(false);
        isFail = false;
}

void Update()
    {
        // Toggle the menu when the Escape key is pressed
        if (!isFail && Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuActive = !isMenuActive;
            menuPanel.SetActive(isMenuActive);
            controlsPanel.SetActive(false);
            warningPanel.SetActive(false);
        }


        // Pause the game if menu is active
        Time.timeScale = isMenuActive ? 0 : 1;
        
    }

    public void PlayGame()
    {
        isMenuActive = false;
        menuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void ShowControls()
    {
        // Show the controls panel
        controlsPanel.SetActive(true);
    }

    public void HideControls()
    {
        // Hide the controls panel
        controlsPanel.SetActive(false);
    }

    public void ShowWarning()
    {
        // Show the controls panel
        warningPanel.SetActive(true);
    }

    public void HideWarning()
    {
        // Hide the controls panel
        warningPanel.SetActive(false);
    }

    public void ReturnTitle()
    {
        // reset everything
        Application.Quit();
    }

    public void ReturnTitleReal()
    {
        SceneManager.LoadScene("Title");
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}