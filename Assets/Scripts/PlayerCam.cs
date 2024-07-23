using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Tooltip("'player' 오브젝트 내에 'CameraTarget'을 찾아 넣기")]
    [SerializeField] GameObject CameraTarget;
    
    [Tooltip("'cameraOrbitAxis 오브젝트 내에 'mainCamera'을 찾아 넣기")]
    [SerializeField] Camera mainCamera;

    [Tooltip("카메라가 플레이어를 따라가는 부드러움 정도")]
    [SerializeField][Range(0, 1)] float cameraFollowSmoothTime;

    private Vector3 cameraVelocity;

    [Space(10f)]
    [Header("Camera Rotation")]
    [SerializeField][Range(0, 10)] float cameraRotateSpeed;
    float rotateCameraYValue = 0;

    [Space(10f)]
    [Header("Camera Collision")]
    [Tooltip("카메라가 장애물을 인식")]
    [SerializeField] LayerMask cameraCollisionLayers;
    private float originCameraZPosition;
    private float currentCameraZposition;
    private float cameraCollisionRadius = 0.2f; //카메라가 장애물에 충돌시 반지름
    private Vector3 cameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        originCameraZPosition = mainCamera.transform.localPosition.z;

    }


    private void FixedUpdate()
    {
        FollowPlayer();
        CameraRotation();
        //HandleCollisions();
    }

    private void FollowPlayer()
    {
        Vector3 moveToPlayerPosition = Vector3.SmoothDamp(this.transform.position, CameraTarget.transform.position, ref cameraVelocity, cameraFollowSmoothTime);
        this.transform.position = moveToPlayerPosition;
    }
    private void CameraRotation()
    {
        if (Input.GetKey(KeyCode.Q)) //GetAxis로 얻은 값이 되돌아가는 것 방지
        {
            rotateCameraYValue -= 0.5f * cameraRotateSpeed;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotateCameraYValue += 0.5f * cameraRotateSpeed;
        }

        transform.rotation = Quaternion.Euler(0, rotateCameraYValue, 0);
    }

    private void HandleCollisions()
    {
        currentCameraZposition = originCameraZPosition; //카메라의 로컬 z좌표(거리)

        RaycastHit hit;
        Vector3 direction = mainCamera.transform.position - transform.position; //카메라 공전축에서 카메라방향
        direction.Normalize();

        //sphere 모양의 raycast. 플레이어에서 시작, 크기, 방향, 결과, 최대거리, 해당레이어
        if (Physics.SphereCast(transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(currentCameraZposition), cameraCollisionLayers))
        {
            float distanceFromHitObject = Vector3.Distance(transform.position, hit.point); //카메라에 부딪히는 장애물과 플레이어간의 거리
            currentCameraZposition = -(distanceFromHitObject - cameraCollisionRadius); //카메라가 벽보다 앞에 위치하여 제대로 찍히도록 만듬
        }

        if (Mathf.Abs(currentCameraZposition) < cameraCollisionRadius) //플레이어와 장애물간의 거리가 너무 좁으면 아예 고정
        {
            currentCameraZposition = -cameraCollisionRadius;
        }

        cameraPosition.z = Mathf.Lerp(mainCamera.transform.localPosition.z, currentCameraZposition, 0.1f); //Z거리 바뀌는걸 Lerp하게 진행
        mainCamera.transform.localPosition = cameraPosition;  //위의 작업을 메인카메라에 실제 적용하는 부분. 
    }
}
