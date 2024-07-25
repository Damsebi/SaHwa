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

    [HideInInspector] public bool plusAttackDamage; // �÷��̾� ��Ÿ�� �߰� ������ �߰�
    [HideInInspector] public bool plusSkillDamage; // �÷��̾� ��ų�� �߰� ������ �߰�
    [HideInInspector] public bool magicGroup; // ���� ����
    private bool isRanged; // �ٰŸ� ���Ÿ� ���� üũ
    private bool isBuffer; // �������� üũ

    private bool isDead; // ��� üũ

    public Transform attackRoot; //������ ���۵Ǵ� �ǹ�, �� �ǹ� �ش� �ݰ� ���� �ִ� �÷��̾ ���ݴ���
    public Transform viewTransform; //�� ��ġ

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
    private void OnDrawGizmosSelected()
    {
        if (attackRoot != null && !isBuffer && !isRanged)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);//������
            Gizmos.DrawSphere(attackRoot.position, flatRange);
            //���� ����� �ٰŸ� ���� ����
        }
        else if (attackRoot != null && !isBuffer && isRanged)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);//������
            Vector3 cubeSize = new Vector3(0.3f, 0.3f, flatRange); // width, height, depth
            Vector3 cubeCenter = attackRoot.position + -attackRoot.forward * (flatRange / 2);
            Gizmos.DrawCube(cubeCenter, cubeSize);
            //�� ���簢������� ���Ÿ� ���� ����
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
}
