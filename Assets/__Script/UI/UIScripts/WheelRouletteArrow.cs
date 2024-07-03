using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRouletteArrow : MonoBehaviour
{
    private float rotateAngle = 30f;  // The angle to rotate towards when hit
    private float rotateDuration = 0.1f;  // Duration of the rotation
    private float resetDuration = 0.05f;  // Duration to reset the rotation

    private Sequence rotationSequence;
    private float initialRotation;

    private void Start()
    {
        initialRotation = transform.eulerAngles.z;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (rotationSequence != null && rotationSequence.IsActive())
        {
            rotationSequence.Kill();  // Kill the current sequence if it's active
        }

        StartRotationSequence();
    }

    private void StartRotationSequence()
    {
        rotationSequence = DOTween.Sequence();

        float currentRotateAngle = transform.eulerAngles.z - initialRotation;

        // Rotate the arrow towards the specified angle
        rotationSequence.Append(transform.DORotate(new Vector3(0, 0, initialRotation + rotateAngle + currentRotateAngle/ 1.5f), rotateDuration));

        // After rotating, reset the arrow back to its original position
        rotationSequence.Append(transform.DORotate(new Vector3(0, 0, initialRotation), resetDuration));
    }
}
