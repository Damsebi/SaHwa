using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data Normal Ranged", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyDataNormalRanged : ScriptableObject
{
    //적 몬스터의 버프 같은걸 고려해서 최대와 현재로 나눔.

    public float f_hpMax;//최대 체력
    public float f_hpCurr;//현재 체력
    public float f_speedMax;//기본 속력
    public float f_speedCurr;//현재 속력 
    public float f_attackSpeedMax;//최대 공격 속도
    public float f_attackSpeedCurr;//현재 공격 속도
    public float f_attackDamage;//공격 데미지
    public float f_paintOver; //덧칠횟수
    public float f_lifeRound; //라이프 수
    public float f_motionSpeed; //모션 공격속도(숫자가 높을수록 빠름)
    public float f_motionCoolTime; //동작 이후 쿨타임(숫자가 적을 수록 빠름)
    public bool b_plusAttackDamage;//플레이어 평타에 데미지 추가
    public bool b_plusSkillDamage;//플레이어 스킬에 데미지 추가
}
