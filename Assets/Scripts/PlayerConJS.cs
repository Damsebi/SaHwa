using System.Collections.Generic;
using UnityEngine;

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

    private static Dictionary<int, string> mappingDictionary = new Dictionary<int, string>();
    #endregion

    #region Start()
    void Start()
    {
        SetDefaultMappings();
    }
    #endregion

    #region Update()
    void Update()
    {
        if (Input.GetButtonDown("Q"))
        {
            PlayAnimation(0);
        }
        else if (Input.GetButtonDown("W"))
        {
            PlayAnimation(1);
        }
        else if (Input.GetButtonDown("E"))
        {
            PlayAnimation(2);
        }
        else if (Input.GetButtonDown("R"))
        {
            PlayAnimation(3);
        }
    }
    #endregion

    #region 애니메이션 설정
    public static void SetAnimation(int index, string animationName)
    {
        if (mappingDictionary.ContainsKey(index))
        {
            mappingDictionary[index] = animationName;
        }
    }

    private void PlayAnimation(int index)
    {
        if (mappingDictionary.TryGetValue(index, out string animationName) && !string.IsNullOrEmpty(animationName))
        {
            animator.Play(animationName);
        }
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

    private void SetDefaultMappings()
    {
        mappingDictionary.Add(0, "axeSwing3"); // Q 키에 대한 기본 애니메이션 이름
        mappingDictionary.Add(1, ""); // W 키에 대한 기본 애니메이션 이름
        mappingDictionary.Add(2, ""); // E 키에 대한 기본 애니메이션 이름
        mappingDictionary.Add(3, ""); // R 키에 대한 기본 애니메이션 이름
    }
}
