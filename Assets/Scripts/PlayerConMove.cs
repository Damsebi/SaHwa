using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerConMove : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private Vector3 movement;
    private float moveAmount;
    private Animator activeAnimator;
    private Quaternion targetRotation;
    private Rigidbody playerRigidbody;
    public float playerRotateSpeed = 25;
    //PlayerData PlayerData;
    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        activeAnimator = GetComponent<Animator>();

        if (playerRigidbody == null)
        {
            Debug.LogError("Rigidbody component is missing from the Player GameObject.");
        }

        if (activeAnimator == null)
        {
            Debug.LogError("Animator component is missing from the Player GameObject.");
        }
    }

    #region 이동
    public void Movement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        movement = new Vector3(horizontal, 0f, vertical);
        moveAmount = Mathf.Clamp01(Mathf.Abs(movement.x) + Mathf.Abs(movement.z));
        movement.Normalize();

        activeAnimator.SetBool("isMove", moveAmount > 0f);
    }
    #endregion

    #region 회전
    public void Rotation()
    {
        if (moveAmount > 0f)
        {
            Vector3 cam = Camera.main.transform.forward;
            movement = Quaternion.LookRotation(new Vector3(cam.x, 0, cam.z)) * movement;
            targetRotation = Quaternion.LookRotation(movement);
        }

        targetRotation = Quaternion.Normalize(targetRotation);
        playerRigidbody.rotation = Quaternion.RotateTowards(playerRigidbody.rotation, targetRotation, playerRotateSpeed);
    }
    #endregion
}
