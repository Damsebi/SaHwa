using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data Melee", menuName = "Scriptable Object/Enemy Data/WeaponType/Melee")]
public class EnemyDataMelee : EnemyData
{
    [Header("���� ��Ÿ ������")]
    public float f_flatMeleeAttackDamage;

    [Header("���� ��Ÿ ���� �ӵ�")]
    public float f_flatMeleeAttackMotionSpeed;

    [Header("���� ��Ÿ ���� ����")]
    public float f_flatMeleeAttackMotionCoolTime;

    [Header("���� ��Ÿ ���� ����")]
    public float f_flatMeleeAttackRange;

    [Header("���� �ּ� ��� �ð�")]
    [Range(1, 10)] public int i_patrolWaitingTimeMin;

    [Header("���� �ִ� ��� �ð�")]
    [Range(1, 10)] public int i_patrolWaitingTimeMax;
}
