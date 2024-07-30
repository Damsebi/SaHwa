using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data Hybird", menuName = "Scriptable Object/Enemy Data/WeaponType/Hybird")]
public class EnemyDataHybird : EnemyData
{
    [Header("���� ��Ÿ ������")]
    public float f_flatMeleeAttackDamage;

    [Header("���� ��Ÿ ���� �ӵ�")]
    public float f_flatMeleeAttackMotionSpeed;

    [Header("���� ��Ÿ ���� ����")]
    public float f_flatMeleeAttackMotionCoolTime;

    [Header("���� ��Ÿ ���� ����")]
    public float f_flatMeleeAttackRange;

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
