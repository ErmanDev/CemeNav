using UnityEngine;

public class POI : MonoBehaviour
{
    [Header("POI Info")]
    public string poiName;               // Display name (e.g., "Grave 1")

    [Header("Navigation")]
    public POICollider poiCollider;      // Trigger to detect user arrival

    void Awake()
    {
        if (poiCollider == null)
            poiCollider = GetComponentInChildren<POICollider>();

        if (poiCollider != null)
            poiCollider.SetPOI(this);
    }

    public void Arrived()
    {
        if (ARNavController.instance != null && ARNavController.instance.currentDestination == this)
        {
            ARNavController.instance.ArrivedAtDestination();
        }
    }

    public string GetTitle()
    {
        return poiName;
    }
}
