using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Enemy : MonoBehaviour
{
    #region ����
    public Transform atkRoot; //������ ���۵Ǵ� �ǹ�, �� �ǹ� �ش� �ݰ� ���� �ִ� �÷��̾ ���ݴ���
    public Transform viewTransform; //�� ��ġ
    public float atkRadius; //���� ����(�⺻ 1)
    public float viewDistance; //�þ� ����(�⺻ 10)
    public float viewAngle; //���Ը�� �þ߰� ����(�⺻ 50)

    #endregion

#if UNITY_EDITOR
    //����Ƽ �����Ϳ����� �۵�
    //���� �ܰ迡���� �۵����� ����
    private void OnDrawGizmosSelected()
    {
        if (atkRoot != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);//������
            Gizmos.DrawSphere(atkRoot.position, atkRadius);
            //���� ����� ���� ����(gui)
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
