using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class KeyManagement : MonoBehaviour
{

    public Boolean red; 
    public Boolean blue; 
    public Boolean green; 
    public Boolean yellow;
    // Start is called before the first frame update
    void Start()
    {
        red = false;
        blue = false;
        green = false;
        yellow = false;
    }

    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Key")){
            if(collision.gameObject.name== "Red"){
                red = true;
            }
            else if (collision.gameObject.name== "Blue"){
                blue = true;
            }
            else if (collision.gameObject.name== "Green"){
                green = true;
            }
            else yellow =true;

            Destroy(collision.gameObject);
        }
    }
}
