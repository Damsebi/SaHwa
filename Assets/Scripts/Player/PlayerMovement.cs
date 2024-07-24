using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Rigidbody rigidbody;

    //ī�޶� �������� ������, ȸ��
    private float moveAmount;
    private Vector3 movement;
    private Quaternion targetRotation;

    public void InputMovement(float hori, float verti) //�̵�, ȸ��
    {
        movement = new Vector3(hori, 0f, verti);
        movement.Normalize();

        //moveAmount �ʿ��Ѱ�

        //ī�޶� ���� �÷��̾� �̵� , ī�޶� �޾ƿͼ� ó��
        Vector3 cam = Camera.main.transform.forward;
        movement = Quaternion.LookRotation(new Vector3(cam.x, 0 , cam.z)) * movement;
        targetRotation = Quaternion.LookRotation(movement);
    }

    public void MovementWithCamera()
    {
        rigidbody.MovePosition(this.gameObject.transform.position + movement * playerData.moveSpeed * Time.deltaTime);
    }

    //Ÿ�� �������� ������, ȸ��


}
