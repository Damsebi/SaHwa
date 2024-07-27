using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthUpdate : MonoBehaviour
{
    [SerializeField] float currentHp;
    [SerializeField] bool isDamaged;
    [SerializeField] bool isDead;
    [SerializeField] bool isStunned;

    public void GetHealth()
    {

    }

    public void AttackedByMelee()
    {

    }

    public void AttackedByRanged()
    {

    }

    public void AttackedByMagic()
    {

    }


    public void Die()
    {
        isDead = true;


    }

}
