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
    public float humanFirstSkillCooldown;
    [Tooltip("먹기둥 스킬 공격이 가능한 적 탐지 범위")]
    public float humanFirstSkillRange;
    [Tooltip("먹기둥 지속 시간")]
    public float humanFirstSkillDuration;
    [Tooltip("먹기둥의 크기 ")]
    public Vector3 humanFirstSkillScale;

    //이동 playData에 

    [Space(10f)]
    [Tooltip("스매쉬 스킬 쿨타임")]
    public float humanSecondSkillCooldown;
    [Tooltip("퍼진 먹 영역 지속 시간")]
    public float humanSecondSkillDuration;

    [Space(10f)]
    [Tooltip("회피 스킬 쿨타임")]
    public float humanAvoidStepCooldown;

    [Space(20f)]
    [Header("동물탈 스킬")]
    [Tooltip("평타 쿨타임")]
    public float animalNormalAttackCooldown;

    [Space(10f)]
    [Tooltip("양손 할퀴기 스킬 쿨타임")]
    public float animalFirstSkillCooldown;

    [Space(10f)]
    [Tooltip("도약 공격 스킬 쿨타임")]
    public float animalSecondCooldown;

    [Space(10f)]
    [Tooltip("회피 스킬 쿨타임")]
    public float animalAvoidStepCooldown;

    [Space(20f)]
    [Header("카메라")]
    [Tooltip("카메라가 플레이어를 따라오는 속도비율. 값이 작을수록 빠름")]
    [Range(0, 1)] public float cameraFollowSpeed;

    [Tooltip("")]
    [Range(0, 1)] public float smoothVerticalRotationRate;


    [Tooltip("Left Shift로 정면회전시 회전시간 조정. 값이 작을수록 회전시간 짧음")]
    [Range(0, 0.1f)] public float turnForwardDuration;

    [Tooltip("Left Shift로 정면회전시 회전 부드러움 조정. 값이 작을수록 부드러움")]
    [Range(0, 1)] public float turnForwardSmoothRate;

    [Tooltip("Left Shift로 타겟락온시 회전 부드러움 조정. 값이 작을수록 부드러움")]
    [Range(0, 1)] public float turnToTargetSmoothRate;


    [Tooltip("주목할 대상 탐지 범위")]
    [Range(0, 20)] public float detectRange;

    [Tooltip("주목할 대상의 레이어(적, 아이템, npc 등등)")]
    public LayerMask targetLayer;

    [Tooltip("탐지할 때 최대 시야각 1/2 (= 플레이어 정면방향에서 타겟 사이 최대 각도")]
    [Range(0, 70)] public float maximumAngleWithTarget;

    //[Tooltip("카메라가 플레이어 정면 방향으로 회전하는 속도")]
    //[Range(0, 1)] public float alignCameraSmoothTime;

    [Tooltip("카메라 좌우 회전각")]
    [Range(0, 10)] public float cameraYValue;
}