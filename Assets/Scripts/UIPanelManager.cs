﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelManager : MonoBehaviour
{
    public GameObject InventoryPanel;
    public GameObject EquipmentPanel;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            TogglePanel(InventoryPanel);
        }

        if (Input.GetButtonDown("Equipment"))
        {
            TogglePanel(EquipmentPanel);
        }
    }

    public void TogglePanel(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
    }
}