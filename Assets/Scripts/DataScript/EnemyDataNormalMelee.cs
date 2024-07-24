using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data Normal Melee", menuName = "ScriptableObjects/EnemyData", order = 2)]
public class EnemyDataNormalMelee : ScriptableObject
{
    //적 몬스터의 버프 같은걸 고려해서 최대와 현재로 나눔.

    public float hpMax;//최대 체력
    public float hpCurr;//현재 체력
    public float speedMax;//최대 속력 
    public float speedCurr;//현재 속력
    public float attackSpeedMax;//최대 공격 속도
    public float attackSpeedCurr;//현재 공격 속도
    public float attackDamage;//공격 데미지
}
