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

    #region 시야, 공격 범위 그리기

    private void OnValidate() //scene 창에 보여지도록
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
