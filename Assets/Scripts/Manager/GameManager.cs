using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{ 
    public CharacterStats playerStats;

    private CinemachineFreeLook followCamera;

    protected override void Awake()
    {
        base.Awake();
        //保证在加载时依然存在，使加载能够顺利进行
        DontDestroyOnLoad(this);
    }

    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    public void RigisterPlayer(CharacterStats player)
    {
        playerStats = player;
        //设置跟踪相机跟随
        followCamera = FindObjectOfType<CinemachineFreeLook>();
        if(followCamera != null)
        {
            followCamera.LookAt = playerStats.transform.GetChild(2);
            followCamera.Follow = playerStats.transform.GetChild(2);
        }
    }

    public void AddObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }

    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach(var observer in endGameObservers)
        {
            observer.EndNotify();
        }
    }

    public Transform GetEntrance()
    {
        foreach (var item in FindObjectsOfType<TransitionDestination>())
        {
            if (item.destinationTag == TransitionDestination.DestinationTag.Enter)
                return item.transform;
        }
        return null;
    }
}
