using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
//using SimpleToggleButton;

/**
 * Handles the agent and other controllers to navigate a user in AR to a selected destination.
 */
public class ARNavController : MonoBehaviour
{
    public static ARNavController instance;

    /** AR camera of scene **/
    Camera ARCamera;

    /** navmesh agent **/
    public NavMeshAgent agent;

    /** holds current destination of navigation **/
    public POI currentDestination;

    /** collider of the ARCamera to detect POI arrival **/
    SphereCollider ARCameraCollider;

    //public ToggleButton agentVisibilityToggle;

    /**
     * Augmented space where we can navigated
     */
    public AugmentedSpace agumentedSpace;

    private void Awake()
    {
        instance = this;
        ARCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Assign ARCameraCollider safely
        ARCameraCollider = ARCamera.GetComponent<SphereCollider>();
        if (ARCameraCollider == null)
        {
            Debug.LogError("ARCamera is missing a SphereCollider.");
        }

        // Make sure ARStateController is initialized
        if (ARStateController.instance == null)
        {
            Debug.LogError("ARStateController.instance is null. Make sure it's in the scene and initialized before ARNavController.");
            return;
        }

        ARStateController.instance.PositionFoundEvent.AddListener(() =>
        {
            NavUIController.instance.ShowScanOverlay(false);
        });

        ARStateController.instance.PositionLostEvent.AddListener(() =>
        {
            NavUIController.instance.ShowScanOverlay(true);
        });

        // Initial state
        ARStateController.instance.PositionLostEvent.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (ARStateController.instance.IsLocalized())
        {
            agent.gameObject.SetActive(true);
            if (agent.isOnNavMesh)
            {
                agent.isStopped = false;
                // TODO: add debug function to simulate the agent walking to destination
            }
        }
        else
        {
            // agent is deactivated when not localized because no NavMesh is available
            agent.gameObject.SetActive(false);
        }

        if (IsCurrentlyNavigating() && ARStateController.instance.IsLocalized() && agent.isOnNavMesh)
        {
            agent.destination = currentDestination.poiCollider.transform.position;

            // when we are navigating and we are localized path needs to go from curent agent position
            //ARPathSteps.instance.SetPositionFrom(agent.transform);
            //ARPathSteps.instance.SetPositionTo(currentDestination.poiCollider.transform);
            ARPathVisualizer.instance.SetPositionFrom(agent.transform);
            ARPathVisualizer.instance.SetPositionTo(currentDestination.poiCollider.transform);
            // TODO: fix nicer

            // enable collider to detect arrival
            ARCameraCollider.enabled = true;
        }
        else if (!IsCurrentlyNavigating() && ARStateController.instance.IsLocalized())
        {
            // localized but not navigating, collider should also be active to detect continuation or end of AT
            ARCameraCollider.enabled = true;
        }
        else if (IsCurrentlyNavigating() && ARStateController.instance.IsLocalized() && !agent.isOnNavMesh)
        {
            // TODO: handle not being on navmesh
        }
        else
        {
            // not localized, not navigating
            ARCameraCollider.enabled = false;
        }
    }

    /**
     * Sets a POI for navigation and gets ready for navigation.
     */
    public void SetPOIForNavigation(POI aPOI)
    {
        currentDestination = aPOI;
        if (ARStateController.instance.IsLocalized())
        {
            StartNavigation();
        }
        else
        {
            // TODO: show error, not localized
        }
    }

    public void StartNavigation()
    {
        // set positions for path visualizer
        //ARPathSteps.instance.SetPositionFrom(agent.transform);
        //ARPathSteps.instance.SetPositionTo(currentDestination.poiCollider.transform);
        ARPathVisualizer.instance.SetPositionFrom(agent.transform);
        ARPathVisualizer.instance.SetPositionTo(currentDestination.poiCollider.transform);
        //NavigationUIController.instance.ShowNavigationUI(true);
    }

    /**
     * Stops navigation.
     */
    public void StopNavigation()
    {
        if (currentDestination != null)
        {
            currentDestination = null;
            //ARPathSteps.instance.ResetPath();
            PathEstimationUtils.instance.ResetEstimation();
        }
    }

    /**
     * Hanles destination arrival
     */
    public void ArrivedAtDestination()
    {
        //BackendController.instance.SendVisitData(currentDestination);
        StopNavigation();
        //NavUIController.instance.ShowArrivedState();
    }

    /**
     * Returns true when user is currently navigating.
     */
    public bool IsCurrentlyNavigating()
    {
        return currentDestination != null;
    }

    public void ToggleAgentVisibility()
    {
        agent.gameObject.GetComponent<MeshRenderer>().enabled = !agent.gameObject.GetComponent<MeshRenderer>().enabled;
    }
}
