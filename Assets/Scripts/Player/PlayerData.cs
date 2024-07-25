using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]

public class PlayerData : ScriptableObject
{
    [Header("������")]
    [Tooltip("���Ż �̵��ӵ�")]
    public float humanMoveSpeed;
    [Tooltip("����Ż�� �̵��ӵ�")]
    public float animalMoveSpeed;

    [Tooltip("�÷��̾��� ȸ���ӵ�")]
    public float playerRotateSpeed;

    [Space(20f)]
    [Header("���Ż ��ų")]
    [Tooltip("1��° ��Ÿ �����߿� �ٸ� �ൿ �����ϴ� �ð�")]
    public float restrictTimeForNormalAttack1_1;
    [Tooltip("2��° ��Ÿ �����߿� �ٸ� �ൿ �����ϴ� �ð�")]
    public float restrictTimeForNormalAttack1_2;
    [Tooltip("3��° ��Ÿ �����߿� �ٸ� �ൿ �����ϴ� �ð�")]
    public float restrictTimeForNormalAttack1_3;

    [Space(10f)]
    [Tooltip("�Ա�� ��ų ��Ÿ��")]
    public float inkPillarCooldown;
    [Tooltip("�Ա�� ��ų ������ ������ �� Ž�� ����")]
    public float inkPillarSkillRange;
    [Tooltip("�Ա�� ���� �ð�")]
    public float inkPillarDuration;
    [Tooltip("�Ա���� ũ�� ")]
    public Vector3 inkPillarScale;

    //�̵� playData�� 

    [Space(10f)]
    [Tooltip("���Ž� ��ų ��Ÿ��")]
    public float inkSmashCooldown;
    [Tooltip("���� �� ���� ���� �ð�")]
    public float inkSmashDuration;

    [Space(10f)]
    [Tooltip("ȸ�� ��ų ��Ÿ��")]
    public float avoidStepCooldown;

    [Space(20f)]
    [Header("����Ż ��ų")]
    [Tooltip("��Ÿ ��Ÿ��")]
    public float animalNormalAttackCooldown;

    [Tooltip("��� ������ ��ų ��Ÿ��")]
    public float xClawCooldown;

    [Tooltip("���� ���� ��ų ��Ÿ��")]
    public float leapClawCooldown;



    [Space(20f)]
    [Header("ī�޶�")]
    [Tooltip("���� X! ī�޶� ���� ���� (�÷��� �����̳� Ż��ü�� �� ���� �����)")]
    public float cameraHeight;

}