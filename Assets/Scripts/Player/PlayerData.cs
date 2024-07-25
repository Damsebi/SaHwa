using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]

public class PlayerData : ScriptableObject
{
    [Header("움직임")]
    [Tooltip("사람탈 이동속도")]
    public float humanMoveSpeed;
    [Tooltip("동물탈의 이동속도")]
    public float animalMoveSpeed;

    [Tooltip("플레이어의 회전속도")]
    public float playerRotateSpeed;

    [Header("사람탈 스킬")]
    [Tooltip("1번째 평타 시전중에 다른 행동 제한하는 시간")]
    public float restrictTimeForNormalAttack1_1;

    [Tooltip("2번째 평타 시전중에 다른 행동 제한하는 시간")]
    public float restrictTimeForNormalAttack1_2;
    
    [Tooltip("3번째 평타 시전중에 다른 행동 제한하는 시간")]
    public float restrictTimeForNormalAttack1_3;

    [Tooltip("먹기둥 스킬 시전중에 다른 행동 제한하는 시간")]
    public float humanInkPillarCooldown;
    [Tooltip("먹기둥 스킬 공격이 가능한 적 탐지 범위 ")]
    public float inkPillarSkillRange;



    [Header("카메라")]
    [Tooltip("아직 X! 카메라 높이 조절 (플레이 시작이나 탈교체할 때 값이 적용됨)")]
    public float cameraHeight;

}