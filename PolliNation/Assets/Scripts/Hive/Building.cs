using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingType Type
    { get; set; }
    public int Cost
    { get; set; }
    public ResourceType? ResourceType 
    { get; set; }
    public Vector3 Position
    { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Building(BuildingType type, int cost, ResourceType? resourceType, Vector3 position)
    {
        Type = type;
        Cost = cost;
        ResourceType = resourceType;
        Position = position;
    }

    public bool CanAfford()
    {
        return true;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
