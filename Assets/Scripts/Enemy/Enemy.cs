using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq.Expressions;
using UnityEngine.AI;
using Unity.XR.Oculus.Input;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Enemy : MonoBehaviour, IDamageable
{
    #region ����
    private enum eState
    {
        Patrol, //����
        Tracking, //����
        AttackBegin, //���ݽ���
        Attacking, //������
        Idle, // ��� ����
        BatIdle //���� ������
    }
    private eState enemyState;
    #endregion
    #region ����
    public EnemyData enemyData;

    [HideInInspector] public float hp; //ü��
    [HideInInspector] public float hpBarCount; //ü�¹� ����
    [HideInInspector] public float moveSpeed; //�̵� �ӵ�
    [HideInInspector] public float patrolSpeed; //���� �ӵ�
    [HideInInspector] public float viewDistance; // �þ� ����
    [HideInInspector] public float viewAngle; // �þ� ��
    [HideInInspector] public float distanceToPlayerMin; // �÷��̾���� �ּ� �Ÿ�
    [HideInInspector] public float distanceToPlayerMax; // �÷��̾���� �ִ� �Ÿ�
    [HideInInspector] public float paintOver; // ��ĥ Ƚ��

    [HideInInspector] public float flatDamage; // ��Ÿ ������
    [HideInInspector] public float flatMotionSpeed; // ��Ÿ ��� �ӵ�
    [HideInInspector] public float flatMotionCoolTime; // ��Ÿ ��� ��Ÿ��
    [HideInInspector] public float flatRange; // ��Ÿ ����

    [HideInInspector] public float flatHybridDamage; // ���̺긮�� ���Ÿ� ��Ÿ ������
    [HideInInspector] public float flatHybridMotionSpeed; // ���̺긮�� ���Ÿ� ��Ÿ ��� �ӵ�
    [HideInInspector] public float flatHybridMotionCoolTime; // ���̺긮�� ���Ÿ� ��Ÿ ��� ��Ÿ��
    [HideInInspector] public float flatHybridRange; // ���̺긮�� ���Ÿ� ��Ÿ ����

    [HideInInspector] public float skillDamage; // ��ų ������
    [HideInInspector] public float skillMotionSpeed; // ��ų ��� �ӵ�
    [HideInInspector] public float skillMotionCoolTime; // ��ų ��� ��Ÿ��
    [HideInInspector] public float skillRange; // ��ų ����

    private Animator animator;
    private NavMeshAgent navAgent;

    private float turnSmoothVelocity; //������ ȸ�� �ӵ�
    private float turnSmoothTime = 0.1f; //���� ȸ�� �� �����ð�

    private float lastDamagedTime; //���������� ���� �޾Ҵ� �ð�
    private const float MIN_TIME_BET_DAMAGE = 0.1f;

    [HideInInspector] public bool plusAttackDamage; // �÷��̾� ��Ÿ�� �߰� ������ �߰�
    [HideInInspector] public bool plusSkillDamage; // �÷��̾� ��ų�� �߰� ������ �߰�
    [HideInInspector] public bool magicGroup; // ���� ����
    private float patrolRange; // ���� ����
    private bool isRanged; // �ٰŸ� ���Ÿ� ���� üũ
    private bool isBuffer; // �������� üũ

    public LayerMask LayerTarget;

    private float lostSightTime = 1.0f;
    private float lostSightTimer = 0.0f;

    private bool isDead; // ��� üũ

    public Transform attackRoot; //������ ���۵Ǵ� �ǹ�, �� �ǹ� �ش� �ݰ� ���� �ִ� �÷��̾ ���ݴ���
    public Transform viewTransform; //�� ��ġ

    public PlayerConJS player;

    private bool hasTarget
    {
        get
        {
            if (player != null && !player.isDead)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    #endregion

    #region Awake()
    private void Awake()
    {
        hp = enemyData.f_hp;
        hpBarCount = enemyData.f_hpBarCount;
        moveSpeed = enemyData.f_moveSpeed;
        patrolSpeed = enemyData.f_patrolSpeed;
        viewAngle = enemyData.f_viewAngle;
        viewDistance = enemyData.f_viewDistance;
        distanceToPlayerMin = enemyData.f_distanceToPlayerMin;
        paintOver = enemyData.f_paintOver;
        turnSmoothVelocity = enemyData.f_turnSmoothVelocity;
        plusAttackDamage = enemyData.b_plusAttackDamage;
        plusSkillDamage = enemyData.b_plusSkillDamage;
        magicGroup = enemyData.b_magicGroup;
        patrolRange = enemyData.f_patrolRange;
        //animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();

        //if (animator == null)
        //{
        //    Debug.LogError("�ִϸ����� ����.");
        //}

        if (navAgent == null)
        {
            Debug.LogError("�׺�޽� ����.");
        }
        var attackPivot = attackRoot.position;
        attackPivot.y = transform.position.y;
        //���̸� �����ϰ� ������ �Ÿ��� ���

        distanceToPlayerMin = Vector3.Distance(transform.position, attackRoot.position) + flatRange;
    }
    #endregion

    #region Start()
    void Start()
    {
        StartCoroutine(UpdatePath());
        InitializeStats();
    }
    #endregion

    #region Update()
    void Update()
    {
        if (isDead)
        {
            return;
        }
        switch (enemyState)
        {
            case eState.Patrol:
                break;
            case eState.Idle:
                break;
            case eState.Tracking:
                var distance = Vector3.Distance(player.transform.position, transform.position);
                if (distance <= distanceToPlayerMin)
                {
                    BeginAttack();
                }
                break;
        }
    }
    #endregion

    #region FixedUpdate()
    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }
        if (enemyState == eState.AttackBegin || enemyState == eState.Attacking)
        {
            var lookRotation = Quaternion.LookRotation(player.transform.position - transform.position);
            var targetAngleY = lookRotation.eulerAngles.y;

            targetAngleY = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY, ref turnSmoothVelocity, turnSmoothTime);
            transform.eulerAngles = Vector3.up * targetAngleY;
            Attack();
        }
    }
    #endregion


    #region �ʱ�ȭ
    private void InitializeStats()
    {
        if (enemyData is EnemyDataMelee meleeData)
        {
            flatDamage = meleeData.f_flatMeleeAttackDamage;
            flatMotionSpeed = meleeData.f_flatMeleeAttackMotionSpeed;
            flatMotionCoolTime = meleeData.f_flatMeleeAttackMotionCoolTime;
            flatRange = meleeData.f_flatMeleeAttackRange;
            isRanged = false;
            isBuffer = false;
        }
        else if (enemyData is EnemyDataRange rangeData)
        {
            flatDamage = rangeData.f_flatRangeAttackDamage;
            flatMotionSpeed = rangeData.f_flatRangeAttackMotionSpeed;
            flatMotionCoolTime = rangeData.f_flatRangeAttackMotionCoolTime;
            flatRange = rangeData.f_flatRangeAttackRange;
            isRanged = true;
            isBuffer = false;
        }
        else if (enemyData is EnemyDataHybird hybridData)
        {
            flatDamage = hybridData.f_flatMeleeAttackDamage;
            flatMotionSpeed = hybridData.f_flatMeleeAttackMotionSpeed;
            flatMotionCoolTime = hybridData.f_flatMeleeAttackMotionCoolTime;
            flatRange = hybridData.f_flatMeleeAttackRange;

            flatHybridDamage = hybridData.f_flatRangeAttackDamage;
            flatHybridMotionSpeed = hybridData.f_flatRangeAttackMotionSpeed;
            flatHybridMotionCoolTime = hybridData.f_flatRangeAttackMotionCoolTime;
            flatHybridRange = hybridData.f_flatRangeAttackRange;
            isRanged = true;
            isBuffer = false;
        }
        else if (enemyData is EnemyDataBuffer bufferData)
        {
            flatMotionSpeed = bufferData.f_buffMotionSpeed;
            flatMotionCoolTime = bufferData.f_buffMotionCoolTime;
            flatRange = bufferData.f_buffRange;
            isRanged = false;
            isBuffer = true;
        }
    }
    #endregion

    #region �þ� �׸���
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (attackRoot != null && !isBuffer && !isRanged)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);//������
            Gizmos.DrawSphere(attackRoot.position, flatRange);
            //���� ����� �ٰŸ� ���� ����
        }
        else if (attackRoot != null && !isBuffer && isRanged)
        {
            //Gizmos.color = new Color(1f, 0f, 0f, 0.5f);//������
            //Vector3 cubeSize = new Vector3(0.3f, 0.3f, flatRange); // width, height, depth
            //Vector3 cubeCenter = attackRoot.position + attackRoot.forward * (flatRange / 2);
            //Gizmos.DrawCube(cubeCenter, cubeSize);
            ////�� ���簢������� ���Ÿ� ���� ����

            Gizmos.color = new Color(1f, 0f, 0f, 0.5f); // ������
            Vector3 cubeSize = new Vector3(0.3f, 0.3f, flatRange); // width, height, depth
            Matrix4x4 oldMatrix = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(attackRoot.position, attackRoot.rotation, attackRoot.lossyScale);
            Gizmos.DrawCube(-Vector3.forward * (flatRange / 2), cubeSize);
            Gizmos.matrix = oldMatrix;

        }
        else if (attackRoot != null && isBuffer && !isRanged)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);//������
            Gizmos.DrawWireSphere(attackRoot.position, flatRange);
            //������� ���Ÿ� ���� ����
        }

        if (viewTransform != null) //�ܼ��� �ݰ� ǥ��
        {
            var leftViewRotation = Quaternion.AngleAxis(-viewAngle * 0.5f, Vector3.up); //����~���� ����
            var leftRayDirection = leftViewRotation * transform.forward;

            Handles.color = new Color(1f, 1f, 1f, 0.2f);//���
            Handles.DrawSolidArc(viewTransform.position, Vector3.up, leftRayDirection, viewAngle, viewDistance);
            //(�߽�, ������ �Ʒ��� ���̰�, ��ũ�� �׸��� �����ϴ� ���� ���� , ��ü �þ߰� /2 , ��ä���� ������
            //��ä�� ����� �þ߰� �����(gui)
        }
    }
#endif
    #endregion

    private void BeginAttack()
    {
        enemyState = eState.AttackBegin;
    }
    private void Attack()
    {
        enemyState = eState.Attacking;

        if (hasTarget)
        {
            // �÷��̾�� �������� ��
            var message = new DamageMessage();
            message.amount = flatDamage;
            message.damager = this.gameObject;
            message.hitPoint = player.transform.position;
            message.hitNormal = (player.transform.position - transform.position).normalized;

            player.ApplyDamage(message);
        }
    }

    private IEnumerator UpdatePath()
    {
        while (!isDead)
        {
            if (hasTarget)
            {
                if (enemyState == eState.Patrol || enemyState == eState.Idle)
                {
                    enemyState = eState.Tracking;
                    navAgent.speed = moveSpeed;
                }
                navAgent.SetDestination(player.transform.position);

                if (!IsTargetOnSight(player.transform))
                {
                    lostSightTimer += Time.deltaTime;
                    if (lostSightTimer >= lostSightTime)
                    {
                        player = null;
                        enemyState = eState.Patrol;
                        navAgent.speed = patrolSpeed;
                    }
                }
                else
                {
                    lostSightTimer = 0.0f; // Ÿ���� �ٽ� �þ߿� ������ Ÿ�̸� �ʱ�ȭ
                }
            }
            else
            {
                if (enemyState != eState.Patrol)
                {
                    enemyState = eState.Patrol;
                    navAgent.speed = patrolSpeed;
                }

                if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
                {
                    enemyState = eState.Idle;
                    navAgent.isStopped = true;
                    float idleTime = UnityEngine.Random.Range(1f, 5f); // 1~5�� ���� ��� �ð�
                    yield return new WaitForSeconds(idleTime);
                    if (!isDead && navAgent.isOnNavMesh)
                    {
                        navAgent.isStopped = false;
                        SetRandomPatrolPoint(); // ���ο� ���� ������ ����
                    }
                }

                var colliders = Physics.OverlapSphere(viewTransform.position, viewDistance, LayerTarget);
                foreach (var collider in colliders)
                {
                    var targetPlayer = collider.GetComponent<PlayerConJS>();
                    if (targetPlayer != null && !targetPlayer.isDead && IsTargetOnSight(targetPlayer.transform))
                    {
                        player = targetPlayer;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(.05f);
        }
    }

    private void SetRandomPatrolPoint()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * patrolRange;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRange, 1))
        {
            navAgent.SetDestination(hit.position);
        }
        //animator.SetFloat("Speed", navAgent.desiredVelocity.magnitude);
    }

    private bool IsTargetOnSight(Transform target)
    {
        var direction = target.position - viewTransform.position;
        direction.y = viewTransform.forward.y;

        if (Vector3.Angle(direction, viewTransform.forward) > viewAngle * 0.5f)
        {
            return false;
        }

        direction = target.position - viewTransform.position;
        RaycastHit hit;

        if (Physics.Raycast(viewTransform.position, direction, out hit, viewDistance, LayerTarget))
        {
            if (hit.transform == target)
            {
                return true;
            }
        }

        return false;
    }
    public bool ApplyDamage(DamageMessage damageMessage)
    {
        if (Time.time < lastDamagedTime + MIN_TIME_BET_DAMAGE || damageMessage.damager == gameObject || isDead)
        {
            return false;
        }

        lastDamagedTime = Time.time;
        hp -= damageMessage.amount;

        //if (damageMessage.amount != 0)
        //{
        //  animator.CrossFade("hit", .2f);
        //}

        if (hp <= 0)
        {
            Die();
        }
        else if (hp > 0 && damageMessage.amount != 0)
        {
            //animator.SetTrigger("hit");
        }

        if (player == null)
        {
            player = damageMessage.damager.GetComponent<PlayerConJS>();
        }
        return true;
    }

    public void Die()
    {
        isDead = true;

        hp = 0;
        //animator.SetTrigger("Die");

        //if (navAgent.isOnNavMesh)
        //{
        //    navAgent.isStopped = true;
        //    navAgent.ResetPath();
        //}
        //navAgent.enabled = false;
    }

}
