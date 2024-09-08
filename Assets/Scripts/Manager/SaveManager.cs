using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    //��Ϊ��ֵ��ʹ��
    string sceneName = "level";

    public string SceneName
    {
        get
        {
            return PlayerPrefs.GetString(sceneName);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        //��֤�ڼ���ʱ��Ȼ���ڣ�ʹ�����ܹ�˳������
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneController.Instance.TransitionToMain();
            //Debug.Log("save!");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlayerData();
            Debug.Log("save!");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayerData();
            Debug.Log("load!");
        }
    }

    public void SavePlayerData()
    {
        Save(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
    }

    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
    }

    //objetc:Base class for all objects Unity can reference key:��ֵ��
    public void Save(object data,string key)
    {
        //ͨ��ToJson��data����string����ʹ��PlayerPrefs�洢
        var jsonData = JsonUtility.ToJson(data);

        //PlayerPrefs is a class that stores Player preferences between game sessions. It can store string, float and integer values into the user��s platform registry.
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);
        //���洢д�����
        PlayerPrefs.Save();
    }

    public void Load(object data, string key)
    {
        if(PlayerPrefs.HasKey(key))
        {
            //���洢���ַ���д��data�ļ�
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }
}
