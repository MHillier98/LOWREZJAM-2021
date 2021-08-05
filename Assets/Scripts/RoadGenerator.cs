using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public float waitTime = 0.1f;

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
            StartCoroutine(RandomWalk(0, 0, 50));
        }
    }

    IEnumerator RandomWalk(float globalX, float globalZ, int counter)
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
                            StartCoroutine(SpawnRoad(globalX, globalZ + z));
                        }

                        yield return new WaitForSeconds(waitTime);
                        StartCoroutine(RandomWalk(globalX, globalZ + length, counter));
                        break;
                    }
                case 1:
                    {
                        for (int z = -length; z <= 0; z++)
                        {
                            StartCoroutine(SpawnRoad(globalX, globalZ + z));
                        }

                        yield return new WaitForSeconds(waitTime);
                        StartCoroutine(RandomWalk(globalX, globalZ - length, counter));
                        break;
                    }

                case 2:
                    {
                        for (int x = 0; x < length; x++)
                        {
                            StartCoroutine(SpawnRoad(globalX + x, globalZ));
                        }

                        yield return new WaitForSeconds(waitTime);
                        StartCoroutine(RandomWalk(globalX + length, globalZ, counter));
                        break;
                    }

                case 3:
                    {
                        for (int x = -length; x <= 0; x++)
                        {
                            StartCoroutine(SpawnRoad(globalX + x, globalZ));
                        }

                        yield return new WaitForSeconds(waitTime);
                        StartCoroutine(RandomWalk(globalX - length, globalZ, counter));
                        break;
                    }
            }
        }
    }

    IEnumerator SpawnRoad(float globalX, float globalZ)
    {
        Vector3 roadLoc = new Vector3(globalX, 0, globalZ);
        if (!roadLocations.Contains(roadLoc))
        {
            Instantiate(roadObject, roadLoc, Quaternion.identity);
            roadLocations.Add(roadLoc);

            yield return new WaitForSeconds(waitTime);

            StartCoroutine(SpawnHouse(globalX, globalZ + 1.5f, 0f));
            StartCoroutine(SpawnHouse(globalX, globalZ - 1.5f, 180f));
            StartCoroutine(SpawnHouse(globalX + 1.5f, globalZ, 90f));
            StartCoroutine(SpawnHouse(globalX - 1.5f, globalZ, 270f));
        }
    }

    IEnumerator SpawnHouse(float globalX, float globalZ, float rotation)
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

                yield return new WaitForSeconds(waitTime);
                StartCoroutine(SpawnTrees(globalX, globalZ));
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

    IEnumerator SpawnTrees(float globalX, float globalZ)
    {
        float sizeOffset = 8f;

        for (float x = globalX - sizeOffset; x <= globalX + sizeOffset; x += 0.5f)
        {
            for (float z = globalZ - sizeOffset; z <= globalZ + sizeOffset; z += 0.5f)
            {
                float newNoise = Random.Range(0.0f, 10000f);
                float noise = Mathf.PerlinNoise(x + newNoise, z + newNoise);

                if (noise >= 0.9f)
                {
                    int randTree = Random.Range(0, treeObjects.Length);
                    GameObject tree = treeObjects[randTree];

                    float randomRotation = Random.Range(0, 360);
                    Vector3 treeLoc = new Vector3(x, 0.0f, z);
                    Instantiate(tree, treeLoc, Quaternion.Euler(0, randomRotation, 0));
                    treeLocations.Add(treeLoc);

                    yield return new WaitForSeconds(waitTime);
                }
            }
        }
    }
}
