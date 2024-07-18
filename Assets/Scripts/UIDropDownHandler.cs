using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDropdownHandler : MonoBehaviour
{
    #region ����
    public Animator animator;
    public Dropdown dropdown;

    [SerializeField]
    List<AnimationMapping> mappingList;
    private Dictionary<int, string> mappingDictionary;
    #endregion

    #region Start()
    void Start()
    {
        initializeDropdownOptions();

        dropdown.onValueChanged.AddListener(value => { DropdownValueChange(dropdown); });
    }
    #endregion

    #region ��Ӵٿ�, ���� �ʱ�ȭ
    private void initializeDropdownOptions()
    {
        mappingDictionary = new Dictionary<int, string>();
        List<string> options = new List<string>();

        dropdown.ClearOptions();

        for(int i = 0; i < mappingList.Count; i++)
        {
            options.Add(mappingList[i].dropdownOption);
            mappingDictionary.Add(i, mappingList[i].animationName);
        }
        dropdown.AddOptions(options);
    }
    #endregion

    #region �ִϸ��̼� �� ����
    void DropdownValueChange(Dropdown change)
    {
        if(mappingDictionary.TryGetValue(change.value, out string animationName))
        {
            PlayAnimation(animationName);
        }
    }
    #endregion

    #region �ִϸ��̼� ���
    private void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }
    #endregion
}
