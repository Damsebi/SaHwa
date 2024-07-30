using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpawnEnemy : MonoBehaviour
{
    [HideInInspector] public Transform attackRoot; 
    [HideInInspector] public Transform viewTransform; 
    [HideInInspector] public float viewAngle; 
    [HideInInspector] public float viewDistance;
    [HideInInspector] public float flatRange;
    [HideInInspector] public bool isRanged;
    [HideInInspector] public bool isBuffer;

    private SpawnPosition spawnPosition;
    private GameObject enemyPrefab;

    void Start()
    {
        spawnPosition = GetComponentInChildren<SpawnPosition>();
        enemyPrefab = spawnPosition.enemyType;

        if (enemyPrefab != null)
        {
            var enemy = enemyPrefab.GetComponent<Enemy>();
            if (enemy != null)
            {
                attackRoot = enemy.attackRoot;
                viewTransform = enemy.viewTransform;
                flatRange = enemy.flatRange;
                viewAngle = enemy.viewAngle;
                viewDistance = enemy.viewDistance;
                isRanged = enemy.isRanged;
                isBuffer = enemy.isBuffer;
            }
        }

        if (spawnPosition != null)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

    }
    private void OnTriggerEnter(Collider player)
    {
        Debug.Log("ontrigger");
        if (player.CompareTag("Player"))
        {
            spawnPosition.SpawnEnemy();
        }
    }

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

}
