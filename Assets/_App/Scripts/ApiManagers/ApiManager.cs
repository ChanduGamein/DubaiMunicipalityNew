using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using TMPro;
using UnityEngine.UI;
using System.Web;

public class ApiManager : MonoBehaviour
{
    public static ApiManager instance = null;
    public APIResponseManager responseHandler;
    string AccessToken = "ca2f61e1-467c-444a-9d67-031b9f1460d3";

    // public static string BASE_URL = "https://rabbithole-apim-dev.azure-api.net/core/";
    string urlRequest = "https://50.17.146.144/api/user/detail/save";
    string Root_URL_getDashboardStats = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/dashboardstatistics";
    string Root_URL_getBatteryStats = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/dashboardstatistics/batterystatistics?pageSize=1000000";
    string Root_URL_getCleaningStats = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/dashboardstatistics/cleaningstatistics?pageSize=1000000";
    string Root_URL_getConnectivityStats = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/dashboardstatistics/connectivitystatistics?pageSize=1000000";
    string Root_URL_getSensorStats = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/dashboardstatistics/sensorstatistics?pageSize=1000000";
    string Root_URL_getTankStats = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/tanksstate?pageSize=1000000";
    string Root_URL_getEnrolledTankStats = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/enrolledtanks?pageSize=1000000";
    string Root_URL_getTankConfigurations = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/tankconfigurations?pageSize=1000000";
    string Root_URL_getHierarchywisemonthlyComplianceScore = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/hierarchywisemonthlycompliancescore";
    string Root_URL_gettanksmonthlycompliancescore = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/tanksmonthlycompliancescore";
    string Root_URL_tanksyearlycompliancescore = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/tanksyearlycompliancescore?pageSize=1000000";

    string Root_URL_getTelemetryHistory = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/telemetry?pageSize=1000000&lastNSec=86400";
    string Root_URL_getAlarms = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/alarms";

   [SerializeField] GameObject DistrictNameObject, SubDistrictNameObject;
    public Text month, year;
    const string LBrace = "{", RBrace = "}", Colon = ": ", City = "\"city\"", Dubai="\"Dubai\"",comma=",",district="\"district\"",area="\"area\"", quote = "\"";
    const string hierarchy = "hierarchy=";
    const string pageIndex = "pageIndex=";
    const string pageSize = "pageSize=";
    const string fromDate = "fromDate=";
    const string toDate = "toDate=";
    const string lastNSec = "lastNSec=";
    const string tankId = "tankId=";
    const string lid = "lid=";
    const string year2 = "year=";
    const string monthno = "monthno=";
    const string lastnmonths = "lastnmonths=";
    const string assettype = "assettype=";
    const string search = "search=";
    int SelectedYear=2022, SelectedMonth=12,SelectedDate=1,LastNseconds=8640000;
    int SelectedYear_From = 2022, SelectedMonth_From = 12, SelectedDate_From = 1;
    int SelectedYear_To = 2022, SelectedMonth_To = 12, SelectedDate_To = 1;
    string OnlyDubai,BaseDubai;
    long FromdateEpoch, ToDateEpoch;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject.GetComponent(instance.GetType()));
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        //month.text = DateTime.Now.Month.ToString();
        //year.text = DateTime.Now.Year.ToString();
        OnlyDubai = LBrace + City + Colon + Dubai+ RBrace;
        BaseDubai = LBrace + City + Colon + Dubai + comma + district + Colon;
        Debug.Log(OnlyDubai);

        //BaseDubai += quote+ DistrictNameObject.name + quote+ RBrace;
        //Debug.Log(BaseDubai);
        //Debug.Log(Uri.EscapeDataString(BaseDubai));
        //Root_URL_getDashboardStats += Uri.EscapeDataString(BaseDubai);


        //Debug.Log(DateTimeOffset.Now.ToUnixTimeSeconds());
        //var epoch = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        //Debug.Log((int)epoch);

        // Debug.Log(DateTime.UtcNow); 

        //DateTimeOffset dateTimeOffSet = DateTimeOffset.ParseExact("14/03/2014 00:00:00", "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-US"));
        //long date = dateTimeOffSet.ToUnixTimeSeconds();
        //Debug.Log(date);

        //DateTimeOffset dateTimeOffSet2 = DateTimeOffset.FromUnixTimeMilliseconds(1581984000000);
        //DateTime dateTime = dateTimeOffSet2.DateTime;
        //Debug.Log(dateTime);

        DashboardStatsForSubDistricts(DistrictNameObject.name, SubDistrictNameObject.name);
    }

    public void SelectDate()
    {
        Debug.Log(year.text);
        // give required year here
       // Root_URL_getHierarchywisemonthlyComplianceScore = Root_URL_getHierarchywisemonthlyComplianceScore + "?year="+ year.text ;
    }
    void FromDate()
    {
        DateTimeOffset dateTimeOffSet = DateTimeOffset.ParseExact(SelectedYear_From.ToString()+"/"+ SelectedMonth_From.ToString() + "/" + SelectedDate_From.ToString() + "00:00:00", "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-US"));
        FromdateEpoch = dateTimeOffSet.ToUnixTimeSeconds();
    }
    void ToDate()
    {
        DateTimeOffset dateTimeOffSet = DateTimeOffset.ParseExact(SelectedYear_To.ToString()+"/"+ SelectedMonth_To.ToString() + "/" + SelectedDate_To.ToString() + "00:00:00", "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-US"));
        ToDateEpoch = dateTimeOffSet.ToUnixTimeSeconds();
    }
    public int GetLastNseconds()
    {
        
        return (int)(ToDateEpoch- FromdateEpoch);
    }
    // this is for complete Dubai data
    public void DashboardStats()
    {

        string url = OnlyDubai;
        Debug.Log("DashboardStats");
        /*
        APIRequest(Root_URL_getDashboardStats, AccessToken, null, OnGetDashboardStatsSuccess, true, false);
        APIRequest(Root_URL_getBatteryStats, AccessToken, null, onGetBatteryStatsSuccess, true, false);
        APIRequest(Root_URL_getCleaningStats, AccessToken, null, onGetCleaningStatsSuccess, true, false);
        APIRequest(Root_URL_getConnectivityStats, AccessToken, null, onGetConnectivityStatsSuccess, true, false);
        APIRequest(Root_URL_getSensorStats, AccessToken, null, onGetSensorStatsSuccess, true, false);
        APIRequest(Root_URL_getTankStats, AccessToken, null, onGetTankStatsSuccess, true, false);
        APIRequest(Root_URL_getHierarchywisemonthlyComplianceScore, AccessToken, null, onGetComplianceScoreStatsSuccess, true, false);
        APIRequest(Root_URL_gettanksmonthlycompliancescore, AccessToken, null, onGetMonthlyComplianceScoreStatsSuccess, true, false);
        APIRequest(Root_URL_tanksyearlycompliancescore, AccessToken, null, onGetYearlyComplianceScoreStatsSuccess, true, false);

        APIRequest(Root_URL_getEnrolledTankStats, AccessToken, null, onGetEnrolledTankStatsSuccess, true, false);
        APIRequest(Root_URL_getTankConfigurations, AccessToken, null, onGetTankConfigurationSuccess, true, false);

        APIRequest(Root_URL_getTelemetryHistory, AccessToken, null, onGetTelemetryHistory, true, false);

        APIRequest(Root_URL_getAlarms, AccessToken, null, onGetAlarmsData, true, false);

        */
        APIRequest(Root_URL_getDashboardStats + "?" + hierarchy + Uri.EscapeDataString(url), AccessToken, null, OnGetDashboardStatsSuccess, true, false);
        APIRequest(Root_URL_getBatteryStats + "&" + hierarchy + Uri.EscapeDataString(url), AccessToken, null, onGetBatteryStatsSuccess, true, false);
        APIRequest(Root_URL_getCleaningStats + "&" + hierarchy + Uri.EscapeDataString(url), AccessToken , null, onGetCleaningStatsSuccess, true, false);
        APIRequest(Root_URL_getConnectivityStats + "&" + hierarchy + Uri.EscapeDataString(url), AccessToken, null, onGetConnectivityStatsSuccess, true, false);
        APIRequest(Root_URL_getSensorStats + "&" + hierarchy + Uri.EscapeDataString(url), AccessToken, null, onGetSensorStatsSuccess, true, false);
        APIRequest(Root_URL_getTankStats + "&" + hierarchy + Uri.EscapeDataString(url), AccessToken, null, onGetTankStatsSuccess, true, false);
        APIRequest(Root_URL_getHierarchywisemonthlyComplianceScore + "?" + year2 + SelectedYear.ToString() + "&" + hierarchy + Uri.EscapeDataString(url) + "&" + monthno + SelectedMonth.ToString(), AccessToken, null, onGetComplianceScoreStatsSuccess, true, false);
        APIRequest(Root_URL_gettanksmonthlycompliancescore + "?" + year2 + SelectedYear.ToString() + "&" + hierarchy + Uri.EscapeDataString(url) + "&" + monthno + SelectedMonth.ToString(), AccessToken, null, onGetMonthlyComplianceScoreStatsSuccess, true, false);
        APIRequest(Root_URL_tanksyearlycompliancescore + "&" + hierarchy + Uri.EscapeDataString(url) + "&" + year2 + SelectedYear.ToString(), AccessToken, null, onGetYearlyComplianceScoreStatsSuccess, true, false);

        APIRequest(Root_URL_getEnrolledTankStats + "&" + hierarchy + Uri.EscapeDataString(url), AccessToken, null, onGetEnrolledTankStatsSuccess, true, false);
        APIRequest(Root_URL_getTankConfigurations + "&" + hierarchy + Uri.EscapeDataString(url), AccessToken, null, onGetTankConfigurationSuccess, true, false);

        //APIRequest(Root_URL_getTelemetryHistory + "?" + hierarchy + Uri.EscapeDataString(url), AccessToken, null, onGetTelemetryHistory, true, false);

        APIRequest(Root_URL_getAlarms + "?" + hierarchy + Uri.EscapeDataString(url) + "&" + pageSize + "10" + "&" + lastNSec + LastNseconds.ToString(), AccessToken, null, onGetAlarmsData, true, false);




        // Updating the UI
        APIDataGet.instance.SetApiDataToUI();
    }
    public void DashboardStatsForDistricts(string DistrictName)
    {
        string url;
        url= BaseDubai + quote + DistrictName + quote + RBrace;
       

        APIRequest(Root_URL_getDashboardStats +"?"+ hierarchy + Uri.EscapeDataString(url), AccessToken, null, OnGetDashboardStatsSuccess, true, false);
        APIRequest(Root_URL_getBatteryStats + "&" + hierarchy + Uri.EscapeDataString(url), AccessToken, null, onGetBatteryStatsSuccess, true, false);
        APIRequest(Root_URL_getCleaningStats + "&" + hierarchy + Uri.EscapeDataString(url), AccessToken , null, onGetCleaningStatsSuccess, true, false);
        APIRequest(Root_URL_getConnectivityStats + "&" + hierarchy + Uri.EscapeDataString(url), AccessToken, null, onGetConnectivityStatsSuccess, true, false);
        APIRequest(Root_URL_getSensorStats + "&" + hierarchy + Uri.EscapeDataString(url), AccessToken, null, onGetSensorStatsSuccess, true, false);
        APIRequest(Root_URL_getTankStats + "&" + hierarchy + Uri.EscapeDataString(url), AccessToken, null, onGetTankStatsSuccess, true, false);
        APIRequest(Root_URL_getHierarchywisemonthlyComplianceScore + "?" + year2 + SelectedYear.ToString() + "&" + hierarchy + Uri.EscapeDataString(url) + "&" + monthno + SelectedMonth.ToString(), AccessToken, null, onGetComplianceScoreStatsSuccess, true, false);
        APIRequest(Root_URL_gettanksmonthlycompliancescore + "?" + year2 + SelectedYear.ToString() + "&" + hierarchy + Uri.EscapeDataString(url) + "&" + monthno + SelectedMonth.ToString(), AccessToken, null, onGetMonthlyComplianceScoreStatsSuccess, true, false);
        APIRequest(Root_URL_tanksyearlycompliancescore + "&" + hierarchy + Uri.EscapeDataString(url) + "&" + year2 + SelectedYear.ToString(), AccessToken, null, onGetYearlyComplianceScoreStatsSuccess, true, false);

        APIRequest(Root_URL_getEnrolledTankStats + "&" + hierarchy + Uri.EscapeDataString(url), AccessToken, null, onGetEnrolledTankStatsSuccess, true, false);
        APIRequest(Root_URL_getTankConfigurations + "&" + hierarchy + Uri.EscapeDataString(url), AccessToken, null, onGetTankConfigurationSuccess, true, false);

        //APIRequest(Root_URL_getTelemetryHistory + "?" + hierarchy + Uri.EscapeDataString(url), AccessToken, null, onGetTelemetryHistory, true, false);
       
        APIRequest(Root_URL_getAlarms + "?" + hierarchy + Uri.EscapeDataString(url) + "&" + pageSize + "10" + "&" + lastNSec + LastNseconds.ToString(), AccessToken, null, onGetAlarmsData, true, false);

    }
    public void DashboardStatsForSubDistricts(string DistrictName,string SubDistrictName)
    {
        string url;
        url = BaseDubai + quote + DistrictName + quote + comma + area+ Colon + quote + SubDistrictName + quote + RBrace;
        
        APIRequest(Root_URL_getDashboardStats + "?" + hierarchy + Uri.EscapeDataString(url), AccessToken, null, OnGetDashboardStatsSuccess, true, false);
        APIRequest(Root_URL_getBatteryStats + "&" + hierarchy + Uri.EscapeDataString(url), AccessToken, null, onGetBatteryStatsSuccess, true, false);
        APIRequest(Root_URL_getCleaningStats + "&" + hierarchy + Uri.EscapeDataString(url), AccessToken , null, onGetCleaningStatsSuccess, true, false);
        APIRequest(Root_URL_getConnectivityStats + "&" + hierarchy + Uri.EscapeDataString(url), AccessToken, null, onGetConnectivityStatsSuccess, true, false);
        APIRequest(Root_URL_getSensorStats + "&" + hierarchy + Uri.EscapeDataString(url), AccessToken, null, onGetSensorStatsSuccess, true, false);
        APIRequest(Root_URL_getTankStats + "&" + hierarchy + Uri.EscapeDataString(url), AccessToken, null, onGetTankStatsSuccess, true, false);
        APIRequest(Root_URL_getHierarchywisemonthlyComplianceScore + "?" +year2+ SelectedYear.ToString() +"&"+hierarchy + Uri.EscapeDataString(url) + "&" + monthno +SelectedMonth.ToString(), AccessToken, null, onGetComplianceScoreStatsSuccess, true, false);
        APIRequest(Root_URL_gettanksmonthlycompliancescore + "?" +year2+ SelectedYear.ToString() +"&"+hierarchy + Uri.EscapeDataString(url) + "&" + monthno +SelectedMonth.ToString(), AccessToken, null, onGetMonthlyComplianceScoreStatsSuccess, true, false);
        APIRequest(Root_URL_tanksyearlycompliancescore + "&" + hierarchy + Uri.EscapeDataString(url)+"&"+year2 + SelectedYear.ToString(), AccessToken, null, onGetYearlyComplianceScoreStatsSuccess, true, false);

        APIRequest(Root_URL_getEnrolledTankStats + "&" + hierarchy + Uri.EscapeDataString(url), AccessToken, null, onGetEnrolledTankStatsSuccess, true, false);
        APIRequest(Root_URL_getTankConfigurations + "&" + hierarchy + Uri.EscapeDataString(url), AccessToken, null, onGetTankConfigurationSuccess, true, false);

        //APIRequest(Root_URL_getTelemetryHistory + "?" + hierarchy + Uri.EscapeDataString(url), AccessToken, null, onGetTelemetryHistory, true, false);

        APIRequest(Root_URL_getAlarms + "?" + hierarchy + Uri.EscapeDataString(url) + "&" +pageSize+"10"+ "&" + lastNSec+LastNseconds.ToString(), AccessToken, null, onGetAlarmsData, true, false);
        
    }
    void OnGetDashboardStatsSuccess(string response, string responseCode)
    {
        JsonUtility.FromJsonOverwrite(response, responseHandler.getDashboardStats);
        Debug.Log(response);
        if (responseCode == "200")
        {
            Debug.Log(responseHandler.getDashboardStats.data.cleanedAfterCompliance);
        }
    }
    void onGetBatteryStatsSuccess(string response, string responseCode)
    {
        JsonUtility.FromJsonOverwrite(response, responseHandler.getDashboardBatteryStats);
        Debug.Log(response);
        if (responseCode == "200")
        {
            Debug.Log(responseHandler.getDashboardBatteryStats.totalCount);
        }
    }
    void onGetCleaningStatsSuccess(string response, string responseCode)
    {
        JsonUtility.FromJsonOverwrite(response, responseHandler.getDashboardCleaningStats);
        Debug.Log(response);
        if (responseCode == "200")
        {
            Debug.Log(responseHandler.getDashboardCleaningStats.totalCount);
        }
    }
    void onGetConnectivityStatsSuccess(string response, string responseCode)
    {
        JsonUtility.FromJsonOverwrite(response, responseHandler.getConnectivityStats);
        Debug.Log(response);
        if (responseCode == "200")
        {
            Debug.Log(responseHandler.getConnectivityStats.totalCount);
        }
    }
    void onGetSensorStatsSuccess(string response, string responseCode)
    {
        JsonUtility.FromJsonOverwrite(response, responseHandler.getSensorStats);
        Debug.Log(response);
        if (responseCode == "200")
        {
            Debug.Log(responseHandler.getSensorStats.totalCount);
        }
    }
    void onGetTankStatsSuccess(string response, string responseCode)
    {
        JsonUtility.FromJsonOverwrite(response, responseHandler.getTankStats);
        Debug.Log(response);
        if (responseCode == "200")
        {

        }
    }
    void onGetComplianceScoreStatsSuccess(string response, string responseCode)
    {
        JsonUtility.FromJsonOverwrite(response, responseHandler.gethierarchywisemonthlycompliancescore);
        Debug.Log(response);
        if (responseCode == "200")
        {

        }
    } 
    
    void onGetMonthlyComplianceScoreStatsSuccess(string response, string responseCode)
    {
        JsonUtility.FromJsonOverwrite(response, responseHandler.gettanksmonthlycompliancescore);
        Debug.Log(response);
        if (responseCode == "200")
        {

        }
    }  
    
    void onGetYearlyComplianceScoreStatsSuccess(string response, string responseCode)
    {
        JsonUtility.FromJsonOverwrite(response, responseHandler.gettanksyearlycompliancescore);
        Debug.Log(response);
        if (responseCode == "200")
        {

        }
    }

    void onGetEnrolledTankStatsSuccess(string response, string responseCode)
    {
        JsonUtility.FromJsonOverwrite(response, responseHandler.getEnrolledTankStats);
        Debug.Log(response);
        if (responseCode == "200")
        {

        }
    } 
    void onGetTankConfigurationSuccess(string response, string responseCode)
    {
        JsonUtility.FromJsonOverwrite(response, responseHandler.getTankConfigurations);
        Debug.Log(response);
        if (responseCode == "200")
        {

        }
    }
    void onGetTelemetryHistory(string response, string responseCode)
    {
        JsonUtility.FromJsonOverwrite(response, responseHandler.getTelemetryHistory);
        Debug.Log(response);
        if (responseCode == "200")
        {

        }
    }
    void onGetAlarmsData(string response, string responseCode)
    {
        JsonUtility.FromJsonOverwrite(response, responseHandler.getAlarmsData);
        Debug.Log(response);
        if (responseCode == "200")
        {

        }
    }

    public static void APIRequest(string APIurl, string authToken, object formData, Action<string, string> OnSuccess, bool showLoading, bool islogin = false)
    {
        string url = APIurl;


        if (!IsConnectedToInternet())
        {
            HelperUtil.HideLoading();
            HelperUtil.ShowMessage(GameMessage.InternetNotWorking);
            return;
        }

        if (instance == null) return;

        instance.StartCoroutine(enumerator());
        IEnumerator enumerator()
        {
            if (formData != null)
            {
                if (showLoading)
                {
                    HelperUtil.ShowLoading();
                }
                using (UnityWebRequest request = UnityWebRequest.Post(url, JsonUtility.ToJson(formData)))
                {
                    if (authToken != null)
                    {
                        request.SetRequestHeader("x-Gateway-APIKey", authToken);
                        request.SetRequestHeader("Content-Type", "application/json");
                    }
                    byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(formData));
                    request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);


                    request.SetRequestHeader("Content-Type", "application/json");
                    yield return request.SendWebRequest();
                    while (!request.isDone) yield return null;

                    if (showLoading) HelperUtil.HideLoading();
                    if (islogin)
                    {
                        if (request.downloadHandler.text == "" || request.downloadHandler.text.Contains("DOCTYPE"))
                        {
                            HelperUtil.HideLoading();
                            HelperUtil.ShowMessage(GameMessage.something_went_wrong, new MessageActionData("OK", () =>
                            {
                                instance.StopAllCoroutines();
                            }));
                        }
                        else if (request.responseCode == 400) HelperUtil.ShowMessage(GameMessage.something_went_wrong, new MessageActionData("OK", () =>
                        {
                            instance.StopAllCoroutines();
                        }));

                        else OnSuccess(request.downloadHandler.text, request.responseCode.ToString());

                    }
                    else
                    {

                        Debug.Log(request.downloadHandler.text);
                        if (request.downloadHandler.text == "" || request.downloadHandler.text.Contains("DOCTYPE"))
                        {
                            HelperUtil.HideLoading();
                            HelperUtil.ShowMessage(GameMessage.something_went_wrong, new MessageActionData("OK", () =>
                            {
                            }));
                        }


                    }
                }
            }

            if (formData == null)
            {
                if (showLoading)
                {
                    HelperUtil.ShowLoading();
                }
                using (UnityWebRequest request = UnityWebRequest.Get(url))
                {
                    if (authToken != null)
                    {
                        request.SetRequestHeader("x-Gateway-APIKey", authToken);
                        request.SetRequestHeader("Accept", "application/json");
                        request.SetRequestHeader("Content-Type", "application/json");

                    }
                    yield return request.SendWebRequest();
                    if (showLoading) HelperUtil.HideLoading();
                    while (!request.isDone) yield return null;
                    //Debug.Log("Info_Log: " + url + " : RESPONSE: " + request.downloadHandler.text);
                    if (request.downloadHandler.text == "" || request.downloadHandler.text.Contains("DOCTYPE"))
                    {
                        HelperUtil.HideLoading();
                        HelperUtil.ShowMessage(GameMessage.something_went_wrong, new MessageActionData("OK", () =>
                        {

                        }));
                    }
                    else if (request.responseCode == 400) HelperUtil.ShowMessage(GameMessage.something_went_wrong);
                    else OnSuccess(request.downloadHandler.text, request.responseCode.ToString());
                }
            }
        }

        bool IsConnectedToInternet() => !(Application.internetReachability == NetworkReachability.NotReachable);
    }

    public static void APIRequest_Form(string APIurl, string authToken, WWWForm formData, Action<string, string> OnSuccess, bool showLoading)
    {
        string url = APIurl;


        instance.StartCoroutine(enumerator());
        IEnumerator enumerator()
        {
            if (formData != null)
            {
                if (showLoading)
                {
                    HelperUtil.ShowLoading();
                }
                using (UnityWebRequest request = UnityWebRequest.Post(url, formData))
                {
                    if (authToken != null)
                    {
                        request.SetRequestHeader("x-Gateway-APIKey", authToken);
                        //request.SetRequestHeader("Content-Type", "application/json");

                    }
                    yield return request.SendWebRequest();
                    while (!request.isDone) yield return null;
                    if (showLoading) HelperUtil.HideLoading();
                    Debug.Log("Info_Log: " + url);
                    if (request.responseCode.ToString() == "400")
                    {
                        Debug.Log(request.downloadHandler.text);
                        HelperUtil.ShowMessage(GameMessage.something_went_wrong);
                        yield return null;
                    }
                    //Code to disable the raycast mask.
                    if (request.responseCode == 400) HelperUtil.ShowMessage(GameMessage.something_went_wrong);
                    else OnSuccess(request.downloadHandler.text, request.responseCode.ToString());
                }
            }
        }
    }

    public static void APIRequest_Put(string APIurl, string authToken, object formData, Action<string, string> OnSuccess, bool showLoading, bool otherCheck = false, bool islogin = false)
    {
        string url = APIurl;


        if (instance == null) return;

        instance.StartCoroutine(enumerator());
        IEnumerator enumerator()
        {
            if (formData != null)
            {
                if (showLoading)
                {
                    HelperUtil.ShowLoading();
                }
                using (UnityWebRequest request = UnityWebRequest.Put(url, JsonUtility.ToJson(formData)))
                {
                    if (authToken != null)
                    {
                        request.SetRequestHeader("x-Gateway-APIKey", authToken);
                    }
                    byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(formData));
                    request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);


                    request.SetRequestHeader("Content-Type", "application/json");
                    yield return request.SendWebRequest();
                    while (!request.isDone) yield return null;

                    if (showLoading) HelperUtil.HideLoading();
                    if (islogin)
                    {
                        if (request.downloadHandler.text == "" || request.downloadHandler.text.Contains("DOCTYPE"))
                        {
                            HelperUtil.HideLoading();
                            HelperUtil.ShowMessage(GameMessage.something_went_wrong, new MessageActionData("OK", () =>
                            {

                                instance.StopAllCoroutines();
                            }));
                        }
                        else if (request.responseCode == 400) HelperUtil.ShowMessage(GameMessage.something_went_wrong, new MessageActionData("OK", () =>
                        {
                            instance.StopAllCoroutines();

                        }));

                        else OnSuccess(request.downloadHandler.text, request.responseCode.ToString());

                    }
                    else
                    {
                        if (request.downloadHandler.text == "" || request.downloadHandler.text.Contains("DOCTYPE"))
                        {
                            HelperUtil.HideLoading();
                            HelperUtil.ShowMessage(GameMessage.something_went_wrong, new MessageActionData("OK", () =>
                            {

                            }));
                        }
                        if (!otherCheck)
                        {
                            if (request.responseCode == 400) HelperUtil.ShowMessage(GameMessage.something_went_wrong);

                            else OnSuccess(request.downloadHandler.text, request.responseCode.ToString());
                        }
                        else
                        {
                            OnSuccess(request.downloadHandler.text, request.responseCode.ToString());
                        }

                    }
                }
            }

            if (formData == null)
            {
                if (showLoading)
                {
                    HelperUtil.ShowLoading();
                }
                using (UnityWebRequest request = UnityWebRequest.Put(url, JsonUtility.ToJson("")))
                {
                    if (authToken != null)
                    {
                        request.SetRequestHeader("x-Gateway-APIKey", authToken);
                        request.SetRequestHeader("Accept", "application/json");
                        request.SetRequestHeader("Content-Type", "application/json");

                    }
                    yield return request.SendWebRequest();
                    if (showLoading) HelperUtil.HideLoading();
                    while (!request.isDone) yield return null;
                    //Debug.Log("Info_Log: " + url + " : RESPONSE: " + request.downloadHandler.text);
                    if (request.downloadHandler.text == "" || request.downloadHandler.text.Contains("DOCTYPE"))
                    {
                        HelperUtil.HideLoading();
                        HelperUtil.ShowMessage(GameMessage.something_went_wrong, new MessageActionData("OK", () =>
                        {

                        }));
                    }
                    else if (request.responseCode == 400) HelperUtil.ShowMessage(GameMessage.something_went_wrong);
                    else OnSuccess(request.downloadHandler.text, request.responseCode.ToString());
                }
            }
        }

    }


}
