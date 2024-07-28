using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingDown : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Collider collider;
    public float shakeDuration = 2f; // ��鸮�� �ð�
    public float shakeAmount = 0.05f; // ��鸲 ����
    public float fallDelay = 1f; // ��鸲 �� �������� ���� �ð�
    private Vector3 initialPosition;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        rigidbody.isKinematic = true;  // ó������ ���� ȿ���� ���� �ʵ��� ����
        initialPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ShakeAndFall());
        }
    }

    private IEnumerator ShakeAndFall()
    {
        float fallingWait= 0f;

        while (fallingWait < shakeDuration)
        {
            transform.position = initialPosition + Random.insideUnitSphere * shakeAmount;
            fallingWait += Time.deltaTime;
            yield return null;
        }
        Debug.Log("�������ϴ�");

        transform.position = initialPosition; // ���� ��ġ�� ����
        yield return new WaitForSeconds(fallDelay); // ��鸲 ���� 1�� ���
        
        rigidbody.isKinematic = false; // 3�� �Ŀ� ���� ȿ���� �޵��� ����

        collider.enabled = false;
    }
}
