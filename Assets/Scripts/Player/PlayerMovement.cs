using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Rigidbody rigidbody;

    //카메라 기준으로 움직임, 회전
    private float moveAmount;
    private Vector3 movement;
    private Quaternion targetRotation;

    public void InputMovement(float hori, float verti) //이동, 회전
    {
        movement = new Vector3(hori, 0f, verti);
        movement.Normalize();

        //moveAmount 필요한가

        //카메라 기준 플레이어 이동 , 카메라 받아와서 처리
        Vector3 cam = Camera.main.transform.forward;
        movement = Quaternion.LookRotation(new Vector3(cam.x, 0 , cam.z)) * movement;
        targetRotation = Quaternion.LookRotation(movement);
    }

    public void MovementWithCamera()
    {
        rigidbody.MovePosition(this.gameObject.transform.position + movement * playerData.moveSpeed * Time.deltaTime);
    }

    //타겟 기준으로 움직임, 회전


}
