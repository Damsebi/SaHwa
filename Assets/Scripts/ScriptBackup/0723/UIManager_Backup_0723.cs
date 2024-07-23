//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class UIManager : MonoBehaviour
//{
//    #region ����
//    [SerializeField] GameObject settingUI;
//    [SerializeField] bool openSetting;
//    [SerializeField] PlayerConJS playerController;
//    #endregion

//    #region Start()
//    void Start()
//    {
//        OpenSettings();
//        if (!openSetting) Cursor.lockState = CursorLockMode.Confined;
//        else Cursor.lockState = CursorLockMode.None;

//        if(playerController == null)
//        {
//            playerController = FindObjectOfType<PlayerConJS>();
//        }
//    }
//    #endregion

//    #region Update()
//    void Update()
//    {
//        if (!openSetting) Cursor.visible = false;
//        else Cursor.visible = true;

//        if (Input.GetKeyDown(KeyCode.Escape)) OpenSettings();
//    }
//    #endregion

//    #region ����
//    private void OpenSettings()
//    {
//        openSetting = !openSetting;
//        if (openSetting)
//        {
//            Time.timeScale = 0;
//            settingUI.SetActive(true);
//            if(playerController != null)
//            {
//                playerController.SetUIActive(true);
//            }
//        }
//        else
//        {
//            Time.timeScale = 1;
//            settingUI.SetActive(false);
//            if (playerController != null)
//            {
//                playerController.SetUIActive(false);
//            }
//        }
//    }
//    public void Restart()
//    {
//        //ù ������� �ٽ÷ε�
//        Time.timeScale = 1;
//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//    }
//    public void Quit()
//    {
//        Application.Quit();
//    }
//    #endregion
//}
