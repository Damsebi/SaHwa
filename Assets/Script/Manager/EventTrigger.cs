using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public List<MonoBehaviour> eventScripts; //IEvent 인터페이스를 구현하는 스크립트들

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (MonoBehaviour script in eventScripts)
            {
                //script를 IEvent로 형변환 하는데, 성공하면 IEvent타입이 되고 실패하면 null이 됨.
                IEvent ievent = script as IEvent;
                if (ievent != null)
                {
                    ievent.TriggerEvent();
                }

            }
        }
    }

}
