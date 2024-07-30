using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    public GameObject enemyType;

    private Transform attackRoot;
    private Transform viewTransform;
    [HideInInspector] public float viewAngle;
    [HideInInspector] public float viewDistance;
    [HideInInspector] public float flatRange;
    [HideInInspector] public bool isRanged;
    [HideInInspector] public bool isBuffer;

    public void SpawnEnemy()
    {
        Instantiate(enemyType, transform.position, transform.rotation);
    }
    private void OnValidate()
    {
        var prefabContents = PrefabUtility.LoadPrefabContents(AssetDatabase.GetAssetPath(enemyType));
        var attackRootObject = prefabContents.transform.Find("AttackRoot");
        var viewObject = prefabContents.transform.Find("ViewPoint");
        var enemy = prefabContents.GetComponent<Enemy>();

        Debug.Log(enemy.flatRange);

        if (enemy != null)
        {
            attackRoot = attackRootObject;
            viewTransform = viewObject;
            flatRange = enemy.flatRange;
            viewAngle = enemy.viewAngle;
            viewDistance = enemy.viewDistance;
            isRanged = enemy.isRanged;
            isBuffer = enemy.isBuffer;
        }
        PrefabUtility.UnloadPrefabContents(prefabContents); //�ε�� ������ ������ ��ε�
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
