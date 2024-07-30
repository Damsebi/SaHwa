using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data Range", menuName = "Scriptable Object/Enemy Data/WeaponType/Range")]
public class EnemyDataRange : EnemyData
{
    [Header("���Ÿ� ��Ÿ ������")]
    public float f_flatRangeAttackDamage;

    [Header("���Ÿ� ��Ÿ ���� �ӵ�")]
    public float f_flatRangeAttackMotionSpeed;

    [Header("���Ÿ� ��Ÿ ���� ����")]
    public float f_flatRangeAttackMotionCoolTime;

    [Header("���Ÿ� ��Ÿ ���� ����")]
    public float f_flatRangeAttackRange;

    [Header("�÷��̾�κ��� �Ÿ� �ִ� �Ÿ� ����")]
    public float f_distanceToPlayerMax;
}
