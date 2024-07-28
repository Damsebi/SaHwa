using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent (typeof (PlayerMovement), typeof(PlayerMaskChange), typeof(PlayerSkillSet))]
public class PlayerControllerDW : MonoBehaviour
{
    #region 수정부분
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
        #region 수정부분
        playerGameObject = transform.Find("HumanMaskCharacter").gameObject;
        #endregion
    }

    private void Start()
    {
        playerMaskChange.InitializeCharacterSetting();
    }

    private void Update()
    {
        #region 상태 체크
        playerSkillSet.AnimationState(); //애니메이션 상태 체크
        #endregion

        #region 이동 입력
        playerMovement.InputMovement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        #endregion

        if (playerSkillSet.RestrictForSkill) return; //행동 중에 제한

        #region 사람탈
        if (playerMaskChange.ActiveCharacter.name == "HumanMaskCharacter")
        {
            //이동 애니메이션에서 Setbool을 통해 연결되서 제한안걸어도 됨
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

        #region 동물탈
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

        #region 일반
        if (Input.GetKeyDown(KeyCode.F))
        {
            playerMaskChange.SwitchCharacter();
        }
        #endregion

        #region 귀신탈
        if (Input.GetKeyDown(KeyCode.D))
        {
            playerSkillSet.StartCoroutine(playerSkillSet.FinishSkill());
        }
        #endregion

        #region 타겟 탐색
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            PlayerFollowCamera.instance.DetectTarget();
        }
        #endregion

        #region 테스트용
        PlayerFollowCamera.instance.MoveToPoint();
        #endregion

    }

    private void FixedUpdate()
    {
        playerMovement.AlignCharactersPosition();

        if (playerSkillSet.RestrictForSkill) return; //행동 중 제한

        #region 움직임
        playerMovement.CharacterRotate();
        playerMovement.CharacterMovement();
        #endregion

        #region 카메라
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
