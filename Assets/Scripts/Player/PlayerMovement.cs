using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Transform transform;

    private float moveAmount; 
    private Vector3 movement;
    private Quaternion targetRotation;

    public void InputMovement(float hori, float verti) //ī�޶� ���� �̵����� ����
    {
        movement = new Vector3(hori, 0f, verti);
        moveAmount = Mathf.Clamp01(Mathf.Abs(movement.x) + Mathf.Abs(movement.z));
        movement.Normalize(); 
    }

    public void MovementWithCamera() //������ ������
    {
        if (moveAmount > 0f)
        {

            Vector3 cam = Camera.main.transform.forward; //���߿� ī�޶��ʿ��� �޴°ɷ�
            movement = Quaternion.LookRotation(new Vector3(cam.x, 0, cam.z)) * movement;
            targetRotation = Quaternion.LookRotation(movement);

            rigidbody.MovePosition(this.gameObject.transform.position + movement * playerData.moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, playerData.playerRotateSpeed);

        }
    }

    public Vector3 DebugMovement()
    {
        Vector3 cam = Camera.main.transform.forward; 
        movement = Quaternion.LookRotation(new Vector3(cam.x, 0, cam.z)) * movement;

        return movement;
    }


    //Ÿ�� �������� ������, ȸ��


}
