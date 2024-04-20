using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders.Simulation;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static object lockObject = new();

    protected static T m_Instance;
    public static T Instance
    {

        get
        {
            lock (lockObject)
            {
                if(m_Instance == null)
                {
                    m_Instance = FindObjectOfType<T>();

                    if(m_Instance == null){
                        var gameObject = new GameObject(m_Instance.ToString());
                        gameObject.AddComponent<T>();
                    }
                }

                return m_Instance;
            }
        }
    }

    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
