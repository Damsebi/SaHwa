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

    


}
