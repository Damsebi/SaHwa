using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimation), typeof(PlayerMove), typeof(PlayerRotate))]
public class PlayerController : MonoBehaviour
{
    #region �ڵ� ������
    /* 
     * �̱���: 
     *      ������ ���� ����(Static). ���� ���� ��ũ��Ʈ�� ��� �޸� ����� ���� �� �ִ�.
     *      ���� ������ ������ ��ü�� ��� �����. ��ü�� ���� ���� �Ǹ� �� ������ �����Ұ� ���� ���� �Ҵ� / ���� ������?
     *      ���� å�� ��Ģ ����: �̱����� �ڽ��� �ν��Ͻ��� �ִ��� Ȯ���ϰ� �����ǵ��� �ϱ� ������ �� ������ ���
     *      ���� ��� ��Ģ ����: �ʹ� ���� å���� �����ų� ���� �����͸� �����ϸ� Ŭ������ ���յ��� ������ OCP(Open Close Principle, ���� ����� ��Ģ)�� ����
     *      ���� �̽��� ����
     *      
     * �����丵:
     *      �ڵ��� ������ �����ϸ鼭 �� �����ϱ� ����, �����ϱ� ����, Ȯ���ϱ� ���� �籸��. ���� ����ȭ�ʹ� �������.
     *      
     * ����å�ӿ�Ģ(�ϳ��� Ŭ������ �ϳ��� å�Ӹ�. � ���ҿ� ���� ��������� �߻����� ��, ������ �޴� ��ɸ� ��Ƶ� ��)
     *      ��ǲ
     *      �̵�
     *      ȸ��
     *      ����
     *      �ǰ�
     *          
     * ��������Ʈ(+�׼�,���)
     * ��ǲ�ý��� ����� �ᵵ �ɵ�
     */
    #endregion

    [SerializeField] PlayerAnimation playerAnimation;
    [SerializeField] PlayerMove playerMove;
    [SerializeField] PlayerRotate playerRotate;

    private static PlayerController instance;
    public static PlayerController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerController();
            }
            return instance;
        }
    }

    private void Awake()
    {
        

    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
