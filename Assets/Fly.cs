using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;



public class Fly : MonoBehaviour
{
    private Vector2  location; 
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        location.x=110;
        location.y=110;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D test){ // this script will only work if the gate wants to move on the Y axis, very easy to make a second script for the X axis this is just a proof of concept
        if (test.CompareTag("Gap")){ // this works
            
            print("bitch im trying");
            location.x = this.transform.position.x;
            if(test.transform.position.y < this.transform.position.y)
            location.y = test.transform.position.y - Mathf.Abs(this.transform.position.y);
            else 
            location.y = this.transform.position.y + Mathf.Abs(test.transform.position.y);
            
            rb.position= location;
            Physics.SyncTransforms(); // this works 
            
            //location. y = test.transform.position.y + this.transform.position.y;

            // set our y cordiniate = to this rigidbody2d y  + the size of the collision box
            // subtract if the player is on the other side prob check it with some form of if wer are above or below the collsion box 
        
            //}
        }
    }
}
