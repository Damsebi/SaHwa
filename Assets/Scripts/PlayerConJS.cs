using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConJS : MonoBehaviour
{
    #region 선언

    public Animator animator;
    public Rigidbody rigidbody;
    public GameObject character1;
    public GameObject character2;

    private GameObject activeCharacter;
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
    [SerializeField] private float playerRotateSpeed;

    #endregion

    #region Start()
    void Start()
    {
        InitializeCharacterMappings();
        SetCharacterActive(character1, true);
        activeCharacter = character1;
        mappingDictionary = new Dictionary<int, string>(character1Mappings);
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
                animator.Play(animationName);
                animationCheckCoroutine = StartCoroutine(CheckAnimationFinished());
            }
        }
    }

    private IEnumerator CheckAnimationFinished()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        while (stateInfo.IsName(currentAnimation) && !animator.IsInTransition(0))
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        isAnimationPlaying = false;
        currentAnimation = "";
    }
    #endregion

    #region FixedUpdate()
    private void FixedUpdate()
    {
        Rotation();
        rigidbody.MovePosition(transform.position + movement * 6 * Time.deltaTime);
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

        animator.SetBool("isMove", moveAmount > 0f);
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

    private void InitializeCharacterMappings()
    {
        character1Mappings.Add(0, "사람탈_애니메이션 1");
        character1Mappings.Add(1, "사람탈_애니메이션 2");
        character1Mappings.Add(2, "사람탈_애니메이션 3");
        character1Mappings.Add(3, "사람탈_애니메이션 4");

        character2Mappings.Add(0, "동물탈_애니메이션 1");
        character2Mappings.Add(1, "동물탈_애니메이션 2");
        character2Mappings.Add(2, "동물탈_애니메이션 3");
        character2Mappings.Add(3, "동물탈_애니메이션 4");
    }

    private void SwitchCharacter()
    {
        if (activeCharacter == character1)
        {
            SetCharacterActive(character1, false);
            SetCharacterActive(character2, true);
            activeCharacter = character2;
            mappingDictionary = new Dictionary<int, string>(character2Mappings);
        }
        else
        {
            SetCharacterActive(character2, false);
            SetCharacterActive(character1, true);
            activeCharacter = character1;
            mappingDictionary = new Dictionary<int, string>(character1Mappings);
        }
    }

    private void SetCharacterActive(GameObject character, bool isActive)
    {
        character.SetActive(isActive);
    }

    public void SetUIActive(bool active)
    {
        isUIActive = active;
    }
}

