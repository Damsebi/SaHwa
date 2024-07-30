using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour, IEvent
{
    public Transform leftDoor; // ���� �� ť��
    public Transform rightDoor; // ������ �� ť��
    public float openAngle; // ���� �� ȸ���� ����
    public float closeAngle; // ���� �� ȸ���� ����
    public float rotationSpeed; // ȸ�� �ӵ�
    private bool isOpening;

    public void TriggerEvent()
    {
        if (!isOpening)
        {
            isOpening = true;
            StartCoroutine(RotateDoor(leftDoor, rightDoor, openAngle));
        }
        else
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
