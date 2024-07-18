using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDropdownHandler : MonoBehaviour
{
    #region ����
    public Animator animator;
    public Dropdown[] dropdowns;

    [SerializeField]
    private List<AnimationMapping> mappingList;
    private Dictionary<string, string> mappingDictionary;
    #endregion

    #region Start()
    void Start()
    {
        InitializeDropdownOptions();
        for (int i = 0; i < dropdowns.Length; i++)
        {
            int index = i;  // local variable to prevent closure issue
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
            PlayerConJS.SetAnimation(index, animationName);
        }
    }
    #endregion
}
