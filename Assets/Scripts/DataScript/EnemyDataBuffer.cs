using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data Buffer", menuName = "Scriptable Objects/WeaponType/Buffer")]
public class EnemyDataBuffer : EnemyData
{
    [Header("버프 동작 속도")]
    public float f_buffMotionSpeed;

    [Header("버프 사이 간격")]
    public float f_buffMotionCoolTime;

    [Header("버프 범위")]
    public float f_buffRange;

    [Header("플레이어로부터 거리 최대 거리 유지")]
    public float f_distanceToPlayerMax;
}
