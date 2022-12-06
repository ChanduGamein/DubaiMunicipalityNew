using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class HelperUtil : MonoBehaviour
{
    #region Variables 

   
    public static HelperUtil instance { get; private set; }
    [SerializeField] GameObject messagePopupPrefab;
    [SerializeField] GameObject messagePopupPrefablandScap;
    private GameObject messagePopup;
    [SerializeField] GameObject loadingScreenPrefab;
    [SerializeField] GameObject downloadLoadingScreenPrefab;
    public GameObject loadingScreen;
    public GameObject downloadLoadingScreen;

    #region Interface Objects

    public IUniversalResponseHandler iUniversalResponseHandler;
   
    public IUniversalUIHandler iUniversalUIHandler;
    public IUniversalLandHandler iuniversalLandHandler;

    #endregion

    #endregion

    #region UnityCallBacks

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
	private void Update()
	{
        //fpscounter.text = (1 / Time.unscaledDeltaTime).ToString("0");
	}
	#endregion

	#region Timer 
	///set Timer for Two text
	public void CreateTimerTwoText(Text minutetext, Text secondtext, int TotalTimer, Action OnTimerEnd)
    {
        StartCoroutine(startTimer(minutetext, secondtext, TotalTimer, OnTimerEnd));
    }
    IEnumerator startTimer(Text minutetext, Text secondtext, float Totalseconds, Action OnTimerEnd)
    {
        do
        {
            float minutes = Mathf.FloorToInt(Totalseconds / 60);
            float seconds = Totalseconds % 60;
            string currentTime = string.Format("{00:00}{1:00}", minutes, seconds);
            minutetext.text = currentTime[0].ToString() + " : " + currentTime[1].ToString();
            secondtext.text = currentTime[2].ToString() + " : " + currentTime[3].ToString();
            Totalseconds--;
            yield return new WaitForSecondsRealtime(1f);
            if (Totalseconds == 0)
            {
                StopCoroutine(startTimer(minutetext, secondtext, Totalseconds, OnTimerEnd));
                OnTimerEnd();
            }
        } while (Totalseconds >= 0);

    }

    ///set Timer for single text
    public void CreateTimerSingleText(Text timerText, int TotalTimer, Action OnTimerEnd)
    {
        StartCoroutine(startTimersingleText(timerText, TotalTimer, OnTimerEnd));
    }
    IEnumerator startTimersingleText(Text timerText, int TotalTimer, Action OnTimerEnd)
    {
        do
        {
            float minutes = Mathf.FloorToInt(TotalTimer / 60);
            float seconds = TotalTimer % 60;
            string currentTime = string.Format("{00:00}{1:00}", minutes, seconds);
            timerText.text = currentTime[0].ToString() + " " + currentTime[1].ToString() + " : " + currentTime[2].ToString() + "  " + currentTime[3].ToString();
            TotalTimer--;
            yield return new WaitForSecondsRealtime(1f);
            if (TotalTimer == 0)
            {
                StopCoroutine(startTimersingleText(timerText, TotalTimer, OnTimerEnd));
                OnTimerEnd();
            }
        } while (TotalTimer >= 0);
    }

    public void StopTimer()
    {
        StopAllCoroutines();
    }

    #endregion

    #region CallBacksOfHelperUtil
    /// <summary>
    /// Helper method to start an Coroutine call.(action will be called after delay).
    /// </summary>
    public static void CallAfterDelay(Action action, float delay, Func<bool> cancelCondition = null)
    {
        float initialTime = Time.time;

        instance.StartCoroutine(enumerator());
        IEnumerator enumerator()
        {
            while (true)
            {
                //If cancel condition gets true, return control from this line.
                if (cancelCondition != null && cancelCondition()) yield break;
                //If delay is reached, break this loop.
                else if (Time.time > initialTime + delay) break;

                //Hold control for set amount of time to decrease CPU pressure.
                yield return new WaitForSeconds(0.02f);
            }

            //Execute the action if delay reached and cancel condition is still false.
            action();
        }
    }

    /// <summary>
    /// Helper method to start an Coroutine call.(action will be called after the condition gets true).
    /// </summary>
    public static void CallAfterCondition(Action action, Func<bool> condition, Func<bool> cancelCondition = null)
    {
        instance.StartCoroutine(enumerator());
        IEnumerator enumerator()
        {
            while (!condition())
            {
                //If cancel condition gets true, return control from this line.
                if (cancelCondition != null && cancelCondition()) yield break;

                yield return new WaitForSeconds(0.5f);
            }
            action();
        }
    }

    public static void ShowMessage(string message, MessageActionData firstAction = null, MessageActionData secondAction = null, bool withBackground = false, bool enableConvertor = false, bool isLandscape = false)
    {
        if (Screen.orientation == ScreenOrientation.Portrait)
        {
            if (enableConvertor) message = GameMessage.MessageConvertor(message);

            if (!instance.messagePopup)
            {
                if (!isLandscape)
                {
                    instance.messagePopup = Instantiate(instance.messagePopupPrefab);
                    DontDestroyOnLoad(instance.messagePopup);
                }
                else
                {
                    instance.messagePopup = Instantiate(instance.messagePopupPrefablandScap);
                    DontDestroyOnLoad(instance.messagePopupPrefablandScap);
                }
            }
            Transform messageTextParent = instance.messagePopup.transform.GetChild(0).GetChild(0);
            Transform buttonHolder = messageTextParent.transform.Find("ButtonHolder");

            //Set popup background visibility.
            Image messageBackground = instance.messagePopup.transform.GetChild(0).GetComponent<Image>();
            Color32 oldColor = messageBackground.color;
            messageBackground.color = new Color32(oldColor.r, oldColor.g, oldColor.b, withBackground ? (byte)255 : (byte)0);

            //Fetching the buttons and remove existing actions if any..
            Button actionButton = buttonHolder.transform.Find("Button_1").GetComponent<Button>();
            Button secondaryActionButton = buttonHolder.transform.Find("Button_2").GetComponent<Button>();
            actionButton.onClick.RemoveAllListeners();
            secondaryActionButton.onClick.RemoveAllListeners();

            if (secondAction != null)
            {
                secondaryActionButton.gameObject.SetActive(true);

                //Adding close action to both buttons.
                actionButton.onClick.AddListener(() => { instance.messagePopup.SetActive(false); });
                actionButton.onClick.AddListener(() =>
                {
                    //if (AvatarHolderManager.instance.avatar != null) {
                    //    AvatarHolderManager.instance.avatar.transform.SetParent(AvatarHolderManager.instance.transform);
                    //    AvatarHolderManager.instance.avatar.transform.localPosition = new Vector3(0,-.11f,5.36f);
                    //    AvatarHolderManager.instance.avatar.transform.localEulerAngles = new Vector3(0,180,0);
                    //    AvatarHolderManager.instance.avatar.transform.localScale = new Vector3(1,1,1);                     

                    //}
                });
                secondaryActionButton.onClick.AddListener(() => { instance.messagePopup.SetActive(false); });

                //Adding additional actions, if any.
                if (firstAction != null && firstAction.action != null) actionButton.onClick.AddListener(firstAction.action);
                if (secondAction != null && secondAction.action != null) secondaryActionButton.onClick.AddListener(secondAction.action);

                //Setting button names, if any.
                if (firstAction != null && !string.IsNullOrEmpty(firstAction.buttonName)) actionButton.GetComponentInChildren<Text>().text = firstAction.buttonName;
                if (secondAction != null && !string.IsNullOrEmpty(secondAction.buttonName)) secondaryActionButton.GetComponentInChildren<Text>().text = secondAction.buttonName;
            }
            else
            {
                secondaryActionButton.gameObject.SetActive(false);
                if (firstAction != null && firstAction.action != null) actionButton.onClick.AddListener(firstAction.action);
                actionButton.onClick.AddListener(() => { instance.messagePopup.SetActive(false); });
                if (firstAction != null) actionButton.GetComponentInChildren<Text>().text = firstAction.buttonName;
                else actionButton.GetComponentInChildren<Text>().text = "Ok";
            }

            //Setting the message.
            messageTextParent.Find("MessageText").GetComponent<Text>().text = message;
            instance.messagePopup.SetActive(true);
        }
        else
        {
            if (enableConvertor) message = GameMessage.MessageConvertor(message);

            if (!instance.messagePopup)
            {
                instance.messagePopup = Instantiate(instance.messagePopupPrefablandScap);
                DontDestroyOnLoad(instance.messagePopupPrefablandScap);
            }
            Transform messageTextParent = instance.messagePopup.transform.GetChild(0).GetChild(0);
            Transform buttonHolder = messageTextParent.transform.Find("ButtonHolder");

            //Set popup background visibility.
            Image messageBackground = instance.messagePopup.transform.GetChild(0).GetComponent<Image>();
            Color32 oldColor = messageBackground.color;
            messageBackground.color = new Color32(oldColor.r, oldColor.g, oldColor.b, withBackground ? (byte)255 : (byte)0);

            //Fetching the buttons and remove existing actions if any..
            Button actionButton = buttonHolder.transform.Find("Button_1").GetComponent<Button>();
            Button secondaryActionButton = buttonHolder.transform.Find("Button_2").GetComponent<Button>();
            actionButton.onClick.RemoveAllListeners();
            secondaryActionButton.onClick.RemoveAllListeners();

            if (secondAction != null)
            {
                secondaryActionButton.gameObject.SetActive(true);

                //Adding close action to both buttons.
                actionButton.onClick.AddListener(() => { instance.messagePopup.SetActive(false); });
                secondaryActionButton.onClick.AddListener(() => { instance.messagePopup.SetActive(false); });

                //Adding additional actions, if any.
                if (firstAction != null && firstAction.action != null) actionButton.onClick.AddListener(firstAction.action);
                if (secondAction != null && secondAction.action != null) secondaryActionButton.onClick.AddListener(secondAction.action);

                //Setting button names, if any.
                if (firstAction != null && !string.IsNullOrEmpty(firstAction.buttonName)) actionButton.GetComponentInChildren<Text>().text = firstAction.buttonName;
                if (secondAction != null && !string.IsNullOrEmpty(secondAction.buttonName)) secondaryActionButton.GetComponentInChildren<Text>().text = secondAction.buttonName;
            }
            else
            {
                secondaryActionButton.gameObject.SetActive(false);
                if (firstAction != null && firstAction.action != null) actionButton.onClick.AddListener(firstAction.action);
                actionButton.onClick.AddListener(() => { instance.messagePopup.SetActive(false); });
                if (firstAction != null) actionButton.GetComponentInChildren<Text>().text = firstAction.buttonName;
                else actionButton.GetComponentInChildren<Text>().text = "Ok";
            }

            //Setting the message.
            messageTextParent.Find("MessageText").GetComponent<Text>().text = message;
            instance.messagePopup.SetActive(true);
        }
    }

    public static void ShowLoading(string textToShow = "Loading...", float hideTime = 0)
    {
        if (!instance.loadingScreen)
        {
            instance.loadingScreen = Instantiate(instance.loadingScreenPrefab);
            DontDestroyOnLoad(instance.loadingScreen);
        }
        //instance.loadingScreen.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = textToShow;

        instance.loadingScreen.SetActive(true);

        if (hideTime != 0) HelperUtil.CallAfterDelay(HideLoading, hideTime);
    }

    public static void HideLoading()
    {
        if (instance.loadingScreen && instance.loadingScreen.activeSelf) instance.loadingScreen.SetActive(false);
    }

    public static void HideMessage()
    {
        if (instance.messagePopup != null && instance.messagePopup.activeInHierarchy)
        {
            instance.messagePopup.SetActive(false);
        }
    }

    public static GameObject Instantiate(string path)
    {
        return GameObject.Instantiate(Resources.Load(path) as GameObject);
    }
    #endregion

    #region Download Image

    public static Dictionary<string, Texture> userProfilePicDictionary = new Dictionary<string, Texture>();
    public static void GetTextureFromURL(RawImage image, string url, Action onSuccess = null)
    {
        //if (userProfilePicDictionary.ContainsKey(url))
        //{
        //    image.texture = userProfilePicDictionary[url];
        //}
        //else
        //{
        instance.StartCoroutine(enumerator());
        IEnumerator enumerator()
        {
            UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url);
            yield return webRequest.SendWebRequest();

            if (webRequest.isHttpError || webRequest.isNetworkError)
            {
                Debug.Log("Network Error" + webRequest.error);
            }
            else
            {
                Texture imageTexture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                //if (!userProfilePicDictionary.ContainsKey(url))
                //{
                //    userProfilePicDictionary.Add(url, imageTexture);
                //}
                if (image) image.texture = imageTexture;

                //Call OnSuccess callback if any.
                onSuccess?.Invoke();
            }
        }
        //}
    }

    public Dictionary<string, Texture> universalTextureData = new Dictionary<string, Texture>();

  
    #endregion

    #region Manage Text Length
    public string ManageLenghtOfText(string Text, int maxlimit)
    {
        string retundata = "";
        if (Text.Length > maxlimit)
        {
            for (int i = 0; i < maxlimit; i++)
            {
                retundata += Text[i].ToString();
            }
            retundata += "...";
            return retundata;

        }
        else
        {
            return Text;
        }
    }
    #endregion

    #region TEXTURE_OPERATIONS

  
    public static Texture2D RotateTexture(Texture2D targetTexture)
    {
        Texture2D newTexture = new Texture2D(targetTexture.height, targetTexture.width, targetTexture.format, false);

        for (int i = 0; i < targetTexture.width; i++)
        {
            for (int j = 0; j < targetTexture.height; j++)
            {
                newTexture.SetPixel(j, i, targetTexture.GetPixel(targetTexture.width - i, j));
            }
        }
        newTexture.Apply();
        return newTexture;
    }

    #endregion

 

}

#region Interfaces

    //This is For All Scenes API Responce Handle
    public interface IUniversalResponseHandler
    {
        void OnGetHouseDataResponse(string response, string responseCode);

        void OnGetLeaseRoomDataResponse(string response, string responseCode);

        void OnRemoveLeaseRoomDataResponse(string response, string responseCode);

        void OnGetHouseItemDataResponse(string response, string responseCode);

        void OnItemPlaceDataResponse(string response,string responceCode);

        void OnGetViewItemResponse(string response, string responseCode);

        void OnGetDetilForPlaceResponse(string response, string responseCode);
    }

    //This is for Handle All Interaction Action Handle
  

    //This is For the event of UI when Player interaction with Door and Other objects
    public interface IUniversalUIHandler 
    {
        string currentMetaObject_Id { get; set; }
        void ActivatePanel(string panelName);
        Text description_ItemPlaceScreenProp { get; set; }
        void OnClickplaceNewObject_ItemPlaceScreen(string itemId, string currentHouseId);
    }

    //For handling land APis
    public interface IUniversalLandHandler
    {
        public void OnGetBasicLandInfoResponse(string response, string responseCode);
    
        public void OnGetAllLandResponse(string response, string responseCode);

        public void OnGetAllLandObjectResponse(string response, string responseCode);

        public void OnGetLeasableLandResponse(string response, string responseCode);

        public void OnGetNonLeasableLandResponse(string response, string responseCode);

        public void OnGetUnRentedLandResponse(string response, string responseCode);

        public void OnGetPaginationLandResponse(string response, string responseCode);
        
        public void OnGetRentedLandResponse(string response, string responseCode);       

        public void OnGetLandbyIDResponse(string response, string responseCode);

        public void OnGetObjectOnLandResponse(string response, string responseCode);

        public void OnSaveLandInfoResponse(string response, string responceCode);

        public void OnRentPaymentConfirmResponse(string response, string responseCode);
    }

#endregion

#region Data Classes

public class MessageActionData
{
    public string buttonName;
    public UnityEngine.Events.UnityAction action;

    public bool InUse
    {
        get
        {
            return !string.IsNullOrEmpty(buttonName) || action != null;
        }
    }

    public MessageActionData(string newStringName, UnityEngine.Events.UnityAction newAction)
    {
        buttonName = newStringName;
        action = newAction;
    }
}


#endregion



public static class HelperExtension
{
  
}