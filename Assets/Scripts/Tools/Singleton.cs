using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���͵���ģʽ�࣬T ����̳��� Singleton<T>����֤�˵��������Ͱ�ȫ
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    // ��̬�ֶΣ����ڴ洢����ʵ��������ʵ��������ֶΣ�ȷ��Ψһ��
    private static T instance;

    // ���о�̬���ԣ��ⲿͨ�������Է��ʵ���ʵ��
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

    // ��̬���ԣ����ص����Ƿ��ѱ���ʼ��
    public static bool IsInitialized
    {
        get { return instance != null; }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null; // ����Ե��������ã�����ϵͳ�����ڴ�
        }
    }
}

// ʾ���÷���
// ���� GameManager ��ϣ��ʹ�õ���ģʽ���̳� Singleton<GameManager>
// public class GameManager : Singleton<GameManager> { }
