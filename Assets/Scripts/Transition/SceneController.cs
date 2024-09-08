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
        //��֤�ڼ���ʱ��Ȼ���ڣ�ʹ�����ܹ�˳������
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
        //������������
        SaveManager.Instance.SavePlayerData();
        
        //ͬ����ת��
        if(SceneManager.GetActiveScene().name == sceneName)
        {
            player = GameManager.Instance.playerStats.gameObject;
            agent = player.GetComponent<NavMeshAgent>();

            agent.enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            agent.enabled = true;
            yield break;
        }
        //��ͬ����ת��
        else
        {
            //ʹ���첽���س�����yield returnָ���ǵȴ��������������ִ�к���Ĵ���
            yield return SceneManager.LoadSceneAsync(sceneName);
            //��������
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            //������������
            SaveManager.Instance.LoadPlayerData();
            yield return null;
        }
    }

    //�õ����ͱ�ǩ����Ӧ��Destination
    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        //entrances�ǳ���������TransitionDestination��ɵ�����
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

            //������Ϸ
            SaveManager.Instance.SavePlayerData();
            yield break;
        }       
    }

    IEnumerator LoadMain()
    {
        yield return SceneManager.LoadSceneAsync("main");
        yield break;
    }

    //player������ص�������
    public void EndNotify()
    {
        if(fadeFinish)
        {
            fadeFinish = false;
            StartCoroutine(LoadMain()); 
        }
    }
}
