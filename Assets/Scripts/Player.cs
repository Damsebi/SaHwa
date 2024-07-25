using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IAttack, IHit
{
    PlayerConMove playerConMove;
    public float f_hp  = 100f;
    private float f_hpCurrent;
    public float f_attakDamage  = 10;
    private float f_attackDamageCurrent;
    public bool isDead;
    #region Start()
    void Start()
    {
        f_hpCurrent = f_hp ;
        f_attackDamageCurrent = f_attakDamage ;
    }
    #endregion

    #region Update()
    void Update()
    {

    }
    #endregion

    #region FixedUpdate()
    private void FixedUpdate()
    {
        playerConMove.Movement();
        playerConMove.Rotation();       
    }
    #endregion

    public void Attack()
    {

    }
    public void Hit(float f_damage)
    {
        f_hpCurrent -= f_damage;
    }

}
