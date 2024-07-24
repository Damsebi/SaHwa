using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataBuffer", menuName = "ScriptableObjects/EnemyData")]
public class EnemyDataBuffer : ScriptableObject
{
    //적 몬스터의 버프 같은걸 고려해서 최대와 현재로 나눔.

    public float hpMax;//최대 체력
    public float hpCurr;//현재 체력
    public float speedMax;//최대 속력 
    public float speedCurr;//현재 속력
    public float buffRange;
}
