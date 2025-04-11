using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorCollsion : MonoBehaviour
{
    public Boolean key; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D door){
        if (key == false){
            print("no key");

        }
        else {
            door.enabled=false;
            
        }


    }
}
