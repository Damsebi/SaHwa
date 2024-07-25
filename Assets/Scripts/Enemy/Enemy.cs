using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Enemy : MonoBehaviour
{
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

    [HideInInspector] public bool plusAttackDamage; // 플레이어 평타에 추가 데미지 추가
    [HideInInspector] public bool plusSkillDamage; // 플레이어 스킬에 추가 데미지 추가
    [HideInInspector] public bool magicGroup; // 마법 세력
    private bool isRanged; // 근거리 원거리 공격 체크
    private bool isBuffer; // 버퍼인지 체크

    private bool isDead; // 사망 체크

    public Transform attackRoot; //공격이 시작되는 피벗, 이 피벗 해당 반경 내에 있는 플레이어가 공격당함
    public Transform viewTransform; //눈 위치

    public Player player;
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
        plusAttackDamage = enemyData.b_plusAttackDamage;
        plusSkillDamage = enemyData.b_plusSkillDamage;
        magicGroup = enemyData.b_magicGroup;
    }
    #endregion

    #region Start()
    void Start()
    {
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
    private void OnDrawGizmosSelected()
    {
        if (attackRoot != null && !isBuffer && !isRanged)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);//붉은색
            Gizmos.DrawSphere(attackRoot.position, flatRange);
            //원형 모양의 근거리 공격 범위
        }
        else if (attackRoot != null && !isBuffer && isRanged)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);//붉은색
            Vector3 cubeSize = new Vector3(0.3f, 0.3f, flatRange); // width, height, depth
            Vector3 cubeCenter = attackRoot.position + -attackRoot.forward * (flatRange / 2);
            Gizmos.DrawCube(cubeCenter, cubeSize);
            //긴 직사각형모양의 원거리 공격 범위
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
}
