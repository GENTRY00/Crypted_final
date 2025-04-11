using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class AlignPolygonToCone : MonoBehaviour
{
    private PolygonCollider2D polyCollider;

    void Start()
    {
        polyCollider = GetComponent<PolygonCollider2D>();
    }

    void Update()
    {
        // Align the polygon collider to the local rotation of the cone
        transform.localRotation = Quaternion.identity;
    }

}
