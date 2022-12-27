using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System.Text.RegularExpressions;
using System;

public class APIResponseManager : MonoBehaviour
{
    #region Variables

    [Header("API Response Holders")]
    public static APIResponseManager instance;

    public GetDashboardStats getDashboardStats;
    public GetDashboardBatteryStats getDashboardBatteryStats;
    public GetDashboardCleaningStats getDashboardCleaningStats;
    public GetConnectivityStats getConnectivityStats;
    public GetSensorStats getSensorStats;
    public GetTankStats getTankStats;
    public Gethierarchywisemonthlycompliancescore gethierarchywisemonthlycompliancescore;
    public Gettanksmonthlycompliancescore gettanksmonthlycompliancescore;
    public Gettanksyearlycompliancescore gettanksyearlycompliancescore;

    public GetEnrolledTankStats getEnrolledTankStats;
    public GetTankConfigurations getTankConfigurations;

    public GetTelemetryHistory getTelemetryHistory;
    public GetAlarmsData getAlarmsData;



    #endregion



    #region Unity CallBacks
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject.GetComponent(instance.GetType()));
            DontDestroyOnLoad(gameObject);
    }
    #endregion






}

