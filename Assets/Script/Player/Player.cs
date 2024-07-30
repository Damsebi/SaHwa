using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    //피격, 공격 
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

        isDamaged = true; //애니메이션 끝나면 풀림
        currentHp -= damageMessage.amount;

        if (currentHp <= 0)
        {
            Die();
        }
        else if (damageMessage.amount > 0 || !playerSkillSet.IgnoreStun) //스킬 사용 중에는 슈퍼아머 느낌(임시)
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
