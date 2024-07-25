using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaskChange : MonoBehaviour
{
    private PlayerFollowCamera playerFollowCamera;

    //private Rigidbody rigidbody;

    [SerializeField] private Animator humanAnimator;
    [SerializeField] private Animator AnimalAnimator;
    
    [SerializeField] private GameObject humanCharacter;
    public GameObject HumanCharacter { get { return humanCharacter; } }

    [SerializeField] private GameObject animalCharacter;
    public GameObject AnimalCharacter { get { return animalCharacter; } }

    private GameObject activeCharacter;
    public GameObject ActiveCharacter { get { return activeCharacter; } }
    
    private Animator activeAnimator;
    public Animator ActiveAnimator { get { return activeAnimator; } }


    private Dictionary<int, string> character1Mappings = new Dictionary<int, string>();
    private Dictionary<int, string> character2Mappings = new Dictionary<int, string>();
    private Dictionary<int, string> currentMappings;

    private void Start()
    {
        //rigidbody = GetComponent<Rigidbody>();
    }


    #region ĳ���� ���� �ʱ�ȭ
    public void InitializeCharacterMappings() 
    {
        character1Mappings.Add(0, "human_mask_1");
        character1Mappings.Add(1, "human_mask_2");
        character1Mappings.Add(2, "human_mask_3");
        character1Mappings.Add(3, "human_mask_4");

        character2Mappings.Add(0, "animal_mask_1");
        character2Mappings.Add(1, "animal_mask_2");
        character2Mappings.Add(2, "animal_mask_3");
        character2Mappings.Add(3, "animal_mask_4");
    }
    #endregion

    #region �ʱ� ĳ���� ����
    public void InitializeCharacterSetting()
    {
        humanCharacter.SetActive(true);
        animalCharacter.SetActive(false);
        activeCharacter = humanCharacter;
        activeAnimator = humanAnimator;
    }

    #endregion

    #region ĳ���� ����Ī
    public void SwitchCharacter()
    {
        Transform currentTransform = activeCharacter.transform;

        if (activeCharacter == humanCharacter)
        {
            activeCharacter.SetActive(false);
            activeCharacter = animalCharacter;
            activeCharacter.SetActive(true);
            activeAnimator = AnimalAnimator;

            //currentMappings = character2Mappings;
        }
        else
        {
            activeCharacter.SetActive(false);
            activeCharacter = humanCharacter;
            activeCharacter.SetActive(true);
            activeAnimator = humanAnimator;
            //currentMappings = character1Mappings;
        }

        activeCharacter.transform.position = currentTransform.position;
        activeCharacter.transform.rotation = currentTransform.rotation;
    }
    #endregion

    //#region ĳ���� Ȱ��ȭ ����
    //public void SetCharacterActive(GameObject character, bool isActive)
    //{
    //    character.SetActive(isActive);
    //}
    //#endregion

}
