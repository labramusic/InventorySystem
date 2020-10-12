using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributesUI : UIPanel
{
    private PlayerAttributes _playerAttributes;
    private List<AttributeNameType> _attributesNames;

    private Text[] _attributeLabelTexts;
    private Text[] _attributeValTexts;

    protected override void Start()
    {
        base.Start();
        ToggleButton = "Attributes";

        _playerAttributes = PlayerAttributes.Instance;
        _attributesNames = _playerAttributes.GetAttributesNames();
        InitializeText();

        EventManager.Instance.AddListener(EventName.AttributesUpdated, OnAttributesUpdated);
        OnAttributesUpdated(new AttributesUpdatedEventArgs());
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(EventName.AttributesUpdated, OnAttributesUpdated);
    }

    private void InitializeText()
    {
        int numAttributes = _attributesNames.Count;
        _attributeLabelTexts = new Text[numAttributes];
        _attributeValTexts = new Text[numAttributes];
        for (int i = 0; i < numAttributes; ++i)
        {
            _attributeLabelTexts[i] = CreateText(_attributesNames[i] + "AttrTitle", _attributesNames[i] + ":", TextAnchor.MiddleLeft);
            _attributeValTexts[i] = CreateText(_attributesNames[i] + "AttrValue", _playerAttributes.GetAttributeValueDisplay(_attributesNames[i]), TextAnchor.MiddleCenter);
        }
    }

    private Text CreateText(string title, string text, TextAnchor alignment)
    {
        var textObject = new GameObject(title);
        textObject.transform.SetParent(Panel.transform);
        textObject.transform.localScale = Vector3.one;
        var textComponent = textObject.AddComponent<Text>();
        textComponent.text = text;
        textComponent.font = Font.CreateDynamicFontFromOSFont("Arial", 15);
        textComponent.fontSize = 15;
        textComponent.color = new Color32(221, 221, 221, 255);
        textComponent.alignment = alignment;

        return textComponent;
    }

    private void OnAttributesUpdated(EventArgs args)
    {
        if (!(args is AttributesUpdatedEventArgs)) return;

        int numAttributes = _attributesNames.Count;
        for (int i = 0; i < numAttributes; ++i)
        {
            _attributeValTexts[i].text = _playerAttributes.GetAttributeValueDisplay(_attributesNames[i]);
        }
    }
}
