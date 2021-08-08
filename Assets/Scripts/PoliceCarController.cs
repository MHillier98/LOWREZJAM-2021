using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceCarController : MonoBehaviour
{
    public NavMeshAgent agent;

    public List<Vector3> destinations;
    [SerializeField] private int destinationIx = 0;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (destinationIx == 0)
        {
            SetNextDestination();
        }
        else
        {
            if (Vector2.Distance(transform.position, destinations[destinationIx]) < 2f)
            {
                SetNextDestination();
            }
        }
    }

    private void SetNextDestination()
    {
        if (destinations.Count > 0)
        {
            int randDestination = Random.Range(1, destinations.Count - 1); // List<>
            destinationIx = randDestination;

            agent.SetDestination(destinations[destinationIx]);
        }
    }

    public void AddDestinations(Vector3 newDestination)
    {
        destinations.Add(newDestination);
    }
}
