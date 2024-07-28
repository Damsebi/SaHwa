using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingDown : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Collider collider;
    public float shakeDuration = 2f; // 흔들리는 시간
    public float shakeAmount = 0.05f; // 흔들림 강도
    public float fallDelay = 1f; // 흔들림 후 떨어지는 지연 시간
    private Vector3 initialPosition;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        rigidbody.isKinematic = true;  // 처음에는 물리 효과를 받지 않도록 설정
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
        Debug.Log("떨어짐니다");

        transform.position = initialPosition; // 원래 위치로 복원
        yield return new WaitForSeconds(fallDelay); // 흔들림 이후 1초 대기
        
        rigidbody.isKinematic = false; // 3초 후에 물리 효과를 받도록 설정

        collider.enabled = false;
    }
}
