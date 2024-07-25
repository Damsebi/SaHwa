using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : ScriptableObject
{
    [Header("ü��")]
    public float f_hp;

    [Header("�޸��� �ӵ�")]
    public float f_moveSpeed;

    [Header("���� �ӵ�")]
    public float f_patrolSpeed;

    [Header("�þ� ����")]
    public float f_viewDistance;

    [Header("�þ� ��")]
    public float f_viewAngle;

    [Header("�÷��̾�κ��� �Ÿ� �ּ� �Ÿ� ����")]
    public float f_distanceToPlayerMin;

    [Header("��ĥ ���ϴ� �� Ƚ��")]
    public float f_paintOver;

    [Header("������ ����")]
    public float f_hpBarCount;

    [Header("�÷��̾� ��Ÿ�� �߰� ������ �߰�")]
    public bool b_plusAttackDamage;

    [Header("�÷��̾� ��ų�� �߰� ������ �߰�")]
    public bool b_plusSkillDamage;

    [Header("���� ����")]
    public bool b_magicGroup;
}
