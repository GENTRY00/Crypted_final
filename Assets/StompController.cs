using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompController : MonoBehaviour
{
    public float stompRadius = 2f;  // The radius of the stomp's effect
    public LayerMask stompableLayer; // A layer to specify which objects can be stomped
    public GameObject hitEffect;  // Reference to the new "HitEffect_B" prefab
    private Animator animator;

    private bool isEnabled = false; // Flag to track if stomp is enabled

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();  // Reference to the Animator
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))  // When the player presses the space key
        {
            Stomp();  // Call the Stomp function
        }
    }

    void Stomp()
    {

        // Instantiate the shockwave effect at Bigfoot's position
        if (hitEffect != null)
        {
            GameObject hitEffectInstance = Instantiate(hitEffect, transform.position, Quaternion.identity);
            hitEffectInstance.transform.localScale = new Vector3(1.5f, 1.5f, 1);  // Adjust the scale of the effect
            Destroy(hitEffectInstance, 1f);  // Destroy the hit effect after 1.5 seconds
        }

        // Find all objects within the stomp radius that belong to the stompable layer
        Collider2D[] objectsHit = Physics2D.OverlapCircleAll(transform.position, stompRadius, stompableLayer);

        foreach (Collider2D obj in objectsHit)
        {
            Debug.Log("Stomped: " + obj.name);

            // Call the OnStomp method on each stompable object
            if (obj.CompareTag("Stompable"))
            {
                obj.GetComponent<StompableObject>().OnStomp();
            }
        }
    }

    // Method to enable the stomp ability
    public void EnableStomp()
    {
        isEnabled = true;
        Debug.Log("Stomp ability enabled!");
    }

    // Visualize the stomp radius in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stompRadius);
    }
}