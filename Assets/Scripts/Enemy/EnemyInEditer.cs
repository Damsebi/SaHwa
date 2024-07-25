using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyInEditer : MonoBehaviour
{
    #region 선언
    public Transform atkRoot; //공격이 시작되는 피벗, 이 피벗 해당 반경 내에 있는 플레이어가 공격당함
    public Transform viewTransform; //눈 위치
    public float atkRadius; //공격 범위(기본 1)
    public float viewDistance; //시야 범위(기본 10)
    public float viewAngle; //원뿔모양 시야각 변수(기본 50)

    #endregion

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        if (atkRoot != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);//붉은색
            Gizmos.DrawSphere(atkRoot.position, atkRadius);
            //원형 모양의 공격 범위(gui)
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
}
