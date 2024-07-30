using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : Enemy
{
    public EnemyDataRange enemyDataRange;
    public GameObject stonePrefab;
    public Transform firePoint;

    #region SO ������(������ & ����)
    [HideInInspector] public float distanceToPlayerMax; // �÷��̾���� �ִ� �Ÿ�
    [HideInInspector] public bool isRanged; // �ٰŸ� ���Ÿ� ���� üũ
    [HideInInspector] public bool isBuffer; // �������� üũ
    [HideInInspector] public float lastAttackTime; // �������� ���� �ð�
    [HideInInspector] public float attackCooltime; // ���� ��Ÿ��
    [HideInInspector] public float projectileSpeed; // ��ü �ӷ�

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
            stone.damage = flatDamage; // Stone�� damage ���� ����
        }
        Rigidbody rigidbody = stoneObject.GetComponent<Rigidbody>();
        rigidbody.velocity = direction * projectileSpeed;
        lastAttackTime = Time.time;
    }
}
