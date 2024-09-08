using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    //作为键值对使用
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
        //保证在加载时依然存在，使加载能够顺利进行
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

    //objetc:Base class for all objects Unity can reference key:键值对
    public void Save(object data,string key)
    {
        //通过ToJson把data化成string方便使用PlayerPrefs存储
        var jsonData = JsonUtility.ToJson(data);

        //PlayerPrefs is a class that stores Player preferences between game sessions. It can store string, float and integer values into the user’s platform registry.
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);
        //将存储写入磁盘
        PlayerPrefs.Save();
    }

    public void Load(object data, string key)
    {
        if(PlayerPrefs.HasKey(key))
        {
            //将存储的字符串写回data文件
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }
}
