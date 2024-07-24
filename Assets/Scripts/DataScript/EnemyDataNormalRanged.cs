using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data Normal Ranged", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyDataNormalRanged : ScriptableObject
{
    //�� ������ ���� ������ ����ؼ� �ִ�� ����� ����.

    public float f_hpMax;//�ִ� ü��
    public float f_hpCurr;//���� ü��
    public float f_speedMax;//�⺻ �ӷ�
    public float f_speedCurr;//���� �ӷ� 
    public float f_attackSpeedMax;//�ִ� ���� �ӵ�
    public float f_attackSpeedCurr;//���� ���� �ӵ�
    public float f_attackDamage;//���� ������
    public float f_paintOver; //��ĥȽ��
    public float f_lifeRound; //������ ��
    public float f_motionSpeed; //��� ���ݼӵ�(���ڰ� �������� ����)
    public float f_motionCoolTime; //���� ���� ��Ÿ��(���ڰ� ���� ���� ����)
    public bool b_plusAttackDamage;//�÷��̾� ��Ÿ�� ������ �߰�
    public bool b_plusSkillDamage;//�÷��̾� ��ų�� ������ �߰�
}
