using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    public GameObject enemyType;

    public Transform attackRoot;
    public Transform viewTransform;
    public EnemyData enemyData;

    [HideInInspector] public float viewAngle;
    [HideInInspector] public float viewDistance;
    [HideInInspector] public float flatRange;
    [HideInInspector] public float flatHybridRange;
    [HideInInspector] public bool isRanged;
    [HideInInspector] public bool isBuffer;

    public void SpawnEnemy()
    {
        Instantiate(enemyType, transform.position, transform.rotation);
    }

    #region �þ�, ���� ���� �׸���

    private void OnValidate() //scene â�� ����������
    {
        viewAngle = enemyData.f_viewAngle;
        viewDistance = enemyData.f_viewDistance;

        if (enemyData is EnemyDataMelee meleeData)
        {
            flatRange = meleeData.f_flatMeleeAttackRange;
            isRanged = false;
            isBuffer = false;
        }
        else if (enemyData is EnemyDataRange rangeData)
        {
            flatRange = rangeData.f_flatRangeAttackRange;
            isRanged = true;
            isBuffer = false;
        }
        else if (enemyData is EnemyDataHybird hybridData)
        {
            flatRange = hybridData.f_flatMeleeAttackRange;
            flatHybridRange = hybridData.f_flatRangeAttackRange;
            isRanged = true;
            isBuffer = false;
        }
        else if (enemyData is EnemyDataBuffer bufferData)
        {
            flatRange = bufferData.f_buffRange;
            isRanged = false;
            isBuffer = true;
        }
    }


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
}
