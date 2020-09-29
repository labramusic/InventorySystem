using System;
using UnityEngine;

public enum CollisionMethodType
{ 
    Spatial, Trigger, OverlapCircle, CircleCasting
}

public class CollisionTester : MonoBehaviour
{
    #region Singleton

    public static CollisionTester Instance { get; private set; }

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

    public CollisionMethodType CurrentCollision;

    public void NextCollision()
    {
        CurrentCollision = (CollisionMethodType)((int)CurrentCollision + 1);
        if ((int) CurrentCollision > Enum.GetNames(typeof(CollisionMethodType)).Length - 1)
        {
            CurrentCollision = (CollisionMethodType) 0;
        }

        Debug.Log($"Using {CurrentCollision} collision.");
    }
}
