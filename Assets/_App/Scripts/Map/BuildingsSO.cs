using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingsData", menuName = "ScriptableObjects/Building", order = 1)]

public class BuildingsSO : ScriptableObject
{
    public string name;
    public string buildingID;
    public string subDistrictName;
    public Material material;
    public float nCoordinate;
    public float eCoordinate;
    public enum DistrictType { BurDubai, Downtown, BusinessBay, AlBastakiya, AlKarama, DubaiMarina, Sector8, Sector9 }
    public enum BuildingType { Hotel, Restaurant, Cafeteria, Catering, Accommodation, ShishaJoints }
    public DistrictType district;
    public BuildingType buildingType;
}
