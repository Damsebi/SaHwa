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
    #region 열거
    private enum eState
    {
        Patrol, //순찰
        Tracking, //추적
        AttackBegin, //공격시작
        Attacking, //공격중
        Idle, // 대기 상태
        BatIdle //공격 대기상태
    }
    private eState enemyState;
    #endregion
    #region 선언
    public EnemyData enemyData;

    [HideInInspector] public float hp; //체력
    [HideInInspector] public float hpBarCount; //체력바 갯수
    [HideInInspector] public float moveSpeed; //이동 속도
    [HideInInspector] public float patrolSpeed; //순찰 속도
    [HideInInspector] public float viewDistance; // 시야 범위
    [HideInInspector] public float viewAngle; // 시야 각
    [HideInInspector] public float distanceToPlayerMin; // 플레이어와의 최소 거리
    [HideInInspector] public float distanceToPlayerMax; // 플레이어와의 최대 거리
    [HideInInspector] public float paintOver; // 덧칠 횟수

    [HideInInspector] public float flatDamage; // 평타 데미지
    [HideInInspector] public float flatMotionSpeed; // 평타 모션 속도
    [HideInInspector] public float flatMotionCoolTime; // 평타 모션 쿨타임
    [HideInInspector] public float flatRange; // 평타 범위

    [HideInInspector] public float flatHybridDamage; // 하이브리드 원거리 평타 데미지
    [HideInInspector] public float flatHybridMotionSpeed; // 하이브리드 원거리 평타 모션 속도
    [HideInInspector] public float flatHybridMotionCoolTime; // 하이브리드 원거리 평타 모션 쿨타임
    [HideInInspector] public float flatHybridRange; // 하이브리드 원거리 평타 범위

    [HideInInspector] public float skillDamage; // 스킬 데미지
    [HideInInspector] public float skillMotionSpeed; // 스킬 모션 속도
    [HideInInspector] public float skillMotionCoolTime; // 스킬 모션 쿨타임
    [HideInInspector] public float skillRange; // 스킬 범위

    private Animator animator;
    private NavMeshAgent navAgent;

    private float turnSmoothVelocity; //몬스터의 회전 속도
    private float turnSmoothTime = 0.1f; //몬스터 회전 시 지연시간

    private float lastDamagedTime; //마지막으로 공격 받았던 시간
    private const float MIN_TIME_BET_DAMAGE = 0.1f;

    [HideInInspector] public bool plusAttackDamage; // 플레이어 평타에 추가 데미지 추가
    [HideInInspector] public bool plusSkillDamage; // 플레이어 스킬에 추가 데미지 추가
    [HideInInspector] public bool magicGroup; // 마법 세력
    private float patrolRange; // 순찰 범위
    private bool isRanged; // 근거리 원거리 공격 체크
    private bool isBuffer; // 버퍼인지 체크

    public LayerMask LayerTarget;

    private float lostSightTime = 1.0f;
    private float lostSightTimer = 0.0f;

    private bool isDead; // 사망 체크

    public Transform attackRoot; //공격이 시작되는 피벗, 이 피벗 해당 반경 내에 있는 플레이어가 공격당함
    public Transform viewTransform; //눈 위치

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
        //    Debug.LogError("애니메이터 없음.");
        //}

        if (navAgent == null)
        {
            Debug.LogError("네비메쉬 없음.");
        }
        var attackPivot = attackRoot.position;
        attackPivot.y = transform.position.y;
        //높이를 무시하고 평면상의 거리만 고려

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


    #region 초기화
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

    #region 시야 그리기
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (attackRoot != null && !isBuffer && !isRanged)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);//붉은색
            Gizmos.DrawSphere(attackRoot.position, flatRange);
            //원형 모양의 근거리 공격 범위
        }
        else if (attackRoot != null && !isBuffer && isRanged)
        {
            //Gizmos.color = new Color(1f, 0f, 0f, 0.5f);//붉은색
            //Vector3 cubeSize = new Vector3(0.3f, 0.3f, flatRange); // width, height, depth
            //Vector3 cubeCenter = attackRoot.position + attackRoot.forward * (flatRange / 2);
            //Gizmos.DrawCube(cubeCenter, cubeSize);
            ////긴 직사각형모양의 원거리 공격 범위

            Gizmos.color = new Color(1f, 0f, 0f, 0.5f); // 붉은색
            Vector3 cubeSize = new Vector3(0.3f, 0.3f, flatRange); // width, height, depth
            Matrix4x4 oldMatrix = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(attackRoot.position, attackRoot.rotation, attackRoot.lossyScale);
            Gizmos.DrawCube(-Vector3.forward * (flatRange / 2), cubeSize);
            Gizmos.matrix = oldMatrix;

        }
        else if (attackRoot != null && isBuffer && !isRanged)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);//붉은색
            Gizmos.DrawWireSphere(attackRoot.position, flatRange);
            //선모양의 원거리 공격 범위
        }

        if (viewTransform != null) //단순히 반경 표시
        {
            var leftViewRotation = Quaternion.AngleAxis(-viewAngle * 0.5f, Vector3.up); //정면~왼쪽 각도
            var leftRayDirection = leftViewRotation * transform.forward;

            Handles.color = new Color(1f, 1f, 1f, 0.2f);//흰색
            Handles.DrawSolidArc(viewTransform.position, Vector3.up, leftRayDirection, viewAngle, viewDistance);
            //(중심, 위에서 아래를 보이게, 아크를 그리기 시작하는 시작 벡터 , 전체 시야각 /2 , 부채꼴의 반지름
            //부채꼴 모양의 시야각 만들기(gui)
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
            // 플레이어에게 데미지를 줌
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
                    lostSightTimer = 0.0f; // 타겟이 다시 시야에 들어오면 타이머 초기화
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
                    float idleTime = UnityEngine.Random.Range(1f, 5f); // 1~5초 랜덤 대기 시간
                    yield return new WaitForSeconds(idleTime);
                    if (!isDead && navAgent.isOnNavMesh)
                    {
                        navAgent.isStopped = false;
                        SetRandomPatrolPoint(); // 새로운 정찰 지점을 설정
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
