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
    public float humanFirstSkillCooldown;
    [Tooltip("�Ա�� ��ų ������ ������ �� Ž�� ����")]
    public float humanFirstSkillRange;
    [Tooltip("�Ա�� ���� �ð�")]
    public float humanFirstSkillDuration;
    [Tooltip("�Ա���� ũ�� ")]
    public Vector3 humanFirstSkillScale;

    //�̵� playData�� 

    [Space(10f)]
    [Tooltip("���Ž� ��ų ��Ÿ��")]
    public float humanSecondSkillCooldown;
    [Tooltip("���� �� ���� ���� �ð�")]
    public float humanSecondSkillDuration;

    [Space(10f)]
    [Tooltip("ȸ�� ��ų ��Ÿ��")]
    public float humanAvoidStepCooldown;

    [Space(20f)]
    [Header("����Ż ��ų")]
    [Tooltip("��Ÿ ��Ÿ��")]
    public float animalNormalAttackCooldown;

    [Space(10f)]
    [Tooltip("��� ������ ��ų ��Ÿ��")]
    public float animalFirstSkillCooldown;

    [Space(10f)]
    [Tooltip("���� ���� ��ų ��Ÿ��")]
    public float animalSecondCooldown;

    [Space(10f)]
    [Tooltip("ȸ�� ��ų ��Ÿ��")]
    public float animalAvoidStepCooldown;

    [Space(20f)]
    [Header("ī�޶�")]
    [Tooltip("ī�޶� �÷��̾ ������� �ӵ�����. ���� �������� ����")]
    [Range(0, 1)] public float cameraFollowSpeed;

    [Tooltip("")]
    [Range(0, 1)] public float smoothVerticalRotationRate;


    [Tooltip("Left Shift�� ����ȸ���� ȸ���ð� ����. ���� �������� ȸ���ð� ª��")]
    [Range(0, 0.1f)] public float turnForwardDuration;

    [Tooltip("Left Shift�� ����ȸ���� ȸ�� �ε巯�� ����. ���� �������� �ε巯��")]
    [Range(0, 1)] public float turnForwardSmoothRate;

    [Tooltip("Left Shift�� Ÿ�ٶ��½� ȸ�� �ε巯�� ����. ���� �������� �ε巯��")]
    [Range(0, 1)] public float turnToTargetSmoothRate;


    [Tooltip("�ָ��� ��� Ž�� ����")]
    [Range(0, 20)] public float detectRange;

    [Tooltip("�ָ��� ����� ���̾�(��, ������, npc ���)")]
    public LayerMask targetLayer;

    [Tooltip("Ž���� �� �ִ� �þ߰� 1/2 (= �÷��̾� ������⿡�� Ÿ�� ���� �ִ� ����")]
    [Range(0, 70)] public float maximumAngleWithTarget;

    //[Tooltip("ī�޶� �÷��̾� ���� �������� ȸ���ϴ� �ӵ�")]
    //[Range(0, 1)] public float alignCameraSmoothTime;

    [Tooltip("ī�޶� �¿� ȸ����")]
    [Range(0, 10)] public float cameraYValue;
}