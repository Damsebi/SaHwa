using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour, IEvent
{
    public Transform leftDoor; // 왼쪽 문 큐브
    public Transform rightDoor; // 오른쪽 문 큐브
    [SerializeField] private float openAngle; // 열릴 때 회전할 각도
    [SerializeField] private float closeAngle; // 닫힐 때 회전할 각도
    [SerializeField] private float rotationSpeed; // 회전 속도
    [SerializeField] private bool isOpenAgain;
    private bool isOpening = false;
    private bool hasOpened = false;

    public void TriggerEvent()
    {
        if (!isOpening && (!hasOpened||isOpenAgain))
        {
            isOpening = true;
            hasOpened = true;
            StartCoroutine(RotateDoor(leftDoor, rightDoor, openAngle));
        }
        else if(isOpening)
        {
            isOpening = false;
            StopAllCoroutines();
            StartCoroutine(RotateDoor(leftDoor, rightDoor, closeAngle));
        }
    }

    private IEnumerator RotateDoor(Transform leftDoor, Transform rightDoor, float targetAngle)
    {
        Quaternion leftTargetRotation = Quaternion.Euler(0, targetAngle, 0);
        Quaternion rightTargetRotation = Quaternion.Euler(0, -targetAngle, 0);

        while (Quaternion.Angle(leftDoor.localRotation, leftTargetRotation) > 0.01f)
        {
            leftDoor.localRotation = Quaternion.Slerp(leftDoor.localRotation, leftTargetRotation, Time.deltaTime * rotationSpeed);
            rightDoor.localRotation = Quaternion.Slerp(rightDoor.localRotation, rightTargetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }
        leftDoor.localRotation = leftTargetRotation;
        rightDoor.localRotation = rightTargetRotation;
    }
}
