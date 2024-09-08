using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class SceneController : Singleton<SceneController>,IEndGameObserver
{
    public GameObject playerPrefab;
    GameObject player;
    NavMeshAgent agent;

    bool fadeFinish;

    protected override void Awake()
    {
        base.Awake();
        //保证在加载时依然存在，使加载能够顺利进行
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        GameManager.Instance.AddObserver(this);
        fadeFinish = true;
    }

    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch(transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;
            case TransitionPoint.TransitionType.DiffentScene:
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;
        }
    }

    IEnumerator Transition(string sceneName,TransitionDestination.DestinationTag destinationTag)
    {
        //保存人物数据
        SaveManager.Instance.SavePlayerData();
        
        //同场景转换
        if(SceneManager.GetActiveScene().name == sceneName)
        {
            player = GameManager.Instance.playerStats.gameObject;
            agent = player.GetComponent<NavMeshAgent>();

            agent.enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            agent.enabled = true;
            yield break;
        }
        //不同场景转换
        else
        {
            //使用异步加载场景，yield return指的是等待场景加载完成再执行后面的代码
            yield return SceneManager.LoadSceneAsync(sceneName);
            //生成人物
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            //加载人物数据
            SaveManager.Instance.LoadPlayerData();
            yield return null;
        }
    }

    //拿到传送标签所对应的Destination
    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        //entrances是场景中所有TransitionDestination组成的数组
        var entrances = FindObjectsOfType<TransitionDestination>();

        foreach(var entrance in entrances)
        {
            if(entrance.destinationTag == destinationTag)
                return entrance;
        }

        return null;
    }

    public void TransitionToMain()
    {
        StartCoroutine(LoadMain());
    }

    public void TransitionToFirstLevel()
    {
        StartCoroutine(LoadLevel("SampleScene"));
    }

    public void TransitionToLoadLevel()
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        if(sceneName != "")
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return player = Instantiate(playerPrefab, GameManager.Instance.GetEntrance().position, GameManager.Instance.GetEntrance().rotation);

            //保存游戏
            SaveManager.Instance.SavePlayerData();
            yield break;
        }       
    }

    IEnumerator LoadMain()
    {
        yield return SceneManager.LoadSceneAsync("main");
        yield break;
    }

    //player死亡后回到主场景
    public void EndNotify()
    {
        if(fadeFinish)
        {
            fadeFinish = false;
            StartCoroutine(LoadMain()); 
        }
    }
}
