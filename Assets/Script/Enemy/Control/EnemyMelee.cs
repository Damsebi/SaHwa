using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : Enemy
{
    private RaycastHit[] hits = new RaycastHit[10];
    private EnemyDataMelee enemyDataMelee;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        InitializeStats(enemyDataMelee.f_hp, enemyDataMelee.i_hpBarCount, enemyDataMelee.f_trackingSpeed,
            enemyDataMelee.f_patrolSpeed, enemyDataMelee.f_viewAngle, enemyDataMelee.f_viewDistance,
            enemyDataMelee.i_paintOver, enemyDataMelee.f_turnSmoothVelocity, enemyDataMelee.b_plusAttackDamage,
            enemyDataMelee.b_plusSkillDamage, enemyDataMelee.b_magicGroup, enemyDataMelee.f_patrolRange);

        flatDamage = enemyDataMelee.f_flatMeleeAttackDamage;
        flatMotionSpeed = enemyDataMelee.f_flatMeleeAttackMotionSpeed;
        flatMotionCoolTime = enemyDataMelee.f_flatMeleeAttackMotionCoolTime;
        flatRange = enemyDataMelee.f_flatMeleeAttackRange;
        patrolWaitingTimeMin = enemyDataMelee.i_patrolWaitingTimeMin;
        patrolWaitingTimeMax = enemyDataMelee.i_patrolWaitingTimeMax;
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //공격 도중 플레이어가 이동해서 시야에서 놓치면?
        //physics.cast계열

        if (enemyState == eState.Attacking)
        {
            //몬스터의 전방 방향을 구함
            var direction = transform.forward;

            //네비의 현재속도를 기준으로 deltaDistance를 계산 -> 한 프레임 동안 몬스터가 이동하는 거리
            var deltaDistance = navAgent.velocity.magnitude * Time.deltaTime;

            // SphereCastNonAlloc을 사용하여 atkRoot.position에서 반지름이 atkRadius인 구 형태로 direction 방향으로 deltaDistance 거리만큼 구체 형태의 캐스트(이동한 궤적에 겹치는 Collider가 있는지 검사)
            // Trigger,Collision 등은 매 프레임 실행, 그 순간의 겹치는 Collider를 잡아내는 경우는 힘듦.
            // hits 배열에 충돌한 결과를 저장하고, 충돌한 객체의 수를 size에 저장
            var size = Physics.SphereCastNonAlloc(attackRoot.position, flatRange, direction, hits, deltaDistance, LayerTarget);

            //충돌한 객체들에 대한 반복
            for (var i = 0; i < size; i++)
            {
                //충돌한 객체가 livingentity인지 확인
                var atkTarget = hits[i].collider.GetComponent<Player>();

                //충돌한 객체가 Player이고 아직 공격한 적 없는 객체라면
                if (atkTarget != null && !lastAttackedTarget.Contains(atkTarget))
                {
                    //damageMessage 객체를 생성해서 데미지 정보를 설정
                    var message = new DamageMessage();
                    message.amount = flatDamage;
                    message.damager = this.gameObject;
                    message.hitPoint = hits[i].point;

                    //충돌한 지점이 0 이하일 때와 그 외의 경우를 나누어 hitpoint 설정
                    if (hits[i].distance <= 0f)
                    {
                        message.hitPoint = attackRoot.position;
                    }
                    else
                    {
                        message.hitPoint = hits[i].point;
                    }

                    //충돌한 지점의 법선을 hitnormal에 설정
                    message.hitNormal = hits[i].normal;

                    //충돌한 livingEntity에게 데미지 적용
                    atkTarget.ApplyDamage(message);

                    //해당객체를 lastAtkTarget 리스트에 추가하여 중복공격 방지
                    lastAttackedTarget.Add(atkTarget);

                    break;
                }
            }
        }
    }

    #region 공격 들어가는 지점
    private void EnableAttack()
    {
        enemyState = eState.Attacking;

        lastAttackedTarget.Clear();
    }
    #endregion


}
