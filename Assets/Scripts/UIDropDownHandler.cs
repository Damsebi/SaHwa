using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDropdownHandler : MonoBehaviour
{
    #region 선언
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

    #region 드롭다운, 맵핑 초기화
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

    #region 애니메이션 값 변경
    void DropdownValueChange(Dropdown change)
    {
        if(mappingDictionary.TryGetValue(change.value, out string animationName))
        {
            PlayAnimation(animationName);
        }
    }
    #endregion

    #region 애니메이션 재생
    private void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }
    #endregion
}
