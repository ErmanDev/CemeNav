using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class POIManager : MonoBehaviour
{
    public GameObject poiButtonPrefab; // a prefab with TMP_Text + Button
    public Transform contentPanel;     // ScrollView Content holder
    public List<Transform> allPOIs;    // Optionally fill via tags
    public ARNavLineRenderer navRenderer; // Reference to nav renderer

    void Start()
    {
        PopulatePOIList();
    }

    void PopulatePOIList()
    {
        foreach (Transform poi in allPOIs)
        {
            GameObject buttonObj = Instantiate(poiButtonPrefab, contentPanel);
            TMP_Text label = buttonObj.GetComponentInChildren<TMP_Text>();
            label.text = poi.name;

            buttonObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                navRenderer.destination = poi;
            });
        }
    }
}
