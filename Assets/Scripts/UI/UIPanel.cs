using UnityEngine;

public class UIPanel : MonoBehaviour
{
    public GameObject Panel;
    public string ToggleButton;

    protected virtual void Start()
    {
        Panel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown(ToggleButton))
        {
            TogglePanel();
        }
    }

    public virtual void TogglePanel()
    {
        Panel.SetActive(!Panel.activeSelf);
    }
}
