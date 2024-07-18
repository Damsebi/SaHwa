using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerConJS : MonoBehaviour
{
    public Animator animator;

    private Dictionary<string, string> animationMapping;


    void Start()
    {
        animationMapping = new Dictionary<string, string>
        {
            { "PlayAni1", "Ani1" },
            { "PlayAni1", "Ani1" },
            { "PlayAni1", "Ani1" },
            { "PlayAni1", "Ani1" }
        };
    }

    // Update is called once per frame
    void Update()
    {
        InputKey();
    }

    private void InputKey()
    {
        foreach(/*KeyValuePair<string, string>*/var mapping in animationMapping)
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

    private void FixedUpdate()
    {
        Rotation();
        GetComponent<Rigidbody>().MovePosition(this.gameObject.transform.position + movement * 6 * Time.deltaTime);
    }
    #region �̵�
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

    #region ȸ��
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
