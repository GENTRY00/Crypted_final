using UnityEngine;

public class FirstLevelAltar : MonoBehaviour
{
    public GameObject player; // Reference to Bigfoot

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Object entered altar: {other.name} with tag: {other.tag}");

        if (other.CompareTag("Snapped")) // Only react to snapped blocks
        {
            Debug.Log("Block detected on altar. Enabling stomp...");

            var stompController = player.GetComponent<StompController>();
            if (stompController != null)
            {
                stompController.enabled = true;
                Debug.Log("Stomp ability successfully activated!");
            }
            else
            {
                Debug.LogError("StompController script missing on player!");
            }
        }
        else
        {
            Debug.LogWarning("Object entered altar but is not a snapped block.");
        }
    }
}