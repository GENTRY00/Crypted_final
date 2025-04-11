using UnityEngine;
using TMPro; // TextMesh Pro
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance; // Singleton instance

    public GameObject dialoguePanel; // Reference to the dialogue box panel
    public TMP_Text dialogueText; // Reference to the TextMesh Pro component
    public float typingSpeed = 0.05f; // Speed of typing effect (adjustable)

    private string[] currentDialogueLines; // Store the current set of dialogue lines
    private int currentLineIndex = 0; // Track the current line in the dialogue
    private bool isTyping = false; // Track if the current line is still being typed

    private PlayerController[] allPlayers; // Array to hold all PlayerController instances

    private void Awake()
    {
        allPlayers = FindObjectsOfType<PlayerController>();
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        dialoguePanel.SetActive(false); // Initially hide the dialogue panel

        // Find the player controller (assuming it's attached to the player)
    }

    private void Update()
    {
        // Check for spacebar press to show the next line of dialogue
        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // Skip typing and show the full line immediately
                StopAllCoroutines();
                dialogueText.text = currentDialogueLines[currentLineIndex - 1];
                isTyping = false;
            }
            else
            {
                ShowNextLine(); // Show the next line when space is pressed
            }
        }
    }

    // Call this method to start a new dialogue
    public void StartDialogue(string[] lines)
    {
        currentDialogueLines = lines;
        currentLineIndex = 0;

        dialoguePanel.SetActive(true); // Show the dialogue panel
        foreach (var playerController in allPlayers)
            playerController.SetCanMove(false); // Re-enable player movement
        ShowNextLine(); // Show the first line of dialogue
    }

    // Coroutine to type out the dialogue
    private IEnumerator TypeDialogue(string line)
    {
        dialogueText.text = ""; // Clear the previous text
        isTyping = true; // Mark as typing

        foreach (char letter in line)
        {
            dialogueText.text += letter; // Add each letter to the text
            yield return new WaitForSeconds(typingSpeed); // Wait for a short time before showing the next character
        }

        isTyping = false; // Mark as finished typing
    }

    // Show the next line of dialogue
    public void ShowNextLine()
    {
        if (currentLineIndex < currentDialogueLines.Length)
        {
            StartCoroutine(TypeDialogue(currentDialogueLines[currentLineIndex])); // Type out the next line
            currentLineIndex++;
        }
        else
        {
            EndDialogue(); // End the dialogue when all lines are shown
        }
    }

    // End the dialogue and hide the panel
    private void EndDialogue()
    {
        dialoguePanel.SetActive(false); // Hide the dialogue panel
        foreach(var playerController in allPlayers)
            playerController.SetCanMove(true); // Re-enable player movement
    }
}
