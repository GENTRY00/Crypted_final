using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject characterSelectPanel;
    public GameObject bookPanel;
    public GameObject pagePanel;

    public GameObject mm_head;  // Reference to a button
    public GameObject mm_select;    // Reference to a text element
    public GameObject nessie_head;
    public GameObject nessie_select;
    public GameObject bf_select;


    void Start()
    {
        UpdateUIBasedOnScene();
        pagePanel.SetActive(false);
    }

    private void UpdateUIBasedOnScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        switch (currentScene)
        {
            case "enterGame":
                // Access the Transform of bookPanel
                Transform bookPanelTransform = bookPanel.transform;

                // Use Transform.Find() to find the "Book" child
                Transform bookIcon = bookPanelTransform.Find("Book");

                if (bookIcon != null)
                {
                    // Set the "Book" child inactive
                    bookIcon.gameObject.SetActive(false);
                }
                break;
            case "BFLevel":
                break;
            case "MMLevel":
                mm_head.SetActive(false);
                mm_select.SetActive(false);
                nessie_head.SetActive(false);
                nessie_select.SetActive(false);
                break;

            case "NessieLevel":
                mm_head.SetActive(false);
                mm_select.SetActive(false);
                nessie_head.SetActive(false);
                nessie_select.SetActive(false);
                break;

            default:
                break;
        }
    }

    public void ShowCharacterHead(int recruitedPlayer)
    {
        if (recruitedPlayer == 1)
        {
            mm_head.SetActive(true);
        }
        else if (recruitedPlayer == 2)
        {
            nessie_head.SetActive(true);
        }
    }

    public void ShowSelectedCharacter(int recruitedPlayer)
    {
        mm_select.SetActive(false);
        bf_select.SetActive(false);
        nessie_select.SetActive(false);
        Debug.Log(recruitedPlayer);
        if (recruitedPlayer == 1) { mm_select.SetActive(true); }
        else if (recruitedPlayer == 2) { nessie_select.SetActive(true); }
        else { bf_select.SetActive(true); }
    }
}
