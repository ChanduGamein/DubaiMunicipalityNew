using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DistrictssData", menuName = "ScriptableObjects/Districts", order = 2)]

public class DistrictSO : ScriptableObject
{
    public string name;
    public enum DistrictType { BurDubai, Downtown, BusinessBay, AlBastakiya, AlKarama, DubaiMarina, Sector8, Sector9 }
    public DistrictType district;
}
