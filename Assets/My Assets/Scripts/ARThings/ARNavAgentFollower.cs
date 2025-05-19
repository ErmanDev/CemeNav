using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Syncs the NavMeshAgent's position with the ARCamera,
/// ensuring it's correctly placed on the NavMesh surface.
/// Attach this to your NavAgent GameObject.
/// </summary>
public class ARNavAgentFollower : MonoBehaviour
{
    public Transform ARCamera; // Reference to the live AR camera
    private NavMeshAgent agent;

    void Start()
    {
        {
            agent = GetComponent<NavMeshAgent>();

            if (agent == null)
            {
                Debug.LogError("? NavMeshAgent component is missing!");
                return;
            }

            if (!agent.isOnNavMesh)
            {
                if (NavMesh.SamplePosition(agent.transform.position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
                {
                    agent.Warp(hit.position);
                    Debug.Log("Agent warped to NavMesh: " + hit.position);
                }
                else
                {
                    Debug.LogWarning(" Couldn't place agent on NavMesh.");
                }
            }
            TryWarpAgentToNavMesh();
        }
    }

    void Update()
    {
        if (ARCamera == null || agent == null) return;

        // Update agent's position to match the ARCamera
        agent.Warp(ARCamera.position);
    }

    private void TryWarpAgentToNavMesh()
    {
        if (agent.isOnNavMesh) return;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
            Debug.Log(" Agent successfully warped to nearest NavMesh position.");
        }
        else
        {
            Debug.LogWarning(" Failed to find a valid NavMesh nearby for agent placement.");
        }
    }
}
