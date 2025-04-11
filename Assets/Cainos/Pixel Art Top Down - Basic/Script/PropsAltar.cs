using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// When something gets into the altar, make the runes glow
namespace Cainos.PixelArtTopDown_Basic
{
    public class PropsAltar : MonoBehaviour
    {
        public List<SpriteRenderer> runes;
        public float lerpSpeed;
        public GameObject player;
        public PushBlock pushBlockScript; // Reference to the PushBlock script
        public bool ison; // Indicates if the altar is active (block snapped on it)

        private Color curColor;
        private Color targetColor;

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Check if the block has the tag "Ground"
            if (other.CompareTag("Ground"))
            {
                Debug.Log("Block entered altar trigger.");

                // Set the altar to "on" and make it glow
                ison = true;
                targetColor = new Color(1, 1, 1, 1);

                // Snap the block to the altar's position
                SnapBlockToPosition(other.gameObject);

                // Stop the block from being pulled/pushed
                if (pushBlockScript)
                {
                    pushBlockScript.isPulling = false;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            // If a block leaves the altar, turn off the glow
            if (other.CompareTag("Ground"))
            {
                Debug.Log("Block exited altar trigger.");
                ison = false;
                targetColor = new Color(1, 1, 1, 0); // Fading out
            }
        }

        private void SnapBlockToPosition(GameObject block)
        {
            // Snap the block to the altar's position (center of the altar)
            Vector3 snapPosition = transform.position;
            block.transform.position = snapPosition;

            // Change the block's tag to "Snapped" to indicate it's locked in place
            block.tag = "Snapped";

            Debug.Log($"Block snapped to altar at position: {snapPosition}");
        }

        private void Update()
        {
            // If the block is snapped, ensure the altar stays on
            Collider2D blockCollider = Physics2D.OverlapCircle(transform.position, 0.5f, LayerMask.GetMask("Block"));
            if (blockCollider && blockCollider.CompareTag("Snapped"))
            {
                ison = true;
                targetColor = new Color(1, 1, 1, 1); // Keep glowing
            }
            else if (!ison) // Only fade out if the altar isn't "on"
            {
                targetColor = new Color(1, 1, 1, 0);
            }

            // Smoothly interpolate the rune color to the target color
            curColor = Color.Lerp(curColor, targetColor, lerpSpeed * Time.deltaTime);

            // Apply the color to all runes
            foreach (var rune in runes)
            {
                if (rune != null)
                {
                    rune.color = curColor;
                }
            }
        }
    }
}