using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    //�ǰ�, ���� 
    [SerializeField] private PlayerData playerData;
    private PlayerMaskChange playerMaskChange;
    private PlayerSkillSet playerSkillSet;

    public bool isDamaged;
    private bool isDead;
    public bool IsDead {  get { return isDead; } }

    [SerializeField] private float currentHp;

    private void Awake()
    {
        playerMaskChange = GetComponentInParent<PlayerMaskChange>();
        playerSkillSet = GetComponentInParent<PlayerSkillSet>();
    }

    private void Start()
    {
        SetUp();
    }

    public void SetUp()
    {
        currentHp = playerData.maxHp;
    }


    public bool ApplyDamage(DamageMessage damageMessage)
    {
        if (isDead)
        {
            return false;
        }

        isDamaged = true; //�ִϸ��̼� ������ Ǯ��
        currentHp -= damageMessage.amount;

        if (currentHp <= 0)
        {
            Die();
        }
        else if (damageMessage.amount > 0 || !playerSkillSet.IgnoreStun) //��ų ��� �߿��� ���۾Ƹ� ����(�ӽ�)
        {
            playerMaskChange.ActiveAnimator.CrossFade("Hit", .2f);
        }

        return true;
    }

    public void Die()
    {
        currentHp = 0;
        isDead = true;
        playerMaskChange.ActiveAnimator.CrossFade("Die", 2f);
    }
}
