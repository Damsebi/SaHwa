using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerConJS : MonoBehaviour
{
    #region 선언

    public Animator animator;
    public Rigidbody rigidbody;

    private float hori;
    private float verti;
    private Vector3 movement;
    private float moveAmount;
    private Quaternion targetRotation;
    [SerializeField] float playerRotateSpeed;

    private Dictionary<string, string> animationMappings;
    #endregion

    #region Start()

    void Start()
    {
        animationMappings = new Dictionary<string, string>
        {
            { "PlayAni1", "Ani1" },
            { "PlayAni1", "Ani1" },
            { "PlayAni1", "Ani1" },
            { "PlayAni1", "Ani1" }
        };
    }
    #endregion

    #region Update()

    // Update is called once per frame
    void Update()
    {
        InputKey();
    }
    #endregion

    #region 매핑

    private void InputKey()
    {
        foreach(/*KeyValuePair<string, string>*/var mapping in animationMappings)
        {
            if(Input.GetButtonDown(mapping.Key))
            {
                PlayAnimation(mapping.Value);
            }
        }
    }

    private void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }
    #endregion

    #region FixedUpdate()
  
    private void FixedUpdate()
    {
        Rotation();
        GetComponent<Rigidbody>().MovePosition(this.gameObject.transform.position + movement * 6 * Time.deltaTime);
    } 
    #endregion

    #region 이동
    void Movement()
    {
        hori = Input.GetAxis("Horizontal");
        verti = Input.GetAxis("Vertical");
        movement = new Vector3(hori, 0f, verti);
        moveAmount = Mathf.Clamp01(Mathf.Abs(movement.x) + Mathf.Abs(movement.z));
        movement.Normalize();

        if (moveAmount > 0f)
        {
            animator.SetBool("isMove", true);
        }
        else
        {
            animator.SetBool("isMove", false);
        }
    }
    #endregion

    #region 회전
    void Rotation()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, playerRotateSpeed);

        if (moveAmount > 0f)
        {
            Vector3 cam = Camera.main.transform.forward;
            movement = Quaternion.LookRotation(new Vector3(cam.x, 0, cam.z)) * movement;
            targetRotation = Quaternion.LookRotation(movement);
        }
    }
    #endregion

}
