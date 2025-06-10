using UnityEngine;
using System.Collections;

public class SkiLocomotion : MonoBehaviour
{
    public Transform headTransform;   // Camera (e.g., Camera.main.transform)
    public Transform xrRig;           // Root object of the player
    public Transform leftHand;        // Left controller transform
    public Transform rightHand;       // Right controller transform

    public float maxTurnSpeed = 50f;           // Max turning speed (deg/sec)
    public float tiltThreshold = 10f;          // Min forward tilt to start moving
    public float leanSensitivity = 1.0f;       // Steering sensitivity

    public float pushBackThreshold = 0.1f;     // Minimum backward movement on controller local Z to count as push
    public float speedBoostMultiplier = 3f;    // How much the speed increases when pushing

    private Vector3 leftHandNeutralLocalPos;
    private Vector3 rightHandNeutralLocalPos;
    private Vector3 lastPosition;

    float initialY;

    void Start()
    {
        if (leftHand != null) leftHandNeutralLocalPos = xrRig.InverseTransformPoint(leftHand.position);
        if (rightHand != null) rightHandNeutralLocalPos = xrRig.InverseTransformPoint(rightHand.position);
        lastPosition = xrRig.position;
        initialY = xrRig.position.y; // Store initial Y position for flat movement
    }

    void Update()
    {

        float deltaTime = Time.deltaTime;
        if (deltaTime <= 0f) return;

        Vector3 currentPosition = xrRig.position;
        Vector3 movementVector = (currentPosition - lastPosition) / deltaTime;
        float currentSpeed = new Vector3(movementVector.x, movementVector.y, movementVector.z).magnitude; // flat speed
        float speedBoost = CalculatePolePushBoost();

        Vector3 headForward = headTransform.forward;
        Vector3 flatForward = new Vector3(headForward.x, 0f, headForward.z).normalized;

        float forwardTilt = Vector3.Angle(Vector3.up, headTransform.forward);

        // Only apply movement if tilted forward enough
        if (forwardTilt > tiltThreshold)
        {
            currentSpeed += speedBoost;
            Vector3 move = flatForward * currentSpeed * deltaTime;
            xrRig.position += new Vector3(move.x, 0f, move.z);
            //ApplyHeadTiltSteering();
        }
        else
        {
            // Only apply push movement without base speed if not tilting
            if (speedBoost > 0f)
            {
                Vector3 move = flatForward * speedBoost * deltaTime;
                xrRig.position += new Vector3(move.x, 0f, move.z);
            }
        }
        lastPosition = xrRig.position;
    }

    private float CalculatePolePushBoost()
    {
        float boost = 0f;

        if (leftHand != null)
        {
            Vector3 leftLocal = xrRig.InverseTransformPoint(leftHand.position);
            float leftPush = leftHandNeutralLocalPos.z - leftLocal.z;
            if (leftPush > pushBackThreshold)
                boost += leftPush;
        }

        if (rightHand != null)
        {
            Vector3 rightLocal = xrRig.InverseTransformPoint(rightHand.position);
            float rightPush = rightHandNeutralLocalPos.z - rightLocal.z;
            if (rightPush > pushBackThreshold)
                boost += rightPush;
        }

        return Mathf.Clamp(boost, 0f, 1f) * speedBoostMultiplier;
    }

    private void ApplyHeadTiltSteering()
    {
        float roll = headTransform.localEulerAngles.z;
        if (roll > 180f) roll -= 360f; // Normalize to [-180,180]

        float turnAmount = Mathf.Clamp(roll * leanSensitivity, -maxTurnSpeed, maxTurnSpeed);
        xrRig.Rotate(Vector3.up, -turnAmount * Time.deltaTime);
    }

}
