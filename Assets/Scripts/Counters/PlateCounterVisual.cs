using System.Collections.Generic;
using UnityEngine;

public class PlateCounterVisual : MonoBehaviour
{
    [SerializeField]
    private Transform counterTopPoint;

    [SerializeField]
    private Transform plateVisualPrefab;

    [SerializeField]
    private PlatesCounter platesCounter;

    private List<GameObject> plateVisualGameObjectList;

    private void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void Awake()
    {
        plateVisualGameObjectList = new List<GameObject>();
    }

    private void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        GameObject plateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];

        plateVisualGameObjectList.Remove(plateGameObject);

        Destroy(plateGameObject);
    }

    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

        float plateOffsetY = .1f;
        plateVisualTransform.localPosition = new Vector3(
            0,
            plateOffsetY * plateVisualGameObjectList.Count,
            0
        );

        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }
}
