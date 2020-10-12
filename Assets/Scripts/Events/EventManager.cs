using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EventName
{
    ItemUsed, EquipmentUsed, ConsumableUsed,
    ItemPickedUp, EquipmentPickedUp,
    InventoryUpdated, EquipmentChanged, AttributesUpdated,
    InventoryPanelToggled, EquipmentPanelToggled,
    WalkDistanceThresholdReached, EquipmentDestroyed
}

public class EventManager : MonoBehaviour
{
    #region Singleton

    public static EventManager Instance { get; private set; }

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

    private Dictionary<EventName, Action<EventArgs>> _events;

    private void OnEnable()
    {
        _events = new Dictionary<EventName, Action<EventArgs>>();
    }

    public void AddListener(EventName eventName, Action<EventArgs> listener)
    {
        if (_events.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent += listener;
            _events[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            _events.Add(eventName, thisEvent);
        }
    }

    public void RemoveListener(EventName eventName, Action<EventArgs> listener)
    {
        if (_events.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent -= listener;
            _events[eventName] = thisEvent;
        }
    }

    public void InvokeEvent(EventName eventName, EventArgs args)
    {
        if (_events.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent.Invoke(args);
        }
    }
}
