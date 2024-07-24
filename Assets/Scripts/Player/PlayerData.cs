using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]

public class PlayerData : ScriptableObject
{
    [Header("움직임")]
    [Tooltip("플레이어의 이동속도")]
    public float moveSpeed;

    [Tooltip("플레이어의 회전속도")]
    public float playerRotateSpeed;
}