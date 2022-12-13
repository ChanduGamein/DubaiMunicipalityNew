using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using TMPro;


public class ApiManager : MonoBehaviour
{
    public static ApiManager instance = null;
    public APIResponseManager responseHandler;
    string AccessToken = "ca2f61e1-467c-444a-9d67-031b9f1460d3";

    // public static string BASE_URL = "https://rabbithole-apim-dev.azure-api.net/core/";
    string urlRequest = "https://50.17.146.144/api/user/detail/save";
    string Root_URL_getDashboardStats = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/dashboardstatistics";
    string Root_URL_getBatteryStats = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/dashboardstatistics/batterystatistics";
    string Root_URL_getCleaningStats = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/dashboardstatistics/cleaningstatistics";
    string Root_URL_getConnectivityStats = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/dashboardstatistics/connectivitystatistics";
    string Root_URL_getSensorStats = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/dashboardstatistics/sensorstatistics";
    string Root_URL_getTankStats = "https://stgapigw-vip.dm.ae/gateway/DMAPIMIntegration/fogtrap/tanksstate";


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


    }


    public void DashboardStats()
    {
        Debug.Log("DashboardStats");
        APIRequest(Root_URL_getDashboardStats, AccessToken, null, OnGetDashboardStatsSuccess, true, false);
        APIRequest(Root_URL_getBatteryStats, AccessToken, null, onGetBatteryStatsSuccess, true, false);
        APIRequest(Root_URL_getCleaningStats, AccessToken, null, onGetCleaningStatsSuccess, true, false);
        APIRequest(Root_URL_getConnectivityStats, AccessToken, null, onGetConnectivityStatsSuccess, true, false);
        APIRequest(Root_URL_getSensorStats, AccessToken, null, onGetSensorStatsSuccess, true, false);
        APIRequest(Root_URL_getTankStats, AccessToken, null, onGetTankStatsSuccess, true, false);
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
