using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq.Expressions;
using UnityEngine.AI;
using Unity.XR.Oculus.Input;
using UnityEditor;

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
    [SerializeField] private eState enemyState;
    #endregion

    #region ����
    public EnemyData enemyData;
    private Animator animator;
    private NavMeshAgent navAgent;
    public LayerMask LayerTarget;
    public Transform attackRoot; //������ ���۵Ǵ� �ǹ�, �� �ǹ� �ش� �ݰ� ���� �ִ� �÷��̾ ���ݴ���
    public Transform viewTransform; //�� ��ġ
    public Player target;
    private RaycastHit[] hits = new RaycastHit[10];
    private List<Player> lastAttackedTarget = new List<Player>();

    #region SO ������(����)
    [HideInInspector] public float hp; //ü��
    [HideInInspector] public float hpBarCount; //ü�¹� ����
    [HideInInspector] public float trackingSpeed; //���� �ӵ�
    [HideInInspector] public float patrolSpeed; //���� �ӵ�
    [HideInInspector] public float viewAngle; // �þ� ��
    [HideInInspector] public float viewDistance; // �þ� ����
    [HideInInspector] public float paintOver; // ��ĥ Ƚ��
    private float turnSmoothVelocity; //������ ȸ�� �ӵ�
    private float patrolRange; // ���� ����
    [HideInInspector] public bool plusAttackDamage; // �÷��̾� ��Ÿ�� �߰� ������ �߰�
    [HideInInspector] public bool plusSkillDamage; // �÷��̾� ��ų�� �߰� ������ �߰�
    [HideInInspector] public bool magicGroup; // ���� ����   
    [HideInInspector] public float flatDamage; // ��Ÿ ������
    [HideInInspector] public float flatMotionSpeed; // ��Ÿ ��� �ӵ�
    [HideInInspector] public float flatMotionCoolTime; // ��Ÿ ��� ��Ÿ��
    [HideInInspector] public float flatRange; // ��Ÿ ����
    #endregion

    #region SO ������(�и�)
    [HideInInspector] public float patrolWaitingTimeMin; // ���� �ּ� ��� �ð�
    [HideInInspector] public float patrolWaitingTimeMax; // ���� �ִ� ��� �ð�
    #endregion

    #region SO ������(������ & ����)
    [HideInInspector] public float distanceToPlayerMax; // �÷��̾���� �ִ� �Ÿ�
    [HideInInspector] public bool isRanged; // �ٰŸ� ���Ÿ� ���� üũ
    [HideInInspector] public bool isBuffer; // �������� üũ
    #endregion

    #region SO ������(���̺긮��)
    [HideInInspector] public float flatHybridDamage; // ���̺긮�� ���Ÿ� ��Ÿ ������
    [HideInInspector] public float flatHybridMotionSpeed; // ���̺긮�� ���Ÿ� ��Ÿ ��� �ӵ�
    [HideInInspector] public float flatHybridMotionCoolTime; // ���̺긮�� ���Ÿ� ��Ÿ ��� ��Ÿ��
    [HideInInspector] public float flatHybridRange; // ���̺긮�� ���Ÿ� ��Ÿ ����
    #endregion

    #region SO ������(����Ʈ & ����)
    [HideInInspector] public float skillDamage; // ��ų ������
    [HideInInspector] public float skillMotionSpeed; // ��ų ��� �ӵ�
    [HideInInspector] public float skillMotionCoolTime; // ��ų ��� ��Ÿ��
    [HideInInspector] public float skillRange; // ��ų ����
    #endregion

    #region ���� ����
    private float attackDistance; //�����Ÿ�
    private float turnSmoothTime = 0.1f; //���� ȸ�� �� �����ð�
    private float lastDamagedTime; //���������� ���� �޾Ҵ� �ð�
    private const float MIN_TIME_BET_DAMAGE = 0.1f; //�������� �־��� ��, ������ ������ �ּ� ����
    private float lostSightTime = 1.0f; //Ÿ�̸Ӱ� �����ϴ� �ð�
    private float lostSightTimer = 0.0f; //0�� ���� �����ϴ� Ÿ�̸�
    private bool isDead; // ��� üũ
    private bool canTriggerHitAnimation = true; // hit �� hit ���̿� ��
    #endregion

    #region ��ĥ �ý��� ����
    private CalliSystem calliSystem;
    [SerializeField] private GameObject[] paintOverStacks;
    [SerializeField] private GameObject stackUI;
    private bool paintOverMax;
    #endregion

    #endregion

    #region �÷��̾� üũ
    private bool hasTarget => target != null && !target.IsDead;
    #endregion

    #region Awake()
    private void Awake()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();   
        calliSystem = GetComponent<CalliSystem>();
        InitializeStats();
        SetUp();
        var attackPivot = attackRoot.position;
        attackPivot.y = transform.position.y;
        //���̸� �����ϰ� ������ �Ÿ��� ���

        attackDistance = Vector3.Distance(transform.position, attackPivot) + flatRange;
        navAgent.stoppingDistance = attackDistance;
        navAgent.speed = patrolSpeed;
    }
    #endregion

    #region Start()
    void Start()
    {
        StartCoroutine(UpdatePath());
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
                var distance = Vector3.Distance(target.transform.position, transform.position);
                if (distance <= attackDistance)
                {
                    BeginAttack();
                }
                
                break;
        }

        //�߰�(���)
        CheckPaintOver();
    }
    #endregion

    #region FixedUpdate()
    private void FixedUpdate()
    {
        //����ó��
        if (isDead)
        {
            return;
        }
        //�����ϰų� ���� ���� �÷��̾ �ٶ󺸰� ��
        if (enemyState == eState.AttackBegin || enemyState == eState.Attacking)
        {
            var lookRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            var targetAngleY = lookRotation.eulerAngles.y;

            targetAngleY = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY, ref turnSmoothVelocity, turnSmoothTime);
            transform.eulerAngles = Vector3.up * targetAngleY;
        }

        //���� ���� �÷��̾ �̵��ؼ� �þ߿��� ��ġ��?
        //physics.cast�迭

        if (enemyState == eState.Attacking)
        {
            //������ ���� ������ ����
            var direction = transform.forward;

            //�׺��� ����ӵ��� �������� deltaDistance�� ��� -> �� ������ ���� ���Ͱ� �̵��ϴ� �Ÿ�
            var deltaDistance = navAgent.velocity.magnitude * Time.deltaTime;

            // SphereCastNonAlloc�� ����Ͽ� atkRoot.position���� �������� atkRadius�� �� ���·� direction �������� deltaDistance �Ÿ���ŭ ��ü ������ ĳ��Ʈ(�̵��� ������ ��ġ�� Collider�� �ִ��� �˻�)
            // Trigger,Collision ���� �� ������ ����, �� ������ ��ġ�� Collider�� ��Ƴ��� ���� ����.
            // hits �迭�� �浹�� ����� �����ϰ�, �浹�� ��ü�� ���� size�� ����
            var size = Physics.SphereCastNonAlloc(attackRoot.position, flatRange, direction, hits, deltaDistance, LayerTarget);

            //�浹�� ��ü�鿡 ���� �ݺ�
            for (var i = 0; i < size; i++)
            {
                //�浹�� ��ü�� livingentity���� Ȯ��
                var atkTarget = hits[i].collider.GetComponent<Player>();

                //�浹�� ��ü�� Player�̰� ���� ������ �� ���� ��ü���
                if (atkTarget != null && !lastAttackedTarget.Contains(atkTarget))
                {
                    //damageMessage ��ü�� �����ؼ� ������ ������ ����
                    var message = new DamageMessage();
                    message.amount = flatDamage;
                    message.damager = this.gameObject;
                    message.hitPoint = hits[i].point;

                    //�浹�� ������ 0 ������ ���� �� ���� ��츦 ������ hitpoint ����
                    if (hits[i].distance <= 0f)
                    {
                        message.hitPoint = attackRoot.position;
                    }
                    else
                    {
                        message.hitPoint = hits[i].point;
                    }

                    //�浹�� ������ ������ hitnormal�� ����
                    message.hitNormal = hits[i].normal;

                    //�浹�� livingEntity���� ������ ����
                    atkTarget.ApplyDamage(message);

                    //�ش簴ü�� lastAtkTarget ����Ʈ�� �߰��Ͽ� �ߺ����� ����
                    lastAttackedTarget.Add(atkTarget);

                    break;
                }
            }
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
            patrolWaitingTimeMin = meleeData.i_patrolWaitingTimeMin;
            patrolWaitingTimeMax = meleeData.i_patrolWaitingTimeMax;
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

    void SetUp()
    {
        hp = enemyData.f_hp;
        hpBarCount = enemyData.i_hpBarCount;
        trackingSpeed = enemyData.f_trackingSpeed;
        patrolSpeed = enemyData.f_patrolSpeed;
        viewAngle = enemyData.f_viewAngle;
        viewDistance = enemyData.f_viewDistance;
        paintOver = enemyData.i_paintOver;
        turnSmoothVelocity = enemyData.f_turnSmoothVelocity;
        plusAttackDamage = enemyData.b_plusAttackDamage;
        plusSkillDamage = enemyData.b_plusSkillDamage;
        magicGroup = enemyData.b_magicGroup;
        patrolRange = enemyData.f_patrolRange;
        navAgent.speed = patrolSpeed;
        enemyState = eState.Idle;
    }
    #endregion
    
    #region �÷��̾� ã��

    #region ��� ������Ʈ
    private IEnumerator UpdatePath()
    {
        while (!isDead)
        {
            if (hasTarget)
            {
                if (enemyState == eState.Patrol || enemyState == eState.Idle)
                {
                    enemyState = eState.Tracking;
                    navAgent.speed = trackingSpeed;
                    animator.SetBool("BattleMode", true);
                }
                navAgent.SetDestination(target.transform.position);

                if (!IsTargetOnSight(target.transform))
                {
                    lostSightTimer += Time.deltaTime;
                    if (lostSightTimer >= lostSightTime)
                    {
                        target = null;
                        enemyState = eState.Idle;
                        navAgent.speed = patrolSpeed;
                        animator.SetBool("BattleMode", false);
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
                    animator.SetBool("BattleMode", false);
                }

                var colliders = Physics.OverlapSphere(viewTransform.position, viewDistance, LayerTarget);

                if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
                {
                    enemyState = eState.Idle;
                    navAgent.isStopped = true;
                    var idleTime = UnityEngine.Random.Range(patrolWaitingTimeMin, patrolWaitingTimeMax);
                    var waitingTime = 1f;

                    while (waitingTime < idleTime)
                    {
                        if (!isDead && navAgent.isOnNavMesh)
                        {
                            // ���̾ ���� ���͸� �߽����� ���� �� �÷��̾ �ν�

                            foreach (var collider in colliders)
                            {
                                if (!IsTargetOnSight(collider.transform))
                                {
                                    continue;
                                }

                                var Player = collider.GetComponent<Player>();

                                if (Player != null && !Player.IsDead)
                                {
                                    target = Player;
                                    break;
                                }
                            }
                            if (hasTarget)
                            {
                                break;
                            }

                        }

                        waitingTime += 0.1f;
                        yield return new WaitForSeconds(0.1f);
                    }

                    if (!isDead && navAgent.isOnNavMesh)
                    {
                        navAgent.isStopped = false;
                        SetRandomPatrolPoint(); // ���ο� ���� ������ ����
                    }
                }
            }
            yield return new WaitForSeconds(.05f);
        }
    }
    #endregion

    #region Ÿ�� ã��
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
    #endregion

    #region ���� �������� ���
    private void SetRandomPatrolPoint()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * patrolRange;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRange, 1))
        {
            navAgent.SetDestination(hit.position);
        }
        animator.SetFloat("Speed", navAgent.desiredVelocity.magnitude);
    }
    #endregion

    #endregion

    #region ��ĥȮ��, �ൿ ���ѽ�Ű��
    private void CheckPaintOver()
    {
        if (stackUI.activeSelf)
        {
            stackUI.transform.rotation
                = PlayerFollowCamera.instance.MainCamera.transform.rotation ; 
        }

        if (paintOverMax || calliSystem.paintOver == 0) return;

        if (calliSystem.paintOver < calliSystem.MaxPaintOver)
        {
            for (int i = 0; i < calliSystem.paintOver + 1; i++)
            {
                paintOverStacks[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < calliSystem.MaxPaintOver + 1; i++)
            {
                paintOverStacks[i].SetActive(true);
                paintOverStacks[i].GetComponent<Image>().color = Color.white;
            }
            paintOverMax = true;

        }
    }
    public void StopAction() //ó���ÿ� ������ ����. 
    {
        isDead = true;
        navAgent.speed = 0;
        navAgent.velocity = Vector3.zero;
    }
    #endregion

    #region ������ ����

    #region ���� ����
    private void BeginAttack()
    {
        enemyState = eState.AttackBegin;

        navAgent.isStopped = true;
        animator.SetTrigger("Attack");
    }
    #endregion

    #region ���� ���� ����
    public void EnableAttack()
    {
        enemyState = eState.Attacking;

        lastAttackedTarget.Clear();
    }
    #endregion

    #region ���� ������ ����
    public void DisableAttack()
    {
        enemyState = eState.Tracking;
        navAgent.isStopped = false;
    }
    #endregion

    #region ������ ����
    public bool ApplyDamage(DamageMessage damageMessage)
    {
        if (Time.time < lastDamagedTime + MIN_TIME_BET_DAMAGE || damageMessage.damager == gameObject || isDead)
        {
            return false;
        }

        lastDamagedTime = Time.time;
        hp -= damageMessage.amount;

        if (hp <= 0)
        {
            Die();
        }
        else if (hp > 0 && damageMessage.amount != 0)
        {
            if (canTriggerHitAnimation)
            {
                StartCoroutine(HandleHitReaction());
            }

            if (target == null)
            {
                target = damageMessage.damager.GetComponent<Player>();
            }
        }
        return true;
    }
    #endregion

    #region ���� �޴� ���� �ൿ ����
    private IEnumerator HandleHitReaction()
    {
        canTriggerHitAnimation = false;
        navAgent.isStopped = true;
        yield return new WaitForSeconds(1f); // 1�� ���� ��� �ൿ ����

        navAgent.isStopped = false;
        yield return new WaitForSeconds(2f); // 2�� ���� �ǰ� �ִϸ��̼� �۵����� ����

        canTriggerHitAnimation = true; // 2�� �Ŀ� �ٽ� �ǰ� �ִϸ��̼� ����
    }
    #endregion

    #region ��� ó��
    public void Die()
    {
        isDead = true;

        hp = 0;
        animator.SetTrigger("Die");

        if (navAgent.isOnNavMesh)
        {
            navAgent.isStopped = true;
            navAgent.ResetPath();
        }
        navAgent.enabled = false;

        stackUI.SetActive(false);
    }
    #endregion

    #endregion
}
