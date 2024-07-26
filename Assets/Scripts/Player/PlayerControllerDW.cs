using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent (typeof (PlayerMovement), typeof(PlayerMaskChange), typeof(PlayerSkillSet))]
public class PlayerControllerDW : MonoBehaviour 
{
    private PlayerMovement playerMovement;
    private PlayerMaskChange playerMaskChange;
    private PlayerSkillSet playerSkillSet;

    private bool restrictControl;
    private bool isRestrictedControl;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerMaskChange = GetComponent<PlayerMaskChange>();
        playerSkillSet = GetComponent<PlayerSkillSet>();
    }

    private void Start()
    {
        playerMaskChange.InitializeCharacterSetting();
    }

    private void Update()
    {
        if (playerSkillSet.RestrictForSkill) return; //�ൿ �߿� ����

        #region �̵�
        playerMovement.InputMovement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        #endregion

        #region �Ϲ�
        if (Input.GetKeyDown(KeyCode.F))
        {
            playerMaskChange.SwitchCharacter();
            //playerFollowCamera.CameraSetting();
        }
        #endregion

        #region ���Ż
        if (playerMaskChange.ActiveCharacter.name == "HumanMaskCharacter")
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                playerSkillSet.StartCoroutine(playerSkillSet.HumanNormalAttack());
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                playerSkillSet.StartCoroutine(playerSkillSet.HumanFirstSkill());
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                playerSkillSet.StartCoroutine(playerSkillSet.HumanSecondSkill());
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                playerSkillSet.StartCoroutine(playerSkillSet.HumanAvoidBack());
            }
        }
        #endregion

        #region ����Ż
        else
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                playerSkillSet.StartCoroutine(playerSkillSet.AnimalNormalAttack());

            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                playerSkillSet.StartCoroutine(playerSkillSet.AnimalFirstSkill());
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                playerSkillSet.StartCoroutine(playerSkillSet.AnimalSecondSkill());
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                playerSkillSet.StartCoroutine(playerSkillSet.AnimalAvoidBack());
            }
        }
        #endregion

        #region ī�޶�
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            PlayerFollowCamera.instance.DetectTarget();
        }
        #endregion
    }

    private void FixedUpdate()
    {
        if (playerSkillSet.RestrictForSkill) return; //�ൿ �� ����

        #region �̵�
        playerMovement.MovementWithCamera();
        #endregion

        #region ī�޶�
        if (Input.GetKey(KeyCode.Q))
        {
            PlayerFollowCamera.instance.RotateLeft();
        }
        else if (Input.GetKey(KeyCode.E))
        {
            PlayerFollowCamera.instance.RotateRight();
        }
        else
        {
            PlayerFollowCamera.instance.NotRotate();
        }

        #endregion
    }
}
