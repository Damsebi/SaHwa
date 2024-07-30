using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHybrid : Enemy
{
    private EnemyDataHybird enemyDataHybird;
    // Start is called before the first frame update
    protected override void Start()
    {


        flatDamage = enemyDataHybird.f_flatMeleeAttackDamage;
        flatMotionSpeed = enemyDataHybird.f_flatMeleeAttackMotionSpeed;
        flatMotionCoolTime = enemyDataHybird.f_flatMeleeAttackMotionCoolTime;
        flatRange = enemyDataHybird.f_flatMeleeAttackRange;

        flatHybridDamage = enemyDataHybird.f_flatRangeAttackDamage;
        flatHybridMotionSpeed = enemyDataHybird.f_flatRangeAttackMotionSpeed;
        flatHybridMotionCoolTime = enemyDataHybird.f_flatRangeAttackMotionCoolTime;
        flatHybridRange = enemyDataHybird.f_flatRangeAttackRange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
