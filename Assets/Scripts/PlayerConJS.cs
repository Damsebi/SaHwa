using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerConJS : MonoBehaviour
{
    #region 선언

    public Animator character1Animator;
    public Animator character2Animator;
    public GameObject character1;
    public GameObject character2;
    public CinemachineFreeLook freeLookCamera;

    private GameObject activeCharacter;
    private Animator activeAnimator;
    private Dictionary<int, string> mappingDictionary = new Dictionary<int, string>();

    private Dictionary<int, string> character1Mappings = new Dictionary<int, string>();
    private Dictionary<int, string> character2Mappings = new Dictionary<int, string>();

    private bool isAnimationPlaying = false;
    private string currentAnimation = "";
    private Coroutine animationCheckCoroutine;
    private bool isUIActive = false;

    private float hori;
    private float verti;
    private Vector3 movement;
    private float moveAmount;
    private Quaternion targetRotation;
    private Rigidbody rb;
    [SerializeField] private float playerRotateSpeed;
    #endregion

    #region Start
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = true;
        rb.isKinematic = false;

        InitializeCharacterMappings();
        SetCharacterActive(character1, true);
        activeCharacter = character1;
        activeAnimator = character1Animator;
        mappingDictionary = new Dictionary<int, string>(character1Mappings);

        freeLookCamera.Follow = activeCharacter.transform;
        freeLookCamera.LookAt = activeCharacter.transform;
    }
    #endregion

    #region Update()
    void Update()
    {
        if (isUIActive) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchCharacter();
        }

        Movement();

        if (!isAnimationPlaying)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlayAnimation(0);
            }
            else if (Input.GetMouseButtonDown(1))
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
    }
    #endregion

    #region 애니메이션 설정
    public void SetAnimation(int index, string animationName)
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
            if (currentAnimation != animationName)
            {
                if (animationCheckCoroutine != null)
                {
                    StopCoroutine(animationCheckCoroutine);
                }

                isAnimationPlaying = true;
                currentAnimation = animationName;
                activeAnimator.Play(animationName);
                animationCheckCoroutine = StartCoroutine(CheckAnimationFinished());
            }
        }
    }

    private IEnumerator CheckAnimationFinished()
    {
        AnimatorStateInfo stateInfo = activeAnimator.GetCurrentAnimatorStateInfo(0);

        while (stateInfo.IsName(currentAnimation) && !activeAnimator.IsInTransition(0))
        {
            yield return null;
        }
        isAnimationPlaying = false;
        currentAnimation = "";
    }
    #endregion

    #region FixedUpdate
    private void FixedUpdate()
    {
        Movement();
        Rotation();
        rb.MovePosition(rb.position + movement * 6 * Time.deltaTime);
        activeCharacter.transform.localPosition = Vector3.zero;
        activeCharacter.transform.localRotation = Quaternion.identity;
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

        activeAnimator.SetBool("isMove", moveAmount > 0f);
    }
    #endregion

    #region 회전
    void Rotation()
    {
        if (moveAmount > 0f)
        {
            Vector3 cam = Camera.main.transform.forward;
            movement = Quaternion.LookRotation(new Vector3(cam.x, 0, cam.z)) * movement;
            targetRotation = Quaternion.LookRotation(movement);
        }

        rb.rotation = Quaternion.RotateTowards(rb.rotation, Quaternion.Normalize(targetRotation), playerRotateSpeed);
    }
    #endregion

    #region 캐릭터 매핑 초기화
    private void InitializeCharacterMappings()
    {
        character1Mappings.Add(0, "human_mask_1");
        character1Mappings.Add(1, "human_mask_2");
        character1Mappings.Add(2, "human_mask_3");
        character1Mappings.Add(3, "human_mask_4");

        character2Mappings.Add(0, "animal_mask_1");
        character2Mappings.Add(1, "animal_mask_2");
        character2Mappings.Add(2, "animal_mask_3");
        character2Mappings.Add(3, "animal_mask_4");
    }
    #endregion

    #region 캐릭터 스위칭
    private void SwitchCharacter()
    {
        Vector3 currentPosition = rb.position;
        Quaternion currentRotation = rb.rotation;

        if (activeCharacter == character1)
        {
            SetCharacterActive(character1, false);
            SetCharacterActive(character2, true);
            activeCharacter = character2;
            activeAnimator = character2Animator;
            mappingDictionary = new Dictionary<int, string>(character2Mappings);
        }
        else
        {
            SetCharacterActive(character2, false);
            SetCharacterActive(character1, true);
            activeCharacter = character1;
            activeAnimator = character1Animator;
            mappingDictionary = new Dictionary<int, string>(character1Mappings);
        }

        rb.position = currentPosition;
        rb.rotation = currentRotation;

        freeLookCamera.Follow = activeCharacter.transform;
        freeLookCamera.LookAt = activeCharacter.transform;

        activeCharacter.transform.localPosition = Vector3.zero;
        activeCharacter.transform.localRotation = Quaternion.identity;
    }
    #endregion

    #region 캐릭터 활성화 설정
    private void SetCharacterActive(GameObject character, bool isActive)
    {
        character.SetActive(isActive);
    }
    #endregion

    #region UI 활성화 설정
    public void SetUIActive(bool active)
    {
        isUIActive = active;
    }
    #endregion
}
