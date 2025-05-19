using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject searchPanel;

    [Header("POI UI")]
    public Transform poiListContainer;     // Parent where buttons will be spawned
    public GameObject poiButtonPrefab;     // POI button prefab (UI)

    [Header("POI Data")]
    public List<POI> activePOIs = new List<POI>(); // The current active POIs in the scene

    [Header("Navigation")]
    public ARNavLineRenderer navLineRenderer;      // The script handling line rendering
    public Transform navAgent;                     // The moving NavMeshAgent

    [Header("Controls")]
    public GameObject stopButton; // Stop button GameObject

    void Awake()
    {
        EnhancedTouchSupport.Enable(); // Fixes Vuforia touch issue

        if (searchPanel != null)
            searchPanel.SetActive(false);

        if (navLineRenderer != null)
            navLineRenderer.gameObject.SetActive(false); // Hide nav line initially

        if (stopButton != null)
            stopButton.SetActive(false); // Hide Stop button initially

        if (poiButtonPrefab != null)
            poiButtonPrefab.SetActive(true); // Ensure POI button prefab is visible for instantiating
    }

    public void ToggleSearchPanel()
    {
        if (searchPanel != null)
        {
            searchPanel.SetActive(!searchPanel.activeSelf);

            if (searchPanel.activeSelf)
            {
                PopulatePOIButtons(); // Refresh list when panel is opened
            }
        }
    }

    public void PopulatePOIButtons()
    {
        foreach (Transform child in poiListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (POI poi in activePOIs)
        {
            GameObject buttonObj = Instantiate(poiButtonPrefab, poiListContainer);

            // Ensure instantiated button is active
            buttonObj.SetActive(true);

            // Set UI text fields
            var nameText = buttonObj.transform.Find("POINameText")?.GetComponent<TMP_Text>();
            var distanceText = buttonObj.transform.Find("DistanceText")?.GetComponent<TMP_Text>();
            var goButton = buttonObj.transform.Find("GoButton")?.GetComponent<Button>();

            if (nameText != null) nameText.text = poi.poiName;

            if (distanceText != null && navAgent != null)
            {
                float dist = Vector3.Distance(navAgent.position, poi.transform.position);
                distanceText.text = Mathf.RoundToInt(dist) + "m";
            }

            if (goButton != null)
            {
                goButton.onClick.AddListener(() => {
                    Debug.Log("Navigating to: " + poi.poiName);
                    if (navLineRenderer != null)
                    {
                        navLineRenderer.SetDestination(poi.transform);
                        navLineRenderer.gameObject.SetActive(true); // Show nav line after GO
                    }
                    if (stopButton != null)
                        stopButton.SetActive(true);
                });
            }
        }
    }

    public void StopNavigation()
    {
        if (navLineRenderer != null)
            navLineRenderer.gameObject.SetActive(false);

        if (stopButton != null)
            stopButton.SetActive(false);

        Debug.Log("Navigation stopped.");
    }
}
