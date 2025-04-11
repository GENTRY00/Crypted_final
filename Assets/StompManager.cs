using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompManager : MonoBehaviour
{
    public GameObject door; // The door to remove
    private int stompableCount = 0; // The number of stompable objects remaining

    // This function registers a stompable object
    public void RegisterStompable()
    {
        stompableCount++;
    }

    // This function is called when a stompable object is stomped
    public void StompableDestroyed()
    {
        stompableCount--;
        Debug.Log("Stompable objects remaining: " + stompableCount);

        if (stompableCount <= 0)
        {
            Destroy(door); // Remove the door once all stompable objects are destroyed
            Debug.Log("All stompable objects destroyed! Door removed.");
        }
    }
}
