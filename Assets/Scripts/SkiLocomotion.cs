using UnityEngine;

public class SkiLocomotion : MonoBehaviour
{
    public Transform headTransform; // Camera (e.g., Camera.main.transform)
    public Transform xrRig;         // Root object of the player
    public float moveSpeed = 5f;    // Forward glide speed
    public float maxTurnSpeed = 50f; // Maximum turning speed in degrees/second
    public float tiltThreshold = 10f; // Minimum forward tilt to start moving
    public float leanSensitivity = 1.0f; // Tuning sensitivity for steering

    void Update()
    {
        Vector3 headForward = headTransform.forward;
        Vector3 flatForward = new Vector3(headForward.x, 0f, headForward.z).normalized;

        float forwardTilt = Vector3.Angle(Vector3.up, headTransform.forward);

        if (forwardTilt > tiltThreshold)
        {
            // Move forward
            xrRig.position += flatForward * moveSpeed * Time.deltaTime;

            // Calculate roll (side tilt)
            float roll = headTransform.localEulerAngles.z;
            if (roll > 180f) roll -= 360f; // Normalize to [-180, 180]

            // Apply steering rotation
            float turnAmount = Mathf.Clamp(roll * leanSensitivity, -maxTurnSpeed, maxTurnSpeed);
            xrRig.Rotate(Vector3.up, -turnAmount * Time.deltaTime);
        }
    }
}
