using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTestPlayerController : MonoBehaviour
{
    private Rigidbody rigidbody;

    private float hori; //좌우 방향
    private float verti; //앞뒤 방향
    private float moveAmount; //움직임의 양
    private Vector3 movement; //최종적으로 움직이는 방향
    private Quaternion targetRotation; //목표방향

    [Space(10f)]
    [Tooltip("플레이어의 이동속도 조절")]
    [SerializeField][Range(0, 20)] float moveSpeed;
    [Tooltip("플레이어의 회전속도 조절")]
    [SerializeField] float playerRotateSpeed;

    //카메라에 의한 이동변화
    [Space(10f)]
    [Tooltip("'cameraOrbitAxis' 오브젝트 넣기")]
    [SerializeField] Transform cameraOrbitAxis; //카메라의 OrbitAxis를 중심으로 회전
    private bool isLockedOnTarget; //카메라가 타겟에게 락온인 상태

    [Tooltip("'Points' 오브젝트 내에 있는 point 1~7까지 드래그해서 'Pointers' 라는 글자에 끌어넣기")]
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

    private void InputDirectionKey() //방향키 눌렀을 때 방향과 크기값을 얻음
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
