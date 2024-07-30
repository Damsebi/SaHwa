using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : Enemy
{
    public EnemyDataRange enemyDataRange;
    public GameObject stonePrefab;
    public Transform firePoint;

    #region SO 데이터(레인지 & 버퍼)
    [HideInInspector] public float distanceToPlayerMax; // 플레이어와의 최대 거리
    [HideInInspector] public bool isRanged; // 근거리 원거리 공격 체크
    [HideInInspector] public bool isBuffer; // 버퍼인지 체크
    [HideInInspector] public float lastAttackTime; // 마지막에 때린 시간
    [HideInInspector] public float attackCooltime; // 공격 쿨타임
    [HideInInspector] public float projectileSpeed; // 물체 속력

    #endregion


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        InitializeStats(enemyDataRange.f_hp, enemyDataRange.i_hpBarCount, enemyDataRange.f_trackingSpeed,
            enemyDataRange.f_patrolSpeed, enemyDataRange.f_viewAngle, enemyDataRange.f_viewDistance,
            enemyDataRange.i_paintOver, enemyDataRange.f_turnSmoothVelocity, enemyDataRange.b_plusAttackDamage,
            enemyDataRange.b_plusSkillDamage, enemyDataRange.b_magicGroup, enemyDataRange.f_patrolRange);

        flatDamage = enemyDataRange.f_flatRangeAttackDamage;
        flatMotionSpeed = enemyDataRange.f_flatRangeAttackMotionSpeed;
        flatMotionCoolTime = enemyDataRange.f_flatRangeAttackMotionCoolTime;
        flatRange = enemyDataRange.f_flatRangeAttackRange;
        attackCooltime = enemyDataRange.f_attackCooltime; 
        projectileSpeed = enemyDataRange.f_projectileSpeed; 
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (enemyState == eState.Attacking && Time.time > lastAttackTime + attackCooltime)
        {
            Attack();
        }
    }

    private void Attack()
    {
        Vector3 direction = (target.transform.position - firePoint.position).normalized;
        GameObject stoneObject = Instantiate(stonePrefab, firePoint.position, Quaternion.LookRotation(direction));
        Stone stone = stoneObject.GetComponent<Stone>();
        if (stone != null)
        {
            stone.damage = flatDamage; // Stone의 damage 값을 설정
        }
        Rigidbody rigidbody = stoneObject.GetComponent<Rigidbody>();
        rigidbody.velocity = direction * projectileSpeed;
        lastAttackTime = Time.time;
    }
}
