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

    [Space(20f)]
    [Header("사람탈 스킬")]
    [Tooltip("1번째 평타 시전중에 다른 행동 제한하는 시간")]
    public float restrictTimeForNormalAttack1_1;
    [Tooltip("2번째 평타 시전중에 다른 행동 제한하는 시간")]
    public float restrictTimeForNormalAttack1_2;
    [Tooltip("3번째 평타 시전중에 다른 행동 제한하는 시간")]
    public float restrictTimeForNormalAttack1_3;

    [Space(10f)]
    [Tooltip("먹기둥 스킬 쿨타임")]
    public float inkPillarCooldown;
    [Tooltip("먹기둥 스킬 공격이 가능한 적 탐지 범위")]
    public float inkPillarSkillRange;
    [Tooltip("먹기둥 지속 시간")]
    public float inkPillarDuration;
    [Tooltip("먹기둥의 크기 ")]
    public Vector3 inkPillarScale;

    //이동 playData에 

    [Space(10f)]
    [Tooltip("스매쉬 스킬 쿨타임")]
    public float inkSmashCooldown;
    [Tooltip("퍼진 먹 영역 지속 시간")]
    public float inkSmashDuration;

    [Space(10f)]
    [Tooltip("회피 스킬 쿨타임")]
    public float avoidStepCooldown;

    [Space(20f)]
    [Header("동물탈 스킬")]
    [Tooltip("평타 쿨타임")]
    public float animalNormalAttackCooldown;

    [Tooltip("양손 할퀴기 스킬 쿨타임")]
    public float xClawCooldown;

    [Tooltip("도약 공격 스킬 쿨타임")]
    public float leapClawCooldown;



    [Space(20f)]
    [Header("카메라")]
    [Tooltip("아직 X! 카메라 높이 조절 (플레이 시작이나 탈교체할 때 값이 적용됨)")]
    public float cameraHeight;

}