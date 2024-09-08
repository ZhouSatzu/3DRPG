using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    Button newGameBtn;
    Button continueBtn;
    Button quitBtn;

    private void Awake()
    {
        newGameBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        quitBtn = transform.GetChild(3).GetComponent<Button>();

        newGameBtn.onClick.AddListener(NewGame);
        continueBtn.onClick.AddListener(ContinueGame);
        quitBtn.onClick.AddListener(QuitGame);
    }

    void NewGame()
    {
        //删除全部存储文件
        PlayerPrefs.DeleteAll();
        
        //转换场景
        SceneController.Instance.TransitionToFirstLevel();
    }

    void ContinueGame()
    {
        //转换场景读取进度
        SceneController.Instance.TransitionToLoadLevel();
    }

    void QuitGame()
    {
        Application.Quit();
        Debug.Log("quit");
    }
}
