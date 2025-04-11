using System.Collections.Generic;
using UnityEngine;
using Cainos.PixelArtTopDown_Basic;


public class AltarDoor : MonoBehaviour
{
    public List<GameObject> altars; // A list of all altars that need to be activated

    void Update()
    {
        bool allActivated = true;
        foreach (GameObject altar in altars)
        {
            PropsAltar propsAltar = altar.GetComponent<PropsAltar>();
            if (propsAltar == null || !propsAltar.ison)
            {
                allActivated = false;
                break;
            }
        }

        if (allActivated)
        {
            Debug.Log("All altars activated! Door opens.");
            Destroy(gameObject);
        }
    }

}
