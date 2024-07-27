using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : ScriptableObject
{
    [Header("체력")]
    [Range(1f, 200f)] public float f_hp;

    [Header("달리기 속도")]
    [Range(1f, 1000f)] public float f_trackingSpeed;

    [Header("순찰 속도")]
    [Range(1f, 1000f)] public float f_patrolSpeed;

    [Header("순찰 범위")]
    [Range(1f, 100f)] public float f_patrolRange;

    [Header("몸통 회전 속도")]
    [Range(0.1f, 0.3f)] public float f_turnSmoothVelocity;

    [Header("시야 범위")]
    [Range(1f, 100f)] public float f_viewDistance;

    [Header("시야 각")]
    [Range(1f, 359f)] public float f_viewAngle;

    [Header("덧칠 당하는 총 횟수")]
    [Range(1, 5)] public int f_paintOver;

    [Header("라이프 갯수")]
    [Range(1, 3)] public int f_hpBarCount;

    [Header("플레이어 평타에 추가 데미지 추가")]
    public bool b_plusAttackDamage;

    [Header("플레이어 스킬에 추가 데미지 추가")]
    public bool b_plusSkillDamage;

    [Header("마법 세력")]
    public bool b_magicGroup;   

}
