using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    #region ����
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
    private float cameraCollisionRadius = 0.2f; //ī�޶� ��ֹ��� �浹�� ������
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

    #region �̺�Ʈ �Լ�(Awake, Start, FixedUpdate, Update, OnTrigger, OnDrawGizmos)
    private void Awake()
    {
        #region �̱���
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject); //�̹� �����Ǿ������� ���� ���� �� ����
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

    #region �Լ�
    private void FollowPlayer()
    {
        Vector3 moveToPlayerPosition 
            = Vector3.SmoothDamp(this.transform.position, playerMaskChange.ActiveCharacter.transform.position, ref cameraVelocity, playerData.cameraFollowSpeed);
        this.transform.position = moveToPlayerPosition;

        //���� ������ �� ī�޶� ������ ���Ѵٸ� "mainCamera" Object�� position, rotation�� ����
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
            if (isTurningForward) //leftShift Ŭ���� ���� �������� ȸ��
            {
                if (checkCoroutineDone) StartCoroutine(TurnDuration());
                transform.rotation = Quaternion.Slerp(transform.rotation, playerMaskChange.ActiveCharacter.transform.rotation, playerData.turnForwardSmoothRate);
                rotateCameraYValue = transform.rotation.eulerAngles.y;
            }
            else //����
            {   
                //float resetXValue = Mathf.Lerp(transform.rotation.eulerAngles.x, 0, playerData.smoothVerticalRotationRate);
                //transform.rotation = Quaternion.Euler(resetXValue, rotateCameraYValue, 0);
            }
        }
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


    #region ���� �Լ�
    public void DetectTarget()
    {
        if (currentTarget) //���� ����
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

                    if (distanceFromTarget > playerData.detectRange) continue; //Ÿ�ٰ� �ָ� X
                    if (angleWithTarget > playerData.maximumAngleWithTarget) continue;
                    //if (detectedTarget.gameObject.GetComponent<Enemy>().dead) continue;

                    if (angleWithTarget < smallestAngle) 
                    {
                        //���� ������ ���� ��� ����
                        smallestAngle = angleWithTarget;
                        currentTarget = colliders[i];
                    }
                }
            }

            //Ÿ���� ������ ����, ������ ���麸��
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
        ////if (Ÿ���� �״� ���) ��Ȱ��ȭ
        //if (currentTarget)
        //{
        //    //if (currentTarget.gameObject.GetComponent<Enemy>().dead)//dead ��ü ����
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
        //        targetMarker.transform.position  //���� �����ϱ�
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
