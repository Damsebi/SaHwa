using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour, IEvent
{
    public Transform leftDoor; // ���� �� ť��
    public Transform rightDoor; // ������ �� ť��
    [SerializeField] private float openAngle; // ���� �� ȸ���� ����
    [SerializeField] private float closeAngle; // ���� �� ȸ���� ����
    [SerializeField] private float rotationSpeed; // ȸ�� �ӵ�
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
