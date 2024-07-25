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

    [Header("���Ż ��ų")]
    [Tooltip("1��° ��Ÿ �����߿� �ٸ� �ൿ �����ϴ� �ð�")]
    public float restrictTimeForNormalAttack1_1;

    [Tooltip("2��° ��Ÿ �����߿� �ٸ� �ൿ �����ϴ� �ð�")]
    public float restrictTimeForNormalAttack1_2;
    
    [Tooltip("3��° ��Ÿ �����߿� �ٸ� �ൿ �����ϴ� �ð�")]
    public float restrictTimeForNormalAttack1_3;

    [Tooltip("�Ա�� ��ų �����߿� �ٸ� �ൿ �����ϴ� �ð�")]
    public float humanInkPillarCooldown;
    [Tooltip("�Ա�� ��ų ������ ������ �� Ž�� ���� ")]
    public float inkPillarSkillRange;



    [Header("ī�޶�")]
    [Tooltip("���� X! ī�޶� ���� ���� (�÷��� �����̳� Ż��ü�� �� ���� �����)")]
    public float cameraHeight;

}