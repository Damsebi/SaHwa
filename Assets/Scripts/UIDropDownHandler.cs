using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDropdownHandler : MonoBehaviour
{
    #region 선언
    public PlayerConJS playerController;
    public Dropdown[] character1Dropdowns;
    public Dropdown[] character2Dropdowns;

    private Dictionary<string, string> character1MappingDictionary;
    private Dictionary<string, string> character2MappingDictionary;

    [System.Serializable]
    public struct AnimationMapping
    {
        public string dropdownOption;
        public string animationName;
    }

    public List<AnimationMapping> character1MappingList;
    public List<AnimationMapping> character2MappingList;

    #endregion

    #region Start()
    void Start()
    {
        InitializeDropdownOptions(character1Dropdowns, character1MappingList, ref character1MappingDictionary);
        InitializeDropdownOptions(character2Dropdowns, character2MappingList, ref character2MappingDictionary);

        for (int i = 0; i < character1Dropdowns.Length; i++)
        {
            int index = i;
            character1Dropdowns[i].onValueChanged.AddListener((value) => DropdownValueChanged(index, value, true));
        }

        for (int i = 0; i < character2Dropdowns.Length; i++)
        {
            int index = i;
            character2Dropdowns[i].onValueChanged.AddListener((value) => DropdownValueChanged(index, value, false));
        }
    }
    #endregion

    #region 드롭다운 초기화 
    void InitializeDropdownOptions(Dropdown[] dropdowns, List<AnimationMapping> mappingList, ref Dictionary<string, string> mappingDictionary)
    {
        mappingDictionary = new Dictionary<string, string>();
        foreach (var mapping in mappingList)
        {
            mappingDictionary.Add(mapping.dropdownOption, mapping.animationName);
        }

        for (int i = 0; i < dropdowns.Length; i++)
        {
            List<string> options = new List<string>(mappingDictionary.Keys);
            dropdowns[i].ClearOptions();
            dropdowns[i].AddOptions(options);
        }
    }
    #endregion

    #region 애니메이션 변경 호출
    void DropdownValueChanged(int index, int value, bool isCharacter1)
    {
        string selectedOption = isCharacter1 ? character1Dropdowns[index].options[value].text : character2Dropdowns[index].options[value].text;
        Dictionary<string, string> mappingDictionary = isCharacter1 ? character1MappingDictionary : character2MappingDictionary;

        if (mappingDictionary.TryGetValue(selectedOption, out string animationName))
        {
            playerController.SetAnimation(index, animationName, isCharacter1);
        }
    }
    #endregion
}
