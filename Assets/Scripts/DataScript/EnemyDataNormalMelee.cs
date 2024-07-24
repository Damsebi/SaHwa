using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data Normal Melee", menuName = "ScriptableObjects/EnemyData", order = 2)]
public class EnemyDataNormalMelee : ScriptableObject
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
