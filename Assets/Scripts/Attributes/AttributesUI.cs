using System;
using UnityEngine;
using UnityEngine.UI;

public class AttributesUI : UIPanel
{
    private PlayerAttributes _playerAttributes;

    private Text[] _attributeLabelTexts;
    private Text[] _attributeValTexts;

    protected override void Start()
    {
        base.Start();
        ToggleButton = "Attributes";

        _playerAttributes = PlayerAttributes.Instance;
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
        int numAttributes = Enum.GetNames(typeof(AttributeNameType)).Length;
        _attributeLabelTexts = new Text[numAttributes];
        _attributeValTexts = new Text[numAttributes];
        for (int i = 0; i < numAttributes; ++i)
        {
            _attributeLabelTexts[i] = CreateText((AttributeNameType)i + "AttrTitle", (AttributeNameType)i + ":", TextAnchor.MiddleLeft);
            _attributeValTexts[i] = CreateText((AttributeNameType)i + "AttrValue", _playerAttributes.GetAttributeValue((AttributeNameType)i).ToString(), TextAnchor.MiddleCenter);
        }
    }

    private Text CreateText(string title, string text, TextAnchor alignment)
    {
        var textObject = new GameObject(title);
        textObject.transform.SetParent(Panel.transform);
        var textComponent = textObject.AddComponent<Text>();
        textComponent.text = text;
        textComponent.font = Font.CreateDynamicFontFromOSFont("Arial", 17);
        textComponent.fontSize = 17;
        textComponent.color = new Color32(221, 221, 221, 255);
        textComponent.alignment = alignment;

        return textComponent;
    }

    private void OnAttributesUpdated(EventArgs args)
    {
        if (!(args is AttributesUpdatedEventArgs)) return;

        int numAttributes = Enum.GetNames(typeof(AttributeNameType)).Length;
        for (int i = 0; i < numAttributes; ++i)
        {
            _attributeValTexts[i].text = _playerAttributes.GetAttributeValue((AttributeNameType) i).ToString();
        }
    }
}
