using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributesUI : MonoBehaviour
{
    public Text StrengthAttrValue;
    public Text VitalityAttrValue;
    public Text IntelligenceAttrValue;
    public Text AgilityAttrValue;

    private PlayerAttributes _playerAttributes;

    // Start is called before the first frame update
    private void Start()
    {
        _playerAttributes = PlayerAttributes.Instance;
        _playerAttributes.OnAttributesUpdateCallback += UpdateUI;
        //UpdateUI();
    }

    private void OnDestroy()
    {
        _playerAttributes.OnAttributesUpdateCallback -= UpdateUI;
    }

    void UpdateUI()
    {
        StrengthAttrValue.text = _playerAttributes.GetAttributeValue(AttributeNameType.Strength).ToString();
        VitalityAttrValue.text = _playerAttributes.GetAttributeValue(AttributeNameType.Vitality).ToString();
        IntelligenceAttrValue.text = _playerAttributes.GetAttributeValue(AttributeNameType.Intelligence).ToString();
        AgilityAttrValue.text = _playerAttributes.GetAttributeValue(AttributeNameType.Agility).ToString();
    }
}
