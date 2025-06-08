using UnityEngine;

public class SkiLocomotion : MonoBehaviour
{
    public Transform headTransform;   // Camera (e.g., Camera.main.transform)
    public Transform xrRig;           // Root object of the player
    public Transform leftHand;        // Left controller transform
    public Transform rightHand;       // Right controller transform

    public float baseMoveSpeed = 5f;          // Base forward glide speed
    public float maxTurnSpeed = 50f;           // Max turning speed (deg/sec)
    public float tiltThreshold = 10f;          // Min forward tilt to start moving
    public float leanSensitivity = 1.0f;       // Steering sensitivity

    public float pushBackThreshold = 0.1f;     // Minimum backward movement on controller local Z to count as push
    public float speedBoostMultiplier = 3f;    // How much the speed increases when pushing

    // You might want to cache neutral controller positions at start
    private Vector3 leftHandNeutralLocalPos;
    private Vector3 rightHandNeutralLocalPos;

    void Start()
    {
        if (leftHand != null) leftHandNeutralLocalPos = xrRig.InverseTransformPoint(leftHand.position);
        if (rightHand != null) rightHandNeutralLocalPos = xrRig.InverseTransformPoint(rightHand.position);
    }

    void Update()
    {
        Vector3 headForward = headTransform.forward;
        Vector3 flatForward = new Vector3(headForward.x, 0f, headForward.z).normalized;

        float forwardTilt = Vector3.Angle(Vector3.up, headTransform.forward);

        if (forwardTilt > tiltThreshold)
        {
            // Calculate speed boost based on pushing back controllers
            float speedBoost = 0f;

            if (leftHand != null)
            {
                Vector3 leftLocalPos = xrRig.InverseTransformPoint(leftHand.position);
                float leftPushBack = leftHandNeutralLocalPos.z - leftLocalPos.z; // positive if pushed back
                if (leftPushBack > pushBackThreshold)
                    speedBoost += leftPushBack;
            }

            if (rightHand != null)
            {
                Vector3 rightLocalPos = xrRig.InverseTransformPoint(rightHand.position);
                float rightPushBack = rightHandNeutralLocalPos.z - rightLocalPos.z;
                if (rightPushBack > pushBackThreshold)
                    speedBoost += rightPushBack;
            }

            speedBoost = Mathf.Clamp(speedBoost, 0f, 1f) * speedBoostMultiplier;

            float currentSpeed = baseMoveSpeed + speedBoost;

            // Move forward
            xrRig.position += flatForward * currentSpeed * Time.deltaTime;

            // Calculate roll (side tilt)
            float roll = headTransform.localEulerAngles.z;
            if (roll > 180f) roll -= 360f; // Normalize to [-180,180]

            // Apply steering rotation
            float turnAmount = Mathf.Clamp(roll * leanSensitivity, -maxTurnSpeed, maxTurnSpeed);
            xrRig.Rotate(Vector3.up, -turnAmount * Time.deltaTime);
        }
    }
}
