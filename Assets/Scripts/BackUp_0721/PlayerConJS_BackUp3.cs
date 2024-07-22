//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerConJS : MonoBehaviour
//{
//    #region 선언

//    public Animator animator;
//    public Rigidbody rigidbody;

//    private float hori;
//    private float verti;
//    private Vector3 movement;
//    private float moveAmount;
//    private Quaternion targetRotation;
//    [SerializeField] float playerRotateSpeed;

//    private static Dictionary<int, string> mappingDictionary = new Dictionary<int, string>();

//    private bool isAnimationPlaying = false;
//    private string currentAnimation = "";
//    private Coroutine animationCheckCoroutine;
//    private bool isUIActive = false;

//    #endregion

//    #region Start()
//    void Start()
//    {
//        SetDefaultMappings();
//    }
//    #endregion

//    #region Update()
//    void Update()
//    {
//        if (isUIActive)
//        {
//            return;
//        }

//        if(Input.GetButtonDown("Space"))
//        {

//        }
        
//        Movement();

//        if (!isAnimationPlaying)
//        {
//            if (Input.GetMouseButtonDown(0))
//            {
//                PlayAnimation(0);
//            }
//            else if (Input.GetMouseButtonDown(1))
//            {
//                PlayAnimation(1);
//            }
//            else if (Input.GetButtonDown("E"))
//            {
//                PlayAnimation(2);
//            }
//            else if (Input.GetButtonDown("R"))
//            {
//                PlayAnimation(3);
//            }
//        }
//    }
//    #endregion   

//    #region FixedUpdate()
//    private void FixedUpdate()
//    {
//        Rotation();
//        rigidbody.MovePosition(transform.position + movement * 6 * Time.deltaTime);
//    }
//    #endregion

//    #region 맵핑
//    private void SetDefaultMappings()
//    {
//        mappingDictionary.Add(0, "Animation_평타");
//        mappingDictionary.Add(1, "Animation_강공격");
//        mappingDictionary.Add(2, "Animation_스킬 1");
//        mappingDictionary.Add(3, "Animation_스킬 2");
//    }
//    #endregion

//    #region 애니메이션 설정
//    public static void SetAnimation(int index, string animationName)
//    {
//        if (mappingDictionary.ContainsKey(index))
//        {
//            mappingDictionary[index] = animationName;
//        }
//    }

//    private void PlayAnimation(int index)
//    {
//        if (mappingDictionary.TryGetValue(index, out string animationName) && !string.IsNullOrEmpty(animationName))
//        {
//            if (currentAnimation != animationName)
//            {
//                if (animationCheckCoroutine != null)
//                {
//                    StopCoroutine(animationCheckCoroutine);
//                }

//                isAnimationPlaying = true;
//                currentAnimation = animationName;
//                animator.Play(animationName);
//                animationCheckCoroutine = StartCoroutine(CheckAnimationFinished());
//            }
//        }
//    }

//    private IEnumerator CheckAnimationFinished()
//    {
//        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

//        while (stateInfo.IsName(currentAnimation) && !animator.IsInTransition(0))
//        {
//            yield return null;
//        }
//        isAnimationPlaying = false;
//        currentAnimation = "";
//    }
//    #endregion

//    #region 이동
//    void Movement()
//    {
//        hori = Input.GetAxis("Horizontal");
//        verti = Input.GetAxis("Vertical");
//        movement = new Vector3(hori, 0f, verti);
//        moveAmount = Mathf.Clamp01(Mathf.Abs(movement.x) + Mathf.Abs(movement.z));
//        movement.Normalize();

//        animator.SetBool("isMove", moveAmount > 0f);
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

//    #region ui활성화
//    public void SetUIActive(bool active)
//    {
//        isUIActive = active;
//    }
//    #endregion
//}
