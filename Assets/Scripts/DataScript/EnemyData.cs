using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : ScriptableObject
{
    [Header("체력")]
    public float f_hp;

    [Header("달리기 속도")]
    public float f_moveSpeed;

    [Header("순찰 속도")]
    public float f_patrolSpeed;

    [Header("시야 범위")]
    public float f_viewDistance;

    [Header("시야 각")]
    public float f_viewAngle;

    [Header("플레이어로부터 거리 최소 거리 유지")]
    public float f_distanceToPlayerMin;

    [Header("덧칠 당하는 총 횟수")]
    public float f_paintOver;

    [Header("라이프 갯수")]
    public float f_hpBarCount;

    [Header("플레이어 평타에 추가 데미지 추가")]
    public bool b_plusAttackDamage;

    [Header("플레이어 스킬에 추가 데미지 추가")]
    public bool b_plusSkillDamage;

    [Header("마법 세력")]
    public bool b_magicGroup;
}
