using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaskChange : MonoBehaviour
{
    //private Rigidbody rigidbody;
    [Header("Human")]
    [SerializeField] private GameObject humanCharacter;
    [SerializeField] private Animator humanAnimator;
    [SerializeField] private Rigidbody humanRigidbody;

    [Header("Animal")]
    [SerializeField] private GameObject animalCharacter;
    [SerializeField] private Animator animalAnimator;
    [SerializeField] private Rigidbody animalRigidbody;

    [Header("Active")]
    private GameObject activeCharacter;
    public GameObject ActiveCharacter { get { return activeCharacter; } }
    
    private Animator activeAnimator;
    public Animator ActiveAnimator { get { return activeAnimator; } }

    private Rigidbody activeRigidbody;
    public Rigidbody ActiveRigidbody { get { return activeRigidbody; } }





    #region ĳ���� ���� �ʱ�ȭ
    private Dictionary<int, string> character1Mappings = new Dictionary<int, string>();
    private Dictionary<int, string> character2Mappings = new Dictionary<int, string>();
    private Dictionary<int, string> currentMappings;

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

    private void Awake()
    {
    }


    #region �ʱ� ĳ���� ����
    public void InitializeCharacterSetting()
    {
        humanCharacter.SetActive(true);
        animalCharacter.SetActive(false);
        activeCharacter = humanCharacter;
        activeAnimator = humanAnimator;
        activeRigidbody = humanRigidbody;
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
            
            activeAnimator = animalAnimator;
            activeRigidbody = animalRigidbody;
            //currentMappings = character2Mappings;
        }
        else
        {
            activeCharacter.SetActive(false);
            activeCharacter = humanCharacter;
            activeCharacter.SetActive(true);

            activeAnimator = humanAnimator;
            activeRigidbody = humanRigidbody;
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
