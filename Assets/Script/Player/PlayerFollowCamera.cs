using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] PlayerMaskChange playerMaskChange;
    public static PlayerFollowCamera instance;

    [SerializeField] private Camera mainCamera;
    public Camera MainCamera { get { return mainCamera; } }
    private Vector3 cameraVelocity;

    float rotateCameraYValue;
    float rotateCameraXValue;
    
    private bool isTurningForward;
    private bool isTurningLeftRight;
    private bool canTurn;
    private bool checkCoroutineDone = true;

    public Transform[] pointers;

    //[SerializeField] LayerMask cameraCollisionLayers;
    //private float originCameraZPosition;
    //private float currentCameraZposition;
    //private float cameraCollisionRadius = 0.2f; //카메라가 장애물에 충돌시 반지름
    //private Vector3 cameraPosition;

    #region 탐지 및 락온
    [SerializeField] private Collider currentTarget;
    public Collider CurrentTarget { get { return currentTarget; } }

    [SerializeField] bool showDetectRange;

    [SerializeField] GameObject targetMarkerUI;
    [SerializeField] GameObject targetMarker;
    #endregion

    #region 이벤트 함수
    private void Awake()
    {
        #region 싱글톤
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject); //이미 생성되어있으면 새로 만든 것 삭제
        #endregion
    }
    void Start()
    {
        //originCameraZPosition = mainCamera.transform.localPosition.z;
        rotateCameraYValue = playerMaskChange.ActiveCharacter.transform.eulerAngles.y; //그냥 rotation.y는 쿼터니언
    }
    private void FixedUpdate()
    {
        FollowPlayer();
        CameraRotation();

        //HandleCollisions();
        //MarkTarget();
    }
    #endregion


    #region 플레이어 팔로우, 기본 회전
    private void FollowPlayer()
    {
        Vector3 moveToPlayerPosition 
            = Vector3.SmoothDamp(this.transform.position, playerMaskChange.ActiveCharacter.transform.position + playerData.originCameraOrbitAxisPosition, ref cameraVelocity, playerData.cameraFollowSpeed);
        this.transform.position = moveToPlayerPosition;

        //만약 락온일 때 카메라 시점이 변한다면 "mainCamera" Object의 position, rotation을 변경
    }
    private void CameraRotation()
    {
        if (currentTarget)
        {
            LockOnTarget();

            if (Vector3.Distance(playerMaskChange.ActiveCharacter.transform.position, currentTarget.transform.position) > playerData.detectRange)
            {
                currentTarget = null;
            }
        }
        else
        {
            if (isTurningForward) //leftShift 클릭을 통해 정면으로 회전
            {
                if (checkCoroutineDone) StartCoroutine(TurnForward());
                transform.rotation = Quaternion.Slerp(transform.rotation, playerMaskChange.ActiveCharacter.transform.rotation, playerData.turnForwardSmoothRate);
                rotateCameraYValue = transform.rotation.eulerAngles.y;
            }
            else if (!isTurningLeftRight)
            {
                //락온해제시, 카메라 위아래 회전 초기화
                float resetXValue
                    = Mathf.Lerp(transform.rotation.eulerAngles.x, rotateCameraXValue, playerData.smoothVerticalRotationRate);
                if (resetXValue < -80) resetXValue = transform.rotation.eulerAngles.x;
                transform.rotation = Quaternion.Euler(0, rotateCameraYValue, 0);
            }
        }
    }
    #endregion

    #region 카메라 장애물 충돌
    //private void HandleCollisions()
    //{
    //    currentCameraZposition = originCameraZPosition; //카메라의 로컬 z좌표(거리)

    //    RaycastHit hit;
    //    Vector3 direction = mainCamera.transform.position - transform.position; //카메라 공전축에서 카메라방향
    //    direction.Normalize();

    //    //sphere 모양의 raycast. 플레이어에서 시작, 크기, 방향, 결과, 최대거리, 해당레이어
    //    if (Physics.SphereCast(transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(currentCameraZposition), cameraCollisionLayers))
    //    {
    //        float distanceFromHitObject = Vector3.Distance(transform.position, hit.point); //카메라에 부딪히는 장애물과 플레이어간의 거리
    //        currentCameraZposition = -(distanceFromHitObject - cameraCollisionRadius); //카메라가 벽보다 앞에 위치하여 제대로 찍히도록 만듬
    //    }

    //    if (Mathf.Abs(currentCameraZposition) < cameraCollisionRadius) //플레이어와 장애물간의 거리가 너무 좁으면 아예 고정
    //    {
    //        currentCameraZposition = -cameraCollisionRadius;
    //    }

    //    cameraPosition.z = Mathf.Lerp(mainCamera.transform.localPosition.z, currentCameraZposition, 0.1f); //Z거리 바뀌는걸 Lerp하게 진행
    //    mainCamera.transform.localPosition = cameraPosition;  //위의 작업을 메인카메라에 실제 적용하는 부분. 
    //}
    #endregion

    #region 좌우 회전
    public void RotateLeft()
    {
        transform.rotation
            = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - playerData.cameraYValue, transform.rotation.eulerAngles.z);
        
        rotateCameraYValue = transform.rotation.eulerAngles.y;
        
        isTurningLeftRight = true;
    }
    public void RotateRight()
    {
        transform.rotation
            = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + playerData.cameraYValue, transform.rotation.eulerAngles.z);

        rotateCameraYValue = transform.rotation.eulerAngles.y;

        isTurningLeftRight = true;
    }
    public void NotRotate()
    {
        isTurningLeftRight = false;
    }
    #endregion

    #region 락온 함수
    public void DetectTarget()
    {
        if (currentTarget) //락온 해제
        {
            currentTarget = null;
            if (!currentTarget) playerMaskChange.ActiveAnimator.SetBool("isFocused", false);
            return;
            //MarkTarget();
        }

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

        if(currentTarget) //타겟이 있으면 락온
        {
            LockOnTarget();
        }
        else //타겟이 없으면 정면보기 허용
        {
            isTurningForward = true;
        }
    }
    private void LockOnTarget()
    {
        //타겟 바라보기
        if(currentTarget) playerMaskChange.ActiveAnimator.SetBool("isFocused", true);

        Vector3 directionTowardTarget = currentTarget.transform.position - transform.position;
        directionTowardTarget.Normalize();
        Quaternion LotateTowardTarget = Quaternion.LookRotation(directionTowardTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, LotateTowardTarget, playerData.turnToTargetSmoothRate);

        rotateCameraYValue = transform.rotation.eulerAngles.y;
    }
    private IEnumerator TurnForward()
    {
        checkCoroutineDone = false;

        //캐릭터 정면과 카메라와의 각도
        float angleWithPlayer 
            = Vector3.Angle(playerMaskChange.ActiveCharacter.transform.forward, mainCamera.transform.forward);

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

    
}
