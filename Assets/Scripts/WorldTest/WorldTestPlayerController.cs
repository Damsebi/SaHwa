using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTestPlayerController : MonoBehaviour
{
    private Rigidbody rigidbody;

    private float hori; //�¿� ����
    private float verti; //�յ� ����
    private float moveAmount; //�������� ��
    private Vector3 movement; //���������� �����̴� ����
    private Quaternion targetRotation; //��ǥ����

    [Space(10f)]
    [Tooltip("�÷��̾��� �̵��ӵ� ����")]
    [SerializeField][Range(0, 20)] float moveSpeed;
    [Tooltip("�÷��̾��� ȸ���ӵ� ����")]
    [SerializeField] float playerRotateSpeed;

    //ī�޶� ���� �̵���ȭ
    [Space(10f)]
    [Tooltip("'cameraOrbitAxis' ������Ʈ �ֱ�")]
    [SerializeField] Transform cameraOrbitAxis; //ī�޶��� OrbitAxis�� �߽����� ȸ��
    private bool isLockedOnTarget; //ī�޶� Ÿ�ٿ��� ������ ����

    [Tooltip("'Points' ������Ʈ ���� �ִ� point 1~7���� �巡���ؼ� 'Pointers' ��� ���ڿ� ����ֱ�")]
    public Transform[] pointers;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void FixedUpdate()
    {
        HandlePlayerRotation();
        rigidbody.MovePosition(this.gameObject.transform.position + movement * moveSpeed * Time.deltaTime);
    }

    private void Update()
    {
        Cursor.visible = false;

        InputDirectionKey();
        MoveToPoint();

    }

    private void InputDirectionKey() //����Ű ������ �� ����� ũ�Ⱚ�� ����
    {
        hori = Input.GetAxis("Horizontal");
        verti = Input.GetAxis("Vertical");
        movement = new Vector3(hori, 0f, verti);
        moveAmount = Mathf.Clamp01(Mathf.Abs(movement.x) + Mathf.Abs(movement.z));
        movement.Normalize();
    }

    private void HandlePlayerRotation()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, playerRotateSpeed);

        if (moveAmount > 0f)
        {
            Vector3 cam = cameraOrbitAxis.transform.forward;
            movement = Quaternion.LookRotation(new Vector3(cam.x, 0, cam.z)) * movement;
            targetRotation = Quaternion.LookRotation(movement);
        }
    }

    private void MoveToPoint()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) transform.position = pointers[0].transform.position;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) transform.position = pointers[1].transform.position;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) transform.position = pointers[2].transform.position;
        else if (Input.GetKeyDown(KeyCode.Alpha4)) transform.position = pointers[3].transform.position;
        else if (Input.GetKeyDown(KeyCode.Alpha5)) transform.position = pointers[4].transform.position;
        else if (Input.GetKeyDown(KeyCode.Alpha6)) transform.position = pointers[5].transform.position;
        else if (Input.GetKeyDown(KeyCode.Alpha7)) transform.position = pointers[6].transform.position;
    }

}
