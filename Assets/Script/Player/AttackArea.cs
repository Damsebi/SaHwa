using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    public float damage;
    public GameObject damager;
    public char color;
    public float value;

    private void OnTriggerEnter(Collider other)
    {
        //스킬별로
        if (this.gameObject.CompareTag("HumanNormalAttack"))
        {
            SetMessage(playerData.humanNormalAttackDamage, 'B', 1);
        }
        else if (this.gameObject.CompareTag("HumanFirstSkill"))
        {
            SetMessage(playerData.humanFirstSkillDamage, 'B', 2);
        }
        else if (this.gameObject.CompareTag("HumanSecondSkill"))
        {
            SetMessage(playerData.humanSecondSkillDamage, 'B', 2);
        }
        else if (this.gameObject.CompareTag("AnimalNormalAttack"))
        {
            SetMessage(playerData.animalNormalAttackDamage, 'W', 1);
        }
        else if (this.gameObject.CompareTag("AnimalFirstSkill"))
        {
            SetMessage(playerData.animalFirstSkillDamage, 'W', 2);

        }
        else if (this.gameObject.CompareTag("AnimalSecondSkill"))
        {
            SetMessage(playerData.animalSecondSkillDamage, 'W', 2);
        }
        
        //처형스킬 따로

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            var atkTarget = other.GetComponent<Enemy>();

            if (atkTarget != null)
            {
                var message = new DamageMessage();
                message.amount = damage;
                message.damager = PlayerMaskChange.instance.ActiveCharacter;
                message.color = color;
                message.value = value;

                atkTarget.ApplyDamage(message);
            }
            //Debug.Log("atkTarget.name" + atkTarget.name);
        }
    }

    private void SetMessage(float setDamage, char setColor, float setStackValue)
    {
        damage = setDamage;
        damager = this.gameObject;
        color = setColor;
        value = setStackValue;
    }
}
