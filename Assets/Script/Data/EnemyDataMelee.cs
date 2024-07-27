using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data Melee", menuName = "Scriptable Objects/WeaponType/Melee")]
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
}
