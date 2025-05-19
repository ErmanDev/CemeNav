using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class ARNavLineRenderer : MonoBehaviour
{
    public NavMeshAgent agent;           // The AR-following agent (not a real character)
    public Transform destination;        // Current POI target

    private LineRenderer line;
    private NavMeshPath path;

    [Header("Line Style")]
    public float lineWidth = 0.1f;
    public Color lineColor = Color.yellow;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        path = new NavMeshPath();

        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = lineColor;
        line.endColor = lineColor;
    }

    void Update()
    {
        if (agent == null || destination == null)
        {
            line.positionCount = 0;
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning("Agent is not on NavMesh!");
            return;
        }

        if (NavMesh.CalculatePath(agent.transform.position, destination.position, NavMesh.AllAreas, path))
        {
            if (path.corners.Length >= 2)
            {
                Debug.Log("Rendering nav line with " + path.corners.Length + " corners.");
                line.positionCount = path.corners.Length;
                line.SetPositions(path.corners);
            }
            else
            {
                Debug.Log("Path corners too short.");
                line.positionCount = 0;
            }
        }
    }

    /// <summary>
    /// Call this to update the target destination from the UI or script.
    /// </summary>
    /// <param name="target">The new POI destination</param>
    public void SetDestination(Transform target)
    {
        destination = target;
    }
}
