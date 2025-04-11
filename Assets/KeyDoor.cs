using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public string doorColor;
    private GameObject holder;

    // Start is called before the first frame update
    public void OnTriggerEnter2D(Collider2D test)
    {
        holder = test.gameObject;
        KeyManagement keyManagement = holder.GetComponent<KeyManagement>();

        if (keyManagement == null)
        {
            Debug.LogWarning("KeyManagement component is missing on the holder!");
            return;
        }

        // Now, check if the player has the key for the corresponding door color
        switch (doorColor)
        {
            case "Red":
                if (keyManagement.red)
                    unlock();
                break;
            case "Blue":
                if (keyManagement.blue)
                    unlock();
                break;
            case "Green":
                if (keyManagement.green)
                    unlock();
                break;
            case "Yellow":
                if (keyManagement.yellow)
                    unlock();
                break;
            default:
                Debug.LogWarning("Unknown door color: " + doorColor);
                break;
        }
    }

    public void unlock()
    {
        print("unlocking");
        Destroy(this.gameObject);
    }
}
