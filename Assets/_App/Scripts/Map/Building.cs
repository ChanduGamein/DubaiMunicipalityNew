using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour, IleaderBoardMember, IVisabilityCheck
{
    public Transform myCameraPosition;
    public Transform MyCameraRotation
    {
        get => myCameraPosition;
    }
    public BuildingsSO buildingData;
    public float Health;

    private SpriteRenderer m_Renderer;
    private Image img;
    private Button button;
    private void Awake()
    {
        m_Renderer = GetComponent<SpriteRenderer>() ? GetComponent<SpriteRenderer>() : gameObject.AddComponent<SpriteRenderer>();
    }

    private void Start()
    {

        Health = UnityEngine.Random.Range(0.0f, 1.0f) * 100;
        GameManager.Instance.GridSystem.AssignToBoardMembers(this);

        button = GetComponent<Button>();
        if (button && !buildingData || buildingData?.name == "No Name")
        {
            button.interactable = false;
        }
        else if (button)
        {
            SetChartColor();
        }
        SetName();
        SearchBox.Instance.visibles.Add(this);

    }

    private void OnEnable()
    {
        var button = GetComponent<Button>();
        if (!buildingData || buildingData?.name == "No Name")
        {
            button.interactable = false;
        }
    }

    private void SetChartColor()
    {
        foreach (Transform child in transform)
        {
            img = child.GetComponent<Image>();
            if (img != null) break;
        }
        if (img && button && button.interactable && GetComponent<Image>())
        {
            img.color = Color.white;
        }
    }

    private void SetName()
    {
        var name = GetComponentInChildren<TextMeshProUGUI>();
        if (!name)
        {
            var go = Instantiate(new GameObject(), transform);
            go.transform.localPosition = new Vector2(55, 70);
            name = go.AddComponent<TextMeshProUGUI>();
            go.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        string n = "";
        if (buildingData.name.Length > 15)
        {
            int count = buildingData.name.Length - 15;
            n = buildingData.name.Remove(15, count);
            n = n + "...";
        }
        else
        {
            n = buildingData.name;
        }
        name.text = n;


    }
    public void ShowDanger()
    {
        buildingData.material.color = Color.red;
    }
    public LeaderBoardMember GetMember()
    {
        LeaderBoardMember m = new LeaderBoardMember()
        {
            Name = buildingData.name,
            Health = this.Health
        };
        return m;
    }

    public void CheckVisibility()
    {
        if (SearchBox.Instance.SearchList.Find(b => b == this) != null)
        {
            if (m_Renderer && m_Renderer.isVisible)
            {
                return;
            }
            else
            {
                SearchBox.Instance.SearchList.Remove(this);
            }
        }
        else
        {
            if (m_Renderer && m_Renderer.isVisible)
            {
                SearchBox.Instance.SearchList.Add(this);
            }
            else
            {
                return;
            }
        }
    }
}
