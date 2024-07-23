using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Tooltip("'player' ������Ʈ ���� 'CameraTarget'�� ã�� �ֱ�")]
    [SerializeField] GameObject CameraTarget;
    
    [Tooltip("'cameraOrbitAxis ������Ʈ ���� 'mainCamera'�� ã�� �ֱ�")]
    [SerializeField] Camera mainCamera;

    [Tooltip("ī�޶� �÷��̾ ���󰡴� �ε巯�� ����")]
    [SerializeField][Range(0, 1)] float cameraFollowSmoothTime;

    private Vector3 cameraVelocity;

    [Space(10f)]
    [Header("Camera Rotation")]
    [SerializeField][Range(0, 10)] float cameraRotateSpeed;
    float rotateCameraYValue = 0;

    [Space(10f)]
    [Header("Camera Collision")]
    [Tooltip("ī�޶� ��ֹ��� �ν�")]
    [SerializeField] LayerMask cameraCollisionLayers;
    private float originCameraZPosition;
    private float currentCameraZposition;
    private float cameraCollisionRadius = 0.2f; //ī�޶� ��ֹ��� �浹�� ������
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
        if (Input.GetKey(KeyCode.Q)) //GetAxis�� ���� ���� �ǵ��ư��� �� ����
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
        currentCameraZposition = originCameraZPosition; //ī�޶��� ���� z��ǥ(�Ÿ�)

        RaycastHit hit;
        Vector3 direction = mainCamera.transform.position - transform.position; //ī�޶� �����࿡�� ī�޶����
        direction.Normalize();

        //sphere ����� raycast. �÷��̾�� ����, ũ��, ����, ���, �ִ�Ÿ�, �ش緹�̾�
        if (Physics.SphereCast(transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(currentCameraZposition), cameraCollisionLayers))
        {
            float distanceFromHitObject = Vector3.Distance(transform.position, hit.point); //ī�޶� �ε����� ��ֹ��� �÷��̾�� �Ÿ�
            currentCameraZposition = -(distanceFromHitObject - cameraCollisionRadius); //ī�޶� ������ �տ� ��ġ�Ͽ� ����� �������� ����
        }

        if (Mathf.Abs(currentCameraZposition) < cameraCollisionRadius) //�÷��̾�� ��ֹ����� �Ÿ��� �ʹ� ������ �ƿ� ����
        {
            currentCameraZposition = -cameraCollisionRadius;
        }

        cameraPosition.z = Mathf.Lerp(mainCamera.transform.localPosition.z, currentCameraZposition, 0.1f); //Z�Ÿ� �ٲ�°� Lerp�ϰ� ����
        mainCamera.transform.localPosition = cameraPosition;  //���� �۾��� ����ī�޶� ���� �����ϴ� �κ�. 
    }
}
