using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]

public class PlayerData : ScriptableObject
{
    [Header("사람탈 이동속도")]
    public float humanMoveSpeed;
    [Header("동물탈의 이동속도")]
    public float animalMoveSpeed;
    [Header("플레이어의 회전속도")]
    public float playerRotateSpeed;

    [Space(20f)]
    [Header("사람탈 평타1-1 시전중에 다른 행동 제한하는 시간")]
    public float restrictTimeForNormalAttack1_1;
    [Header("사람탈 평타1-2 시전중에 다른 행동 제한하는 시간")]
    public float restrictTimeForNormalAttack1_2;
    [Header("사람탈 평타1-3 시전중에 다른 행동 제한하는 시간")]
    public float restrictTimeForNormalAttack1_3;

    [Space(10f)]
    [Header("먹기둥 스킬 쿨타임")]
    public float humanFirstSkillCooldown;
    [Header("먹기둥 스킬 공격이 가능한 적 탐지 범위")]
    public float humanFirstSkillRange;
    [Header("먹기둥 지속 시간")]
    public float humanFirstSkillDuration;
    [Header("먹기둥의 크기 ")]
    public Vector3 humanFirstSkillScale;

    [Space(10f)]
    [Header("스매쉬 스킬 쿨타임")]
    public float humanSecondSkillCooldown;
    [Header("퍼진 먹 영역 지속 시간")]
    public float humanSecondSkillDuration;

    [Space(10f)]
    [Header("사람탈 회피 스킬 쿨타임")]
    public float humanAvoidStepCooldown;

    [Space(20f)]
    [Header("동물탈 평타 쿨타임")]
    public float animalNormalAttackCooldown;

    [Space(10f)]
    [Header("양손 할퀴기 스킬 쿨타임")]
    public float animalFirstSkillCooldown;

    [Space(10f)]
    [Header("도약 공격 스킬 쿨타임")]
    public float animalSecondCooldown;

    [Space(10f)]
    [Header("동물탈 회피 스킬 쿨타임")]
    public float animalAvoidStepCooldown;

    [Space(20f)]
    [Header("카메라")]
    [Header("카메라가 플레이어를 따라오는 속도비율. 값이 작을수록 빠름")]
    [Range(0, 1)] public float cameraFollowSpeed;

    [Header("메인 카메라 X,Y,Z 위치값")]
    public Vector3 originCameraOrbitAxisPosition;
    
    [Header("위아래 회전 부드러움 정도")]
    [Range(0, 1)] public float smoothVerticalRotationRate;
    [Header("좌우 회전 부드러움 정도")]
    [Range(0, 1)] public float smoothHorizontalRotationRate;

    [Header("Left Shift로 정면회전시 회전시간 조정. 값이 작을수록 회전시간 짧음")]
    [Range(0, 0.1f)] public float turnForwardDuration;

    [Header("Left Shift로 정면회전시 회전 부드러움 조정. 값이 작을수록 부드러움")]
    [Range(0, 1)] public float turnForwardSmoothRate;

    [Header("Left Shift로 타겟락온시 회전 부드러움 조정. 값이 작을수록 부드러움")]
    [Range(0, 1)] public float turnToTargetSmoothRate;


    [Header("주목할 대상 탐지 범위")]
    [Range(0, 20)] public float detectRange;

    [Header("주목할 대상의 레이어(적, 아이템, npc 등등)")]
    public LayerMask targetLayer;

    [Header("최대 시야각 / 2")]
    [Range(0, 70)] public float maximumAngleWithTarget;

    //[Header("카메라가 플레이어 정면 방향으로 회전하는 속도")]
    //[Range(0, 1)] public float alignCameraSmoothTime;

    [Header("카메라 좌우 회전각")]
    [Range(0, 10)] public float cameraYValue;
}