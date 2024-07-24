using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimation), typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    #region 코드 나중에
    /* 
     * 싱글톤: 
     *      전역에 접근 가능(Static). 자주 쓰는 스크립트의 경우 메모리 사용을 줄일 수 있다.
     *      최초 생성한 동일한 객체를 계속 사용함. 객체가 여러 개가 되면 한 곳에만 수정할게 여러 곳에 할당 / 수정 될지도?
     *      단일 책임 원칙 위반: 싱글턴은 자신의 인스턴스가 있는지 확인하고 유지되도록 하기 때문에 두 가지를 담당
     *      개발 폐쇄 원칙 위반: 너무 많은 책임을 가지거나 많은 데이터를 공유하면 클래스간 결합도가 높아져 OCP(Open Close Principle, 개발 폐쇄의 원칙)를 위반
     *      성능 이슈에 조심
     *      
     * 리패토링:
     *      코드의 동작은 유지하면서 더 이해하기 쉽고, 생각하기 쉽고, 확장하기 쉽게 재구성. 성능 최적화와는 관계없다.
     *      
     * 단일책임원칙(하나의 클래스는 하나의 책임만. 어떤 역할에 대해 변경사항이 발생했을 때, 영향을 받는 기능만 모아둔 것)
     *      인풋
     *      이동
     *      회전
     *      공격
     *      피격
     *          
     * 델리게이트(+액션,펑션)
     * 인풋시스템 배워서 써도 될듯
     */
    #endregion

    [SerializeField] PlayerAnimation playerAnimation;
    [SerializeField] PlayerMovement playerMove;

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
        instance = this;

        playerAnimation = GetComponent<PlayerAnimation>();
        playerMove = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        playerMove.InputDirection(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}
