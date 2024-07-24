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
}