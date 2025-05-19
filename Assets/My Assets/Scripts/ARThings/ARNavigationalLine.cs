using UnityEngine;
using System.Collections.Generic;

public class ARNavigationLine : MonoBehaviour
{
    public Transform arCamera;
    public Transform pointOfInterest;
    public int pointCount = 30;
    public float raycastHeight = 1.5f;
    public LayerMask groundLayer;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = pointCount;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
    }

    void Update()
    {
        if (arCamera == null || pointOfInterest == null) return;

        List<Vector3> groundedPoints = new List<Vector3>();

        UnityEngine.AI.NavMeshPath navPath = new UnityEngine.AI.NavMeshPath();
        if (UnityEngine.AI.NavMesh.CalculatePath(arCamera.position, pointOfInterest.position, UnityEngine.AI.NavMesh.AllAreas, navPath))
        {
            List<Vector3> groundedCorners = new List<Vector3>();

            foreach (Vector3 corner in navPath.corners)
            {
                Vector3 rayOrigin = corner + Vector3.up * raycastHeight;
                if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastHeight * 2f, groundLayer))
                {
                    groundedCorners.Add(hit.point);
                }
                else
                {
                    groundedCorners.Add(corner); // fallback if no hit
                }
            }

            lineRenderer.positionCount = groundedCorners.Count;
            lineRenderer.SetPositions(groundedCorners.ToArray());
        }


        lineRenderer.positionCount = groundedPoints.Count;
        lineRenderer.SetPositions(groundedPoints.ToArray());
    }
}
