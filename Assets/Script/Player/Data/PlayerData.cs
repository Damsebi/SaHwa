using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]

public class PlayerData : ScriptableObject
{
    /*��������
    �⺻���� ��ũ����
    ������ �� ���� �ٶ󺸱�
    ���� �ٶ󺸸� �ȴ� �ִ�

    [�ִϸ��̼� ���� �� �ൿ ����]
    - animationState �Լ��� ����� ����Ǵ� �ִϸ��̼��� �������̶�� ���� false
    - �ƴϸ� �ִϸ��̼� �߿� ������ ���� ��Ű��
     */


    //����
    [Header("ĳ���� ü��")]
    public float maxHp;
    [Header("���Ż �̵��ӵ�")]
    public float humanMoveSpeed;
    [Header("����Ż�� �̵��ӵ�")]
    public float animalMoveSpeed;
    [Header("�÷��̾��� ȸ���ӵ�")]
    public float playerRotateSpeed;


    [Space(20f)]
    [Header("���Ż ��Ÿ ������(��� ����")]
    public float humanNormalAttackDamage;
    [Header("���Ż ��Ÿ1-1 �����߿� �ٸ� �ൿ �����ϴ� �ð�")]
    public float restrictTimeForNormalAttack1_1;
    [Header("���Ż ��Ÿ1-2 �����߿� �ٸ� �ൿ �����ϴ� �ð�")]
    public float restrictTimeForNormalAttack1_2;

    [Space(10f)]
    [Header("�Ա�� ������")]
    public float humanFirstSkillDamage;
    [Header("�Ա�� ��ų ��Ÿ��")]
    public float humanFirstSkillCooldown;
    [Header("�Ա�� ��ų ������ ������ �� Ž�� ����")]
    public float humanFirstSkillRange;
    [Header("�Ա�� ���� �ð�")]
    public float humanFirstSkillDuration;
    [Header("�Ա���� ũ�� ")]
    public Vector3 humanFirstSkillScale;

    [Space(10f)]
    [Header("���Ž� ������")]
    public float humanSecondSkillDamage;
    [Header("���Ž� ��ų ��Ÿ��")]
    public float humanSecondSkillCooldown;
    [Header("���� �� ���� ���� �ð�")]
    public float humanSecondSkillDuration;

    [Space(10f)]
    [Header("���Ż ȸ�� ��ų ��Ÿ��")]
    public float humanAvoidStepCooldown;


    [Space(20f)]
    [Header("����Ż ��Ÿ ������")]
    public float animalNormalAttackDamage;
    [Header("����Ż ��Ÿ ��Ÿ��")]
    public float animalNormalAttackCooldown;

    [Space(10f)]
    [Header("��� ������ ������")]
    public float animalFirstSkillDamage;
    [Header("��� ������ ��ų ��Ÿ��")]
    public float animalFirstSkillCooldown;

    [Space(10f)]
    [Header("���� ���� ������")]
    public float animalSecondSkillDamage;
    [Header("���� ���� ��ų ��Ÿ��")]
    public float animalSecondCooldown;

    [Space(10f)]
    [Header("����Ż ȸ�� ��ų ��Ÿ��")]
    public float animalAvoidStepCooldown;

    [Space(20f)]
    [Header("ī�޶�")]
    [Header("ī�޶� �÷��̾ ������� �ӵ�����. ���� �������� ����")]
    [Range(0, 1)] public float cameraFollowSpeed;

    [Header("���� ī�޶� X,Y,Z ��ġ��")]
    public Vector3 originCameraOrbitAxisPosition;
    
    [Header("���Ʒ� ȸ�� �ε巯�� ����")]
    [Range(0, 1)] public float smoothVerticalRotationRate;
    [Header("�¿� ȸ�� �ε巯�� ����")]
    [Range(0, 1)] public float smoothHorizontalRotationRate;

    [Header("Left Shift�� ����ȸ���� ȸ���ð� ����. ���� �������� ȸ���ð� ª��")]
    [Range(0, 0.1f)] public float turnForwardDuration;

    [Header("Left Shift�� ����ȸ���� ȸ�� �ε巯�� ����. ���� �������� �ε巯��")]
    [Range(0, 1)] public float turnForwardSmoothRate;

    [Header("Left Shift�� Ÿ�ٶ��½� ȸ�� �ε巯�� ����. ���� �������� �ε巯��")]
    [Range(0, 1)] public float turnToTargetSmoothRate;


    [Header("�ָ��� ��� Ž�� ����")]
    [Range(0, 20)] public float detectRange;

    [Header("�ָ��� ����� ���̾�(��, ������, npc ���)")]
    public LayerMask targetLayer;

    [Header("�ִ� �þ߰� / 2")]
    [Range(0, 70)] public float maximumAngleWithTarget;

    //[Header("ī�޶� �÷��̾� ���� �������� ȸ���ϴ� �ӵ�")]
    //[Range(0, 1)] public float alignCameraSmoothTime;

    [Header("ī�޶� �¿� ȸ����")]
    [Range(0, 10)] public float cameraYValue;
}