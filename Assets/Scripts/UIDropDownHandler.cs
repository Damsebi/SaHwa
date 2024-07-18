using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDropdownHandler : MonoBehaviour
{
    #region ����
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

    #region ��Ӵٿ�, ���� �ʱ�ȭ
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

    #region �ε��� Ȯ��
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
