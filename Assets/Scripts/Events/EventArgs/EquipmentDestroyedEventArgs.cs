using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentDestroyedEventArgs : EventArgs
{
    public ExpendableItem Expendable { get; }

    public EquipmentDestroyedEventArgs(ExpendableItem expendable)
    {
        Expendable = expendable;
    }
}
