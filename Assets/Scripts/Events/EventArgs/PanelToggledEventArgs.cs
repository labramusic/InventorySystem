using System;

public class PanelToggledEventArgs : EventArgs
{
    public bool PanelActive { get; }

    public PanelToggledEventArgs(bool panelActive)
    {
        PanelActive = panelActive;
    }
}
