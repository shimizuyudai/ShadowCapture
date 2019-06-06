using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyZone : MonoBehaviour {

    public void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
