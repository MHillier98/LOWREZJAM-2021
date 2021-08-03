using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Road"))
        {
            Destroy(gameObject);
        }
        else if (other.transform.CompareTag("House"))
        {
            //Destroy(gameObject);
        }
    }
}
