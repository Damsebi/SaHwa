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
    List<string> options;
    private Dictionary<int, string> animationsMappings;
    #endregion

    #region Start()
    void Start()
    {
        initializeDropdownOptions();

        dropdown.onValueChanged.AddListener(value => { DropdownValueChange(dropdown); });


    }
    #endregion

    #region Update()
    void Update()
    {

    }
    #endregion


    #region 드롭다운, 맵핑 초기화
    private void initializeDropdownOptions()
    {
        animationsMappings = new Dictionary<int, string>();

        dropdown.ClearOptions();
        dropdown.AddOptions(options);

        for(int i = 0; i < options.Count; i++)
        {
            animationsMappings.Add(i, options[i]);
        }
    }
    #endregion

    #region 애니메이션 값 변경
    void DropdownValueChange(Dropdown change)
    {
        if(animationsMappings.TryGetValue(change.value, out string animationName))
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
