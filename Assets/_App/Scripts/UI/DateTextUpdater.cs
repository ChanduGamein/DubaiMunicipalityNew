using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DateTextUpdater : MonoBehaviour
{
    public Text month;

    public void UpdateMonth()
    {
        month.text = this.GetComponentInChildren<Text>().text;
    }

}
