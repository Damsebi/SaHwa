using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDropdownHandler : MonoBehaviour
{
    #region 선언
    public Animator animator;
    public Dropdown[] dropdowns;

    [SerializeField]
    List<AnimationMapping> mappingList;
    private Dictionary<int, string> mappingDictionary;
    #endregion

    #region Start()
    void Start()
    {
        initializeDropdownOptions();
        for (int i = 0; i < dropdowns.Length; i++)
        {
            int index = i;
            dropdowns[i].onValueChanged.AddListener(value => { DropdownValueChange(index, value); });
        }
    }
    #endregion

    #region 드롭다운, 맵핑 초기화
    private void initializeDropdownOptions()
    {
        mappingDictionary = new Dictionary<int, string>();

        foreach (var mapping in mappingList)
        {
            int index = mappingList.IndexOf(mapping);
            mappingDictionary.Add(index, mapping.animationName);

            if (index < dropdowns.Length)
            {
                Dropdown dropdown = dropdowns[index];
                dropdown.ClearOptions();
                List<string> options = new List<string> { mapping.dropdownOption };
                dropdown.AddOptions(options);
            }
        }

        #region 
        //List<string> options = new List<string>();

        //dropdown.ClearOptions();

        //for(int i = 0; i < mappingList.Count; i++)
        //{
        //    options.Add(mappingList[i].dropdownOption);
        //    mappingDictionary.Add(i, mappingList[i].animationName);
        //}
        //dropdown.AddOptions(options);
        #endregion
    }
    #endregion

    #region 인덱스 확인
    void DropdownValueChange(int index, int value)
    {
        if (mappingDictionary.TryGetValue(index, out string animationName) && !string.IsNullOrEmpty(animationName))
        {
            PlayerConJS.SetAnimation(index, animationName);
        }
    }
    #endregion

    #region
    //private void PlayAnimation(string animationName)
    //{
    //    animator.Play(animationName);
    //}
    #endregion
}
