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
        movement.Normalize(); //���⺤�ͷ� ����
    }

    public void MoveWithCameraDirection() //ī�޶� �������� �÷��̾� ������
    {
        Vector3 cam = Camera.main.transform.forward; //���߿� ĳ�� ���°ɷ�

        //cam ���͸� Ȱ���Ͽ� movement�� ������Ʈ

        //movement �������� �̵�

        //movement �������� ȸ��
    }




}
