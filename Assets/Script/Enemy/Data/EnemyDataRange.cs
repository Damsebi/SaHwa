using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data Range", menuName = "Scriptable Object/Enemy Data/WeaponType/Range")]
public class EnemyDataRange : EnemyData
{
    [Header("원거리 평타 데미지")]
    public float f_flatRangeAttackDamage;

    [Header("원거리 평타 동작 속도")]
    public float f_flatRangeAttackMotionSpeed;

    [Header("원거리 평타 사이 간격")]
    public float f_flatRangeAttackMotionCoolTime;

    [Header("원거리 평타 공격 범위")]
    public float f_flatRangeAttackRange;

    [Header("플레이어로부터 거리 최대 거리 유지")]
    public float f_distanceToPlayerMax;
}
