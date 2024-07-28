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
        AttackBegin, //공격시작 (공격이 시작하자마자 데미지가 들어가진 않게)
        Attacking, //공격중
        Idle, // 대기 상태
        BatIdle //공격 대기상태
    }
    [SerializeField] private eState enemyState;
    #endregion

    #region 선언
    public EnemyData enemyData;

    [HideInInspector] public float hp; //체력
    [HideInInspector] public float hpBarCount; //체력바 갯수
    [HideInInspector] public float trackingSpeed; //추적 속도
    [HideInInspector] public float patrolSpeed; //순찰 속도
    [HideInInspector] public float viewDistance; // 시야 범위
    [HideInInspector] public float viewAngle; // 시야 각
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

    [HideInInspector] public float patrolWaitingTimeMin; // 순찰 최소 대기 시간
    [HideInInspector] public float patrolWaitingTimeMax; // 순찰 최대 대기 시간

    private Animator animator;
    private NavMeshAgent navAgent;

    private float attackDistance;

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

    private bool canTriggerHitAnimation = true; // hit 과 hit 사이에 텀

    public LayerMask LayerTarget;

    private float lostSightTime = 1f;
    private float lostSightTimer = 0.0f;

    private bool isDead; // 사망 체크

    public Transform attackRoot; //공격이 시작되는 피벗, 이 피벗 해당 반경 내에 있는 플레이어가 공격당함
    public Transform viewTransform; //눈 위치

    public Player target;
    private RaycastHit[] hits = new RaycastHit[10];
    private List<Player> lastAttackedTarget = new List<Player>();


    private bool hasTarget => target != null && !target.IsDead;

    #endregion

    #region Awake()
    private void Awake()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();

        var attackPivot = attackRoot.position;
        attackPivot.y = transform.position.y;
        //높이를 무시하고 평면상의 거리만 고려

        attackDistance = Vector3.Distance(transform.position, attackPivot) + flatRange;
        navAgent.stoppingDistance = attackDistance;
        navAgent.speed = patrolSpeed;
    }
    #endregion

    #region Start()
    void Start()
    {
        InitializeStats();
        SetUp();
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
        if (enemyState == eState.Tracking)
        {
            var distance = Vector3.Distance(target.transform.position, transform.position);
            if (distance <= attackDistance)
            {
                Debug.Log("attackbegin");
                BeginAttack();
            }
        }
        //navmash의 desiredVelocity는 현재 속도값이 아니라 의도한 만큼 지정되는 값이다. 
        //벽 앞에서 적이 걷는다면 실제 속도는 0이기 때문에
        //애니메이션이 나오지 않는것을 방지하기 위함(애니메이터의 조건을 speed > 0.1로 걸어두었음)
        animator.SetFloat("Speed", navAgent.desiredVelocity.magnitude);
    }
    #endregion

    #region FixedUpdate()
    private void FixedUpdate()
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

        //공격 도중 플레이어가 이동해서 시야에서 놓치면?
        //physics.cast계열
        if (enemyState == eState.Attacking)
        {
            //몬스터의 전방 방향을 구함
            var direction = transform.forward;

            //네비의 현재속도를 기준으로 deltaDistance를 계산 -> 한 프레임 동안 몬스터가 이동하는 거리
            var deltaDistance = navAgent.velocity.magnitude * Time.deltaTime;

            // SphereCastNonAlloc을 사용하여 atkRoot.position에서 반지름이 atkRadius인 구 형태로 direction 방향으로 deltaDistance 거리만큼 구체 형태의 캐스트(이동한 궤적에 겹치는 Collider가 있는지 검사)
            // Trigger,Collision 등은 매 프레임 실행, 그 순간의 겹치는 Collider를 잡아내는 경우는 힘듦.
            // hits 배열에 충돌한 결과를 저장하고, 충돌한 객체의 수를 size에 저장
            //nonalloc은 우리가 만든 raycast hits 배열을 사용할 수 있음, 배열은 ref, out을 쓰지 않더라도 변경사항이 저장되므로 안써도 됨.
            var size = Physics.SphereCastNonAlloc(attackRoot.position, flatRange, direction, hits, deltaDistance, LayerTarget);

            //충돌한 객체들에 대한 반복
            //배열 크기보다 적은 값이 들어왔을 때, 나머지 공간에는 직전 프레임의 정보들이 들어있어서 그것을 방지하기 위해서 size를 사용함
            for (var i = 0; i < size; i++)
            {
                var attackTarget = hits[i].collider.GetComponent<Player>();

                //충돌한 객체가 Player인지 확인, 아직 공격한 적 없는 player이라면
                if (attackTarget != null && !lastAttackedTarget.Contains(attackTarget))
                {
                    //damageMessage 객체를 생성해서 데미지 정보를 설정
                    var message = new DamageMessage();
                    message.amount = flatDamage;
                    message.damager = this.gameObject;
                    message.hitPoint = hits[i].point;

                    //가상의 구가 움직이기도 전에 이미 닿아있는 colider가 있다면 무조건 0을 반환
                    //그러므로 충돌한 지점이 0 이하일 때와 그 외의 경우를 나누어 hitpoint 설정
                    if (hits[i].distance <= 0f)
                    {
                        //hit.point가 0이 나온다면 hit.point를 쓰는게 아니라 attackroot의 위치를 사용함.
                        message.hitPoint = attackRoot.position;
                    }
                    else
                    {
                        message.hitPoint = hits[i].point;
                    }

                    //충돌한 지점의 법선을 hitnormal에 설정
                    message.hitNormal = hits[i].normal;

                    //충돌한 player에게 데미지 적용
                    attackTarget.ApplyDamage(message);

                    //해당객체를 lastAttackedTarget 리스트에 attackTarget을 추가하여 중복공격 방지
                    lastAttackedTarget.Add(attackTarget);

                    break;
                }
            }
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
                        enemyState = eState.Idle;
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
                            // 레이어를 통해 몬스터를 중심으로 범위 내 플레이어를 인식

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
                        SetRandomPatrolPoint(); // 새로운 정찰 지점을 설정
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

    #region 공격

    private void BeginAttack()
    {
        enemyState = eState.AttackBegin;

        navAgent.isStopped = true;
        animator.SetTrigger("Attack");
    }

    public void EnableAttack()
    {
        enemyState = eState.Attacking;

        lastAttackedTarget.Clear();
    }

    public void DisableAttack()
    {
        enemyState = eState.Tracking;
        navAgent.isStopped = false;
    }

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

    #region 사망처리

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
    }
    #endregion
}
