using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerConJS : MonoBehaviour, IDamageable
{
    #region 선언
    public Animator character1Animator;
    public Animator character2Animator;
    public GameObject character1;
    public GameObject character2;
    public CinemachineFreeLook freeLookCamera;

    private GameObject activeCharacter;
    private Animator activeAnimator;

    private Dictionary<int, string> character1Mappings = new Dictionary<int, string>();
    private Dictionary<int, string> character2Mappings = new Dictionary<int, string>();

    private Dictionary<int, string> currentMappings;
    private bool isAnimationPlaying = false;
    private string currentAnimation = "";
    private Coroutine animationCheckCoroutine;
    private bool isUIActive = false;

    public bool isDead;
    private float hori;
    private float verti;
    private Vector3 movement;
    private float moveAmount;
    private Quaternion targetRotation;
    private Rigidbody playerRigidbody;
    [SerializeField] private float playerRotateSpeed;
    #endregion

    #region Start()
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();

        playerRigidbody.useGravity = true;
        playerRigidbody.isKinematic = false;

        InitializeCharacterMappings();
        SetCharacterActive(character1, true);
        activeCharacter = character1;
        activeAnimator = character1Animator;
        currentMappings = character1Mappings;

        freeLookCamera.Follow = activeCharacter.transform;
        freeLookCamera.LookAt = activeCharacter.transform;
    }
    #endregion

    #region Update()
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetUIActive(!isUIActive);
        }

        if (isUIActive) return;

        if (Input.GetKeyDown(KeyCode.Space) && !isAnimationPlaying)
        {
            SwitchCharacter();
        }

        if (!isAnimationPlaying)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                PlayAnimation(0);
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                PlayAnimation(1);
            }
            else if (Input.GetButtonDown("ActionSkill1"))
            {
                PlayAnimation(2);
            }
            else if (Input.GetButtonDown("ActionSkill2"))
            {
                PlayAnimation(3);
            }
        }

        if (freeLookCamera.m_YAxis.Value < 0.34f)
        {
            freeLookCamera.m_YAxis.Value = 0.34f;
        }

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            AdjustCameraOrbits(scrollInput);
        }

    }
    #endregion

    #region FixedUpdate()
    private void FixedUpdate()
    {
        if (isUIActive) return;

        Movement();
        Rotation();
        playerRigidbody.MovePosition(playerRigidbody.position + movement * 6 * Time.deltaTime);
        activeCharacter.transform.localPosition = Vector3.zero;
        activeCharacter.transform.localRotation = Quaternion.identity;
    }
    #endregion

    #region 카메라 스크롤
    private void AdjustCameraOrbits(float scrollInput)
    {
        float adjustment = scrollInput * 2.0f;
        freeLookCamera.m_Orbits[0].m_Radius = Mathf.Clamp(freeLookCamera.m_Orbits[0].m_Radius
            + adjustment, 5f, 10f);
        freeLookCamera.m_Orbits[1].m_Radius = Mathf.Clamp(freeLookCamera.m_Orbits[1].m_Radius
            + adjustment, 5f, 10f);
        freeLookCamera.m_Orbits[2].m_Radius = Mathf.Clamp(freeLookCamera.m_Orbits[2].m_Radius
            + adjustment, 5f, 10f);
    }
    #endregion

    #region 애니메이션 설정
    public void SetAnimation(int index, string animationName, bool isCharacter1)
    {
        if (isCharacter1)
        {
            if (character1Mappings.ContainsKey(index))
            {
                character1Mappings[index] = animationName;
            }
        }
        else
        {
            if (character2Mappings.ContainsKey(index))
            {
                character2Mappings[index] = animationName;
            }
        }

        if (activeCharacter == (isCharacter1 ? character1 : character2))
        {
            currentMappings[index] = animationName;
        }
    }

    private void PlayAnimation(int index)
    {
        if (currentMappings.TryGetValue(index, out string animationName) && !string.IsNullOrEmpty(animationName))
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
        while (true)
        {
            AnimatorStateInfo stateInfo = activeAnimator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(currentAnimation) && stateInfo.normalizedTime < 1.0f)
            {
                yield return null;
            }
            else
            {
                break;
            }
        }
        isAnimationPlaying = false;
        currentAnimation = "";
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

        targetRotation = Quaternion.Normalize(targetRotation);
        playerRigidbody.rotation = Quaternion.RotateTowards(playerRigidbody.rotation, targetRotation, playerRotateSpeed);
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
        Vector3 currentPosition = playerRigidbody.position;
        Quaternion currentRotation = playerRigidbody.rotation;

        if (activeCharacter == character1)
        {
            SetCharacterActive(character1, false);
            SetCharacterActive(character2, true);
            activeCharacter = character2;
            activeAnimator = character2Animator;
            currentMappings = character2Mappings;
        }
        else
        {
            SetCharacterActive(character2, false);
            SetCharacterActive(character1, true);
            activeCharacter = character1;
            activeAnimator = character1Animator;
            currentMappings = character1Mappings;
        }

        playerRigidbody.position = currentPosition;
        playerRigidbody.rotation = currentRotation;

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
        Time.timeScale = active ? 0 : 1;
    }
    #endregion

    public bool ApplyDamage(DamageMessage damageMessage)
    {
        return true;
    }
    public void Die()
    {

    }

}
