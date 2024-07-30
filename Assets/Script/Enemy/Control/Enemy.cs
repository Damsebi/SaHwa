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
using UnityEngine.XR;
using UnityEditor.Rendering;

public class Enemy : MonoBehaviour, IDamageable
{
    #region ����
    protected enum eState
    {
        Patrol, //����
        Tracking, //����
        AttackBegin, //���ݽ���
        Attacking, //������
        Idle, // ��� ����
        BatIdle //���� ������
    }
    [SerializeField] protected eState enemyState;
    #endregion

    #region ����
    protected Animator animator;
    protected NavMeshAgent navAgent;
    public LayerMask LayerTarget;
    public Transform attackRoot; //������ ���۵Ǵ� �ǹ�, �� �ǹ� �ش� �ݰ� ���� �ִ� �÷��̾ ���ݴ���
    public Transform viewTransform; //�� ��ġ
    public Player target;

    protected List<Player> lastAttackedTarget = new List<Player>();

    #region SO ������(����)
    [HideInInspector] public float hp; //ü��
    [HideInInspector] public float hpBarCount; //ü�¹� ����
    [HideInInspector] public float trackingSpeed; //���� �ӵ�
    [HideInInspector] public float patrolSpeed; //���� �ӵ�
    [HideInInspector] public float viewAngle; // �þ� ��
    [HideInInspector] public float viewDistance; // �þ� ����
    [HideInInspector] public float paintOver; // ��ĥ Ƚ��
    protected float turnSmoothVelocity; //������ ȸ�� �ӵ�
    protected float patrolRange; // ���� ����
    [HideInInspector] public bool plusAttackDamage; // �÷��̾� ��Ÿ�� �߰� ������ �߰�
    [HideInInspector] public bool plusSkillDamage; // �÷��̾� ��ų�� �߰� ������ �߰�
    [HideInInspector] public bool magicGroup; // ���� ����   
    [HideInInspector] public float flatDamage; // ��Ÿ ������
    [HideInInspector] public float flatMotionSpeed; // ��Ÿ ��� �ӵ�
    [HideInInspector] public float flatMotionCoolTime; // ��Ÿ ��� ��Ÿ��
    [HideInInspector] public float flatRange; // ��Ÿ ����
    [HideInInspector] public float patrolWaitingTimeMin; // ���� �ּ� ��� �ð�
    [HideInInspector] public float patrolWaitingTimeMax; // ���� �ִ� ��� �ð�
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
    protected float attackDistance; //�����Ÿ�
    protected float turnSmoothTime = 0.1f; //���� ȸ�� �� �����ð�
    protected float lastDamagedTime; //���������� ���� �޾Ҵ� �ð�
    protected const float MIN_TIME_BET_DAMAGE = 0.1f; //�������� �־��� ��, ������ ������ �ּ� ����
    protected float lostSightTime = 1.0f; //Ÿ�̸Ӱ� �����ϴ� �ð�
    protected float lostSightTimer = 0.0f; //0�� ���� �����ϴ� Ÿ�̸�
    protected bool isDead; // ��� üũ
    protected bool canTriggerHitAnimation = true; // hit �� hit ���̿� ��
    #endregion

    #region ��ĥ �ý��� ����
    protected CalliSystem calliSystem;
    [SerializeField] protected GameObject[] paintOverStacks;
    [SerializeField] protected GameObject stackUI;
    protected bool paintOverMax;
    #endregion

    #endregion

    #region �÷��̾� üũ
    protected bool hasTarget => target != null && !target.IsDead;
    #endregion

    #region Awake()
    private void Awake()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        calliSystem = GetComponent<CalliSystem>();

        navAgent.speed = patrolSpeed;
    }
    #endregion

    #region Start()
    protected virtual void Start()
    {
        StartCoroutine(UpdatePath());

        var attackPivot = attackRoot.position;
        attackPivot.y = transform.position.y;
        //���̸� �����ϰ� ������ �Ÿ��� ���

        attackDistance = Vector3.Distance(transform.position, attackPivot) + flatRange;
        navAgent.stoppingDistance = attackDistance;
        Debug.Log(navAgent.stoppingDistance);
    }
    #endregion

    #region Update()
    void Update()
    {
        if (isDead)
        {
            return;
        }
        if (enemyState == eState.Tracking)
        {
            var distance = Vector3.Distance(target.transform.position, transform.position);
            if (distance <= attackDistance)
            {
                Debug.Log("1");
                BeginAttack();
            }
        }

        //navmash�� desiredVelocity�� ���� �ӵ����� �ƴ϶� �ǵ��� ��ŭ �����Ǵ� ���̴�. 
        //�� �տ��� ���� �ȴ´ٸ� ���� �ӵ��� 0�̱� ������
        //�ִϸ��̼��� ������ �ʴ°��� �����ϱ� ����(�ִϸ������� ������ speed > 0.1�� �ɾ�ξ���)
        animator.SetFloat("Speed", navAgent.desiredVelocity.magnitude);

        //��ĥ üũ
        CheckPaintOver();
    }
    #endregion

    #region FixedUpdate()
    protected virtual void FixedUpdate()
    {
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
        if (enemyState == eState.AttackBegin)
        {
            // ���� ���� �� �ִϸ��̼� ���
            animator.SetTrigger("Attack");
            enemyState = eState.Attacking;
        }
    }
    #endregion

    #region �ʱ�ȭ
    protected void InitializeStats(float f_hp, int i_hpBarCount, float f_trackingSpeed, float f_patrolSpeed, float f_viewAngle, 
        float f_viewDistance, int i_paintOver, float f_turnSmoothVelocity, bool b_plusAttackDamage, bool b_plusSkillDamage, bool b_magicGroup, float f_patrolRange)
    {
        hp = f_hp;
        hpBarCount = i_hpBarCount;
        trackingSpeed = f_trackingSpeed;
        patrolSpeed = f_patrolSpeed;
        viewAngle = f_viewAngle;
        viewDistance = f_viewDistance;
        paintOver = i_paintOver;
        turnSmoothVelocity = f_turnSmoothVelocity;
        plusAttackDamage = b_plusAttackDamage;
        plusSkillDamage = b_plusSkillDamage;
        magicGroup = b_magicGroup;
        patrolRange = f_patrolRange;
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
                        enemyState = eState.Patrol;
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
                //���������� ���� ���°� �������°� �ƴ϶�� ���� ���·� ����
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

                    float timer = 0f;
                    while (timer < idleTime)
                    {
                        timer += Time.deltaTime;

                        // Idle ���¿����� �÷��̾� ����
                        var colliders = Physics.OverlapSphere(viewTransform.position,
                            viewDistance, LayerTarget);

                        foreach (var collider in colliders)
                        {
                            //�ش� transform�� ���� ��ü�� �þ� ���� �����ϴ���
                            if (!IsTargetOnSight(collider.transform))
                            {
                                continue;
                            }

                            var Player = collider.GetComponent<Player>();

                            if (Player != null && !Player.IsDead)
                            {
                                target = Player;
                                enemyState = eState.Tracking;
                                navAgent.speed = trackingSpeed;
                                animator.SetBool("BattleMode", true);
                                navAgent.isStopped = false;
                                break;
                            }
                        }

                        if (hasTarget)
                            break;

                        yield return null;
                    }

                    if (!isDead && navAgent.isOnNavMesh && !hasTarget)
                    {
                        navAgent.isStopped = false;
                        SetRandomPatrolPoint(); // ���ο� ���� ������ ����
                    }
                }

                var patrolColliders = Physics.OverlapSphere(viewTransform.position,
                    viewDistance, LayerTarget);

                foreach (var collider in patrolColliders)
                {
                    // �ش� transform�� ���� ��ü�� �þ� ���� �����ϴ���
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


    #region ��ĥȮ��, �ൿ ���ѽ�Ű��
    private void CheckPaintOver()
    {
        if (stackUI.activeSelf)
        {
            stackUI.transform.rotation
                = PlayerFollowCamera.instance.MainCamera.transform.rotation;
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

    #region ���� ������ ����
    private void DisableAttack()
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
    #endregion
}
