using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all Singleton manager classes.
/// Singleton pattern ensures a class has only one instance and provides a global point of access to it.
/// </summary>
/// @ingroup managers
/// @brief Base class for Singleton Managers
public abstract class SingletonManager<T> : Manager where T : SingletonManager<T>
{
    // Static variable that holds the single instance of the manager
    private static T instance;

    // Public property to access the singleton instance
    public static T Instance
    {
        get
        {
            // If the instance doesn't exist yet, try to find it in the scene
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                // If it's still null, create a new GameObject and add the singleton component
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    instance = singletonObject.AddComponent<T>();
                    // Make sure the new object persists across scene loads
                    DontDestroyOnLoad(singletonObject);
                }
            }
            // Return the instance
            return instance;
        }
    }

    // Awake is called when the script instance is being loaded
    protected override void Awake()      
    {
        base.Awake();
        // If this is the first instance, set it and make it persistent
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        // If another instance exists, destroy this one to enforce singleton property
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

}