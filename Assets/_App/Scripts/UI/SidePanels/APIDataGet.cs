using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class APIDataGet : MonoBehaviour
{
    [Header(" Cleaning Stats")]
    #region Cleaning Stats Variables
    public Text tanksCleanedWithinComplaince;
    public Text cleanedAfterComplaince;
    public Text yetToBeCleaned;
    public Text failedToClean;
    public Text totalNumber;
    public Text totalCleaned;
    public Text totalNotCleaned;
    #endregion

    private void Update()
    {
        SetApiDataToUI();
    }

    public void SetApiDataToUI()
    {
        SetCleaningStatesToUI();
    }
    public void SetCleaningStatesToUI()
    {
        tanksCleanedWithinComplaince.text = APIResponseManager.instance.getDashboardStats.data.cleanedWithinCompliance.ToString();
        cleanedAfterComplaince.text = APIResponseManager.instance.getDashboardStats.data.cleanedAfterCompliance.ToString();
        yetToBeCleaned.text = APIResponseManager.instance.getDashboardStats.data.yetToBeCleanedWithinCompliance.ToString();
        failedToClean.text = APIResponseManager.instance.getDashboardStats.data.yetToBeCleanedAfterCompliance.ToString();

        var _totalCleanedCount = (APIResponseManager.instance.getDashboardStats.data.cleanedWithinCompliance + APIResponseManager.instance.getDashboardStats.data.cleanedAfterCompliance);
        totalCleaned.text = _totalCleanedCount.ToString();

        var _totalNotCleanedCount = (APIResponseManager.instance.getDashboardStats.data.yetToBeCleanedWithinCompliance + APIResponseManager.instance.getDashboardStats.data.yetToBeCleanedAfterCompliance);
        totalNotCleaned.text = _totalNotCleanedCount.ToString();

        var _totalNumberCount = _totalCleanedCount + _totalNotCleanedCount;
        totalNumber.text = _totalNumberCount.ToString();
    }
}
