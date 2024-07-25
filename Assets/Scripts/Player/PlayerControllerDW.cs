using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof (PlayerMovement), typeof(PlayerMaskChange), typeof(PlayerSkillSet))]
[RequireComponent(typeof(PlayerFollowCamera))]
public class PlayerControllerDW : MonoBehaviour 
{
    private PlayerMovement playerMovement;
    private PlayerMaskChange playerMaskChange;
    private PlayerSkillSet playerSkillSet;

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
        playerMovement.InputMovement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (playerMaskChange.ActiveCharacter == playerMaskChange.HumanCharacter)
            {
                playerSkillSet.StartCoroutine(playerSkillSet.HumanNormalAttack());
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            playerMaskChange.SwitchCharacter();
        }



    }

    private void FixedUpdate()
    {
        playerMovement.MovementWithCamera();
    }
}
