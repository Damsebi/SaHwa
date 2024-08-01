using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapCustomizeController : MonoBehaviour
{
    [SerializeField] PlayerMaskChange playerMaskChange;

    [SerializeField] private bool openPointerWindow;
    [SerializeField] private GameObject PointerUI;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        openPointerWindow = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown (KeyCode.Escape) ) 
        {
            openPointerWindow = !openPointerWindow;
            
            if (openPointerWindow)
            {
                PointerUI.SetActive (true);
            }
            else if (!openPointerWindow)
            {
                PointerUI.SetActive(false);
            }
        }
    }

    public void TransformToPointer(GameObject pointer)
    {
        playerMaskChange.ActiveCharacter.transform.position = pointer.transform.position;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
