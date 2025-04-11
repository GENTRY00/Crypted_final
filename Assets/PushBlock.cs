using System;
using UnityEngine;
using Cainos.PixelArtTopDown_Basic;


public class PushBlock : MonoBehaviour
{
    public Collider2D trigger;        // Current trigger object
    public bool isPulling = false;    // Whether the player is pulling a block
    public Collider2D attachedTo;     // The block currently attached

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isPulling)
            {
                DetachBlock();
            }
            else if (trigger != null && !isPulling)
            {
                TryAttachBlock(trigger);
            }
        }
    }

    private void DetachBlock()
    {
        if (attachedTo != null)
        {
            var fixedJoint = attachedTo.GetComponent<FixedJoint2D>();
            var rigidbody = attachedTo.GetComponent<Rigidbody2D>();

            if (fixedJoint != null)
            {
                fixedJoint.connectedBody = null;
                fixedJoint.enabled = false;
            }

            if (rigidbody != null)
            {
                rigidbody.bodyType = RigidbodyType2D.Static;
            }

            isPulling = false;
            attachedTo = null;
            Debug.Log("Block detached.");
        }
    }

    private void TryAttachBlock(Collider2D block)
    {
        var rigidbody = block.GetComponent<Rigidbody2D>();
        var fixedJoint = block.GetComponent<FixedJoint2D>();

        if (rigidbody != null && fixedJoint != null)
        {
            if (!block.CompareTag("icon") && !block.CompareTag("Snapped"))
            {
                rigidbody.bodyType = RigidbodyType2D.Dynamic;
                fixedJoint.enabled = true;
                fixedJoint.connectedBody = GetComponent<Rigidbody2D>();
                isPulling = true;
                attachedTo = block;
                Debug.Log("Block attached.");
            }
        }
        else
        {
            Debug.LogWarning("The object does not have the required Rigidbody2D or FixedJoint2D components.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the block is entering an altar trigger
        if (other.CompareTag("Altar"))
        {
            // Get the altar's snap position
            Transform snapPosition = other.transform.Find("SnapPosition");
            if (snapPosition != null && attachedTo != null)
            {
                SnapToPosition(attachedTo.transform, snapPosition.position);
                Debug.Log("Block snapped to altar.");
            }
        }

        // Assign the trigger only if it has the required components
        if (other.GetComponent<Rigidbody2D>() != null && other.GetComponent<FixedJoint2D>() != null)
        {
            trigger = other;
            Debug.Log("Entered trigger: " + other.name);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (trigger == other)
        {
            trigger = null;
            Debug.Log("Exited trigger: " + other.name);
        }
    }

    private void SnapToPosition(Transform block, Vector3 snapPosition)
    {
        // Instantly snap the block to the position
        block.position = snapPosition;

        // Optional: Disable movement components on the block after snapping
        var rigidbody = block.GetComponent<Rigidbody2D>();
        if (rigidbody != null)
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.angularVelocity = 0f;
            rigidbody.bodyType = RigidbodyType2D.Static;
        }

        // Change the tag to indicate it is snapped
        block.gameObject.tag = "Snapped";
        DetachBlock();

        // Check for an object with the 'hidden' tag
        GameObject hiddenObject = GameObject.FindWithTag("hidden");
        if (hiddenObject != null)
        {
            // Get the EdgeCollider2D component
            EdgeCollider2D edgeCollider = hiddenObject.GetComponent<EdgeCollider2D>();

            if (edgeCollider != null)
            {
                // Enable the EdgeCollider2D
                edgeCollider.enabled = true;
            }
        }
        else
        {
            Debug.LogWarning("No object with 'hidden' tag found.");
        }
    }

}