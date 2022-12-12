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
[Serializable]
public class CleaningRequestState
{
    public string cro_id;
    public bool is_cro;
    public int cro_ts;
    public string cro_rsn;
    public int etc_ts;
}
[Serializable]
public class CleaningState
{
    public bool is_tc;
    public int ltc_ts;
    public bool is_tcwcp;
    public bool is_tcacpn;
    public bool is_stc;
    public int lstc_ts;
    public bool is_stcwcp;
    public int nsn_ts;
    public int nsc_ts;
}
[Serializable]
public class ConnectivityState
{
    public bool is_off;
    public int off_ts;
    public int offd_ts;
    public bool is_dm;
    public int dm_ts;
    public int dmd_ts;
}
[Serializable]
public class Data
{
    public string tid;
    public string lid;
    public LatestMeasuredTelemetry latestMeasuredTelemetry;
    public LatestCloudDerivedTelemetry latestCloudDerivedTelemetry;
    public long lastTelemetryTime;
    public LatestState latestState;
    public int lastStateUpdatedTime;
    public LatestKpiInfo latestKpiInfo;
}
[Serializable]
public class InvalidReadingState
{
    public bool is_ir;
    public int ir_ts;
}
[Serializable]
public class LatestCloudDerivedTelemetry
{
    public int bl;
    public int tv;
    public int fv;
    public int wwv;
    public int wwl;
    public int bl_corr;
    public int bsr1;
    public int ft;
    public int bsr;
}
[Serializable]
public class LatestKpiInfo
{
    public int fvc;
    public int wwvc;
    public int cs;
    public int tcc;
    public int tcwcc;
    public int tcacc;
}
[Serializable]
public class LatestMeasuredTelemetry
{
    public int bsr1;
    public double cv;
    public int bl;
    public bool s1;
    public bool sri;
}
[Serializable]

public class LatestState
{
    public ConnectivityState connectivityState;
    public CleaningState cleaningState;
    public CleaningRequestState cleaningRequestState;
    public SensorState sensorState;
    public OverflowState overflowState;
    public InvalidReadingState invalidReadingState;
}
[Serializable]
public class OverflowState
{
    public bool is_tof;
    public int tof_ts;
}
[Serializable]
public class Root
{
    public Data data;
}
[Serializable]
public class SensorState
{
    public int s1d_ts;
    public int s2d_ts;
    public int s3d_ts;
    public string s1d_rsn;
    public string s2d_rsn;
    public string s3d_rsn;
}








