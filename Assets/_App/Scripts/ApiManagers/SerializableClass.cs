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
    public bool is_cro;
    public object cro_ts;
    public string cro_rsn;
    public object etc_ts;
}
[Serializable]
public class CleaningState
{
    public bool is_tc;
    public object ltc_ts;
    public bool is_tcwcp;
    public bool is_tcacpn;
    public bool is_stc;
    public object lstc_ts;
    public bool is_stcwcp;
    public object nsn_ts;
    public object nsc_ts;
}
[Serializable]
public class ConnectivityState
{
    public bool is_off;
    public bool is_dm;
    public object dm_ts;
    public object dmd_ts;
    public long off_ts;
    public long offd_ts;
}
[Serializable]
public class TankData
{
    public string tid;
    public string lid;
    public LatestMeasuredTelemetry latestMeasuredTelemetry;
    public LatestCloudDerivedTelemetry latestCloudDerivedTelemetry;
    public object lastTelemetryTime;
    public LatestState latestState;
    public object lastStateUpdatedTime;
    public LatestKpiInfo latestKpiInfo;
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
    public long fvc;
    public long wwvc;
    public double cs;
    public int tcc;
    public int tcwcc;
    public int tcacc;
}
[Serializable]
public class LatestMeasuredTelemetry
{
    public List<int> bsrsp1;
    public List<object> bsrsp2;
    public List<object> bsrsp3;
    public double cv;
    public int bl;
    public bool s1;
    public bool sri;
    public int bsr1;
    public bool s2;
}
[Serializable]
public class LatestState
{
    public ConnectivityState connectivityState;
    public CleaningState cleaningState;
    public CleaningRequestState cleaningRequestState;
    public SensorState sensorState;
}
[Serializable]
public class GetTankStats
{
    public int pageIndex;
    public int pageSize;
    public int totalCount;
    public bool hasNext;
    public bool hasPrevious;
    public List<TankData> data;
}
[Serializable]
public class SensorState
{
    public object s1d_ts;
    public long s2d_ts;
    public long s3d_ts;
}
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
[Serializable]
public class GetComplianceScore
{
    public List<Data> data;
    public YearlySummaryData yearlySummaryData;
    public TotalData totalData;
}
[Serializable]
public class Data
{
    public int totalTanks ;
    public int monthNo ;
    public string monthName ;
    public double complianceScore ;
    public int wasteWaterVolumeCollected ;
}
[Serializable]
public class TotalData
{
    public int totalTanks ;
    public double complianceScore ;
    public long wasteWaterVolumeCollected ;
}
[Serializable]
public class YearlySummaryData
{
    public int totalTanks ;
    public double complianceScore ;
    public long wasteWaterVolumeCollected ;
}

[Serializable]
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class CleaningServiceCompanyInfo
{
    public string companyName ;
    public List<CompanyUser> companyUsers ;
}
[Serializable]
public class CompanyUser
{
    public string rid ;
    public string contactPersonFirstName ;
    public string contactPersonLastName ;
    public object contactPersonRole ;
    public string contactPersonEmail ;
    public string contactPersonPhoneNo ;
    public string contactPersonWhatsappNo ;
}
[Serializable]
public class EnrolledTankData
{
    public LogicalAssetInfo logicalAssetInfo ;
    public TankInfo tankInfo ;
    public object tankConfigurationInfo ;
    public GatewayInfo gatewayInfo ;
    public SimInfo simInfo ;
    public SensorUnitInfo sensorUnitInfo ;
    public CleaningServiceCompanyInfo cleaningServiceCompanyInfo ;
    public string remarks ;
    public string ioTHubId ;
    public string installedBy ;
    public object installationDate ;
    public string createdBy ;
    public string updatedBy ;
    public object tankLastCleanDetails ;
    public object tankPhysicalObservations ;
}
[Serializable]
public class ExternalUser
{
    public string rid ;
    public string role ;
    public string firstName ;
    public string lastName ;
    public string email ;
    public string phoneNo ;
    public string whatsappNo ;
}
[Serializable]
public class GatewayInfo
{
    public string type ;
    public string make ;
    public string model ;
    public string uid ;
    public string serialNumber ;
    public string firmwareVersion ;
}
[Serializable]
public class Hierarchy
{
    public string district ;
    public string area ;
    public string city ;
}
[Serializable]
public class LogicalAssetInfo
{
    public string id ;
    public string name ;
    public string assetType ;
    public string latitude ;
    public string longitude ;
    public string address ;
    public string postalCode ;
    public Hierarchy hierarchy ;
    public List<ExternalUser> externalUsers ;
}
[Serializable]
public class GetEnrolledTankStats
{
    public int pageIndex ;
    public int pageSize ;
    public int totalCount ;
    public bool hasNext ;
    public bool hasPrevious ;
    public List<EnrolledTankData> data ;
}
[Serializable]
public class SensorUnitInfo
{
    public string serialNumber ;
    public object sensorUnitType ;
}
[Serializable]
public class SimInfo
{
    public string type ;
    public string iccid ;
    public string msisdn ;
    public string imsi ;
    public string signalQuality ;
}
[Serializable]
public class TankInfo
{
    public string id ;
    public string tid ;
    public string type ;
    public string make ;
    public string model ;
    public object madeOf ;
    public int basketQty ;
    public string location ;
    public string position ;
    public string locationLat ;
    public string locationLong ;
    public int lengthInMm ;
    public int widthInMm ;
    public int depthInMm ;
    public int airLayerReferenceValue ;
    public int? totalBottomSensors ;
    public int? outletPipeDiameterInMm ;
    public int? maxBottomSensor1Reading ;
    public int? maxBottomSensor2Reading ;
    public int? maxBottomSensor3Reading ;
    public int? bottomSensor1Offset ;
    public int? bottomSensor2Offset ;
    public int? bottomSensor3Offset ;
    public int? bottomSensor1ToOutletBottom ;
    public int? bottomSensor2ToOutletBottom ;
    public int? bottomSensor3ToOutletBottom ;
    public int? bottomSensor1ToOutletMiddle ;
    public int? bottomSensor2ToOutletMiddle ;
    public int? bottomSensor3ToOutletMiddle ;
    public int? bottomSensor1ToOutletTop ;
    public int? bottomSensor2ToOutletTop ;
    public int? bottomSensor3ToOutletTop ;
}






