using UnityEngine;
using UnityEngine.AI;

/**
 * Keeps NavMeshAgent locked to the NavMesh surface near the AR camera.
 */
public class NavMeshAgentHelper : MonoBehaviour
{
    GameObject ARCamera;
    NavMeshAgent agent;
    MeshRenderer mesh;

    // Max distance to search for nearby NavMesh
    public float maxNavMeshDistance = 2.0f;

    void Awake()
    {
        ARCamera = Camera.main.gameObject;
        agent = GetComponent<NavMeshAgent>();
        mesh = GetComponent<MeshRenderer>();
        if (mesh != null)
            mesh.enabled = false;
    }

    void Update()
    {
        Vector3 cameraPos = ARCamera.transform.position;

        // Try to find a position on the NavMesh near the AR camera
        if (NavMesh.SamplePosition(cameraPos, out NavMeshHit hit, maxNavMeshDistance, NavMesh.AllAreas))
        {
            // Only move if the agent is far from the correct NavMesh surface position
            float dist = Vector3.Distance(agent.transform.position, hit.position);
            if (dist > 0.1f)
            {
                agent.Warp(hit.position);
            }
        }
        else
        {
            Debug.LogWarning("AR camera is off NavMesh or too far from it.");
        }
    }

    public void ToggleNavMeshAgentVisibility()
    {
        if (mesh != null)
            mesh.enabled = !mesh.enabled;
    }
}
