using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
{
    private static volatile T _instance = null;
    private static readonly object _lock = new object();

    private  void Awake()
    {
        if (_instance == null)
        {
            Debug.Log(typeof(T).Name+"实例化");
            _instance = this as T;

        }
        else
        {
            Debug.Log(typeof(T).Name+"重复实例");
            // 重复实例直接销毁，防止BUG
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        Init();
    }

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();
                        if (_instance == null)
                        {
                            GameObject obj = new GameObject(typeof(T).Name);
                            _instance = obj.AddComponent<T>();
                        }
                    }
                }
            }
            return _instance;
        }
    }
    protected virtual void Init() { }

    public void DestroySelf()
    {
        Dispose();
        Destroy(gameObject);
    }

    public virtual void Dispose() { }

    //销毁时清空静态实例，避免空引用报错
    protected virtual void OnDestroy()
    {
        Dispose();
        if (_instance == this)
        {
            _instance = null;
        }
    }
}