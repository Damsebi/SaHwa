using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebug : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + playerMovement.DebugMovement() * 3);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Camera.main.transform.forward * 3);

    }

}
