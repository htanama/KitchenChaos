using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter plateCounter;

    // We can access the location of the counterTopPoint
    [SerializeField] private Transform counterTopPoint;

    // we can access the location of the plateVisualPrefab
    [SerializeField] private Transform plateVisualPrefab; // grab the visual from prefab


    private List<GameObject> plateVisualGameObjectList;

    private void Awake()
    {
        plateVisualGameObjectList = new List<GameObject>();
    }

    private void Start()
    {
        // Event Subscriber
        plateCounter.OnPlateSpawned += PlateCounter_OnPlateSpawned;
    }

    // Event Subscriber
    private void PlateCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

        float plateOffSetY = 0.1f;
        // Modified the local position to show the plate visual is stacking upward 
        plateVisualTransform.localPosition = new Vector3(0, plateOffSetY * plateVisualGameObjectList.Count, 0);

        // Adding plate to a List as GameObject 
        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }
}
