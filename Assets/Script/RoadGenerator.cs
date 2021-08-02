using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObject[] houses;

    void Start()
    {
        for (int x = -9; x < 10; x += 3)
        {
            for (int z = -9; z < 10; z += 3)
            {
                int randIx = Random.Range(0, houses.Length);
                GameObject house = houses[randIx];
                //Instantiate(house, new Vector3(x, 0, z), Quaternion.identity);
            }
        }
    }
}
