using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillSet : MonoBehaviour
{//스킬 스크립트 분할할지는 리팩토링할 때
    [SerializeField] private PlayerData playerData;
    [SerializeField] LayerMask enemyLayer;

    private PlayerMaskChange playerMaskChange;
    private PlayerMovement playerMovement;

    [SerializeField] private bool restrictForSkill;
    public bool RestrictForSkill { get { return restrictForSkill; } }

    #region 사람탈
    #region 기본공격
    private bool isNormalAttacking;
    private int normalAttackCount;

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
    #endregion

    #region 동물탈
    #region 기본공격
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
    #endregion

    #region 귀신탈
    private bool canUseFinish;
    private bool turnOriginMask;

    [SerializeField] private GameObject characterSkin;

    [SerializeField] GameObject ghostWeapon;
    [SerializeField] GameObject humanWeapon;

    [SerializeField] GameObject finishSkillArea; // 처형 영역 오브젝트
    //[SerializeField][Range(0, 20)] float executionRange; //처형 범위 나중에 
    //[SerializeField] float executionAreaSizeRate; //처형 영역 크기비율 나중에

    #endregion


    private void Awake()
    {
        playerMaskChange = GetComponent<PlayerMaskChange>();
        playerMovement = GetComponent<PlayerMovement>();    
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

        canUseFinish = true;

        normalAttackCount = 0;
    }
    #region 애니메이션 상태 체크(다음 빌드때 애니메이션 스크립트 하나 만들 예정)
    public void AnimationState() 
    {
     

        var currentAnimation = playerMaskChange.ActiveAnimator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimation.IsName("NormalAttack"))
        {
            if (currentAnimation.normalizedTime < .3f)
            {
                playerMovement.canRotate = true;
                playerMovement.canMove = false;

            }
            else if (currentAnimation.normalizedTime < .35f)
            {
                restrictForSkill = true;
                playerMovement.canRotate = false;
            }
        }
        else if (currentAnimation.IsName("NormalAttack2"))
        {
            if (currentAnimation.normalizedTime < .3f)
            {
                playerMovement.canRotate = true;
                playerMovement.canMove = false;

            }
            else if (currentAnimation.normalizedTime < .35f)
            {
                restrictForSkill = true;
                playerMovement.canRotate = false;
            }
        }
        else if (currentAnimation.IsName("NormalAttack3"))
        {
            if (currentAnimation.normalizedTime < .3f)
            {
                playerMovement.canRotate = true;
                playerMovement.canMove = false;

            }
            else if (currentAnimation.normalizedTime < .35f)
            {
                restrictForSkill = true;
                playerMovement.canRotate = false;
            }
        }
        else if (currentAnimation.IsName("Movement") || currentAnimation.IsName("LockOnMovement")) //버그풀어주는 용도로도 활용
        {
            //restrictForSkill = false; // 다른 스킬에도 영향을 줌...
            playerMovement.canRotate = true;
            playerMovement.canMove = true;
        }
    }
    #endregion

    #region 사람탈 함수
    public IEnumerator HumanNormalAttack()
    {
        var currentAnimation = playerMaskChange.ActiveAnimator.GetCurrentAnimatorStateInfo(0);
        if (!isNormalAttacking)
        {
            isNormalAttacking = true;

            playerMaskChange.ActiveAnimator.SetTrigger("isNormalAttacking");

            if (currentAnimation.IsName("Movement") || currentAnimation.IsName("LockOnMovement"))
            {
                yield return new WaitForSeconds(playerData.restrictTimeForNormalAttack1_1);
                playerMaskChange.ActiveAnimator.SetInteger("normalAttackCount",1);
            }
            else if (currentAnimation.IsName("NormalAttack"))
            {
                yield return new WaitForSeconds(playerData.restrictTimeForNormalAttack1_2);
                playerMaskChange.ActiveAnimator.SetInteger("normalAttackCount", 2);
            }
            else if (currentAnimation.IsName("NormalAttack2"))
            {
                yield return new WaitForSeconds(playerData.restrictTimeForNormalAttack1_3);
                playerMaskChange.ActiveAnimator.SetInteger("normalAttackCount", 0);
            }

            yield return new WaitForSeconds(.2f);

            isNormalAttacking = false;
            restrictForSkill = false;
        }
    }
    public IEnumerator HumanFirstSkill()
    {
        if (canUseInkPillar)
        {
            playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = true);

            canUseInkPillar = false;

            playerMaskChange.ActiveAnimator.CrossFade("FirstSkill", .1f);

            yield return new WaitForSeconds(.8f);
            Collider[] colliders = Physics.OverlapSphere(transform.position, playerData.humanFirstSkillRange, enemyLayer);

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
                inkPillar[i].transform.localScale = playerData.humanFirstSkillScale;
            }

            yield return new WaitForSeconds(.7f);
            playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = false);

            //먹기둥 지속시간과 쿨타임 어느게 일찍 끝나냐에 따라 다름

            if ((playerData.humanFirstSkillDuration + 0.8f) < playerData.humanFirstSkillCooldown)
            {
                yield return new WaitForSeconds(playerData.humanFirstSkillDuration - .7f);
                for (int i = 0; i < countEnemies; i++)
                {
                    Destroy(inkPillar[i]);
                }

                yield return new WaitForSeconds(playerData.humanFirstSkillCooldown - .8f - playerData.humanFirstSkillDuration);
                canUseInkPillar = true;
            }
            else
            {
                yield return new WaitForSeconds(playerData.humanFirstSkillCooldown - .8f - playerData.humanFirstSkillDuration);
                canUseInkPillar = true;

                yield return new WaitForSeconds(playerData.humanFirstSkillDuration - .7f);
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
            playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = true);
            canUseInkSmash = false;

            playerMaskChange.ActiveAnimator.CrossFade("SecondSkill", .1f);
            for (int i = 0; i < 80; i++)
            {
                yield return new WaitForSeconds(.01f);
                playerMaskChange.ActiveRigidbody.MovePosition
                    (playerMaskChange.ActiveCharacter.transform.position + playerMaskChange.ActiveCharacter.transform.forward * 10 * Time.deltaTime);
            }

            inkSmashArea = Instantiate(inkSmashPrefab,playerMaskChange.ActiveCharacter.transform.position + playerMaskChange.ActiveCharacter.transform.forward * 1, playerMaskChange.ActiveCharacter.transform.rotation);

            yield return new WaitForSeconds(.5f);
            playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = false);

            yield return new WaitForSeconds(playerData.humanSecondSkillDuration);
            Destroy(inkSmashArea);

            yield return new WaitForSeconds(playerData.humanSecondSkillCooldown - 1.6f - playerData.humanSecondSkillDuration);
            canUseInkSmash = true;
        }
    }
    public IEnumerator HumanAvoidBack()
    {
        if (canUseHumanAvoidStep)
        {
            restrictForSkill = true;
            playerMaskChange.ActiveAnimator.SetBool("restrict", true);
            canUseHumanAvoidStep = false;
            playerMaskChange.ActiveAnimator.CrossFade("AvoidBack", .1f);

            for (int i = 0; i < 50; i++) 
            {
                yield return new WaitForSeconds(.01f);
                playerMaskChange.ActiveRigidbody.MovePosition
                    (playerMaskChange.ActiveCharacter.transform.position - playerMaskChange.ActiveCharacter.transform.forward * 10 * Time.deltaTime);
            }

            playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = false);

            yield return new WaitForSeconds(playerData.humanAvoidStepCooldown - .6f);
            canUseHumanAvoidStep = true;
        }
    }
    #endregion

    #region 짐승탈 함수

    public IEnumerator AnimalNormalAttack()
    {
        if (canUseHumanNormalAttack)
        {
            playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = true);
            canUseHumanNormalAttack = false;

            playerMaskChange.ActiveAnimator.CrossFade("NormalAttack", .1f);

            yield return new WaitForSeconds(1f);
            playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = false);

            yield return new WaitForSeconds(playerData.animalNormalAttackCooldown - 1f);
            canUseHumanNormalAttack = true;
        }
    }

    public IEnumerator AnimalFirstSkill()
    {
        if (canUseXClaw)
        {
            playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = true);
            canUseXClaw = false;

            playerMaskChange.ActiveAnimator.CrossFade("FirstSkill", .1f);
            
            yield return new WaitForSeconds(1.5f);
            playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = false);

            yield return new WaitForSeconds(playerData.animalFirstSkillCooldown - 2f);
            canUseXClaw = true;
        }
    }

    public IEnumerator AnimalSecondSkill()
    {
        if (canUseLeapClaw)
        {
            playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = true);
            canUseLeapClaw = false;

            playerMaskChange.ActiveAnimator.CrossFade("SecondSkill", .1f);
            for (int i = 0; i < 60; i++)
            {
                yield return new WaitForSeconds(.01f);
                playerMaskChange.ActiveRigidbody.MovePosition
                    (playerMaskChange.ActiveCharacter.transform.position + playerMaskChange.ActiveCharacter.transform.forward * 15 * Time.deltaTime);
            }
            yield return new WaitForSeconds(.3f);
            playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = false);

            yield return new WaitForSeconds(playerData.animalSecondCooldown -2f);
            canUseLeapClaw = true;
        }
    }

    public IEnumerator AnimalAvoidBack()
    {
        if (canUseAnimalAvoidBack)
        {
            playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = true);
            canUseAnimalAvoidBack = false;
            playerMaskChange.ActiveAnimator.CrossFade("AvoidBack", .1f);
            yield return new WaitForSeconds(.6f);

            playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = false);

            yield return new WaitForSeconds(playerData.humanAvoidStepCooldown - .6f);
            canUseAnimalAvoidBack = true;
        }
    }
    #endregion

    #region 귀신탈 함수
    public IEnumerator FinishSkill()
    {
        //사람탈로 바꿔서 처형
        //색깔바꾸기
        if (playerMaskChange.ActiveCharacter.name == "AnimalMaskCharacter")
        {
            playerMaskChange.SwitchCharacter();
            turnOriginMask = true;
        }

        playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = true);
        canUseFinish = false;

        ghostWeapon.SetActive(true);
        humanWeapon.SetActive(false);
        Color characterOriginColor = characterSkin.GetComponent<Renderer>().material.color;
        characterSkin.GetComponent<Renderer>().material.color = Color.black;

        playerMaskChange.ActiveAnimator.CrossFade("Finish", .2f);

        Vector3 originAngle = transform.forward;

        //영역 색깔 점점 진하게 바꾸기
        finishSkillArea.SetActive(true);
        Color areaOriginColor = finishSkillArea.GetComponentInChildren<Renderer>().material.color;
        float areaColorAlpha = 0.4f;

        while (areaColorAlpha <= 1f)
        {
            areaColorAlpha += .1f;
            finishSkillArea.GetComponentInChildren<Renderer>().material.color
                = new Color(areaOriginColor.r, areaOriginColor.g, areaOriginColor.b, areaColorAlpha);
            yield return new WaitForSeconds(.15f);
        }

        //타겟리스트에 있는 몬스터 죽이기
        //for (int i = 0; i < executionTargetList.Count; i++)
        //{
        //    var target = executionTargetList[i].GetComponent<Enemy>();
        //    target.Die();
        //}

        yield return new WaitForSeconds(1.1f);

        finishSkillArea.SetActive(false);
        ghostWeapon.SetActive(false);
        humanWeapon.SetActive(true);

        characterSkin.GetComponent<Renderer>().material.color = characterOriginColor;
        finishSkillArea.GetComponentInChildren<Renderer>().material.color = areaOriginColor;

        if (turnOriginMask)
        {
            playerMaskChange.SwitchCharacter();
            turnOriginMask = false;
        }

        yield return new WaitForSeconds(.2f);
        playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = false);

        yield return new WaitForSeconds(1f);
        canUseFinish = true;
    }
    #endregion

}
