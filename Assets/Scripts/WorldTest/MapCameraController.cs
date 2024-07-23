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

    [Tooltip("������ �밢�� �Ʒ��� �ٶ󺸴� ����, ���� �ٶ󺸴� ����")]
    [SerializeField] Transform Point_SpaceBar;

    [Header("Move")]
    [Tooltip("�̵��ӵ�")]
    [SerializeField] float moveSpeed = 1f;
    [Tooltip("���� Ű")]
    [SerializeField] KeyCode forwardKey = KeyCode.W;
    [Tooltip("���� Ű")]
    [SerializeField] KeyCode backKey = KeyCode.S;
    [Tooltip("���� Ű")]
    [SerializeField] KeyCode leftKey = KeyCode.A;
    [Tooltip("������ Ű")]
    [SerializeField] KeyCode rightKey = KeyCode.D;
    [Tooltip("�� Ű")]
    [SerializeField] KeyCode upKey = KeyCode.E;
    [Tooltip("�Ʒ� Ű")]
    [SerializeField] KeyCode downKey = KeyCode.Q;
    [Space]
    [Tooltip("���")]
    [SerializeField] float highSpeed = 2f;
    [Tooltip("��� Ű")]
    [SerializeField] KeyCode highSpeedKey = KeyCode.LeftShift;

    [Header("Rotate")]
    [Tooltip("ȸ���ӵ�")]
    [SerializeField] float rotateSpeed = 1f;
    [Tooltip("X�� ȸ������")]
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

    #region ȸ��, �̵�
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

        cam.position += speed * Time.deltaTime * Vector3.Normalize(dir); //���⺤���� ���̸� 1�� ���
    }
    private void Rotate()
    {
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Mouse Y");

        Vector3 camAngles = cam.eulerAngles;
        camAngles.x = camAngles.x > 180f ? camAngles.x - 360f : camAngles.x; //eulerAngles�� ������ ���� �������� �����Ƿ�, Inspector�� �����ϰ� ����

        camAngles.x = Mathf.Clamp(camAngles.x - vertical * rotateSpeed, angleRangeX.x, angleRangeX.y); //X�� ���� ����
        camAngles.y += horizontal * rotateSpeed;

        cam.eulerAngles = camAngles;
    }
    #endregion

    #region ��ǥ�� �̵�
    private void ButtonDown()
    {
        //��ư�� ������ �ش� ���� ��ǥ�� �̵�
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

    #region ����
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