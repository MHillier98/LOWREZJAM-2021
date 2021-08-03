using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Road"))
        {
            Destroy(gameObject);
        }
        else if (other.transform.CompareTag("Tree"))
        {
            Destroy(gameObject);
        }
    }
}
