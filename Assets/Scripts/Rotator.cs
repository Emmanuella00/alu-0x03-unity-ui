using UnityEngine;

public class Rotator : MonoBehaviour
{
    void Update()
    {
        // Rotate 45 degrees per second around the X axis.
        // Time.deltaTime is the time since the last frame, so multiplying by it
        // makes the rotation speed independent of frame rate -- smooth on any machine.
        transform.Rotate(45f * Time.deltaTime, 0f, 0f);
    }
}