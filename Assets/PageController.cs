using UnityEngine;
using UnityEngine.UI;

public class PagePanelToggle : MonoBehaviour
{
    // Reference to the PagePanel and BookPanel GameObjects
    public GameObject pagePanel;
    public GameObject bookPanel;
    private Transform bookIcon;
    public GameObject[] pages = new GameObject[5];
    private int currentPageIndex = 0; // Index of the current page being viewed
    private bool isBookOpen = false;

    public Button nextPageButton;
    public Button previousPageButton;

    void Start()
    {
        // Find the "Book" icon inside the bookPanel
        if (bookPanel != null)
        {
            bookIcon = bookPanel.transform.Find("Book");
        }
    }

    private void Update()
    {
        // Check if bookIcon exists and if it is inactive
        if (bookIcon != null && !bookIcon.gameObject.activeSelf)
        {
            // If the book icon is inactive, find the "BookIcon" object in the scene and make it active
            GameObject book = GameObject.Find("bookIcon");
            if (!book)
            {
                bookIcon.gameObject.SetActive(true);
            }
        }

        // Now check if the player presses the "B" key to toggle pagePanel visibility
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log(bookPanel.activeSelf);
            Debug.Log(bookIcon);
            Debug.Log(bookIcon.gameObject.activeSelf);
            // Ensure bookPanel is active and the bookIcon is active
            if (bookPanel.activeSelf && bookIcon != null && bookIcon.gameObject.activeSelf)
            {
                ToggleBook();
            }
        }
    }

    private void ToggleBook()
    {
        isBookOpen = !isBookOpen;
        pagePanel.SetActive(isBookOpen); // Show or hide the book panel

        if (isBookOpen)
        {
            ShowCollectedPages(); // Show only the pages that have been collected
        }
    }

    private void ShowCollectedPages()
    {
        // Hide all pages first
        foreach (var page in pages)
        {
            if (page != null)
                page.SetActive(false);
        }

        // Only show pages that have been collected and show the current page
        if (currentPageIndex < 5 && pages[currentPageIndex] != null)
        {
            Debug.Log(currentPageIndex);
            pages[currentPageIndex].SetActive(true);
        }

        // Enable or disable navigation buttons based on page index
        previousPageButton.interactable = currentPageIndex > 0;
        nextPageButton.interactable = pages[currentPageIndex + 1] != null;
    }

    public void NextPage()
    {
        Debug.Log("Next button clicked");
        if (currentPageIndex < 4)
        {
            currentPageIndex++;
            ShowCollectedPages();
        }
    }

    public void PreviousPage()
    {
        Debug.Log("Previous button clicked");
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            ShowCollectedPages();
        }
    }

    public void addPage(string name)
    {
        int nextIndex = 0;

        while (pages[nextIndex] != null)
        {
            nextIndex++;
        }

        Debug.Log(nextIndex);
        if (name == "bfpage-trigger") {
            GameObject newpage = pagePanel.transform.Find("bf-pg")?.gameObject;

            if (newpage != null)
            {
                // Set the page at the next index in the array to be the "bf-pg" page
                pages[nextIndex] = newpage;
                
            }
        }
        else if (name == "mmpage-trigger")
        {
            GameObject newpage = pagePanel.transform.Find("mm-pg")?.gameObject;

            if (newpage != null)
            {
                // Set the page at the next index in the array to be the "bf-pg" page
                pages[nextIndex] = newpage;
            }
        }
        else if (name == "lnpage-trigger")
        {
            GameObject newpage = pagePanel.transform.Find("ln-pg")?.gameObject;

            if (newpage != null)
            {
                // Set the page at the next index in the array to be the "bf-pg" page
                pages[nextIndex] = newpage;
            }
        }
    }
}
