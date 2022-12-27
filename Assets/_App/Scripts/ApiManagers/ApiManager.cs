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
    string Root_URL_getHierarchywisemonthlyComplianceScore = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/hierarchywisemonthlycompliancescore?";
    string Root_URL_gettanksmonthlycompliancescore = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/tanksmonthlycompliancescore?pageSize=1000000&year=2022";
    string Root_URL_tanksyearlycompliancescore = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/tanksyearlycompliancescore?pageSize=1000000&year=2022";

    string Root_URL_getTelemetryHistory = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/telemetry?pageSize=1000000&lastNSec=86400";
    string Root_URL_getAlarms = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/alarms?pageSize=50&lastNSec=8640000";

   [SerializeField] GameObject DistrictNameObject, SubDistrictNameObject;
    public Text month, year;
    const string LBrace = "{", RBrace = "}", Colon = ": ", City = "\"city\"", Dubai="\"Dubai\"",comma=",",district="\"district\"",area="\"area\"", quote = "\"";
    const string hierarchy = "?hierarchy=";
    string BaseDubai;
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
        month.text = DateTime.Now.Month.ToString();
        year.text = DateTime.Now.Year.ToString();

         BaseDubai = LBrace + City + Colon + Dubai + comma + district + Colon;
        //BaseDubai += quote+ DistrictNameObject.name + quote+ RBrace;
        //Debug.Log(BaseDubai);
        //Debug.Log(Uri.EscapeDataString(BaseDubai));
        //Root_URL_getDashboardStats += Uri.EscapeDataString(BaseDubai);

        DashboardStatsForSubDistricts(DistrictNameObject.name, SubDistrictNameObject.name);


    }

    public void SelectDate()
    {
        Debug.Log(year.text);
        // give required year here
        Root_URL_getHierarchywisemonthlyComplianceScore = Root_URL_getHierarchywisemonthlyComplianceScore + "?year="+ year.text ;
    }
    // this is for complete Dubai data
    public void DashboardStats()
    {
        Debug.Log("DashboardStats");
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


        // Updating the UI
        APIDataGet.instance.SetApiDataToUI();
    }
    public void DashboardStatsForDistricts(string DistrictName)
    {
        string url;
        url= BaseDubai + quote + DistrictName + quote + RBrace;
        Debug.Log(url);
        Debug.Log(Uri.EscapeDataString(url));
        APIRequest(Root_URL_getDashboardStats + hierarchy + Uri.EscapeDataString(url), AccessToken, null, OnGetDashboardStatsSuccess, true, false);

    }
    public void DashboardStatsForSubDistricts(string DistrictName,string SubDistrictName)
    {
        string url;
        url = BaseDubai + quote + DistrictName + quote + comma + area+ Colon + quote + SubDistrictName + quote + RBrace;
        Debug.Log(url);
        Debug.Log(Uri.EscapeDataString(url));
        APIRequest(Root_URL_getDashboardStats+ hierarchy+ Uri.EscapeDataString(url), AccessToken, null, OnGetDashboardStatsSuccess, true, false);

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
