using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data Melee", menuName = "Scriptable Object/Enemy Data/WeaponType/Melee")]
public class EnemyDataMelee : EnemyData
{
    [Header("근접 평타 데미지")]
    public float f_flatMeleeAttackDamage;

    [Header("근접 평타 동작 속도")]
    public float f_flatMeleeAttackMotionSpeed;

    [Header("근접 평타 사이 간격")]
    public float f_flatMeleeAttackMotionCoolTime;

    [Header("근접 평타 공격 범위")]
    public float f_flatMeleeAttackRange;

    [Header("순찰 최소 대기 시간")]
    [Range(1, 10)] public int i_patrolWaitingTimeMin;

    [Header("순찰 최대 대기 시간")]
    [Range(1, 10)] public int i_patrolWaitingTimeMax;
}
