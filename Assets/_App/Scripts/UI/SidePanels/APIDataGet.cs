using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CleaningStatsVariables
{
    #region Cleaning Stats Variables
    public Text tanksCleanedWithinComplaince;
    public Text cleanedAfterComplaince;
    public Text yetToBeCleaned;
    public Text failedToClean;
    public Text totalNumber;
    public Text totalCleaned;
    public Text totalNotCleaned;
    #endregion
}
[System.Serializable]
public class BuildingCanvasVariables
{
    #region Building Stats Variables
    public Text fogThickness;
    public Text fogVolume;
    public Text wasteWaterLevel;
    public Text wasteWaterVolume;
    public Text batteryLevel;
    public Text lastCleanedOn;
    public Text nextCleanedOn;
    public Text coverageState;
    #endregion
}
[System.Serializable]
public class WasteWaterStatsVariables
{
    #region Waste Water Stats Variables
    public List<Image> WasteWaterHistograms;
    #endregion
}
public class APIDataGet : MonoBehaviour
{
    public static APIDataGet instance = null;

    public CleaningStatsVariables cleaningStatsVariables;
    public BuildingCanvasVariables buildingCanvasVariables;
    public WasteWaterStatsVariables wasteWaterStatsVariables;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject.GetComponent(instance.GetType()));
        DontDestroyOnLoad(gameObject);
    }
    public void SetApiDataToUI()
    {
        SetCleaningStatesToUI();
        SetBuildingStatsToUI();
    }
    public void SetCleaningStatesToUI()
    {
        cleaningStatsVariables.tanksCleanedWithinComplaince.text = APIResponseManager.instance.getDashboardStats.data.cleanedWithinCompliance.ToString();
        cleaningStatsVariables.cleanedAfterComplaince.text = APIResponseManager.instance.getDashboardStats.data.cleanedAfterCompliance.ToString();
        cleaningStatsVariables.yetToBeCleaned.text = APIResponseManager.instance.getDashboardStats.data.yetToBeCleanedWithinCompliance.ToString();
        cleaningStatsVariables.failedToClean.text = APIResponseManager.instance.getDashboardStats.data.yetToBeCleanedAfterCompliance.ToString();

        var _totalCleanedCount = (APIResponseManager.instance.getDashboardStats.data.cleanedWithinCompliance + APIResponseManager.instance.getDashboardStats.data.cleanedAfterCompliance);
        cleaningStatsVariables.totalCleaned.text = _totalCleanedCount.ToString();

        var _totalNotCleanedCount = (APIResponseManager.instance.getDashboardStats.data.yetToBeCleanedWithinCompliance + APIResponseManager.instance.getDashboardStats.data.yetToBeCleanedAfterCompliance);
        cleaningStatsVariables.totalNotCleaned.text = _totalNotCleanedCount.ToString();

        var _totalNumberCount = _totalCleanedCount + _totalNotCleanedCount;
        cleaningStatsVariables.totalNumber.text = _totalNumberCount.ToString();
    }
    public void SetBuildingStatsToUI()
    {
        if (APIResponseManager.instance.getTankStats != null)
        {
            buildingCanvasVariables.fogThickness.text = APIResponseManager.instance.getTankStats.data[0].latestCloudDerivedTelemetry.ft.ToString();
            buildingCanvasVariables.fogVolume.text = APIResponseManager.instance.getTankStats.data[0].latestCloudDerivedTelemetry.fv.ToString();
            buildingCanvasVariables.wasteWaterLevel.text = APIResponseManager.instance.getTankStats.data[0].latestCloudDerivedTelemetry.wwl.ToString();
            buildingCanvasVariables.wasteWaterVolume.text = APIResponseManager.instance.getTankStats.data[0].latestCloudDerivedTelemetry.wwv.ToString();
            buildingCanvasVariables.batteryLevel.text = APIResponseManager.instance.getTankStats.data[0].latestCloudDerivedTelemetry.bl.ToString();
            buildingCanvasVariables.lastCleanedOn.text = APIResponseManager.instance.getTankStats.data[0].latestState.cleaningState.ltc_ts.ToString();
            buildingCanvasVariables.nextCleanedOn.text = APIResponseManager.instance.getTankStats.data[0].latestState.cleaningState.nsn_ts.ToString();
            buildingCanvasVariables.coverageState.text = APIResponseManager.instance.getTankStats.data[0].latestMeasuredTelemetry.cv.ToString();
        }

    }
    public void SetWasteWaterDateToUI()
    {
        if (APIResponseManager.instance.getComplianceScoreStats != null)
        {
            for (int i = 0; i < wasteWaterStatsVariables.WasteWaterHistograms.Count; i++)
            {
                wasteWaterStatsVariables.WasteWaterHistograms[i].fillAmount = APIResponseManager.instance.getComplianceScoreStats.data[i].wasteWaterVolumeCollected;
            }
        }
    }
}
