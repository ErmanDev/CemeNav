using UnityEngine;
using Vuforia;

public class ShowUIOnAreaTarget : MonoBehaviour
{
    public GameObject canvasToEnable;

    void Start()
    {
        var observer = GetComponent<ObserverBehaviour>();
        if (observer)
        {
            observer.OnTargetStatusChanged += OnStatusChanged;
        }

        // Hide UI at startup
        if (canvasToEnable != null)
            canvasToEnable.SetActive(false);
    }

    private void OnStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            if (canvasToEnable != null && !canvasToEnable.activeSelf)
            {
                canvasToEnable.SetActive(true);
                Debug.Log("UI enabled after Area Target detection.");
            }
        }
    }
}
