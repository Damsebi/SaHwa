using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    private PlayerMaskChange playerMaskChange;
    private PlayerSkillSet playerSkillSet;

    private float moveAmount;
    public float MoveAmount { get { return moveAmount; } }

    private float moveSpeed;
    private Vector3 movement;
    private Quaternion targetRotation;
    //private bool isMove;
    
    public bool canRotate;
    public bool canMove;

    private void Awake()
    {
        playerMaskChange = GetComponent<PlayerMaskChange>();
        playerSkillSet = GetComponent<PlayerSkillSet>();
    }

    //���� �Է�
    public void InputMovement(float horizontal, float vertical)
    {
        movement = new Vector3(horizontal, 0f, vertical);
        moveAmount = Mathf.Clamp01(Mathf.Abs(movement.x) + Mathf.Abs(movement.z));
        movement.Normalize();

        playerMaskChange.ActiveAnimator.SetFloat("horizontal", horizontal);
        playerMaskChange.ActiveAnimator.SetFloat("vertical", vertical);
        playerMaskChange.ActiveAnimator.SetFloat("moveAmount", moveAmount);
    }

    //ī�޶� ���� ȸ��, �̵�
    public void CharacterMovement()
    {
        if (canMove)
        {
            if (playerMaskChange.ActiveCharacter.name == "HumanMaskCharacter")
            {
                moveSpeed = playerData.humanMoveSpeed;
            }
            else
            {
                moveSpeed = playerData.animalMoveSpeed;
            }

            playerMaskChange.ActiveRigidbody.MovePosition(playerMaskChange.ActiveCharacter.transform.position + movement * moveSpeed * Time.deltaTime);
        }
    }

    public void CharacterRotate() 
    {
        if (canRotate)
        {
            if (PlayerFollowCamera.instance.CurrentTarget)
            {
                Vector3 turnToTargetDirection = PlayerFollowCamera.instance.CurrentTarget.transform.position - playerMaskChange.ActiveCharacter.transform.position;
                turnToTargetDirection.y = 0;
                targetRotation = Quaternion.LookRotation(turnToTargetDirection);
                playerMaskChange.ActiveCharacter.transform.rotation = Quaternion.Slerp(playerMaskChange.ActiveCharacter.transform.rotation, targetRotation, .5f);
            }
            else
            {
                playerMaskChange.ActiveCharacter.transform.rotation
                = Quaternion.RotateTowards(playerMaskChange.ActiveCharacter.transform.rotation, targetRotation, playerData.playerRotateSpeed);
            }

            if (moveAmount > 0f)
            {
                Vector3 cam = Camera.main.transform.forward; //���߿� ī�޶��ʿ��� �޴°ɷ�
                movement = Quaternion.LookRotation(new Vector3(cam.x, 0, cam.z)) * movement;
                targetRotation = Quaternion.LookRotation(movement);
            }
        }

      
    }
    
    public void AlignCharactersPosition()
    {
        if (playerMaskChange.ActiveCharacter.name == "HumanMaskCharacter")
        {
            playerMaskChange.AnimalCharacter.transform.position = playerMaskChange.ActiveCharacter.transform.position;
        }
        else
        {
            playerMaskChange.HumanCharacter.transform.position = playerMaskChange.ActiveCharacter.transform.position;
        }
    }
}
