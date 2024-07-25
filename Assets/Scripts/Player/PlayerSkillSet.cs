using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerSkillSet : MonoBehaviour
{//스킬 스크립트 분할할지는 리팩토링할 때
    [SerializeField] private PlayerData playerData;
    [SerializeField] LayerMask enemyLayer;

    private PlayerMaskChange playerMaskChange;

    [SerializeField] private bool restrictForSkill;
    public bool RestrictForSkill { get { return restrictForSkill; } }

    #region 사람탈 기본공격
    private bool isNormalAttacking;
    private bool isSkillCoroutineRunning;
    public bool IsSkillCoroutineRunning { get { return isSkillCoroutineRunning; } }
    [SerializeField] private bool canUseHumanNormalAttack;
    #endregion

    #region 먹기둥
    [SerializeField] private bool canUseInkPillar;

    [SerializeField] GameObject inkPillarPrefab;
    private GameObject[] inkPillar = new GameObject[3];
    #endregion

    #region 먹 스매쉬
    [SerializeField] GameObject inkSmashPrefab;
    private GameObject inkSmashArea;
    [SerializeField] private bool canUseInkSmash;
    #endregion

    #region 회피
    [SerializeField] private bool canUseHumanAvoidStep;
    #endregion 


    #region 동물탈 기본공격
    [SerializeField] private bool canUseAnimalNormalAttack;
    #endregion

    #region 양손공격
    [SerializeField] private bool canUseXClaw;

    #endregion

    #region 도약공격
    [SerializeField] private bool canUseLeapClaw;

    #endregion

    #region 회피
    [SerializeField] private bool canUseAnimalAvoidBack;

    #endregion



    private void Awake()
    {
        playerMaskChange = GetComponent<PlayerMaskChange>();
    }

    private void Start()
    {
        canUseHumanNormalAttack = true;
        canUseInkPillar = true;
        canUseInkSmash = true;
        canUseHumanAvoidStep = true;

        canUseAnimalNormalAttack = true;
        canUseXClaw = true;
        canUseLeapClaw = true;
        canUseAnimalAvoidBack = true;
    }
    #region 사람탈 함수
    public IEnumerator HumanNormalAttack()
    {
        if (!isSkillCoroutineRunning)
        {
            isSkillCoroutineRunning = true;

            playerMaskChange.ActiveAnimator.SetBool("isNormalAttacking", true);
            yield return null;
            playerMaskChange.ActiveAnimator.SetBool("isNormalAttacking", false);


            if (playerMaskChange.ActiveAnimator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
            {
                playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = true);
                yield return new WaitForSeconds(playerData.restrictTimeForNormalAttack1_1);
            }
            else if (playerMaskChange.ActiveAnimator.GetCurrentAnimatorStateInfo(0).IsName("NormalAttack"))
            {
                playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = true);
                yield return new WaitForSeconds(playerData.restrictTimeForNormalAttack1_2);
            }
            else if (playerMaskChange.ActiveAnimator.GetCurrentAnimatorStateInfo(0).IsName("NormalAttack2"))
            {
                playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = true);
                yield return new WaitForSeconds(playerData.restrictTimeForNormalAttack1_3);
            }

            playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = false);
            isSkillCoroutineRunning = false;
        }
    }


    public IEnumerator HumanFirstSkill()
    {
        if (canUseInkPillar)
        {
            restrictForSkill = true;
            canUseInkPillar = false;

            playerMaskChange.ActiveAnimator.CrossFade("FirstSkill", .1f);

            yield return new WaitForSeconds(.8f);
            Collider[] colliders = Physics.OverlapSphere(transform.position, playerData.inkPillarSkillRange, enemyLayer);

            #region SelectionSort
            int countEnemies = colliders.Length;
            if (countEnemies >= 3) countEnemies = 3;
            Collider[] selectEnemies = new Collider[countEnemies];

            for (int i = 0; i < countEnemies; i++)
            {
                int index = i;
                float distance1 = Vector3.Distance(playerMaskChange.ActiveCharacter.transform.position, colliders[index].transform.position);
                for (int j = i + 1; j < colliders.Length; j++)
                {
                    float distance2 = Vector3.Distance(playerMaskChange.ActiveCharacter.transform.position, colliders[j].transform.position);
                    if (distance1 > distance2) index = j;
                }

                Collider swapPosition = colliders[i];
                colliders[i] = colliders[index];
                colliders[index] = swapPosition;

                selectEnemies[i] = colliders[i];
            }
            #endregion

            for (int i = 0; i < countEnemies; i++)
            {
                inkPillar[i] = Instantiate(inkPillarPrefab, selectEnemies[i].transform.position, Quaternion.identity);
                inkPillar[i].transform.localScale = playerData.inkPillarScale;
            }

            yield return new WaitForSeconds(.7f);
            restrictForSkill = false;

            //먹기둥 지속시간과 쿨타임 어느게 일찍 끝나냐에 따라 다름

            if ((playerData.inkPillarDuration + 0.8f) < playerData.inkPillarCooldown)
            {
                yield return new WaitForSeconds(playerData.inkPillarDuration - .7f);
                for (int i = 0; i < countEnemies; i++)
                {
                    Destroy(inkPillar[i]);
                }

                yield return new WaitForSeconds(playerData.inkPillarCooldown - .8f - playerData.inkPillarDuration);
                canUseInkPillar = true;
            }
            else
            {
                yield return new WaitForSeconds(playerData.inkPillarCooldown - .8f - playerData.inkPillarDuration);
                canUseInkPillar = true;

                yield return new WaitForSeconds(playerData.inkPillarDuration - .7f);
                for (int i = 0; i < countEnemies; i++)
                {
                    Destroy(inkPillar[i]);
                }
            }

            //범위 데미지 
        }
    }

    public IEnumerator HumanSecondSkill()
    {
        if (canUseInkSmash)
        {
            restrictForSkill = true;
            canUseInkSmash = false;

            playerMaskChange.ActiveAnimator.CrossFade("SecondSkill", .1f);
            for (int i = 0; i < 40; i++)
            {
                yield return new WaitForSeconds(.025f);
                playerMaskChange.ActiveRigidbody.MovePosition
                    (playerMaskChange.ActiveCharacter.transform.position + playerMaskChange.ActiveCharacter.transform.forward * 30 * Time.deltaTime);
            }

            inkSmashArea = Instantiate(inkSmashPrefab,playerMaskChange.ActiveCharacter.transform.position + playerMaskChange.ActiveCharacter.transform.forward * 1.3f, playerMaskChange.ActiveCharacter.transform.rotation);

            yield return new WaitForSeconds(.5f);
            restrictForSkill = false;

            yield return new WaitForSeconds(playerData.inkSmashDuration);
            Destroy(inkSmashArea);

            yield return new WaitForSeconds(playerData.inkSmashCooldown - 1.6f - playerData.inkSmashDuration);
            canUseInkSmash = true;
        }
    }

    public IEnumerator HumanAvoidBack()
    {
        if (canUseHumanAvoidStep)
        {
            restrictForSkill = true;
            canUseHumanAvoidStep = false;
            playerMaskChange.ActiveAnimator.CrossFade("AvoidBack", .1f);

            for (int i = 0; i < 50; i++) 
            {
                yield return new WaitForSeconds(.01f);
                playerMaskChange.ActiveRigidbody.MovePosition
                    (playerMaskChange.ActiveCharacter.transform.position - playerMaskChange.ActiveCharacter.transform.forward * 20 * Time.deltaTime);
            }

            restrictForSkill = false;

            yield return new WaitForSeconds(playerData.avoidStepCooldown - .6f);
            canUseHumanAvoidStep = true;
        }
    }
    #endregion

    #region 짐승탈 함수

    public IEnumerator AnimalNormalAttack()
    {
        if (canUseHumanNormalAttack)
        {
            restrictForSkill = true;
            canUseHumanNormalAttack = false;

            playerMaskChange.ActiveAnimator.CrossFade("NormalAttack", .1f);

            yield return new WaitForSeconds(1f);
            restrictForSkill = false;

            yield return new WaitForSeconds(playerData.animalNormalAttackCooldown - 1f);
            canUseHumanNormalAttack = true;
        }
    }

    public IEnumerator AnimalFirstSkill()
    {
        if (canUseXClaw)
        {
            restrictForSkill = true;
            canUseXClaw = false;

            playerMaskChange.ActiveAnimator.CrossFade("FirstSkill", .1f);
            
            yield return new WaitForSeconds(2f);
            restrictForSkill = false;

            yield return new WaitForSeconds(playerData.xClawCooldown - 2f);
            canUseXClaw = true;
        }
    }

    public IEnumerator AnimalSecondSkill()
    {
        if (canUseLeapClaw)
        {
            restrictForSkill = true;
            canUseLeapClaw = false;

            playerMaskChange.ActiveAnimator.CrossFade("SecondSkill", .1f);
            for (int i = 0; i < 60; i++)
            {
                yield return new WaitForSeconds(.01f);
                playerMaskChange.ActiveRigidbody.MovePosition
                    (playerMaskChange.ActiveCharacter.transform.position + playerMaskChange.ActiveCharacter.transform.forward * 30 * Time.deltaTime);
            }
            yield return new WaitForSeconds(.3f);
            restrictForSkill = false;

            yield return new WaitForSeconds(playerData.leapClawCooldown -2f);
            canUseLeapClaw = true;
        }
    }

    public IEnumerator AnimalAvoidBack()
    {
        if (canUseAnimalAvoidBack)
        {
            restrictForSkill = true;
            canUseAnimalAvoidBack = false;
            playerMaskChange.ActiveAnimator.CrossFade("AvoidBack", .1f);
            yield return new WaitForSeconds(.6f);

            restrictForSkill = false;

            yield return new WaitForSeconds(playerData.avoidStepCooldown - .6f);
            canUseAnimalAvoidBack = true;
        }
    }
    #endregion

    #region 귀신탈 함수


    #endregion

}
