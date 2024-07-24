using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataBuffer", menuName = "ScriptableObjects/EnemyData")]
public class EnemyDataBuffer : ScriptableObject
{
    //�� ������ ���� ������ ����ؼ� �ִ�� ����� ����.

    public float hpMax;//�ִ� ü��
    public float hpCurr;//���� ü��
    public float speedMax;//�ִ� �ӷ� 
    public float speedCurr;//���� �ӷ�
    public float buffRange;
}
