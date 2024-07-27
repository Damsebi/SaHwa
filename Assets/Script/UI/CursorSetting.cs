using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSetting: MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
}
