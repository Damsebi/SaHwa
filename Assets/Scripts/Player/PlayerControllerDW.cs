using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent (typeof (PlayerMovement), typeof(PlayerMaskChange), typeof(PlayerSkillSet))]
[RequireComponent(typeof(PlayerFollowCamera))]
public class PlayerControllerDW : MonoBehaviour 
{
    private PlayerMovement playerMovement;
    private PlayerMaskChange playerMaskChange;
    private PlayerSkillSet playerSkillSet;
    private PlayerFollowCamera playerFollowCamera;

    private bool restrictControl;
    private bool isRestrictedControl;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerMaskChange = GetComponent<PlayerMaskChange>();
        playerSkillSet = GetComponent<PlayerSkillSet>();
        playerFollowCamera = GetComponent<PlayerFollowCamera>();
    }

    private void Start()
    {
        playerMaskChange.InitializeCharacterSetting();
        playerFollowCamera.CameraSetting();
    }

    private void Update()
    {
        if (playerSkillSet.RestrictForSkill) return; //행동 중에 제한

        playerMovement.InputMovement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(KeyCode.F))
        {
            playerMaskChange.SwitchCharacter();
            playerFollowCamera.CameraSetting();
        }

        #region 사람탈
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

        #region 동물탈
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

        #region 락온


        #endregion


    }

    private void FixedUpdate()
    {
        if (playerSkillSet.RestrictForSkill) return; //행동 중 제한

        playerMovement.MovementWithCamera();
    }
}
