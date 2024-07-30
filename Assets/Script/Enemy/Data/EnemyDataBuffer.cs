using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data Buffer", menuName = "Scriptable Object/Enemy Data/WeaponType/Buffer")]
public class EnemyDataBuffer : EnemyData
{
    [Header("���� ���� �ӵ�")]
    public float f_buffMotionSpeed;

    [Header("���� ���� ����")]
    public float f_buffMotionCoolTime;

    [Header("���� ����")]
    public float f_buffRange;

    [Header("�÷��̾�κ��� �Ÿ� �ִ� �Ÿ� ����")]
    public float f_distanceToPlayerMax;
}
