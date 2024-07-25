using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]

public class PlayerData : ScriptableObject
{
    [Header("������")]
    [Tooltip("�÷��̾��� �̵��ӵ�")]
    public float moveSpeed;

    [Tooltip("�÷��̾��� ȸ���ӵ�")]
    public float playerRotateSpeed;

    [Header("���Ż ��ų")]
    [Tooltip("��ų ���� �߿� �ٸ� �ൿ�� �����ϴ� �ð�")]
    public float restrictTimeForNormalAttack1_1;
    public float restrictTimeForNormalAttack1_2;
    public float restrictTimeForNormalAttack1_3;

}