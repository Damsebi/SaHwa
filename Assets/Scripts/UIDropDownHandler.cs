using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDropdownHandler1 : MonoBehaviour
{
    #region ����
    public Animator animator;
    public Dropdown[] dropdowns;
    public PlayerConJS playerController;

    [SerializeField]
    private List<AnimationMapping> mappingList;
    private Dictionary<string, string> mappingDictionary;
    #endregion

    #region Start()
    void Start()
    {
        if (playerController == null)
        {
            Debug.LogError("�÷��̾� ��Ʈ�ѷ� ����.");
            return;
        }

        InitializeDropdownOptions();
        for (int i = 0; i < dropdowns.Length; i++)
        {
            int index = i;
            dropdowns[i].onValueChanged.AddListener(value => DropdownValueChange(index, value));
        }
    }
    #endregion

    #region ��Ӵٿ�, ���� �ʱ�ȭ
    private void InitializeDropdownOptions()
    {
        mappingDictionary = new Dictionary<string, string>();

        foreach (var mapping in mappingList)
        {
            mappingDictionary[mapping.dropdownOption] = mapping.animationName;
        }

        for (int i = 0; i < dropdowns.Length; i++)
        {
            List<string> options = new List<string>();
            foreach (var mapping in mappingList)
            {
                options.Add(mapping.dropdownOption);
            }
            dropdowns[i].ClearOptions();
            dropdowns[i].AddOptions(options);
        }
    }
    #endregion

    #region �ε��� Ȯ��
    void DropdownValueChange(int index, int value)
    {
        string selectedOption = dropdowns[index].options[value].text;

        if (mappingDictionary.TryGetValue(selectedOption, out string animationName))
        {
            playerController.SetAnimation(index, animationName);
        }
    }
    #endregion
}
