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

    [Header("사람탈 스킬")]
    [Tooltip("스킬 진행 중에 다른 행동을 제한하는 시간")]
    public float restrictTimeForNormalAttack1_1;
    public float restrictTimeForNormalAttack1_2;
    public float restrictTimeForNormalAttack1_3;

}