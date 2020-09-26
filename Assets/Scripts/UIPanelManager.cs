using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelManager : MonoBehaviour
{
    public GameObject InventoryPanel;
    public GameObject EquipmentPanel;
    public GameObject AttributesPanel;

    // Start is called before the first frame update
    void Start()
    {
        InventoryPanel.SetActive(false);
        EquipmentPanel.SetActive(false);
        AttributesPanel.SetActive(false);
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

        if (Input.GetButtonDown("Attributes"))
        {
            TogglePanel(AttributesPanel);
        }
    }

    public void TogglePanel(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
    }
}
