using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float hori;
    private float verti;
    private Vector3 movement;

    public void InputDirection(float horizontal,float vertical) //q
    {
        movement = new Vector3(horizontal, 0, vertical);
        movement.Normalize(); //방향벡터로 만듬
    }

    public void MoveWithCameraDirection() //카메라 기준으로 플레이어 움직임
    {
        Vector3 cam = Camera.main.transform.forward; //나중에 캐시 쓰는걸로

        //cam 벡터를 활용하여 movement를 업데이트

        //movement 방향으로 이동

        //movement 방향으로 회전
    }




}
