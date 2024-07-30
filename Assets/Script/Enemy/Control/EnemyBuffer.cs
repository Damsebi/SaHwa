using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuffer : Enemy
{
    private EnemyDataBuffer enemyDataBuffer;
    // Start is called before the first frame update
    protected override void Start()
    {
        InitializeStats(enemyDataBuffer.f_hp, enemyDataBuffer.i_hpBarCount, enemyDataBuffer.f_trackingSpeed,
            enemyDataBuffer.f_patrolSpeed, enemyDataBuffer.f_viewAngle, enemyDataBuffer.f_viewDistance,
            enemyDataBuffer.i_paintOver, enemyDataBuffer.f_turnSmoothVelocity, enemyDataBuffer.b_plusAttackDamage,
            enemyDataBuffer.b_plusSkillDamage, enemyDataBuffer.b_magicGroup, 0);

        flatMotionSpeed = enemyDataBuffer.f_buffMotionSpeed;
        flatMotionCoolTime = enemyDataBuffer.f_buffMotionCoolTime;
        flatRange = enemyDataBuffer.f_buffRange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
