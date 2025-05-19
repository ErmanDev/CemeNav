using UnityEngine;
using UnityEngine.AI;

public class ArrowNavigator : MonoBehaviour
{
    public Transform destination;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destination.position);
    }

    void Update()
    {
        if (agent.isOnNavMesh && destination != null)
        {
            agent.SetDestination(destination.position);

            // Optional: rotate to face movement direction
            if (agent.velocity.sqrMagnitude > 0.1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(agent.velocity.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }
    }

}
