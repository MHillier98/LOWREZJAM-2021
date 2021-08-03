using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObject roadObject;
    public GameObject[] houseObjects;
    public GameObject[] treeObjects;

    public List<Vector3> roadLocations;
    public List<Vector3> houseLocations;
    public List<Vector3> treeLocations;

    private void Start()
    {
        for (int x = 0; x < 4; x++)
        {
            RandomWalk(0, 0, 200);
        }

        SpawnTrees();
    }

    private void RandomWalk(float globalX, float globalZ, int counter)
    {
        if (counter > 0)
        {
            counter--;

            int length = 4;
            int randLength = Random.Range(1, 5);
            length *= randLength;

            SpawnRoad(globalX, globalZ);

            int randDirection = Random.Range(0, 4);
            if (globalZ > 90 && randDirection == 0) { randDirection = 1; }
            if (globalZ < -90 && randDirection == 1) { randDirection = 0; }
            if (globalX > 90 && randDirection == 2) { randDirection = 3; }
            if (globalX < -90 && randDirection == 3) { randDirection = 2; }

            switch (randDirection)
            {
                case 0:
                    {
                        for (int z = 0; z < length; z++)
                        {
                            SpawnRoad(globalX, globalZ + z);
                        }
                        RandomWalk(globalX, globalZ + length, counter);
                        break;
                    }
                case 1:
                    {
                        for (int z = -length; z < 0; z++)
                        {
                            SpawnRoad(globalX, globalZ + z);
                        }
                        RandomWalk(globalX, globalZ - length, counter);
                        break;
                    }

                case 2:
                    {
                        for (int x = 0; x < length; x++)
                        {
                            SpawnRoad(globalX + x, globalZ);
                        }
                        RandomWalk(globalX + length, globalZ, counter);
                        break;
                    }

                case 3:
                    {
                        for (int x = -length; x < 0; x++)
                        {
                            SpawnRoad(globalX + x, globalZ);
                        }
                        RandomWalk(globalX - length, globalZ, counter);
                        break;
                    }
            }

        }
    }

    private void SpawnRoad(float globalX, float globalZ)
    {
        Vector3 roadLoc = new Vector3(globalX, 0, globalZ);
        if (!roadLocations.Contains(roadLoc))
        {
            Instantiate(roadObject, roadLoc, Quaternion.identity);
            roadLocations.Add(roadLoc);

            SpawnHouse(globalX, globalZ + 1.5f, 0f);
            SpawnHouse(globalX, globalZ - 1.5f, 180f);
            SpawnHouse(globalX + 1.5f, globalZ, 90f);
            SpawnHouse(globalX - 1.5f, globalZ, 270f);
        }
    }

    private void SpawnHouse(float globalX, float globalZ, float rotation)
    {
        Vector3 houseLoc = new Vector3(globalX, 0, globalZ);
        if (!houseLocations.Contains(houseLoc))
        {
            if (HouseHasSpace(houseLoc))
            {
                int randHouse = Random.Range(0, houseObjects.Length);
                GameObject house = houseObjects[randHouse];
                Instantiate(house, houseLoc, Quaternion.Euler(0, rotation, 0));
                houseLocations.Add(houseLoc);
            }
        }
    }

    private bool HouseHasSpace(Vector3 newPos)
    {
        foreach (Vector3 house in houseLocations)
        {
            if (Vector3.Distance(house, newPos) < 1.5f)
            {
                return false;
            }
        }
        return true;
    }

    private void SpawnTrees()
    {
        float size = 110f;

        for (float x = -size; x <= size; x += 0.5f)
        {
            for (float z = -size; z <= size; z += 0.5f)
            {
                float newNoise = Random.Range(0.0f, 10000f);
                float noise = Mathf.PerlinNoise(x + newNoise, z + newNoise);

                if (noise >= 0.8f)
                {
                    int randTree = Random.Range(0, treeObjects.Length);
                    GameObject tree = treeObjects[randTree];

                    float randomRotation = Random.Range(0, 360);
                    Vector3 treeLoc = new Vector3(x, 0.0f, z);
                    Instantiate(tree, treeLoc, Quaternion.Euler(0, randomRotation, 0));
                    treeLocations.Add(treeLoc);
                }
            }
        }
    }
}
