using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public string[] dialogueLines; // Lines of dialogue to trigger upon pickup
    public PagePanelToggle pageController; // Reference to the UIController


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Check if the player collides with the item
        {
            // Trigger the dialogue
            DialogueManager.Instance.StartDialogue(dialogueLines);

            // Optionally, hide the book after the player interacts with it
            gameObject.SetActive(false);

            if (CompareTag("page"))
            {
                pageController.addPage(gameObject.name);
            }
        }
    }
}