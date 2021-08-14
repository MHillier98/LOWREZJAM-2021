using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class RoadGenerator : MonoBehaviour
{
    // How long to wait between instantiating anything new
    [Header("General Params")]
    public float waitTime = 0.1f;
    public int roadSpawnCount = 3;
    public int maxRoadLength = 20;

    public int policeSpawnCount = 3;
    public int policeDestinationCount = 6;

    public NavMeshSurface navMeshSurface;

    // Prefabs to instantiate
    [Header("Prefabs")]
    public GameObject policeCarObjects;
    public GameObject roadObject;
    public GameObject[] houseObjects;
    public GameObject[] treeObjects;
    public GameObject[] animalObjects;

    // Lists of where prefabs have been instantiated
    [Header("Instantiated locations")]
    public List<Vector3> roadLocations;
    public List<Vector3> houseLocations;
    public List<Vector3> treeLocations;
    public List<Vector3> animalLocations;

    // The police cars driving around
    [Header("Instantiated police cars")]
    public List<GameObject> policeCars;

    private void Start()
    {
        for (int i = 0; i <= roadSpawnCount; i++)
        {
            StartCoroutine(RandomWalk(0, 0, maxRoadLength));
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
        else
        {
            for (int i = 0; i <= policeSpawnCount; i++)
            {
                StartCoroutine(SpawnPolice());
            }

            //navMeshSurface.BuildNavMesh(); // big pause while getting baked
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
                StartCoroutine(SpawnAnimals(globalX, globalZ));
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
        float sizeOffset = 4f;

        for (float x = globalX - sizeOffset; x <= globalX + sizeOffset; x += 0.5f)
        {
            for (float z = globalZ - sizeOffset; z <= globalZ + sizeOffset; z += 0.5f)
            {
                float newNoise = Random.Range(0.0f, 10000f);
                float noise = Mathf.PerlinNoise(x + newNoise, z + newNoise);

                if (noise >= 0.85f)
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

    IEnumerator SpawnAnimals(float globalX, float globalZ)
    {
        float sizeOffset = 4f;

        for (float x = globalX - sizeOffset; x <= globalX + sizeOffset; x += 0.5f)
        {
            for (float z = globalZ - sizeOffset; z <= globalZ + sizeOffset; z += 0.5f)
            {
                float newNoise = Random.Range(0.0f, 10000f);
                float noise = Mathf.PerlinNoise(x + newNoise, z + newNoise);

                if (noise >= 0.95f)
                {
                    int randAnimal = Random.Range(0, animalObjects.Length);
                    GameObject animal = animalObjects[randAnimal];

                    float randomRotation = Random.Range(0, 360);
                    Vector3 animalLoc = new Vector3(x, 0.0f, z);

                    Instantiate(animal, animalLoc, Quaternion.Euler(0, randomRotation, 0));
                    animalLocations.Add(animalLoc);

                    yield return new WaitForSeconds(waitTime);
                }
            }
        }
    }

    public IEnumerator SpawnPolice()
    {
        int randSpawnIx = Random.Range(1, roadLocations.Count - 1);
        Vector3 policeCarLoc = roadLocations[randSpawnIx];

        GameObject policeCar = Instantiate(policeCarObjects, policeCarLoc, Quaternion.identity);
        policeCars.Add(policeCar);

        yield return new WaitForSeconds(waitTime);

        for (int j = 0; j <= policeDestinationCount; j++)
        {
            int randDestinationIx = Random.Range(1, roadLocations.Count - 1);
            Vector3 destination = roadLocations[randDestinationIx];

            if (policeCar != null)
            {
                PoliceCarController policeCarController = policeCar.GetComponent<PoliceCarController>();

                if (policeCarController != null)
                {
                    policeCarController.AddDestinations(destination);
                }
            }
        }
    }
}
