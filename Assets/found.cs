using UnityEngine;

public class Found : MonoBehaviour
{
    public GameObject failCanvas; // Reference to the fail canvas
    public MenuController menuController; // Reference to the MenuController

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Get the FollowThePath script on the Zookeeper
            FollowThePath zookeeperPath = GetComponentInParent<FollowThePath>();

            // Check if the Zookeeper can detect the player
            if (zookeeperPath != null && zookeeperPath.CanDetectPlayer())
            {
                Debug.Log("Player Found!");
                // Trigger the player's detection logic (freeze, fail state, etc.)
                collision.GetComponent<PlayerController>().enabled = false;
            }

            ShowFailCanvas();
        }
    }

    private void ShowFailCanvas()
    {
        if (failCanvas != null)
        {
            failCanvas.SetActive(true); // Activate the fail canvas UI
            menuController.isFail = true;
        }
    }
}
