using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    //�� ������ ���� ������ ����ؼ� �ִ�� ����� ����.

    public float hpMax;//�ִ� ü��
    public float hpCurr;//���� ü��
    public float speedMax;//�ִ� �ӷ� 
    public float speedCurr;//���� �ӷ�
    public float attackSpeedMax;//�ִ� ���� �ӵ�
    public float attackSpeedCurr;//���� ���� �ӵ�
    public float attackDamage;//���� ������
}
