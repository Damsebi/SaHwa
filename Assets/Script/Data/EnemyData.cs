using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : ScriptableObject
{
    [Header("ü��")]
    [Range(1f, 200f)] public float f_hp;

    [Header("�޸��� �ӵ�")]
    [Range(1f, 1000f)] public float f_trackingSpeed;

    [Header("���� �ӵ�")]
    [Range(1f, 1000f)] public float f_patrolSpeed;

    [Header("���� ����")]
    [Range(1f, 100f)] public float f_patrolRange;

    [Header("���� ȸ�� �ӵ�")]
    [Range(0.1f, 0.3f)] public float f_turnSmoothVelocity;

    [Header("�þ� ����")]
    [Range(1f, 100f)] public float f_viewDistance;

    [Header("�þ� ��")]
    [Range(1f, 359f)] public float f_viewAngle;

    [Header("��ĥ ���ϴ� �� Ƚ��")]
    [Range(1, 5)] public int f_paintOver;

    [Header("������ ����")]
    [Range(1, 3)] public int f_hpBarCount;

    [Header("�÷��̾� ��Ÿ�� �߰� ������ �߰�")]
    public bool b_plusAttackDamage;

    [Header("�÷��̾� ��ų�� �߰� ������ �߰�")]
    public bool b_plusSkillDamage;

    [Header("���� ����")]
    public bool b_magicGroup;   

}
