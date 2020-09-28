using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelManager : MonoBehaviour
{
    #region Singleton

    public static UIPanelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    #endregion

    public GameObject InventoryPanel;
    public GameObject EquipmentPanel;
    public GameObject AttributesPanel;

    [NonSerialized]
    public Image DraggedIcon = null;
    [NonSerialized]
    public int SelectedInventorySlotIndex = -1;
    [NonSerialized]
    public int SelectedEquipSlotIndex = -1;

    // Start is called before the first frame update
    private void Start()
    {
        InventoryPanel.SetActive(false);
        EquipmentPanel.SetActive(false);
        AttributesPanel.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
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

    private void LateUpdate()
    {
        if (DraggedIcon)
        {
            DraggedIcon.transform.position = Input.mousePosition;
        }
    }

    public void TogglePanel(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);

        if (!InventoryPanel.activeSelf && !EquipmentPanel.activeSelf)
        {
            StopDraggingIcon();
        }
    }

    public void StartDraggingIcon(Image Icon)
    {
        DraggedIcon = Instantiate(Icon, transform);
        DraggedIcon.GetComponent<Canvas>().sortingOrder += 1;
        // hide mouse?

    }

    public void StopDraggingIcon()
    {
        if (DraggedIcon == null) return;
        Destroy(DraggedIcon.gameObject);

        SelectedInventorySlotIndex = -1;
        SelectedEquipSlotIndex = -1;
    }
}
