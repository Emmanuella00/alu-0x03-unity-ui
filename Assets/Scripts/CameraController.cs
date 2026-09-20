using UnityEngine;

using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Drag the Player GameObject into this slot in the Inspector
    public GameObject player;

    // How far the camera sits from the player, calculated once at the start
    private Vector3 offset;

    void Start()
    {
        // Calculate the initial gap between camera and player.
        // Whatever this distance is when the game starts, it stays constant forever after.
        offset = transform.position - player.transform.position;
    }

    // LateUpdate runs after all Update()/FixedUpdate() calls each frame,
    // ensuring the camera repositions only after the Player has already moved
    void LateUpdate()
    {
        // Keep the camera at the same relative offset from the player's current position
        transform.position = player.transform.position + offset;
    }
}
