using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    private PlayerMaskChange playerMaskChange;
    private PlayerData playerData;

    [SerializeField] private CinemachineFreeLook freeLookCamera;
    [SerializeField] private Transform humanCameraTarget;
    [SerializeField] private Transform animalCameraTarget;

    private void Awake()
    {
        playerMaskChange = GetComponent<PlayerMaskChange>();
    }

    public void CameraSetting()
    {
        freeLookCamera.Follow = playerMaskChange.ActiveCharacter.transform;

        if (playerMaskChange.ActiveCharacter.name == "HumanMaskCharacter")
        {
            freeLookCamera.LookAt = humanCameraTarget;
        }
        else
        {
            freeLookCamera.LookAt = animalCameraTarget;
        }
    }

    //left shift 누르면
    //      적 찾으면 look at을 락온에
    //      적 못 찾으면 플레이어 정면 보기 = x axis value를 0으로


    //왼쪽은 x axis value를 음수로
    //왼쪽은 x axis value를 양수로






}
