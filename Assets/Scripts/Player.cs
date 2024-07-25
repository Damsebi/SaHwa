using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    private PlayerConMove playerConMove;
    public float f_hp = 100f;
    private float f_hpCurrent;
    public float f_attakDamage = 10;
    private float f_attackDamageCurrent;
    public bool isDead;

    #region Start()
    void Start()
    {
        f_hpCurrent = f_hp;
        f_attackDamageCurrent = f_attakDamage;
        playerConMove = GetComponent<PlayerConMove>();

        if (playerConMove == null)
        {
            Debug.LogError("PlayerConMove component is missing from the Player GameObject.");
        }
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
        if (playerConMove != null)
        {
            playerConMove.Movement();
            playerConMove.Rotation();
        }
    }
    #endregion

    public bool ApplyDamage(DamageMessage damageMessage)
    {
        return true;
    }

    public void Die()
    {

    }
}
