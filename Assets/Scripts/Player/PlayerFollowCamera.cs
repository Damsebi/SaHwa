using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    #region 변수
    [SerializeField] PlayerData playerData;
    [SerializeField] PlayerMaskChange playerMaskChange;
    public static PlayerFollowCamera instance;

    [Header("Follow Player")]
    [SerializeField] private Camera mainCamera;
    private Vector3 cameraVelocity;

    [Space(10f)]
    [Header("Camera Rotation")]
    float rotateCameraYValue;

    [Space(10f)]
    private bool isTurningForward;
    private bool canTurn;
    private bool checkCoroutineDone = true;


    [Space(10f)]
    [Header("Camera Collision")]
    [SerializeField] LayerMask cameraCollisionLayers;
    private float originCameraZPosition;
    private float currentCameraZposition;
    private float cameraCollisionRadius = 0.2f; //카메라가 장애물에 충돌시 반지름
    private Vector3 cameraPosition;

    [Space(10f)]
    [Header("LockOnTarget")]
    [SerializeField] private Collider currentTarget;
    public Collider CurrentTarget { get { return currentTarget; } }

    [Space(10f)]
    [SerializeField] bool showDetectRange;

    [Space(10f)]
    [SerializeField] GameObject targetMarkerUI;
    [SerializeField] GameObject targetMarker;
    #endregion

    #region 이벤트 함수(Awake, Start, FixedUpdate, Update, OnTrigger, OnDrawGizmos)
    private void Awake()
    {
        #region 싱글톤
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject); //이미 생성되어있으면 새로 만든 것 삭제
        #endregion
    }
    void Start()
    {
        originCameraZPosition = mainCamera.transform.localPosition.z;

    }
    private void FixedUpdate()
    {
        FollowPlayer();
        CameraRotation();

        //HandleCollisions();
        //MarkTarget();
    }

    #endregion

    #region 함수
    private void FollowPlayer()
    {
        Vector3 moveToPlayerPosition 
            = Vector3.SmoothDamp(this.transform.position, playerMaskChange.ActiveCharacter.transform.position, ref cameraVelocity, playerData.cameraFollowSpeed);
        this.transform.position = moveToPlayerPosition;

        //만약 락온일 때 카메라 시점이 변한다면 "mainCamera" Object의 position, rotation을 변경
    }
    private void CameraRotation()
    {
        if (currentTarget)
        {
            Vector3 directionTowardTarget = currentTarget.transform.position - transform.position;
            directionTowardTarget.Normalize();
            Quaternion LotateTowardTarget = Quaternion.LookRotation(directionTowardTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, LotateTowardTarget, playerData.turnToTargetSmoothRate);

            rotateCameraYValue = transform.rotation.eulerAngles.y;
        }
        else
        {
            if (isTurningForward) //leftShift 클릭을 통해 정면으로 회전
            {
                if (checkCoroutineDone) StartCoroutine(TurnDuration());
                transform.rotation = Quaternion.Slerp(transform.rotation, playerMaskChange.ActiveCharacter.transform.rotation, playerData.turnForwardSmoothRate);
                rotateCameraYValue = transform.rotation.eulerAngles.y;
            }
            else //평상시
            {   
                //float resetXValue = Mathf.Lerp(transform.rotation.eulerAngles.x, 0, playerData.smoothVerticalRotationRate);
                //transform.rotation = Quaternion.Euler(resetXValue, rotateCameraYValue, 0);
            }
        }
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

    public void RotateLeft()
    {
        transform.rotation
            = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - playerData.cameraYValue, transform.rotation.eulerAngles.z);
    }
    public void RotateRight()
    {
        transform.rotation
            = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + playerData.cameraYValue, transform.rotation.eulerAngles.z);
    }


    #region 락온 함수
    public void DetectTarget()
    {
        if (currentTarget) //락온 해제
        {
            currentTarget = null;
            //MarkTarget();
        }
        else
        {
            Collider[] colliders 
                = Physics.OverlapSphere(playerMaskChange.ActiveCharacter.transform.position, playerData.detectRange, playerData.targetLayer);

            if (colliders.Length != 0 && colliders != null)
            {
                float smallestAngle = Mathf.Infinity;

                for (int i = 0; i < colliders.Length; i++)
                {
                    Vector3 directionTowardTarget = colliders[i].transform.position - mainCamera.transform.position;
                    float angleWithTarget = Vector3.Angle(directionTowardTarget, mainCamera.transform.forward);
                    float distanceFromTarget = Vector3.Distance(playerMaskChange.ActiveCharacter.transform.position, colliders[i].transform.position);

                    if (distanceFromTarget > playerData.detectRange) continue; //타겟과 멀면 X
                    if (angleWithTarget > playerData.maximumAngleWithTarget) continue;
                    //if (detectedTarget.gameObject.GetComponent<Enemy>().dead) continue;

                    if (angleWithTarget < smallestAngle) 
                    {
                        //점점 각도가 작은 얘들 선별
                        smallestAngle = angleWithTarget;
                        currentTarget = colliders[i];
                    }
                }
            }

            //타겟이 있으면 락온, 없으면 정면보기
            if(currentTarget)
            {
                LockOnTarget();
            }
            else 
            {
                isTurningForward = true;
            }
        }
    }

    private void LockOnTarget()
    {
    }

    IEnumerator TurnDuration()
    {
        checkCoroutineDone = false;
        float angleWithPlayer = Vector3.Angle(playerMaskChange.ActiveCharacter.transform.forward, mainCamera.transform.forward);

        yield return new WaitForSeconds(angleWithPlayer * playerData.turnForwardDuration);

        isTurningForward = false;
        yield return null;
        checkCoroutineDone = true;
    }
    private void MarkTarget()
    {
        ////if (타겟이 죽는 경우) 비활성화
        //if (currentTarget)
        //{
        //    //if (currentTarget.gameObject.GetComponent<Enemy>().dead)//dead 전체 수정
        //    //{
        //    //    currentTarget = null;
        //    //}
        //    if (Vector3.Distance(currentTarget.transform.position, playerMaskChange.ActiveCharacter.transform.position) > detectRange)
        //    {
        //        currentTarget = null;
        //    }
        //    else
        //    {
        //        targetMarkerUI.SetActive(true);
        //        targetMarker.transform.position  //높이 수정하기
        //            = Camera.main.WorldToScreenPoint(currentTarget.transform.position);
        //    }
        //}
        //else
        //{
        //    targetMarkerUI.SetActive(false);
        //}
    }
    #endregion
    #endregion
}
