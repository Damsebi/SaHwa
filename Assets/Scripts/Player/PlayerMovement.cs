using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    private PlayerMaskChange playerMaskChange;
    private Rigidbody rigidbody;
    private Transform transform;

    private float moveAmount;
    private Vector3 movement;
    private Quaternion targetRotation;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        transform = GetComponent<Transform>();

        playerMaskChange = GetComponent<PlayerMaskChange>();
    }

    //방향 입력
    public void InputMovement(float horizontal, float vertical)
    {
        movement = new Vector3(horizontal, 0f, vertical);
        moveAmount = Mathf.Clamp01(Mathf.Abs(movement.x) + Mathf.Abs(movement.z));
        movement.Normalize(); 
    }

    //카메라 기준 회전, 이동
    public void MovementWithCamera() 
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, playerData.playerRotateSpeed);

        if (moveAmount > 0f)
        {
            Vector3 cam = Camera.main.transform.forward; //나중에 카메라쪽에서 받는걸로
            movement = Quaternion.LookRotation(new Vector3(cam.x, 0, cam.z)) * movement;
            targetRotation = Quaternion.LookRotation(movement);

            playerMaskChange.ActiveAnimator.SetFloat("moveAmount" , moveAmount); //반대방향 움직일때 멈추는거 수정
        }

        rigidbody.MovePosition(this.gameObject.transform.position + movement * playerData.moveSpeed * Time.deltaTime);
    }


    public void MovementWithTarget()
    {

    }


}
