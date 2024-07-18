using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_BackUp : MonoBehaviour
{
    [SerializeField] GameObject settingUI;
    [SerializeField] GameObject ToggleUI1;
    [SerializeField] bool openSetting;

    void Start()
    {
        if (!openSetting)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void Update()
    {
        Cursor.visible = openSetting;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenSettings();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleUI();
        }
    }

    #region ����
    private void OpenSettings()
    {
        openSetting = !openSetting;
        if (openSetting)
        {
            Time.timeScale = 0;
            settingUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            settingUI.SetActive(false);
        }
    }
    private void ToggleUI()
    {
        settingUI.SetActive(!settingUI.activeSelf);
        Time.timeScale = settingUI.activeSelf ? 0 : 1;
    }
    public void Restart()
    {
        //ù ������� �ٽ÷ε�
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Quit()
    {
        Application.Quit();
    }
    #endregion
}