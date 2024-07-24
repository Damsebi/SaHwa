using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerConMove playerConMove;

    #region Start()
    void Start()
    {
        
    }
    #endregion

    #region Update()
    void Update()
    {

    }
    #endregion

    #region FixedUpdate()
    private void FixedUpdate()
    {
        playerConMove.Movement();
        playerConMove.Rotation();       
    }
    #endregion

}
