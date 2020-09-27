using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    #region Singleton

    public static PlayerAttributes Instance { get; private set; }

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

    //
    public delegate void OnAttributesUpdate();
    public OnAttributesUpdate OnAttributesUpdateCallback;
    //

    [SerializeField] 
    private List<Attribute> _baseAttributes = new List<Attribute>();

    private readonly Dictionary<AttributeNameType, Attribute> _attributes = new Dictionary<AttributeNameType, Attribute>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (var attribute in _baseAttributes)
        {
            _attributes[attribute.AttributeName] = attribute;
        }
        OnAttributesUpdateCallback?.Invoke();
    }

    public int GetAttributeValue(AttributeNameType attributeName)
    {
        return _attributes.ContainsKey(attributeName) ?
            _attributes[attributeName].GetValue() : 0;
    }
}