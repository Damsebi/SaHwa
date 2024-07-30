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
    #region 열거
    protected enum eState
    {
        Patrol, //순찰
        Tracking, //추적
        AttackBegin, //공격시작
        Attacking, //공격중
        Idle, // 대기 상태
        BatIdle //공격 대기상태
    }
    [SerializeField] protected eState enemyState;
    #endregion

    #region 선언
    protected Animator animator;
    protected NavMeshAgent navAgent;
    public LayerMask LayerTarget;
    public Transform attackRoot; //공격이 시작되는 피벗, 이 피벗 해당 반경 내에 있는 플레이어가 공격당함
    public Transform viewTransform; //눈 위치
    public Player target;

    protected List<Player> lastAttackedTarget = new List<Player>();

    #region SO 데이터(공통)
    [HideInInspector] public float hp; //체력
    [HideInInspector] public float hpBarCount; //체력바 갯수
    [HideInInspector] public float trackingSpeed; //추적 속도
    [HideInInspector] public float patrolSpeed; //순찰 속도
    [HideInInspector] public float viewAngle; // 시야 각
    [HideInInspector] public float viewDistance; // 시야 범위
    [HideInInspector] public float paintOver; // 덧칠 횟수
    protected float turnSmoothVelocity; //몬스터의 회전 속도
    protected float patrolRange; // 순찰 범위
    [HideInInspector] public bool plusAttackDamage; // 플레이어 평타에 추가 데미지 추가
    [HideInInspector] public bool plusSkillDamage; // 플레이어 스킬에 추가 데미지 추가
    [HideInInspector] public bool magicGroup; // 마법 세력   
    [HideInInspector] public float flatDamage; // 평타 데미지
    [HideInInspector] public float flatMotionSpeed; // 평타 모션 속도
    [HideInInspector] public float flatMotionCoolTime; // 평타 모션 쿨타임
    [HideInInspector] public float flatRange; // 평타 범위
    [HideInInspector] public float patrolWaitingTimeMin; // 순찰 최소 대기 시간
    [HideInInspector] public float patrolWaitingTimeMax; // 순찰 최대 대기 시간
    #endregion


    #region SO 데이터(하이브리드)
    [HideInInspector] public float flatHybridDamage; // 하이브리드 원거리 평타 데미지
    [HideInInspector] public float flatHybridMotionSpeed; // 하이브리드 원거리 평타 모션 속도
    [HideInInspector] public float flatHybridMotionCoolTime; // 하이브리드 원거리 평타 모션 쿨타임
    [HideInInspector] public float flatHybridRange; // 하이브리드 원거리 평타 범위
    #endregion

    #region SO 데이터(엘리트 & 에픽)
    [HideInInspector] public float skillDamage; // 스킬 데미지
    [HideInInspector] public float skillMotionSpeed; // 스킬 모션 속도
    [HideInInspector] public float skillMotionCoolTime; // 스킬 모션 쿨타임
    [HideInInspector] public float skillRange; // 스킬 범위
    #endregion

    #region 내부 변수
    protected float attackDistance; //사정거리
    protected float turnSmoothTime = 0.1f; //몬스터 회전 시 지연시간
    protected float lastDamagedTime; //마지막으로 공격 받았던 시간
    protected const float MIN_TIME_BET_DAMAGE = 0.1f; //데미지를 주었을 때, 데미지 사이의 최소 간격
    protected float lostSightTime = 1.0f; //타이머가 도달하는 시간
    protected float lostSightTimer = 0.0f; //0초 부터 시작하는 타이머
    protected bool isDead; // 사망 체크
    protected bool canTriggerHitAnimation = true; // hit 과 hit 사이에 텀
    #endregion

    #region 덧칠 시스템 관련
    protected CalliSystem calliSystem;
    [SerializeField] protected GameObject[] paintOverStacks;
    [SerializeField] protected GameObject stackUI;
    protected bool paintOverMax;
    #endregion

    #endregion

    #region 플레이어 체크
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
        //높이를 무시하고 평면상의 거리만 고려

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

        //navmash의 desiredVelocity는 현재 속도값이 아니라 의도한 만큼 지정되는 값이다. 
        //벽 앞에서 적이 걷는다면 실제 속도는 0이기 때문에
        //애니메이션이 나오지 않는것을 방지하기 위함(애니메이터의 조건을 speed > 0.1로 걸어두었음)
        animator.SetFloat("Speed", navAgent.desiredVelocity.magnitude);

        //덧칠 체크
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
        //공격하거나 공격 도중 플레이어를 바라보게 함
        if (enemyState == eState.AttackBegin || enemyState == eState.Attacking)
        {
            var lookRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            var targetAngleY = lookRotation.eulerAngles.y;

            targetAngleY = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY, ref turnSmoothVelocity, turnSmoothTime);
            transform.eulerAngles = Vector3.up * targetAngleY;
        }
        if (enemyState == eState.AttackBegin)
        {
            // 공격 시작 시 애니메이션 재생
            animator.SetTrigger("Attack");
            enemyState = eState.Attacking;
        }
    }
    #endregion

    #region 초기화
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

    #region 플레이어 찾기

    #region 경로 업데이트  
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
                    lostSightTimer = 0.0f; // 타겟이 다시 시야에 들어오면 타이머 초기화
                }
            }
            else
            {
                //직전까지의 몹의 상태가 정찰상태가 아니라면 정찰 상태로 변경
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

                    float timer = 0f;
                    while (timer < idleTime)
                    {
                        timer += Time.deltaTime;

                        // Idle 상태에서도 플레이어 감지
                        var colliders = Physics.OverlapSphere(viewTransform.position,
                            viewDistance, LayerTarget);

                        foreach (var collider in colliders)
                        {
                            //해당 transform을 가진 객체가 시야 내에 존재하는지
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
                        SetRandomPatrolPoint(); // 새로운 정찰 지점을 설정
                    }
                }

                var patrolColliders = Physics.OverlapSphere(viewTransform.position,
                    viewDistance, LayerTarget);

                foreach (var collider in patrolColliders)
                {
                    // 해당 transform을 가진 객체가 시야 내에 존재하는지
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

    #region 타겟 찾기
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

    #region 랜덤 순찰지점 찍기
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


    #region 덧칠확인, 행동 제한시키기
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
    public void StopAction() //처형시에 움짐임 봉쇄. 
    {
        isDead = true;
        navAgent.speed = 0;
        navAgent.velocity = Vector3.zero;
    }
    #endregion

    #region 데미지 관련

    #region 공격 시작
    private void BeginAttack()
    {
        enemyState = eState.AttackBegin;

        navAgent.isStopped = true;
        animator.SetTrigger("Attack");
    }
    #endregion

    #region 공격 나오는 지점
    private void DisableAttack()
    {
        enemyState = eState.Tracking;
        navAgent.isStopped = false;
    }
    #endregion

    #region 데미지 받음
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

    #region 공격 받는 도중 행동 멈춤
    private IEnumerator HandleHitReaction()
    {
        canTriggerHitAnimation = false;
        navAgent.isStopped = true;
        yield return new WaitForSeconds(1f); // 1초 동안 모든 행동 멈춤

        navAgent.isStopped = false;
        yield return new WaitForSeconds(2f); // 2초 동안 피격 애니메이션 작동하지 않음

        canTriggerHitAnimation = true; // 2초 후에 다시 피격 애니메이션 가능
    }
    #endregion

    #region 사망 처리
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
