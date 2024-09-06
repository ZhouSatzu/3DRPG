using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 泛型单例模式类，T 必须继承自 Singleton<T>，保证了单例的类型安全
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    // 静态字段，用于存储单例实例，所有实例共享此字段，确保唯一性
    private static T instance;

    // 公有静态属性，外部通过该属性访问单例实例
    public static T Instance
    {
        get { return instance; }
    }

    protected virtual void Awake()
    {
        if (instance != null)
            Destroy(gameObject); 
        else
            instance = (T)this;  
    }

    // 静态属性，返回单例是否已被初始化
    public static bool IsInitialized
    {
        get { return instance != null; }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null; // 清除对单例的引用，允许系统回收内存
        }
    }
}

// 示例用法：
// 假设 GameManager 类希望使用单例模式，继承 Singleton<GameManager>
// public class GameManager : Singleton<GameManager> { }
