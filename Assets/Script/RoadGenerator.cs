using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObject[] housePrefabs;
    public GameObject road;

    private void Start()
    {
        RandomWalk(0, 0, 1000);
    }

    private void RandomWalk(int globalX, int globalZ, int counter)
    {
        if (counter > 0)
        {
            counter--;
            int length = 3;

            Instantiate(road, new Vector3(globalX, 0, globalZ), Quaternion.identity);

            int randDirection = Random.Range(0, 4);
            switch (randDirection)
            {
                case 0:
                    {
                        for (int z = 0; z < length; z++)
                        {
                            Instantiate(road, new Vector3(globalX, 0, globalZ + z), Quaternion.identity);
                        }
                        RandomWalk(globalX, globalZ + length, counter);
                        break;
                    }

                case 1:
                    {
                        for (int z = -length; z < 0; z++)
                        {
                            Instantiate(road, new Vector3(globalX, 0, globalZ + z), Quaternion.identity);
                        }
                        RandomWalk(globalX, globalZ - length, counter);
                        break;
                    }

                case 2:
                    {
                        for (int x = 0; x < length; x++)
                        {
                            Instantiate(road, new Vector3(globalX + x, 0, globalZ), Quaternion.identity);
                        }
                        RandomWalk(globalX + length, globalZ, counter);
                        break;
                    }

                case 3:
                    {
                        for (int x = -length; x < 0; x++)
                        {
                            Instantiate(road, new Vector3(globalX + x, 0, globalZ), Quaternion.identity);
                        }
                        RandomWalk(globalX - length, globalZ, counter);
                        break;
                    }
            }

        }
    }

    private void GenerateRoads()
    {
        for (float x = -9.5f; x < 10; x++)
        {
            for (float z = -9.5f; z < 10; z++)
            {
                Instantiate(road, new Vector3(x, 0, z), Quaternion.identity);
            }
        }
    }

    private void GenerateHouses()
    {
        for (int x = -9; x < 10; x += 3)
        {
            for (int z = -9; z < 10; z += 3)
            {
                int randIx = Random.Range(0, housePrefabs.Length);
                GameObject house = housePrefabs[randIx];
                Instantiate(house, new Vector3(x, 0, z), Quaternion.identity);
            }
        }
    }
}
