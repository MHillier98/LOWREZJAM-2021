using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObject roadObject;

    public List<Vector3> roadLocations;

    private void Start()
    {
        for (int x = 0; x < 4; x++)
        {
            RandomWalk(0, 0, 250);
        }
    }

    private void RandomWalk(int globalX, int globalZ, int counter)
    {
        if (counter > 0)
        {
            counter--;

            int length = 4;
            int randLength = Random.Range(1, 5);
            length *= randLength;

            Vector3 roadLocBase = new Vector3(globalX, 0, globalZ);
            if (!roadLocations.Contains(roadLocBase))
            {
                Instantiate(roadObject, roadLocBase, Quaternion.identity);
                roadLocations.Add(roadLocBase);
            }

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
                            Vector3 roadLoc = new Vector3(globalX, 0, globalZ + z);
                            if (!roadLocations.Contains(roadLoc))
                            {
                                Instantiate(roadObject, roadLoc, Quaternion.identity);
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
                                Instantiate(roadObject, roadLoc, Quaternion.identity);
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
                                Instantiate(roadObject, roadLoc, Quaternion.identity);
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
                                Instantiate(roadObject, roadLoc, Quaternion.identity);
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
