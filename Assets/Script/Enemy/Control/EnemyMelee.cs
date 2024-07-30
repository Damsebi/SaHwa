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
        //���� ���� �÷��̾ �̵��ؼ� �þ߿��� ��ġ��?
        //physics.cast�迭

        if (enemyState == eState.Attacking)
        {
            //������ ���� ������ ����
            var direction = transform.forward;

            //�׺��� ����ӵ��� �������� deltaDistance�� ��� -> �� ������ ���� ���Ͱ� �̵��ϴ� �Ÿ�
            var deltaDistance = navAgent.velocity.magnitude * Time.deltaTime;

            // SphereCastNonAlloc�� ����Ͽ� atkRoot.position���� �������� atkRadius�� �� ���·� direction �������� deltaDistance �Ÿ���ŭ ��ü ������ ĳ��Ʈ(�̵��� ������ ��ġ�� Collider�� �ִ��� �˻�)
            // Trigger,Collision ���� �� ������ ����, �� ������ ��ġ�� Collider�� ��Ƴ��� ���� ����.
            // hits �迭�� �浹�� ����� �����ϰ�, �浹�� ��ü�� ���� size�� ����
            var size = Physics.SphereCastNonAlloc(attackRoot.position, flatRange, direction, hits, deltaDistance, LayerTarget);

            //�浹�� ��ü�鿡 ���� �ݺ�
            for (var i = 0; i < size; i++)
            {
                //�浹�� ��ü�� livingentity���� Ȯ��
                var atkTarget = hits[i].collider.GetComponent<Player>();

                //�浹�� ��ü�� Player�̰� ���� ������ �� ���� ��ü���
                if (atkTarget != null && !lastAttackedTarget.Contains(atkTarget))
                {
                    //damageMessage ��ü�� �����ؼ� ������ ������ ����
                    var message = new DamageMessage();
                    message.amount = flatDamage;
                    message.damager = this.gameObject;
                    message.hitPoint = hits[i].point;

                    //�浹�� ������ 0 ������ ���� �� ���� ��츦 ������ hitpoint ����
                    if (hits[i].distance <= 0f)
                    {
                        message.hitPoint = attackRoot.position;
                    }
                    else
                    {
                        message.hitPoint = hits[i].point;
                    }

                    //�浹�� ������ ������ hitnormal�� ����
                    message.hitNormal = hits[i].normal;

                    //�浹�� livingEntity���� ������ ����
                    atkTarget.ApplyDamage(message);

                    //�ش簴ü�� lastAtkTarget ����Ʈ�� �߰��Ͽ� �ߺ����� ����
                    lastAttackedTarget.Add(atkTarget);

                    break;
                }
            }
        }
    }

    #region ���� ���� ����
    private void EnableAttack()
    {
        enemyState = eState.Attacking;

        lastAttackedTarget.Clear();
    }
    #endregion


}
