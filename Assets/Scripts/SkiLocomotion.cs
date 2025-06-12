using UnityEngine;
using System.Collections;

public class SkiLocomotion : MonoBehaviour
{
    public Transform headTransform;
    public Transform xrRig;           // Root object of the player
    public Transform leftHand;        // Left controller transform
    public Transform rightHand;       // Right controller transform

    public float pushBackThreshold = 0.1f;     // Minimum backward movement on controller local Z to count as push
    public float speedBoostMultiplier = 3f;    // How much the speed increases when pushing
    public float frictionFactor = 0.99f; // Reduce speed by 1% per frame

    private Vector3 leftHandNeutralLocalPos;
    private Vector3 rightHandNeutralLocalPos;

    private float currentSpeed = 0f;

    void Start()
    {
        if (leftHand != null) leftHandNeutralLocalPos = xrRig.InverseTransformPoint(leftHand.position);
        if (rightHand != null) rightHandNeutralLocalPos = xrRig.InverseTransformPoint(rightHand.position);
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;
        if (deltaTime <= 0f) return;

        currentSpeed *= frictionFactor;

        float speedBoost = CalculatePolePushBoost();


        if (speedBoost > 0f)
        {
            currentSpeed += speedBoost;

        }

        Vector3 headForward = headTransform.forward;
        Vector3 flatForward = new Vector3(headForward.x, 0f, headForward.z).normalized;

        Vector3 move = flatForward * currentSpeed * deltaTime;
        Vector3 newFlatPosition = xrRig.position + new Vector3(move.x, 0f, move.z);

        Vector3 rayOrigin = new Vector3(newFlatPosition.x, xrRig.position.y + 5f, newFlatPosition.z);
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 20f, LayerMask.GetMask("Berg")))
        {
            newFlatPosition.y = hit.point.y;
        }
        xrRig.position = newFlatPosition;
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
}
