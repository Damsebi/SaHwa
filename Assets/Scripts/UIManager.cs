using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject settingUI;
    [SerializeField] bool openSetting;
    [SerializeField] PlayerConJS playerController;

    void Start()
    {
        if (!openSetting) Cursor.lockState = CursorLockMode.Confined;
        else Cursor.lockState = CursorLockMode.None;

        if(playerController == null)
        {
            playerController = FindObjectOfType<PlayerConJS>();
        }
    }
    
    void Update()
    {
        if (!openSetting) Cursor.visible = false;
        else Cursor.visible = true;

        if (Input.GetKeyDown(KeyCode.Escape)) OpenSettings();
    }

    #region 설정
    private void OpenSettings()
    {
        openSetting = !openSetting;
        if (openSetting)
        {
            Time.timeScale = 0;
            settingUI.SetActive(true);
            if(playerController != null)
            {
                playerController.SetUIActive(true);
            }
        }
        else
        {
            Time.timeScale = 1;
            settingUI.SetActive(false);
            if (playerController != null)
            {
                playerController.SetUIActive(false);
            }
        }
    }
    public void Restart()
    {
        //첫 장면으로 다시로드
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Quit()
    {
        Application.Quit();
    }
    #endregion
}
