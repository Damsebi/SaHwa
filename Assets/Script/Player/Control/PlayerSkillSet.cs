using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class PlayerSkillSet : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] LayerMask enemyLayer;

    private PlayerMaskChange playerMaskChange;
    private PlayerMovement playerMovement;
    [SerializeField] private Player[] player;

    [SerializeField] private bool restrictForSkill;
    public bool RestrictForSkill { get { return restrictForSkill; } }
    private bool ignoreStun;
    public bool IgnoreStun { get { return ignoreStun; } }

    #region ���Ż ����
    #region �⺻����
    private bool isNormalAttacking;
    private bool canUseHumanNormalAttack;
    [SerializeField] private GameObject humanNormalAttackArea;
    [SerializeField] private GameObject humanWeaponTrail;

    #endregion

    #region �Ա��
    private bool canUseInkPillar;

    [SerializeField] GameObject inkPillarPrefab;
    private GameObject[] inkPillar = new GameObject[3];
    #endregion

    #region �� ���Ž�
    [SerializeField] GameObject inkSmashPrefab;
    private GameObject inkSmashArea;
    private bool canUseInkSmash;
    #endregion

    #region ȸ��
    private bool canUseHumanAvoidStep;
    [SerializeField] private GameObject humanAvoidStepArea;

    #endregion
    #endregion

    #region ����Ż ����
    #region �⺻����
    private bool canUseAnimalNormalAttack;
    [SerializeField] private GameObject animalNormalAttackArea;

    #endregion

    #region ��հ���
    private bool canUseFirstSkill;
    [SerializeField] private GameObject animalFirstSkillArea;

    #endregion

    #region �������
    private bool canUseSecondSkill;
    [SerializeField] private GameObject animalSecondSkillArea;
    #endregion

    #region ȸ��
    [SerializeField] private bool canUseAnimalAvoidBack;
    #endregion
    #endregion

    #region �ͽ�Ż ����
    private bool canUseFinish;
    private bool turnOriginMask; //ó���� ���Ż���� ������

    [SerializeField] private GameObject characterSkin; //��Ų ����������
    [SerializeField] private GameObject ghostWeapon; //��� ���¿��� ó��. �׿��� ������ ��ü
    [SerializeField] private GameObject humanWeapon;

    [SerializeField] private GameObject finishSkillArea; // ó�� ���� ������Ʈ
    private List<Transform> finishTargetList;
    

    //[SerializeField][Range(0, 20)] float executionRange; //ó�� ���� ���߿� 
    //[SerializeField] float executionAreaSizeRate; //ó�� ���� ũ����� ���߿�
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
        canUseFirstSkill = true;
        canUseSecondSkill = true;
        canUseAnimalAvoidBack = true;

        canUseFinish = true;
        finishTargetList = new();
    }

    #region �ִϸ��̼� ����, ���൵ üũ(���� ���嶧 �ִϸ��̼� ��ũ��Ʈ �ϳ� ���� ����), + �ִϸ��̼� ���൵�� ���� �Լ������ص� ������ ��
    public void AnimationState() 
    {
        var currentAnimation = playerMaskChange.ActiveAnimator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimation.IsName("Die"))
        {
            restrictForSkill = true;
            StopAllCoroutines();
            ResetAttackArea();
        }
        else if (currentAnimation.IsName("Hit"))
        {
            restrictForSkill = true;
            if (currentAnimation.normalizedTime > 0.95f)
            {
                player[0].isDamaged = false;
                player[1].isDamaged = false;
            }
            StopAllCoroutines();
            ResetAttackArea();
        }

        //���Ż, ����Ż �ִ� �̸��� �����ѵ� ���X => ���߿� ��ų�̸� Ȯ���Ǹ� ��ġ��
        else if (currentAnimation.IsName("NormalAttack"))
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

            SwitchEffect(humanWeaponTrail, 0.15f, true);
            SwitchEffect(humanWeaponTrail, .24f, false);
            SwitchEffect(humanWeaponTrail, .4f, true);
            SwitchEffect(humanWeaponTrail, .5f, false);

            SwitchAttakArea(humanNormalAttackArea, 0.17f, true);
            SwitchAttakArea(humanNormalAttackArea, 0.246f, false);
            SwitchAttakArea(humanNormalAttackArea, 0.42f, true);
            SwitchAttakArea(humanNormalAttackArea, 0.46f, false);

            SwitchAttakArea(animalNormalAttackArea, 0.2f, true);
            SwitchAttakArea(animalNormalAttackArea, 0.52f, false);
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

            SwitchEffect(humanWeaponTrail, 0.2f, true);
            SwitchEffect(humanWeaponTrail, .32f, false);

            SwitchAttakArea(humanNormalAttackArea, 0.21f, true);
            SwitchAttakArea(humanNormalAttackArea, 0.32f, false);
        }
        else if (currentAnimation.IsName("FirstSkill"))
        {
            SwitchEffect(humanWeaponTrail, 0.41f, true);
            SwitchEffect(humanWeaponTrail, .5f, false);

            SwitchAttakArea(animalFirstSkillArea, 0.48f, true);
            SwitchAttakArea(animalFirstSkillArea, 0.71f, false);
        }
        else if (currentAnimation.IsName("SecondSkill"))
        {
            SwitchEffect(humanWeaponTrail, 0.09f, true);
            SwitchEffect(humanWeaponTrail, .35f, false);

            SwitchAttakArea(animalSecondSkillArea, 0.37f, true);
            SwitchAttakArea(animalSecondSkillArea, 0.49f, false);
        }
        else if (currentAnimation.IsName("AvoidBack")) //CrossFade("AvoidBack",.2f) .2f -> 0 (O)
        {
            SwitchAttakArea(humanAvoidStepArea, 0.1f, true);
            SwitchAttakArea(humanAvoidStepArea, 0.2f, false);
        }
        
        else if (currentAnimation.IsName("Movement") || currentAnimation.IsName("LockOnMovement")) //����Ǯ���ִ� �뵵�ε� Ȱ��
        {
            if (currentAnimation.normalizedTime > 0.5f && currentAnimation.normalizedTime < 0.51f)
            {
                restrictForSkill = false;
                ResetAttackArea();
            }
            
            humanWeaponTrail.SetActive(false);
            playerMovement.canRotate = true;
            playerMovement.canMove = true;
        }
      
    }
    #endregion

    #region ���ݹ���,Ʈ���� On/Off.
    //�ǵ�ġ ���� ���ݹ��� Ȱ��ȭ ����
    private void ResetAttackArea() 
    {
        humanNormalAttackArea.SetActive(false);
        humanAvoidStepArea.SetActive(false);

        animalNormalAttackArea.SetActive(false);
        animalFirstSkillArea.SetActive(false);
        animalSecondSkillArea.SetActive(false);

        finishSkillArea.SetActive(false);
    }
    private void SwitchAttakArea(GameObject attackArea, float normalizedTime, bool onOffSwitch)
    {
        var currentAnimation = playerMaskChange.ActiveAnimator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimation.normalizedTime >= normalizedTime && currentAnimation.normalizedTime < normalizedTime + 0.01f)
        {
            attackArea.SetActive(onOffSwitch);
        }
    }

    private void SwitchEffect(GameObject Effect, float normalizedTime, bool onOffSwitch)
    {
        var currentAnimation = playerMaskChange.ActiveAnimator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimation.normalizedTime >= normalizedTime && currentAnimation.normalizedTime < normalizedTime + 0.01f)
        {
            Effect.SetActive(onOffSwitch);
        }
    }

    #endregion

    #region ���Ż �Լ�
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
            Collider[] colliders = Physics.OverlapSphere(playerMaskChange.ActiveCharacter.transform.position, playerData.humanFirstSkillRange, enemyLayer);

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
                inkPillar[i] = Instantiate(inkPillarPrefab, selectEnemies[i].transform.position, selectEnemies[i].transform.rotation);
                inkPillar[i].transform.localScale = playerData.humanFirstSkillScale;
            }

            yield return new WaitForSeconds(.7f);
            playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = false);

            //�Ա�� ���ӽð��� ��Ÿ�� ����� ���� �����Ŀ� ���� �ٸ�
            
            yield return new WaitForSeconds(2f);

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
        }
    }
    public IEnumerator HumanSecondSkill()
    {
        if (canUseInkSmash)
        {
            playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = true);
            canUseInkSmash = false;

            playerMaskChange.ActiveAnimator.CrossFade("SecondSkill", .1f);
            for (int i = 0; i < 45; i++)
            {
                yield return new WaitForSeconds(.02f);
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
            playerMaskChange.ActiveAnimator.CrossFade("AvoidBack", 0f);

            for (int i = 0; i < 30; i++) 
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

    #region ����Ż �Լ�

    public IEnumerator AnimalNormalAttack()
    {
        if (canUseAnimalNormalAttack)
        {
            playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = true);
            canUseAnimalNormalAttack = false;

            playerMaskChange.ActiveAnimator.CrossFade("NormalAttack", .1f);

            yield return new WaitForSeconds(1f);
            playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = false);

            yield return new WaitForSeconds(playerData.animalNormalAttackCooldown - 1f);
            canUseAnimalNormalAttack = true;
        }
    }

    public IEnumerator AnimalFirstSkill()
    {
        if (canUseFirstSkill)
        {
            playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = true);
            canUseFirstSkill = false;

            playerMaskChange.ActiveAnimator.CrossFade("FirstSkill", .1f);
            
            yield return new WaitForSeconds(1.5f);
            playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = false);

            yield return new WaitForSeconds(playerData.animalFirstSkillCooldown - 2f);
            canUseFirstSkill = true;
        }
    }

    public IEnumerator AnimalSecondSkill()
    {
        if (canUseSecondSkill)
        {
            playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = true);
            canUseSecondSkill = false;

            playerMaskChange.ActiveAnimator.CrossFade("SecondSkill", .1f);
            for (int i = 0; i < 60; i++)
            {
                yield return new WaitForSeconds(.01f);
                playerMaskChange.ActiveRigidbody.MovePosition
                    (playerMaskChange.ActiveCharacter.transform.position + playerMaskChange.ActiveCharacter.transform.forward * 25 * Time.deltaTime);
            }
            yield return new WaitForSeconds(.3f);
            playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill = false);

            yield return new WaitForSeconds(playerData.animalSecondCooldown -2f);
            canUseSecondSkill = true;
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

    #region �ͽ�Ż �Լ�
    public void CheckEnableFinish()
    {
        if (!canUseFinish) return;
        if (PlayerFollowCamera.instance.CurrentTarget)
        {
            if (PlayerFollowCamera.instance.CurrentTarget.gameObject.GetComponent<CalliSystem>().IsPaintOverMax())
            {
                StartCoroutine(FinishSkill());
            }
        }
    }

    public IEnumerator FinishSkill()
    {
        Collider[] colliders = Physics.OverlapSphere(playerMaskChange.ActiveCharacter. transform.position, playerData.detectRange, enemyLayer);
        finishTargetList.Clear();

        //��ֹ��ν� �ʿ�, ��� ���� �ʿ�, 
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.GetComponent<CalliSystem>().IsPaintOverMax())
            {
                finishTargetList.Add(colliders[i].transform);
            }
        }    

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
        for (int i = 0; i < finishTargetList.Count; i++)
        {
            var target = finishTargetList[i].GetComponent<Enemy>(); //GetComponent �� ���̾��µ�
            target.StopAction();
        }

        Vector3 originAngle = transform.forward;

        //���� ���� ���� ���ϰ� �ٲٱ�
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

        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < finishTargetList.Count; i++)
        {
            var target = finishTargetList[i].GetComponent<Enemy>();
            target.Die();
        }

        yield return new WaitForSeconds(.6f);

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

        yield return new WaitForSeconds(playerData.finishSkillCooldown > 1.8 ? playerData.finishSkillCooldown : 0);
        canUseFinish = true;
    }
    #endregion

}
