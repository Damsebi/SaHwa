using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerSkillSet : MonoBehaviour
{//½ºÅ³ ½ºÅ©¸³Æ® ºÐÇÒÇÒÁö´Â ¸®ÆÑÅä¸µÇÒ ¶§
    [SerializeField] private PlayerData playerData;
    [SerializeField] LayerMask enemyLayer;

    private PlayerMaskChange playerMaskChange;

    [SerializeField] private bool restrictForSkill;
    public bool RestrictForSkill { get { return restrictForSkill; } }

    private bool isNormalAttacking;
    private bool isSkillCoroutineRunning;
    public bool IsSkillCoroutineRunning { get { return isSkillCoroutineRunning; } }

    private bool canUseNormalAttack;
    [SerializeField] private bool canUseInkPillar;





    private void Awake()
    {
        playerMaskChange = GetComponent<PlayerMaskChange>();
    }

    private void Start()
    {
        canUseInkPillar = true;
    }
    #region »ç¶÷Å»
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
                float distance1 = Vector3.Distance(transform.position, colliders[index].transform.position);
                for (int j = i + 1; j < colliders.Length; j++)
                {
                    float distance2 = Vector3.Distance(transform.position, colliders[j].transform.position);
                    if (distance1 > distance2) index = j;
                }

                Collider swapPosition = colliders[i];
                colliders[i] = colliders[index];
                colliders[index] = swapPosition;

                selectEnemies[i] = colliders[i];
            }
            #endregion

            yield return new WaitForSeconds(.7f);
            restrictForSkill = false;










            yield return new WaitForSeconds(playerData.humanInkPillarCooldown - .4f);
            canUseInkPillar = true;

            //¸Ô±âµÕ¿¡ ´êÀº Áö¼Ó µ¥¹ÌÁö
        }

    }

    public void HumanSecondSkill()
    {
        playerMaskChange.ActiveAnimator.CrossFade("SecondSkill", .1f);

    }

    public void HumanAvoidBack()
    {
        playerMaskChange.ActiveAnimator.CrossFade("AvoidBack", .1f);
    }

    #endregion

    #region Áü½ÂÅ»

    public void AnimalNormalAttack()
    {
        playerMaskChange.ActiveAnimator.CrossFade("NormalAttack", .1f);
    }

    public void AnimalFirstSkill()
    {
        playerMaskChange.ActiveAnimator.CrossFade("FirstSkill", .1f);

    }

    public void AnimalSecondSkill()
    {
        playerMaskChange.ActiveAnimator.CrossFade("SecondSkill", .1f);

    }

    public void AnimalAvoidBack()
    {
        playerMaskChange.ActiveAnimator.CrossFade("AvoidBack", .1f);
    }


    #endregion
}
