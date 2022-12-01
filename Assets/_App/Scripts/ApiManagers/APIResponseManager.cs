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

