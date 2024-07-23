//using System.Collections;
//using System.Collections.Generic;
//using Unity.Mathematics;
//using UnityEngine;

//public class PlayerController : MonoBehaviour
//{
//    Animator animator;
//    Rigidbody rigidbody;

//    float hori;
//    float verti;
//    Vector3 movement;
//    float moveAmount;
//    Quaternion targetRotation;

//    [Tooltip("플레이어의 회전 스피드. 값이 클수록 빠르게 회전한다")]
//    [SerializeField] float playerRotateSpeed;
//    private void Awake()
//    {
//        animator = GetComponent<Animator>();    
//        rigidbody = GetComponent<Rigidbody>();
//    }

//    void Update()
//    {
//        Movement();

//        #region 활 애니
//        if (Input.GetKeyDown(KeyCode.Alpha0))
//        {
//            ShootArrowAnimation1();
//        }
//        else if (Input.GetKeyDown(KeyCode.Alpha5))
//        {
//            ShootArrowAnimation2();
//        }
//        #endregion

//        #region 검 애니
//        if (Input.GetKeyDown(KeyCode.Q))
//        {
//            SwordAnimation1();
//        }
  
//        else if (Input.GetKeyDown(KeyCode.E))
//        {
//            SwordAnimation3();
//        }
//        else if (Input.GetKeyDown(KeyCode.R))
//        {
//            SwordAnimation4();
//        }
//        else if (Input.GetKeyDown(KeyCode.T))
//        {
//            SwordAnimation5();
//        }
//        else if (Input.GetKeyDown(KeyCode.Y))
//        {
//            SwordAnimation6();
//        }
//        else if (Input.GetKeyDown(KeyCode.U))
//        {
//            SwordAnimation7();
//        }


//        else if (Input.GetKeyDown(KeyCode.I))
//        {
//            SwordShieldAnimation1();
//        }
//        else if (Input.GetKeyDown(KeyCode.O))
//        {
//            SwordShieldAnimation2();
//        }
//        else if (Input.GetKeyDown(KeyCode.P))
//        {
//            SwordShieldAnimation3();
//        }
//        else if (Input.GetKeyDown(KeyCode.Alpha6))
//        {
//            SwordShieldAnimation4();
//        }
//        #endregion

//        #region 도끼 애니
//        else if (Input.GetKeyDown(KeyCode.Alpha7))
//        {
//            AxeAnimation1();
//        }
//        else if (Input.GetKeyDown(KeyCode.Alpha8))
//        {
//            AxeAnimation2();
//        }
//        else if (Input.GetKeyDown(KeyCode.F))
//        {
//            AxeAnimation3();
//        }
//        else if (Input.GetKeyDown(KeyCode.G))
//        {
//            AxeAnimation4();
//        }
//        else if (Input.GetKeyDown(KeyCode.H))
//        {
//            AxeAnimation5();
//        }
//        else if (Input.GetKeyDown(KeyCode.J))
//        {
//            AxeAnimation6();
//        }


//        else if (Input.GetKeyDown(KeyCode.K))
//        {
//            AxeAnimation7();
//        }
//        else if (Input.GetKeyDown(KeyCode.Alpha9))
//        {
//            AxeAnimation8();
//        }
//        else if (Input.GetKeyDown(KeyCode.L))
//        {
//            AxeAnimation9();
//        }
//        else if (Input.GetKeyDown(KeyCode.Z))
//        {
//            AxeAnimation10();
//        }
//        else if (Input.GetKeyDown(KeyCode.X))
//        {
//            AxeAnimation11();
//        }
//        else if (Input.GetKeyDown(KeyCode.C))
//        {
//            AxeAnimation12();
//        }


//        #endregion

//        #region 메이지 애니
//        else if (Input.GetKeyDown(KeyCode.V))
//        {
//            MageAnimation1();
//        }
//        else if (Input.GetKeyDown(KeyCode.B))
//        {
//            MageAnimation2();
//        }
//        else if (Input.GetKeyDown(KeyCode.N))
//        {
//            MageAnimation3();
//        }
//        else if (Input.GetKeyDown(KeyCode.M))
//        {
//            MageAnimation4();
//        }
//        else if (Input.GetKeyDown(KeyCode.Alpha1))
//        {
//            MageAnimation5();
//        }
//        else if (Input.GetKeyDown(KeyCode.Alpha2))
//        {
//            MageAnimation6();
//        }
//        else if (Input.GetKeyDown(KeyCode.Alpha3))
//        {
//            MageAnimation7();
//        }
//        else if (Input.GetKeyDown(KeyCode.Alpha4))
//        {
//            MageAnimation8();
//        }
       
//        #endregion
//    }

//    private void FixedUpdate()
//    {
//        Rotation();
//        rigidbody.MovePosition(this.gameObject.transform.position + movement * 6 * Time.deltaTime);
//    }

//    #region 이동
//    void Movement()
//    {
//        hori = Input.GetAxis("Horizontal");
//        verti = Input.GetAxis("Vertical");
//        movement = new Vector3(hori, 0f, verti);
//        moveAmount = Mathf.Clamp01(Mathf.Abs(movement.x) + Mathf.Abs(movement.z));
//        movement.Normalize();

//        if (moveAmount > 0f)
//        {
//            animator.SetBool("isMove", true);
//        }
//        else
//        {
//            animator.SetBool("isMove", false);
//        }
//    }
//    #endregion

//    #region 회전
//    void Rotation()
//    {
//        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, playerRotateSpeed);

//        if (moveAmount > 0f)
//        {
//            Vector3 cam = Camera.main.transform.forward;
//            movement = Quaternion.LookRotation(new Vector3(cam.x, 0, cam.z)) * movement;
//            targetRotation = Quaternion.LookRotation(movement);
//        }
//    }
//    #endregion

//    #region 활 애니
//    private void ShootArrowAnimation1()
//    {
//        animator.CrossFade("shootingArrow", .2f);
//    }
//    private void ShootArrowAnimation2()
//    {
//        animator.CrossFade("Standing Draw Arrow", .2f);
//    }
//    #endregion

//    #region 검 애니
//    private void SwordAnimation1()
//    {
//        animator.CrossFade("2Hand-Sword-Attack1", .2f);
//    }
    
//    private void SwordAnimation3()
//    {
//        animator.CrossFade("2Hand-Sword-Attack3", .2f);
//    }
//    private void SwordAnimation4()
//    {
//        animator.CrossFade("2Hand-Sword-Attack4", .2f);
//    }
//    private void SwordAnimation5()
//    {
//        animator.CrossFade("2Hand-Sword-Attack5", .2f);
//    }
//    private void SwordAnimation6()
//    {
//        animator.CrossFade("2Hand-Sword-Attack6", .2f);
//    }
//    private void SwordAnimation7()
//    {
//        animator.CrossFade("Two Hand Sword Combo", .2f);
//    }


//    private void SwordShieldAnimation1()
//    {
//        animator.CrossFade("Sword And Shield Attack", .2f);
//    }
//    private void SwordShieldAnimation2()
//    {
//        animator.CrossFade("Sword And Shield Slash1", .2f);
//    }
//    private void SwordShieldAnimation3()
//    {
//        animator.CrossFade("Sword And Shield Slash2", .2f);
//    }
//    private void SwordShieldAnimation4()
//    {
//        animator.CrossFade("Sword And Shield Slash3", .2f);
//    }
//    #endregion

//    #region 도끼 애니
//    private void AxeAnimation1()
//    {
//        animator.CrossFade("Standing Melee Attack 360 High", .2f);
//    }
//    private void AxeAnimation2()
//    {
//        animator.CrossFade("Standing Melee Attack Backhand", .2f);
//    }
//    private void AxeAnimation3()
//    {
//        animator.CrossFade("Standing Melee Combo Attack Ver1", .2f);
//    }
//    private void AxeAnimation4()
//    {
//        animator.CrossFade("Standing Melee Combo Attack Ver2", .2f);
//    }
//    private void AxeAnimation5()
//    {
//        animator.CrossFade("Standing Melee Run Jump Attack", .2f);
//    }


//    private void AxeAnimation6()
//    {
//        animator.CrossFade("axeSwing1", .2f);
//    }
//    private void AxeAnimation7()
//    {
//        animator.CrossFade("axeSwing2", .2f);
//    }
//    private void AxeAnimation8()
//    {
//        animator.CrossFade("axeSwing3", .2f);
//    }
//    private void AxeAnimation9()
//    {
//        animator.CrossFade("axeSwing4", .2f);
//    }
//    private void AxeAnimation10()
//    {
//        animator.CrossFade("axeSwing5", .2f);
//    }
//    private void AxeAnimation11()
//    {
//        animator.CrossFade("axeSwing6", .2f);
//    }
//    private void AxeAnimation12()
//    {
//        animator.CrossFade("axeSwing7", .2f);
//    }

//    #endregion

//    #region 메이지 애니
//    private void MageAnimation1()
//    {
//        animator.CrossFade("Frank_RPG_Mage_Attack02_Demo", .2f);
//    }
//    private void MageAnimation2()
//    {
//        animator.CrossFade("Frank_RPG_Mage_Attack03_Demo", .2f);
//    }
//    private void MageAnimation3()
//    {
//        animator.CrossFade("Frank_RPG_Mage_Attack06_Demo", .2f);
//    }
//    private void MageAnimation4()
//    {
//        animator.CrossFade("Frank_RPG_Mage_Combo03_All_Demo", .2f);
//    }
//    private void MageAnimation5()
//    {
//        animator.CrossFade("Frank_RPG_Mage_Skill01_Demo", .2f);
//    }
//    private void MageAnimation6()
//    {
//        animator.CrossFade("Frank_RPG_Mage_Skill02_Demo", .2f);
//    }
//    private void MageAnimation7()
//    {
//        animator.CrossFade("Frank_RPG_Mage_Skill03_Demo", .2f);
//    }
//    private void MageAnimation8()
//    {
//        animator.CrossFade("Frank_RPG_Mage_Skill04_Demo", .2f);
//    }
//    #endregion
//}
