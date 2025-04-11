using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompableObject : MonoBehaviour
{
    public GameObject dustEffect;  // Reference to the dust effect prefab
    public StompManager stompManager;  // Reference to the StompManager

    private void Start()
    {
        if (stompManager != null)
        {
            stompManager.RegisterStompable();  // Register this object with the manager
        }
    }

    // This method will be called when the object is stomped
    public void OnStomp()
    {
        Debug.Log("Stomped!");

        if (dustEffect != null)
        {
            GameObject dust = Instantiate(dustEffect, transform.position, Quaternion.identity);
            Destroy(dust, 1f);  // Destroy the dust effect after 1.5 seconds
        }

         // Notify the StompManager that this object was stomped
        if (stompManager != null)
        {
            stompManager.StompableDestroyed();
        }


        // For now, we’ll just destroy the object to simulate it being affected
        Destroy(gameObject);

        
    }
}
