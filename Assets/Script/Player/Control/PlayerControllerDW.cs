using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent (typeof (PlayerMovement), typeof(PlayerMaskChange), typeof(PlayerSkillSet))]
public class PlayerControllerDW : MonoBehaviour
{
    #region �����κ�
    private GameObject playerGameObject;

    public GameObject GetPlayerGameObject()
    {
        return playerGameObject;
    }
    #endregion

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
        #region �����κ�
        playerGameObject = transform.Find("HumanMaskCharacter").gameObject;
        #endregion
    }

    private void Start()
    {
        playerMaskChange.InitializeCharacterSetting();
    }

    private void Update()
    {
        #region ���� üũ
        playerSkillSet.AnimationState(); //�ִϸ��̼� ���� üũ
        #endregion

        #region �̵� �Է�
        playerMovement.InputMovement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        #endregion

        if (playerSkillSet.RestrictForSkill) return; //�ൿ �߿� ����

        #region ���Ż
        if (playerMaskChange.ActiveCharacter.name == "HumanMaskCharacter")
        {
            //�̵� �ִϸ��̼ǿ��� Setbool�� ���� ����Ǽ� ���ѾȰɾ ��
            if (Input.GetKeyDown(KeyCode.C))
            {
                playerSkillSet.StartCoroutine(playerSkillSet.HumanNormalAttack());
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                playerSkillSet.StartCoroutine(playerSkillSet.HumanFirstSkill());
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                playerSkillSet.StartCoroutine(playerSkillSet.HumanSecondSkill());
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerSkillSet.StartCoroutine(playerSkillSet.HumanAvoidBack());
            }
        }
        #endregion

        #region ����Ż
        if (playerMaskChange.ActiveCharacter.name == "AnimalMaskCharacter")
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                playerSkillSet.StartCoroutine(playerSkillSet.AnimalNormalAttack());

            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                playerSkillSet.StartCoroutine(playerSkillSet.AnimalFirstSkill());
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                playerSkillSet.StartCoroutine(playerSkillSet.AnimalSecondSkill());
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                playerSkillSet.StartCoroutine(playerSkillSet.AnimalAvoidBack());
            }
        }
        #endregion

        #region �Ϲ�
        if (Input.GetKeyDown(KeyCode.F))
        {
            playerMaskChange.SwitchCharacter();
        }
        #endregion

        #region �ͽ�Ż
        if (Input.GetKeyDown(KeyCode.D))
        {
            playerSkillSet.StartCoroutine(playerSkillSet.FinishSkill());
        }
        #endregion

        #region Ÿ�� Ž��
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            PlayerFollowCamera.instance.DetectTarget();
        }
        #endregion

        #region �׽�Ʈ��
        PlayerFollowCamera.instance.MoveToPoint();
        #endregion

    }

    private void FixedUpdate()
    {
        playerMovement.AlignCharactersPosition();

        if (playerSkillSet.RestrictForSkill) return; //�ൿ �� ����

        #region ������
        playerMovement.CharacterRotate();
        playerMovement.CharacterMovement();
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
