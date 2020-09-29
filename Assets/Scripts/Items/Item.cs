using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string ItemName = "New Item";
    public Sprite Icon = null;

    public abstract bool Interact();
}
