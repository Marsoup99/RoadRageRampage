using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : ObjectPooling
{
    public static RoadGenerator Instance { get; private set; }
    private List<Transform> _activateRoadList = new List<Transform>();

    [Header("Some setting")]
    [SerializeField] private int numberOfRoadsExist = 4;
    public Transform choiceRoad;
    public int _roadLength = 100; //TODO: get from the road prefab not a fixed number like this.
    public Transform lastRoadInList => _activateRoadList[_activateRoadList.Count - 1];
    public Transform firstRoadInList =>  _activateRoadList[0];

    void Awake()
    {
         // If there is an instance, and it's not this, delete itself.
        if (Instance != null && Instance != this) 
        { 
            Debug.Log("there more than one RoadGenerator");
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
    public void SpawnStartingRoad()
    {
        if(_activateRoadList != null)
        {
            int count = _activateRoadList.Count;
            for (int i = 0; i < count; i++)
            {
                DeSpawnRoad();
            }
        }
        _activateRoadList.Add(SpawnObject(PlayerCtrl.Instance.transform.position - 1.5f * _roadLength * transform.forward, transform.rotation));
        for(int i = 0; i < numberOfRoadsExist - 1; i++)
        {
            SpawnRoad();
        }
    }
    public void DeSpawnRoad()
    {
        DeSpawnObject(_activateRoadList[0]);
        _activateRoadList.RemoveAt(0);
    }
    public void SpawnRoad()
    {
        _activateRoadList.Add(SpawnObject(lastRoadInList.position + _roadLength * lastRoadInList.forward, lastRoadInList.rotation));  
    }

    public void SpawnChoicesRoad()
    {
        DeSpawnObject(_activateRoadList[_activateRoadList.Count - 1]);
        _activateRoadList.RemoveAt(_activateRoadList.Count - 1);
        
        _activateRoadList.Add(SpawnObject(choiceRoad, lastRoadInList.position + _roadLength * lastRoadInList.forward, lastRoadInList.rotation));
    }
}
