using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[System.Serializable]
public class GetDashboardStats
{
    public DashboardDataStats data; 


}
[Serializable]
public class DashboardDataStats
{
    public int cleanedWithinCompliance;
    public int cleanedAfterCompliance;
    public int yetToBeCleanedWithinCompliance;
    public int yetToBeCleanedAfterCompliance;
    public int online;
    public int dataMissingSince6Hours;
    public int dataMissingSince24Hours;
    public int sufficientBatteryLevel;
    public int warningBatteryLevel;
    public int criticalBatteryLevel;
    public int allSensorConnected;
    public int oneSensorDisconnected;
    public int moreThan1SensorDisconnected;
}
[Serializable]
public class BatteryData
{
    public int currentBatteryLevel;
    public string assetType;
    public string assetName;
    public string type;
    public string tid;
}
[System.Serializable]
public class GetDashboardBatteryStats
{
    public int pageIndex;
    public int pageSize;
    public int totalCount;
    public bool hasNext;
    public bool hasPrevious;
    public List<BatteryData> data;
}

[Serializable]
public class CleaningData
{
    public object actualCleaningDate;
    public object dueCleaningDate;
    public object cleaningRequestedDate;
    public string cleaningRequestReason;
    public string assetType;
    public string assetName;
    public string type;
    public string tid;
}

[Serializable]
public class GetDashboardCleaningStats
{
    public int pageIndex;
    public int pageSize;
    public int totalCount;
    public bool hasNext;
    public bool hasPrevious;
    public List<CleaningData> data;
}

[Serializable]
public class ConnectivityStatsData
{
    public object lastSync;
    public int dataMissingSinceInSecs;
    public string assetType;
    public string assetName;
    public string type;
    public string tid;
}
[Serializable]
public class GetConnectivityStats
{
    public int pageIndex;
    public int pageSize;
    public int totalCount;
    public bool hasNext;
    public bool hasPrevious;
    public List<ConnectivityStatsData> data;
}
[Serializable]
public class SensorStatsdata
{
    public string status;
    public long sensorDisconnectedSince;
    public int sensorDisconnectedSinceInSecs;
    public string assetType;
    public string assetName;
    public string type;
    public string tid;
}
[Serializable]
public class GetSensorStats
{
    public int pageIndex;
    public int pageSize;
    public int totalCount;
    public bool hasNext;
    public bool hasPrevious;
    public List<SensorStatsdata> data;
}







