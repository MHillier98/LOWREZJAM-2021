using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObject[] housePrefabs;
    public GameObject road;

    public List<Vector3> roadLocations;

    private void Start()
    {
        for (int x = 0; x < 10; x++)
        {
            RandomWalk(0, 0, 50);
        }
    }

    private void RandomWalk(int globalX, int globalZ, int counter)
    {
        if (counter > 0)
        {
            counter--;
            int length = 4;
            int randLength = Random.Range(3, 5);
            length = randLength * 2;

            Vector3 roadLocBase = new Vector3(globalX, 0, globalZ);
            if (!roadLocations.Contains(roadLocBase))
            {
                Instantiate(road, roadLocBase, Quaternion.identity);
                roadLocations.Add(roadLocBase);
            }

            int randDirection = Random.Range(0, 4);
            switch (randDirection)
            {
                case 0:
                    {
                        for (int z = 0; z < length; z++)
                        {
                            Vector3 roadLoc = new Vector3(globalX, 0, globalZ + z);
                            if (!roadLocations.Contains(roadLoc))
                            {
                                Instantiate(road, roadLoc, Quaternion.identity);
                                roadLocations.Add(roadLoc);
                            }
                        }
                        RandomWalk(globalX, globalZ + length, counter);
                        break;
                    }
                case 1:
                    {
                        for (int z = -length; z < 0; z++)
                        {
                            Vector3 roadLoc = new Vector3(globalX, 0, globalZ + z);
                            if (!roadLocations.Contains(roadLoc))
                            {
                                Instantiate(road, roadLoc, Quaternion.identity);
                                roadLocations.Add(roadLoc);
                            }
                        }
                        RandomWalk(globalX, globalZ - length, counter);
                        break;
                    }

                case 2:
                    {
                        for (int x = 0; x < length; x++)
                        {
                            Vector3 roadLoc = new Vector3(globalX + x, 0, globalZ);
                            if (!roadLocations.Contains(roadLoc))
                            {
                                Instantiate(road, roadLoc, Quaternion.identity);
                                roadLocations.Add(roadLoc);
                            }
                        }
                        RandomWalk(globalX + length, globalZ, counter);
                        break;
                    }

                case 3:
                    {
                        for (int x = -length; x < 0; x++)
                        {
                            Vector3 roadLoc = new Vector3(globalX + x, 0, globalZ);
                            if (!roadLocations.Contains(roadLoc))
                            {
                                Instantiate(road, roadLoc, Quaternion.identity);
                                roadLocations.Add(roadLoc);
                            }
                        }
                        RandomWalk(globalX - length, globalZ, counter);
                        break;
                    }
            }

        }
    }
}
