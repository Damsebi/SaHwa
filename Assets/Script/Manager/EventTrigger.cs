using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public List<MonoBehaviour> eventScripts; //IEvent �������̽��� �����ϴ� ��ũ��Ʈ��

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (MonoBehaviour script in eventScripts)
            {
                //script�� IEvent�� ����ȯ �ϴµ�, �����ϸ� IEventŸ���� �ǰ� �����ϸ� null�� ��.
                IEvent ievent = script as IEvent;
                if (ievent != null)
                {
                    ievent.TriggerEvent();
                }

            }
        }
    }

}
