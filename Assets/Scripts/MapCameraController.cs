using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MapCameraController : MonoBehaviour
{
    [Header("Component")]
    public Transform cam;

    [Header("Hotkey")]
    [SerializeField] Transform Point_1;
    [SerializeField] Transform Point_2;
    [SerializeField] Transform Point_3;
    [SerializeField] Transform Point_4;
    [SerializeField] Transform Point_5;
    [SerializeField] Transform Point_6;
    [SerializeField] Transform Point_7;
    [SerializeField] Transform Point_8;
    [SerializeField] Transform Point_9;
    [SerializeField] Transform Point_0;
    [SerializeField] Transform Point_Z;
    [SerializeField] Transform Point_X;

    [Tooltip("위에서 대각선 아래를 바라보는 시점, 새가 바라보는 시점")]
    [SerializeField] Transform Point_SpaceBar;

    [Header("Move")]
    [Tooltip("이동속도")]
    [SerializeField] float moveSpeed = 1f;
    [Tooltip("전진 키")]
    [SerializeField] KeyCode forwardKey = KeyCode.W;
    [Tooltip("후진 키")]
    [SerializeField] KeyCode backKey = KeyCode.S;
    [Tooltip("왼쪽 키")]
    [SerializeField] KeyCode leftKey = KeyCode.A;
    [Tooltip("오른쪽 키")]
    [SerializeField] KeyCode rightKey = KeyCode.D;
    [Tooltip("위 키")]
    [SerializeField] KeyCode upKey = KeyCode.E;
    [Tooltip("아래 키")]
    [SerializeField] KeyCode downKey = KeyCode.Q;
    [Space]
    [Tooltip("고속")]
    [SerializeField] float highSpeed = 2f;
    [Tooltip("고속 키")]
    [SerializeField] KeyCode highSpeedKey = KeyCode.LeftShift;

    [Header("Rotate")]
    [Tooltip("회전속도")]
    [SerializeField] float rotateSpeed = 1f;
    [Tooltip("X축 회전범위")]
    [SerializeField] Vector2 angleRangeX = new Vector2(-89f, 89f);

    [Header("Setting")]
    [SerializeField] GameObject settingUI;
    [SerializeField] bool openSetting = false;
    [SerializeField] bool cursorVisible = false;

    private void Awake()
    {
        if (cam == null)
            cam = transform;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        if (cursorVisible) Cursor.visible = true;
        else Cursor.visible = false;

        OpenSettings();
        if (openSetting) return;

        Move();
        Rotate();

        ButtonDown();
        ClickMoveToPoint();
    }

    #region 회전, 이동
    private void Move()
    {
        float speed = Input.GetKey(highSpeedKey) ? highSpeed : moveSpeed;

        Vector3 dir = Vector3.zero;
        if (Input.GetKey(forwardKey))
            dir += cam.forward;
        if (Input.GetKey(backKey))
            dir -= cam.forward;
        if (Input.GetKey(leftKey))
            dir -= cam.right;
        if (Input.GetKey(rightKey))
            dir += cam.right;
        if (Input.GetKey(upKey))
            dir += cam.up;
        if (Input.GetKey(downKey))
            dir -= cam.up;

        cam.position += speed * Time.deltaTime * Vector3.Normalize(dir); //방향벡터의 길이를 1로 계산
    }
    private void Rotate()
    {
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Mouse Y");

        Vector3 camAngles = cam.eulerAngles;
        camAngles.x = camAngles.x > 180f ? camAngles.x - 360f : camAngles.x; //eulerAngles는 음수로 값을 저장하지 않으므로, Inspector와 동일하게 변경

        camAngles.x = Mathf.Clamp(camAngles.x - vertical * rotateSpeed, angleRangeX.x, angleRangeX.y); //X축 범위 제한
        camAngles.y += horizontal * rotateSpeed;

        cam.eulerAngles = camAngles;
    }
    #endregion

    #region 좌표로 이동
    private void ButtonDown()
    {
        //버튼을 누르면 해당 저장 좌표로 이동
        switch (Input.inputString)
        {
            case "1":
                this.transform.position = Point_1.transform.position;
                this.transform.rotation = Point_1.transform.rotation;
                break;
            case "2":
                this.transform.position = Point_2.transform.position;
                this.transform.rotation = Point_2.transform.rotation;
                break;
            case "3":
                this.transform.position = Point_3.transform.position;
                this.transform.rotation = Point_3.transform.rotation;
                break;
            case "4":
                this.transform.position = Point_4.transform.position;
                this.transform.rotation = Point_4.transform.rotation;
                break;
            case "5":
                this.transform.position = Point_5.transform.position;
                this.transform.rotation = Point_5.transform.rotation;
                break;
            case "6":
                this.transform.position = Point_6.transform.position;
                this.transform.rotation = Point_6.transform.rotation;
                break;
            case "7":
                this.transform.position = Point_7.transform.position;
                this.transform.rotation = Point_7.transform.rotation;
                break;
            case "8":
                this.transform.position = Point_8.transform.position;
                this.transform.rotation = Point_8.transform.rotation;
                break;
            case "9":
                this.transform.position = Point_9.transform.position;
                this.transform.rotation = Point_9.transform.rotation;
                break;
            case "0":
                this.transform.position = Point_0.transform.position;
                this.transform.rotation = Point_0.transform.rotation;
                break;
            case "z":
                this.transform.position = Point_Z.transform.position;
                this.transform.rotation = Point_Z.transform.rotation;
                break;
            case "x":
                this.transform.position = Point_X.transform.position;
                this.transform.rotation = Point_X.transform.rotation;
                break;
            case " ":
                this.transform.position = Point_SpaceBar.transform.position;
                this.transform.rotation = Point_SpaceBar.transform.rotation;
                break;
        }
    }
    private void ClickMoveToPoint()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            cursorVisible = true;

            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (transform.position.y > 50)
                    {
                        transform.position = hit.point;
                    }
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            cursorVisible = false;
        }
    }
    #endregion

    #region 설정
    private void OpenSettings()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            openSetting = !openSetting;
            if (openSetting)
            {
                cursorVisible = true;
                Time.timeScale = 0;
                settingUI.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                cursorVisible = false;
                settingUI.SetActive(false);
            }
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    #endregion
}