using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone : MonoBehaviour
{
    public float windForce = 10f; // °­Ç³ÀÇ Èû
    public Vector3 windDirection = Vector3.forward; // °­Ç³ÀÇ ¹æÇâ

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.AddForce(windDirection.normalized * windForce);
        }
    }
}
