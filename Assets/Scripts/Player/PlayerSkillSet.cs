using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillSet : MonoBehaviour
{//��ų ��ũ��Ʈ ���������� �����丵�� ��
    [SerializeField] private PlayerData playerData;

    private PlayerMaskChange playerMaskChange;

    [SerializeField] private bool restrictForSkill;
    private int attackCount = 0;

    private void Awake()
    {
        playerMaskChange = GetComponent<PlayerMaskChange>();
    }

    public IEnumerator HumanNormalAttack() //����� ���� ��ٸ� ĳ���϶��?
    {
        // �� 3�� ����, �ѹ� ���� ������ �ൿ����, �߰��� ���㰡��, ���㿡 �������ֱ�
        if (!restrictForSkill)
        {
            if (attackCount == 0)
            {
                playerMaskChange.ActiveAnimator.CrossFade("humanNormalAttack1-1", .1f);
                restrictForSkill = true;
                playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill);
                yield return new WaitForSeconds(playerData.restrictTimeForNormalAttack1_1);
                restrictForSkill = false;
                playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill);
                attackCount++;
            }
            else if (attackCount == 1)
            {
                playerMaskChange.ActiveAnimator.CrossFade("humanNormalAttack1-2", .1f);
                restrictForSkill = true;
                playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill);
                yield return new WaitForSeconds(playerData.restrictTimeForNormalAttack1_2);
                restrictForSkill = false;
                playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill);
                attackCount++;
            }
            else
            {
                playerMaskChange.ActiveAnimator.CrossFade("humanNormalAttack1-3", .1f);
                restrictForSkill = true;
                playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill);
                yield return new WaitForSeconds(playerData.restrictTimeForNormalAttack1_3);
                restrictForSkill = false;
                playerMaskChange.ActiveAnimator.SetBool("restrict", restrictForSkill);
                attackCount = 0;
            }
        }
    }

    public void HumanFirstSkill()
    {

    }

    public void HumanSecondSkill()
    {

    }

    public void AvoidBack()
    {

    }


}
