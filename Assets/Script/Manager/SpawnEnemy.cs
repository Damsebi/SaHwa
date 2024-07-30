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

}
